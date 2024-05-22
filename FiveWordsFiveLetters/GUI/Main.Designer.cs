namespace GUI
{
    partial class FiveLettersFiveWords
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.labelWordLength = new System.Windows.Forms.Label();
            this.buttonCalculate = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.labelFilePath = new System.Windows.Forms.Label();
            this.displayFilePath = new System.Windows.Forms.Label();
            this.displayWordLength = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelProcentage = new System.Windows.Forms.Label();
            this.labelWordCount = new System.Windows.Forms.Label();
            this.labelTime = new System.Windows.Forms.Label();
            this.displayWordCount = new System.Windows.Forms.Label();
            this.displayTime = new System.Windows.Forms.Label();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(12, 12);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(105, 48);
            this.buttonBrowse.TabIndex = 0;
            this.buttonBrowse.Text = "Browse";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // trackBar
            // 
            this.trackBar.Location = new System.Drawing.Point(273, 12);
            this.trackBar.Maximum = 26;
            this.trackBar.Minimum = 1;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(388, 56);
            this.trackBar.TabIndex = 1;
            this.trackBar.Value = 1;
            this.trackBar.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // labelWordLength
            // 
            this.labelWordLength.AutoSize = true;
            this.labelWordLength.Location = new System.Drawing.Point(142, 28);
            this.labelWordLength.Name = "labelWordLength";
            this.labelWordLength.Size = new System.Drawing.Size(85, 16);
            this.labelWordLength.TabIndex = 2;
            this.labelWordLength.Text = "Word length: ";
            // 
            // buttonCalculate
            // 
            this.buttonCalculate.Location = new System.Drawing.Point(682, 12);
            this.buttonCalculate.Name = "buttonCalculate";
            this.buttonCalculate.Size = new System.Drawing.Size(106, 48);
            this.buttonCalculate.TabIndex = 3;
            this.buttonCalculate.Text = "Calculate";
            this.buttonCalculate.UseVisualStyleBackColor = true;
            this.buttonCalculate.Click += new System.EventHandler(this.buttonCalculate_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.Location = new System.Drawing.Point(303, 403);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(193, 45);
            this.buttonExport.TabIndex = 5;
            this.buttonExport.Text = "Export";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(12, 91);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowHeadersWidth = 50;
            this.dataGridView.RowTemplate.Height = 24;
            this.dataGridView.Size = new System.Drawing.Size(776, 306);
            this.dataGridView.TabIndex = 6;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // labelFilePath
            // 
            this.labelFilePath.AutoSize = true;
            this.labelFilePath.Location = new System.Drawing.Point(13, 67);
            this.labelFilePath.Name = "labelFilePath";
            this.labelFilePath.Size = new System.Drawing.Size(61, 16);
            this.labelFilePath.TabIndex = 7;
            this.labelFilePath.Text = "File path:";
            // 
            // displayFilePath
            // 
            this.displayFilePath.AutoSize = true;
            this.displayFilePath.Location = new System.Drawing.Point(80, 67);
            this.displayFilePath.Name = "displayFilePath";
            this.displayFilePath.Size = new System.Drawing.Size(0, 16);
            this.displayFilePath.TabIndex = 8;
            // 
            // displayWordLength
            // 
            this.displayWordLength.AutoSize = true;
            this.displayWordLength.Location = new System.Drawing.Point(227, 28);
            this.displayWordLength.Name = "displayWordLength";
            this.displayWordLength.Size = new System.Drawing.Size(0, 16);
            this.displayWordLength.TabIndex = 9;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(578, 413);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(210, 23);
            this.progressBar.TabIndex = 10;
            this.progressBar.Visible = false;
            // 
            // labelProcentage
            // 
            this.labelProcentage.AutoSize = true;
            this.labelProcentage.Location = new System.Drawing.Point(530, 417);
            this.labelProcentage.Name = "labelProcentage";
            this.labelProcentage.Size = new System.Drawing.Size(30, 16);
            this.labelProcentage.TabIndex = 11;
            this.labelProcentage.Text = "N/A";
            this.labelProcentage.Visible = false;
            // 
            // labelWordCount
            // 
            this.labelWordCount.AutoSize = true;
            this.labelWordCount.Location = new System.Drawing.Point(13, 403);
            this.labelWordCount.Name = "labelWordCount";
            this.labelWordCount.Size = new System.Drawing.Size(126, 16);
            this.labelWordCount.TabIndex = 12;
            this.labelWordCount.Text = "Word combinations:";
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(13, 420);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(116, 16);
            this.labelTime.TabIndex = 13;
            this.labelTime.Text = "Time(min:sec:ms):";
            // 
            // displayWordCount
            // 
            this.displayWordCount.AutoSize = true;
            this.displayWordCount.Location = new System.Drawing.Point(154, 403);
            this.displayWordCount.Name = "displayWordCount";
            this.displayWordCount.Size = new System.Drawing.Size(30, 16);
            this.displayWordCount.TabIndex = 14;
            this.displayWordCount.Text = "N/A";
            // 
            // displayTime
            // 
            this.displayTime.AutoSize = true;
            this.displayTime.Location = new System.Drawing.Point(154, 420);
            this.displayTime.Name = "displayTime";
            this.displayTime.Size = new System.Drawing.Size(30, 16);
            this.displayTime.TabIndex = 15;
            this.displayTime.Text = "N/A";
            // 
            // FiveLettersFiveWords
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 455);
            this.Controls.Add(this.displayTime);
            this.Controls.Add(this.displayWordCount);
            this.Controls.Add(this.labelTime);
            this.Controls.Add(this.labelWordCount);
            this.Controls.Add(this.labelProcentage);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.displayWordLength);
            this.Controls.Add(this.displayFilePath);
            this.Controls.Add(this.labelFilePath);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.buttonCalculate);
            this.Controls.Add(this.labelWordLength);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.buttonBrowse);
            this.Name = "FiveLettersFiveWords";
            this.Text = "Five letter - five words";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.Label labelWordLength;
        private System.Windows.Forms.Button buttonCalculate;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label labelFilePath;
        private System.Windows.Forms.Label displayFilePath;
        private System.Windows.Forms.Label displayWordLength;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelProcentage;
        private System.Windows.Forms.Label labelWordCount;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Label displayWordCount;
        private System.Windows.Forms.Label displayTime;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}

