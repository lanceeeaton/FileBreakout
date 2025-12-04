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
            breakoutFilesButton = new Button();
            progressBar = new ProgressBar();
            keepCopyCheckbox = new CheckBox();
            breakoutByMonthCheckbox = new CheckBox();
            fileTreeView = new TreeView();
            logTextBox = new TextBox();
            SuspendLayout();
            // 
            // breakoutFilesButton
            // 
            breakoutFilesButton.Location = new Point(12, 12);
            breakoutFilesButton.Name = "breakoutFilesButton";
            breakoutFilesButton.Size = new Size(154, 36);
            breakoutFilesButton.TabIndex = 0;
            breakoutFilesButton.Text = "Select Folder";
            breakoutFilesButton.UseVisualStyleBackColor = true;
            breakoutFilesButton.Click += BreakoutFilesButtonClick;
            // 
            // progressBar
            // 
            progressBar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            progressBar.Location = new Point(12, 375);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(627, 23);
            progressBar.TabIndex = 3;
            // 
            // keepCopyCheckbox
            // 
            keepCopyCheckbox.AutoSize = true;
            keepCopyCheckbox.Checked = true;
            keepCopyCheckbox.CheckState = CheckState.Checked;
            keepCopyCheckbox.Location = new Point(13, 57);
            keepCopyCheckbox.Name = "keepCopyCheckbox";
            keepCopyCheckbox.Size = new Size(123, 19);
            keepCopyCheckbox.TabIndex = 4;
            keepCopyCheckbox.Text = "Keep Copy of Files";
            keepCopyCheckbox.UseVisualStyleBackColor = true;
            // 
            // breakoutByMonthCheckbox
            // 
            breakoutByMonthCheckbox.AutoSize = true;
            breakoutByMonthCheckbox.Location = new Point(13, 82);
            breakoutByMonthCheckbox.Name = "breakoutByMonthCheckbox";
            breakoutByMonthCheckbox.Size = new Size(128, 19);
            breakoutByMonthCheckbox.TabIndex = 5;
            breakoutByMonthCheckbox.Text = "Breakout by Month";
            breakoutByMonthCheckbox.UseVisualStyleBackColor = true;
            // 
            // fileTreeView
            // 
            fileTreeView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            fileTreeView.Location = new Point(188, 12);
            fileTreeView.Name = "fileTreeView";
            fileTreeView.Size = new Size(451, 357);
            fileTreeView.TabIndex = 6;
            // 
            // logTextBox
            // 
            logTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            logTextBox.Enabled = false;
            logTextBox.Location = new Point(12, 107);
            logTextBox.Multiline = true;
            logTextBox.Name = "logTextBox";
            logTextBox.Size = new Size(154, 262);
            logTextBox.TabIndex = 7;
            // 
            // FileBreakout
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(651, 410);
            Controls.Add(logTextBox);
            Controls.Add(fileTreeView);
            Controls.Add(breakoutByMonthCheckbox);
            Controls.Add(keepCopyCheckbox);
            Controls.Add(progressBar);
            Controls.Add(breakoutFilesButton);
            Name = "FileBreakout";
            Text = "FileBreakout";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button breakoutFilesButton;
        private ProgressBar progressBar;
        private CheckBox keepCopyCheckbox;
        private CheckBox breakoutByMonthCheckbox;
        private TreeView fileTreeView;
        private TextBox logTextBox;
    }
}
