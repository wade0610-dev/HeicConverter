namespace HeicConverter
{
    partial class MainForm
    {
        /// <summary>
        /// 设计工具所需的变数
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的资源
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                _cts?.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 设计工具产生的程式码

        private void InitializeComponent()
        {
            this.btnAddFiles = new System.Windows.Forms.Button();
            this.btnAddFolder = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblDropHint = new System.Windows.Forms.Label();
            this.lvFiles = new System.Windows.Forms.ListView();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colFolder = new System.Windows.Forms.ColumnHeader();
            this.colStatus = new System.Windows.Forms.ColumnHeader();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.rbPng = new System.Windows.Forms.RadioButton();
            this.rbJpg = new System.Windows.Forms.RadioButton();
            this.lblQuality = new System.Windows.Forms.Label();
            this.trkQuality = new System.Windows.Forms.TrackBar();
            this.lblQualityValue = new System.Windows.Forms.Label();
            this.lblFormatNote = new System.Windows.Forms.Label();
            this.lblOutput = new System.Windows.Forms.Label();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnOpenOutput = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkQuality)).BeginInit();
            this.SuspendLayout();
            //
            // btnAddFiles
            //
            this.btnAddFiles.Location = new System.Drawing.Point(12, 12);
            this.btnAddFiles.Name = "btnAddFiles";
            this.btnAddFiles.Size = new System.Drawing.Size(100, 30);
            this.btnAddFiles.TabIndex = 0;
            this.btnAddFiles.Text = "加入檔案…";
            this.btnAddFiles.UseVisualStyleBackColor = true;
            this.btnAddFiles.Click += new System.EventHandler(this.BtnAddFiles_Click);
            //
            // btnAddFolder
            //
            this.btnAddFolder.Location = new System.Drawing.Point(118, 12);
            this.btnAddFolder.Name = "btnAddFolder";
            this.btnAddFolder.Size = new System.Drawing.Size(100, 30);
            this.btnAddFolder.TabIndex = 1;
            this.btnAddFolder.Text = "加入資料夾…";
            this.btnAddFolder.UseVisualStyleBackColor = true;
            this.btnAddFolder.Click += new System.EventHandler(this.BtnAddFolder_Click);
            //
            // btnRemove
            //
            this.btnRemove.Location = new System.Drawing.Point(224, 12);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(100, 30);
            this.btnRemove.TabIndex = 2;
            this.btnRemove.Text = "移除選取";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.BtnRemove_Click);
            //
            // btnClear
            //
            this.btnClear.Location = new System.Drawing.Point(330, 12);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 30);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "清除清單";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.BtnClear_Click);
            //
            // lblDropHint
            //
            this.lblDropHint.AutoSize = true;
            this.lblDropHint.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblDropHint.Location = new System.Drawing.Point(442, 20);
            this.lblDropHint.Name = "lblDropHint";
            this.lblDropHint.TabIndex = 4;
            this.lblDropHint.Text = "也可以直接把 HEIC 檔案或資料夾拖進清單";
            //
            // lvFiles
            //
            this.lvFiles.AllowDrop = true;
            this.lvFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colFolder,
            this.colStatus});
            this.lvFiles.FullRowSelect = true;
            this.lvFiles.HideSelection = false;
            this.lvFiles.Location = new System.Drawing.Point(12, 50);
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.Size = new System.Drawing.Size(736, 278);
            this.lvFiles.TabIndex = 5;
            this.lvFiles.UseCompatibleStateImageBehavior = false;
            this.lvFiles.View = System.Windows.Forms.View.Details;
            this.lvFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.LvFiles_DragDrop);
            this.lvFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.LvFiles_DragEnter);
            this.lvFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LvFiles_KeyDown);
            //
            // colName
            //
            this.colName.Text = "檔名";
            this.colName.Width = 180;
            //
            // colFolder
            //
            this.colFolder.Text = "所在資料夾";
            this.colFolder.Width = 330;
            //
            // colStatus
            //
            this.colStatus.Text = "狀態";
            this.colStatus.Width = 200;
            //
            // grpOptions
            //
            this.grpOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpOptions.Controls.Add(this.rbPng);
            this.grpOptions.Controls.Add(this.rbJpg);
            this.grpOptions.Controls.Add(this.lblQuality);
            this.grpOptions.Controls.Add(this.trkQuality);
            this.grpOptions.Controls.Add(this.lblQualityValue);
            this.grpOptions.Controls.Add(this.lblFormatNote);
            this.grpOptions.Location = new System.Drawing.Point(12, 336);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Size = new System.Drawing.Size(736, 84);
            this.grpOptions.TabIndex = 6;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "輸出格式";
            //
            // rbPng
            //
            this.rbPng.AutoSize = true;
            this.rbPng.Checked = true;
            this.rbPng.Location = new System.Drawing.Point(16, 24);
            this.rbPng.Name = "rbPng";
            this.rbPng.TabIndex = 0;
            this.rbPng.TabStop = true;
            this.rbPng.Text = "PNG（無損）";
            this.rbPng.UseVisualStyleBackColor = true;
            this.rbPng.CheckedChanged += new System.EventHandler(this.FormatOption_CheckedChanged);
            //
            // rbJpg
            //
            this.rbJpg.AutoSize = true;
            this.rbJpg.Location = new System.Drawing.Point(16, 52);
            this.rbJpg.Name = "rbJpg";
            this.rbJpg.TabIndex = 1;
            this.rbJpg.Text = "JPG（有損）";
            this.rbJpg.UseVisualStyleBackColor = true;
            this.rbJpg.CheckedChanged += new System.EventHandler(this.FormatOption_CheckedChanged);
            //
            // lblQuality
            //
            this.lblQuality.AutoSize = true;
            this.lblQuality.Location = new System.Drawing.Point(180, 26);
            this.lblQuality.Name = "lblQuality";
            this.lblQuality.TabIndex = 2;
            this.lblQuality.Text = "JPG 品質：";
            //
            // trkQuality
            //
            this.trkQuality.AutoSize = false;
            this.trkQuality.LargeChange = 10;
            this.trkQuality.Location = new System.Drawing.Point(250, 20);
            this.trkQuality.Maximum = 100;
            this.trkQuality.Minimum = 1;
            this.trkQuality.Name = "trkQuality";
            this.trkQuality.Size = new System.Drawing.Size(300, 30);
            this.trkQuality.TabIndex = 3;
            this.trkQuality.TickFrequency = 10;
            this.trkQuality.Value = 100;
            this.trkQuality.ValueChanged += new System.EventHandler(this.TrkQuality_ValueChanged);
            //
            // lblQualityValue
            //
            this.lblQualityValue.AutoSize = true;
            this.lblQualityValue.Location = new System.Drawing.Point(556, 26);
            this.lblQualityValue.Name = "lblQualityValue";
            this.lblQualityValue.TabIndex = 4;
            this.lblQualityValue.Text = "100";
            //
            // lblFormatNote
            //
            this.lblFormatNote.AutoSize = true;
            this.lblFormatNote.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblFormatNote.Location = new System.Drawing.Point(180, 56);
            this.lblFormatNote.Name = "lblFormatNote";
            this.lblFormatNote.TabIndex = 5;
            this.lblFormatNote.Text = "";
            //
            // lblOutput
            //
            this.lblOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblOutput.AutoSize = true;
            this.lblOutput.Location = new System.Drawing.Point(12, 435);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.TabIndex = 7;
            this.lblOutput.Text = "輸出資料夾：";
            //
            // txtOutput
            //
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(100, 431);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.Size = new System.Drawing.Size(548, 23);
            this.txtOutput.TabIndex = 8;
            //
            // btnBrowse
            //
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(656, 428);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(92, 30);
            this.btnBrowse.TabIndex = 9;
            this.btnBrowse.Text = "選擇…";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
            //
            // progressBar
            //
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(12, 468);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(736, 20);
            this.progressBar.TabIndex = 10;
            //
            // lblStatus
            //
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 500);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.TabIndex = 11;
            this.lblStatus.Text = "請加入要轉換的 HEIC 檔案";
            //
            // btnOpenOutput
            //
            this.btnOpenOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenOutput.Location = new System.Drawing.Point(412, 522);
            this.btnOpenOutput.Name = "btnOpenOutput";
            this.btnOpenOutput.Size = new System.Drawing.Size(112, 32);
            this.btnOpenOutput.TabIndex = 12;
            this.btnOpenOutput.Text = "開啟輸出資料夾";
            this.btnOpenOutput.UseVisualStyleBackColor = true;
            this.btnOpenOutput.Click += new System.EventHandler(this.BtnOpenOutput_Click);
            //
            // btnStart
            //
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(530, 522);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(106, 32);
            this.btnStart.TabIndex = 13;
            this.btnStart.Text = "開始轉換";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            //
            // btnCancel
            //
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Enabled = false;
            this.btnCancel.Location = new System.Drawing.Point(642, 522);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(106, 32);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 566);
            this.Controls.Add(this.btnAddFiles);
            this.Controls.Add(this.btnAddFolder);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lblDropHint);
            this.Controls.Add(this.lvFiles);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.lblOutput);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnOpenOutput);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnCancel);
            this.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F);
            this.MinimumSize = new System.Drawing.Size(720, 520);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HEIC 轉檔器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkQuality)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnAddFiles;
        private System.Windows.Forms.Button btnAddFolder;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lblDropHint;
        private System.Windows.Forms.ListView lvFiles;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colFolder;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.RadioButton rbPng;
        private System.Windows.Forms.RadioButton rbJpg;
        private System.Windows.Forms.Label lblQuality;
        private System.Windows.Forms.TrackBar trkQuality;
        private System.Windows.Forms.Label lblQualityValue;
        private System.Windows.Forms.Label lblFormatNote;
        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnOpenOutput;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCancel;
    }
}
