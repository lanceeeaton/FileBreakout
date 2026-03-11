using System;
using System.IO;
using static FileBreakout.FileBreakout;

namespace FileBreakout;
/// <summary>
/// Main form for the File Breakout application.
/// </summary>
public partial class FileBreakout : Form
{
    string fileBreakoutFolder = "";
    List<string> addedDateFolderPaths = new List<string>();
    public record FilePaths(string sourceFilePath, string destinationFilePath);
    List<FilePaths> filePaths = new List<FilePaths>();
    CancellationTokenSource? cancellationTokenSource;
    CancellationToken cancellationToken;

    /// <summary>
    /// Reverts changes made to the destination files by either deleting them or restoring the original source files,
    /// depending on the specified option.
    /// </summary>
    /// <param name="keptCopy">If <see langword="true"/>, deletes the destination files and leaves the original source files unchanged; if <see
    /// langword="false"/>, restores the original source files by moving the destination files back to their original
    /// locations.</param>
    private void UndoFiles (bool keptCopy)
    {
        foreach (var filePath in filePaths)
        {
            if (File.Exists(filePath.destinationFilePath))
            {
                if (keptCopy)
                {
                    File.Delete(filePath.destinationFilePath);
                }
                else
                {
                    File.Move(filePath.destinationFilePath, filePath.sourceFilePath, overwrite: true);
                }
            }
        }
    }
    /// <summary>
    /// Removes any date-based folders that were previously added by the operation.
    /// </summary>
    /// <remarks>This method deletes directories listed in the collection of added date folder paths if they
    /// exist. It is intended to reverse folder creation performed earlier in the workflow.
    /// Use with caution, as any data within these folders will be
    /// lost.</remarks>
    private void UndoDateFolders()
    {
        foreach (var dateFolderPath in addedDateFolderPaths)
        {
            Directory.Delete(dateFolderPath);
        }
    }
    /// <summary>
    /// Reverts the last processing operation by undoing file changes and removing empty directories.
    /// </summary>
    private void UndoProcessing()
    {
        if (Directory.Exists(fileBreakoutFolder))
        {
            UndoFiles(keepCopyCheckbox.Checked);
            UndoDateFolders();
            if (Directory.GetFiles(fileBreakoutFolder).Length == 0 && Directory.GetDirectories(fileBreakoutFolder).Length == 0)
            {
                Directory.Delete(fileBreakoutFolder);
            }
        }
    }
    private void ResetCancellation()
    {
        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }
    }
    /// <summary>
    /// Resets the user interface controls to their initial enabled and cleared state, preparing the application for a
    /// new operation.
    /// </summary>
    private void InitialState()
    {
        selectPathButton.Enabled = true;
        keepCopyCheckbox.Enabled = true;
        stopProcessing.Enabled = false;
        folderPathTextBox.Clear();
    }
    /// <summary>
    /// Clears all nodes from the file tree view and resets the progress bar to its initial state.
    /// </summary>
    private void ResetNodesAndProgress()
    {
        fileTreeView.Nodes.Clear();
        progressBar.Value = 0;
    }
    /// <summary>
    /// Prepares the UI for processing by disabling controls, clearing the file tree view, and resetting the progress bar.
    /// </summary>
    private void PrepareForProcessing()
    {
        selectPathButton.Enabled = false;
        keepCopyCheckbox.Enabled = false;
        startProcessingButton.Enabled = false;
        stopProcessing.Enabled = true;
        ResetNodesAndProgress();
        progressBar.Minimum = 0;
        filePaths.Clear();
        addedDateFolderPaths.Clear();
    }
    /// <summary>
    /// Adds the specified file to a breakout folder organized by year, and updates the
    /// corresponding tree node to reflect the new file location.
    /// </summary>
    /// <remarks>The file is placed in a subfolder based on its last write time, either by year
    /// depending on application settings. The method also updates the UI tree to reflect the new folder and
    /// file. If the 'keep copy' option is enabled, the file is copied; otherwise, it is moved.</remarks>
    /// <param name="fileInfo">The file to be added to the breakout folder. The file's last write time determines its placement within the
    /// folder structure.</param>
    /// <param name="fileBreakoutFolder">The root directory where the breakout folder structure will be created or updated.</param>
    /// <param name="fileBreakoutFolderNode">The tree node representing the breakout folder in the user interface, which will be updated to include the new
    /// file.</param>
    private void AddFile(FileInfo fileInfo, string fileBreakoutFolder, TreeNode fileBreakoutFolderNode)
    {
        var fileYear = fileInfo.LastWriteTime.Year.ToString();

        var fileDatePath = Path.Combine(fileBreakoutFolder, fileYear);

        if (!Directory.Exists(fileDatePath))
        {
            Directory.CreateDirectory(fileDatePath);
            addedDateFolderPaths.Add(fileDatePath);
        }

        Invoke(() =>
        {
            AddTreeNodeDateFolder(fileBreakoutFolderNode, fileYear);
            AddTreeNodeFile(fileBreakoutFolderNode, fileYear, fileInfo.Name);
        });
        var targetPath = Path.Combine(fileDatePath, fileInfo.Name);

        if (keepCopyCheckbox.Checked)
        {
            File.Copy(fileInfo.FullName, targetPath, overwrite: true);
        }
        else
        {
            File.Move(fileInfo.FullName, targetPath, overwrite: true);
        }
        filePaths.Add(new FilePaths(fileInfo.FullName, targetPath));
    }
    /// <summary>
    /// Adds a child node representing a date folder to the specified parent tree node if it does not already exist.
    /// </summary>
    /// <remarks>This method ensures that duplicate date folder nodes are not added to the parent node. If a
    /// node with the specified key already exists, no action is taken.</remarks>
    /// <param name="fileBreakoutFolderNode">The parent <see cref="TreeNode"/> to which the date folder node will be added.</param>
    /// <param name="fileYear">The key and text for the date folder node to add. Cannot be null or empty.</param>
    private void AddTreeNodeDateFolder(TreeNode fileBreakoutFolderNode, string fileYear)
    {
        if (!fileBreakoutFolderNode.Nodes.ContainsKey(fileYear))
        {
            fileBreakoutFolderNode.Nodes.Add(fileYear, fileYear);
        }
    }
    /// <summary>
    /// Adds a new file node with the specified name to the child node identified by the given partial date path within
    /// the provided folder node.
    /// </summary>
    /// <param name="fileBreakoutFolderNode">The parent <see cref="TreeNode"/> representing the folder under which the file node will be added. Cannot be
    /// null.</param>
    /// <param name="fileYear">The key used to locate the child node within <paramref name="fileBreakoutFolderNode"/> where the file node
    /// should be added. Must correspond to an existing child node.</param>
    /// <param name="fileName">The name of the file node to add as a child to the located node. Cannot be null or empty.</param>
    private void AddTreeNodeFile(TreeNode fileBreakoutFolderNode, string fileYear, string fileName)
    {
        fileBreakoutFolderNode.Nodes.Find(fileYear, false)[0].Nodes.Add(fileName);
    }

    public FileBreakout()
    {
        InitializeComponent();
    }
    /// <summary>
    /// Handles the click event for the Select Path button, allowing the user to choose a folder using a dialog. Updates
    /// the folder path text box with the selected folder path.
    /// </summary>
    /// <remarks>If the user cancels the folder selection dialog, no changes are made. If an error occurs
    /// while displaying the dialog or updating the text box, an error message is shown to the user and logged in the
    /// log text box.</remarks>
    private void SelectPathButtonClick(object sender, EventArgs e)
    {
        try
        {
            using var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;

            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            folderPathTextBox.Text = folderBrowserDialog.SelectedPath;
            logTextBox.Clear();
            ResetNodesAndProgress();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message);
        }
    }
    /// <summary>
    /// Handles the click event for the Start Processing button, initiating the processing of files in the selected
    /// folder and updating the UI to reflect progress and results.
    /// </summary>
    /// <remarks>This method prepares the UI for processing, creates necessary directories, and processes each
    /// file asynchronously. Progress and status messages are displayed in the UI. If an error occurs during processing,
    /// an error message is shown to the user.</remarks>
    private async void StartProcessingButtonClick(object sender, EventArgs e)
    {
        try
        {
            if (cancellationTokenSource == null)
            {
                cancellationTokenSource = new CancellationTokenSource();
                cancellationToken = cancellationTokenSource.Token;
                const string fileBreakoutFolderName = "FileBreakout";

                PrepareForProcessing();
                fileBreakoutFolder = Path.Combine(folderPathTextBox.Text, fileBreakoutFolderName);
                var rootNode = new TreeNode(folderPathTextBox.Text);
                logTextBox.AppendText($"Folder: {folderPathTextBox.Text}");
                logTextBox.AppendText(Environment.NewLine);

                fileTreeView.Nodes.Add(rootNode);
                if (!Directory.Exists(fileBreakoutFolder))
                {
                    Directory.CreateDirectory(fileBreakoutFolder);
                }
                var fileBreakoutFolderNode = new TreeNode(fileBreakoutFolderName);
                rootNode.Nodes.Add(fileBreakoutFolderNode);
                rootNode.Expand();
                var _filePaths = Directory.GetFiles(folderPathTextBox.Text);

                progressBar.Maximum = _filePaths.Length;

                logTextBox.AppendText($"Total Files: {_filePaths.Length}");
                logTextBox.AppendText(Environment.NewLine);

                foreach (var fileInfo in _filePaths.Select(path => new FileInfo(path)).OrderBy(fileInfo => fileInfo.LastWriteTime))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await Task.Run(() => AddFile(fileInfo, fileBreakoutFolder, fileBreakoutFolderNode));

                    Invoke(() =>
                    {
                        if (!fileBreakoutFolderNode.IsExpanded)
                        {
                            fileBreakoutFolderNode.Expand();
                        }
                        progressBar.Value += 1;
                    });
                }
                logTextBox.AppendText($"Done");
                ResetCancellation();
                InitialState();
            }
        }
        catch (OperationCanceledException)
        {
            InitialState();
            //Directory.Delete(fileBreakoutFolder, recursive: true);
            UndoProcessing();
            MessageBox.Show("Operation canceled. Any folders or files created have been removed.");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.Message);
        }
    }
    /// <summary>
    /// Handles the TextChanged event for the folder path text box, enabling or disabling the start processing button
    /// based on whether the text box contains any text.
    /// </summary>
    /// <remarks>The start processing button is only enabled when the folder path text box is not empty,
    /// preventing users from starting processing without specifying a folder path.</remarks>
    private void FolderPathTextBoxTextChanged(object sender, EventArgs e)
    {
        if (folderPathTextBox.Text.Length > 0)
        {
            startProcessingButton.Enabled = true;
        }
        else
        {
            startProcessingButton.Enabled = false;
        }
    }
    /// <summary>
    /// Handles a user action to stop ongoing processing by requesting cancellation of the current operation.
    /// </summary>
    /// <remarks>This method is intended to be used as an event handler for a stop button. It
    /// signals cancellation to any ongoing operation that supports cancellation via a
    /// CancellationTokenSource.</remarks>
    private void StopProcessingClick(object sender, EventArgs e)
    {
        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel();
            ResetCancellation();
        }
        else
        {
            UndoProcessing();
        }
    }
}
