namespace FileBreakout;
/// <summary>
/// Main form for the File Breakout application.
/// </summary>
public partial class FileBreakout : Form
{
    /// <summary>
    /// Enables controls
    /// </summary>
    private void EnableControls()
    {
        breakoutFilesButton.Enabled = true;
        keepCopyCheckbox.Enabled = true;
        breakoutByMonthCheckbox.Enabled = true;
    }
    /// <summary>
    /// Prepares the UI for processing by disabling controls, clearing the file tree view, and resetting the progress bar.
    /// </summary>
    private void PrepareForProcessing()
    {
        breakoutFilesButton.Enabled = false;
        keepCopyCheckbox.Enabled = false;
        breakoutByMonthCheckbox.Enabled = false;
        fileTreeView.Nodes.Clear();
        progressBar.Value = 0;
        progressBar.Minimum = 0;
    }
    /// <summary>
    /// Adds the specified file to a breakout folder organized by year or by year and month, and updates the
    /// corresponding tree node to reflect the new file location.
    /// </summary>
    /// <remarks>The file is placed in a subfolder based on its last write time, either by year or by year and
    /// month, depending on application settings. The method also updates the UI tree to reflect the new folder and
    /// file. If the 'keep copy' option is enabled, the file is copied; otherwise, it is moved.</remarks>
    /// <param name="fileInfo">The file to be added to the breakout folder. The file's last write time determines its placement within the
    /// folder structure.</param>
    /// <param name="fileBreakoutFolder">The root directory where the breakout folder structure will be created or updated.</param>
    /// <param name="fileBreakoutFolderNode">The tree node representing the breakout folder in the user interface, which will be updated to include the new
    /// file.</param>
    private void AddFile(FileInfo fileInfo, string fileBreakoutFolder, TreeNode fileBreakoutFolderNode)
    {
        var fileYear = fileInfo.LastWriteTime.Year.ToString();
        var fileMonth = fileInfo.LastWriteTime.Month.ToString("D2");
        var partialDatePath = (breakoutByMonthCheckbox.Checked) ? $"{fileYear}-{fileMonth}" : fileYear;

        var fileDatePath = Path.Combine(fileBreakoutFolder, partialDatePath);

        Directory.CreateDirectory(fileDatePath);
        Invoke(() =>
        {
            AddTreeNodeDateFolder(fileBreakoutFolderNode, partialDatePath);
            AddTreeNodeFile(fileBreakoutFolderNode, partialDatePath, fileInfo.Name);
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
    }
    /// <summary>
    /// Adds a child node representing a date folder to the specified parent tree node if it does not already exist.
    /// </summary>
    /// <remarks>This method ensures that duplicate date folder nodes are not added to the parent node. If a
    /// node with the specified key already exists, no action is taken.</remarks>
    /// <param name="fileBreakoutFolderNode">The parent <see cref="TreeNode"/> to which the date folder node will be added.</param>
    /// <param name="partialDatePath">The key and text for the date folder node to add. Cannot be null or empty.</param>
    private void AddTreeNodeDateFolder(TreeNode fileBreakoutFolderNode, string partialDatePath)
    {
        if (!fileBreakoutFolderNode.Nodes.ContainsKey(partialDatePath))
        {
            fileBreakoutFolderNode.Nodes.Add(partialDatePath, partialDatePath);
        }
    }
    /// <summary>
    /// Adds a new file node with the specified name to the child node identified by the given partial date path within
    /// the provided folder node.
    /// </summary>
    /// <param name="fileBreakoutFolderNode">The parent <see cref="TreeNode"/> representing the folder under which the file node will be added. Cannot be
    /// null.</param>
    /// <param name="partialDatePath">The key used to locate the child node within <paramref name="fileBreakoutFolderNode"/> where the file node
    /// should be added. Must correspond to an existing child node.</param>
    /// <param name="fileName">The name of the file node to add as a child to the located node. Cannot be null or empty.</param>
    private void AddTreeNodeFile(TreeNode fileBreakoutFolderNode, string partialDatePath, string fileName)
    {
        fileBreakoutFolderNode.Nodes.Find(partialDatePath, false)[0].Nodes.Add(fileName);
    }

    public FileBreakout()
    {
        InitializeComponent();
    }

    private async void BreakoutFilesButtonClick(object sender, EventArgs e)
    {
        try
        {
            const string fileBreakoutFolderName = "FileBreakout";
            using var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;

            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            PrepareForProcessing();
            var fileBreakoutFolder = Path.Combine(folderBrowserDialog.SelectedPath, fileBreakoutFolderName);
            var rootNode = new TreeNode(folderBrowserDialog.SelectedPath);
            logTextBox.AppendText($"Folder: {folderBrowserDialog.SelectedPath}\n");

            fileTreeView.Nodes.Add(rootNode);
            Directory.CreateDirectory(fileBreakoutFolder);

            var fileBreakoutFolderNode = new TreeNode(fileBreakoutFolderName);
            rootNode.Nodes.Add(fileBreakoutFolderNode);
            rootNode.Expand();
            var filePaths = Directory.GetFiles(folderBrowserDialog.SelectedPath);

            progressBar.Maximum = filePaths.Length;

            logTextBox.AppendText($"Total Files: {filePaths.Length}\n");

            foreach (var fileInfo in filePaths.Select(path => new FileInfo(path)).OrderBy(fileInfo => fileInfo.LastWriteTime))
            {
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
            logTextBox.AppendText($"Done\n\n");
            EnableControls();
        }
        catch (Exception ex)
        {
            logTextBox.AppendText($"Error: {ex.Message}\n");
            MessageBox.Show("Error: " + ex.Message);
        } 
    }
}
