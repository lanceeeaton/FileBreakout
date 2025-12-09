namespace FileBreakout
{
    partial class FileBreakout
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            selectPathButton = new Button();
            progressBar = new ProgressBar();
            keepCopyCheckbox = new CheckBox();
            fileTreeView = new TreeView();
            logTextBox = new TextBox();
            folderPathTextBox = new TextBox();
            startProcessingButton = new Button();
            stopProcessing = new Button();
            SuspendLayout();
            // 
            // selectPathButton
            // 
            selectPathButton.Location = new Point(12, 12);
            selectPathButton.Name = "selectPathButton";
            selectPathButton.Size = new Size(154, 23);
            selectPathButton.TabIndex = 0;
            selectPathButton.Text = "Select Folder";
            selectPathButton.UseVisualStyleBackColor = true;
            selectPathButton.Click += SelectPathButtonClick;
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(12, 375);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(717, 23);
            progressBar.TabIndex = 3;
            // 
            // keepCopyCheckbox
            // 
            keepCopyCheckbox.AutoSize = true;
            keepCopyCheckbox.Checked = true;
            keepCopyCheckbox.CheckState = CheckState.Checked;
            keepCopyCheckbox.Location = new Point(13, 41);
            keepCopyCheckbox.Name = "keepCopyCheckbox";
            keepCopyCheckbox.Size = new Size(123, 19);
            keepCopyCheckbox.TabIndex = 4;
            keepCopyCheckbox.Text = "Keep Copy of Files";
            keepCopyCheckbox.UseVisualStyleBackColor = true;
            // 
            // fileTreeView
            // 
            fileTreeView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            fileTreeView.Location = new Point(188, 41);
            fileTreeView.Name = "fileTreeView";
            fileTreeView.Size = new Size(541, 328);
            fileTreeView.TabIndex = 6;
            // 
            // logTextBox
            // 
            logTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            logTextBox.Enabled = false;
            logTextBox.Location = new Point(12, 66);
            logTextBox.Multiline = true;
            logTextBox.Name = "logTextBox";
            logTextBox.Size = new Size(154, 303);
            logTextBox.TabIndex = 7;
            // 
            // folderPathTextBox
            // 
            folderPathTextBox.Enabled = false;
            folderPathTextBox.Location = new Point(188, 12);
            folderPathTextBox.Name = "folderPathTextBox";
            folderPathTextBox.Size = new Size(361, 23);
            folderPathTextBox.TabIndex = 8;
            folderPathTextBox.TextChanged += FolderPathTextBoxTextChanged;
            // 
            // startProcessingButton
            // 
            startProcessingButton.Enabled = false;
            startProcessingButton.Location = new Point(555, 12);
            startProcessingButton.Name = "startProcessingButton";
            startProcessingButton.Size = new Size(84, 23);
            startProcessingButton.TabIndex = 9;
            startProcessingButton.Text = "Start";
            startProcessingButton.UseVisualStyleBackColor = true;
            startProcessingButton.Click += StartProcessingButtonClick;
            // 
            // stopProcessing
            // 
            stopProcessing.Enabled = false;
            stopProcessing.Location = new Point(645, 12);
            stopProcessing.Name = "stopProcessing";
            stopProcessing.Size = new Size(84, 23);
            stopProcessing.TabIndex = 10;
            stopProcessing.Text = "Stop";
            stopProcessing.UseVisualStyleBackColor = true;
            stopProcessing.Click += StopProcessingClick;
            // 
            // FileBreakout
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(741, 410);
            Controls.Add(stopProcessing);
            Controls.Add(startProcessingButton);
            Controls.Add(folderPathTextBox);
            Controls.Add(logTextBox);
            Controls.Add(fileTreeView);
            Controls.Add(keepCopyCheckbox);
            Controls.Add(progressBar);
            Controls.Add(selectPathButton);
            Name = "FileBreakout";
            Text = "FileBreakout";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button selectPathButton;
        private ProgressBar progressBar;
        private CheckBox keepCopyCheckbox;
        private TreeView fileTreeView;
        private TextBox logTextBox;
        private TextBox folderPathTextBox;
        private Button startProcessingButton;
        private Button stopProcessing;
    }
}
