namespace VideoPlaylist
{
    partial class VideoOnboardingForm
    {
        private System.Windows.Forms.Label lblDirectory;
        private System.Windows.Forms.TextBox txtDirectory;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.DataGridView dgvVideos;
        private System.Windows.Forms.Label lblGenre;
        private System.Windows.Forms.ComboBox cmbGenre;
        private System.Windows.Forms.Label lblTagsGroup1;
        private System.Windows.Forms.CheckedListBox clbTagsGroup1;
        private System.Windows.Forms.Label lblTagsGroup2;
        private System.Windows.Forms.CheckedListBox clbTagsGroup2;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblError;

        private void InitializeComponent()
        {
            this.lblDirectory = new System.Windows.Forms.Label();
            this.txtDirectory = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.dgvVideos = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblGenre = new System.Windows.Forms.Label();
            this.cmbGenre = new System.Windows.Forms.ComboBox();
            this.lblTagsGroup1 = new System.Windows.Forms.Label();
            this.clbTagsGroup1 = new System.Windows.Forms.CheckedListBox();
            this.lblTagsGroup2 = new System.Windows.Forms.Label();
            this.clbTagsGroup2 = new System.Windows.Forms.CheckedListBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblError = new System.Windows.Forms.Label();
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.htnRefresh = new System.Windows.Forms.Button();
            this.clbActors = new System.Windows.Forms.CheckedListBox();
            this.btnAddActor = new System.Windows.Forms.Button();
            this.txtAddActor = new System.Windows.Forms.TextBox();
            this.btnAddVideo = new System.Windows.Forms.Button();
            this.btnStopRun = new System.Windows.Forms.Button();
            this.labRecordExists = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVideos)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDirectory
            // 
            this.lblDirectory.Location = new System.Drawing.Point(15, 16);
            this.lblDirectory.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDirectory.Name = "lblDirectory";
            this.lblDirectory.Size = new System.Drawing.Size(76, 58);
            this.lblDirectory.TabIndex = 0;
            this.lblDirectory.Text = "Directory:";
            // 
            // txtDirectory
            // 
            this.txtDirectory.Location = new System.Drawing.Point(99, 16);
            this.txtDirectory.Margin = new System.Windows.Forms.Padding(2);
            this.txtDirectory.Name = "txtDirectory";
            this.txtDirectory.ReadOnly = true;
            this.txtDirectory.Size = new System.Drawing.Size(302, 31);
            this.txtDirectory.TabIndex = 1;
            this.txtDirectory.TextChanged += new System.EventHandler(this.ValidateInputs);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(472, 10);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(2);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(133, 46);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // dgvVideos
            // 
            this.dgvVideos.AllowUserToAddRows = false;
            this.dgvVideos.AllowUserToDeleteRows = false;
            this.dgvVideos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVideos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            this.dgvVideos.Location = new System.Drawing.Point(0, 76);
            this.dgvVideos.Margin = new System.Windows.Forms.Padding(2);
            this.dgvVideos.Name = "dgvVideos";
            this.dgvVideos.ReadOnly = true;
            this.dgvVideos.RowHeadersWidth = 62;
            this.dgvVideos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVideos.Size = new System.Drawing.Size(1023, 375);
            this.dgvVideos.TabIndex = 3;
            this.dgvVideos.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVideos_CellClick);
            this.dgvVideos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVideos_CellContentClick);
            this.dgvVideos.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVideos_RowEnter);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "File Name";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // lblGenre
            // 
            this.lblGenre.Location = new System.Drawing.Point(11, 463);
            this.lblGenre.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblGenre.Name = "lblGenre";
            this.lblGenre.Size = new System.Drawing.Size(76, 58);
            this.lblGenre.TabIndex = 4;
            this.lblGenre.Text = "Genre:";
            // 
            // cmbGenre
            // 
            this.cmbGenre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGenre.Location = new System.Drawing.Point(113, 473);
            this.cmbGenre.Margin = new System.Windows.Forms.Padding(2);
            this.cmbGenre.Name = "cmbGenre";
            this.cmbGenre.Size = new System.Drawing.Size(199, 33);
            this.cmbGenre.TabIndex = 5;
            this.cmbGenre.SelectedIndexChanged += new System.EventHandler(this.ValidateInputs);
            // 
            // lblTagsGroup1
            // 
            this.lblTagsGroup1.Location = new System.Drawing.Point(15, 530);
            this.lblTagsGroup1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTagsGroup1.Name = "lblTagsGroup1";
            this.lblTagsGroup1.Size = new System.Drawing.Size(76, 58);
            this.lblTagsGroup1.TabIndex = 6;
            this.lblTagsGroup1.Text = "Tags (Group 1):";
            // 
            // clbTagsGroup1
            // 
            this.clbTagsGroup1.Location = new System.Drawing.Point(113, 530);
            this.clbTagsGroup1.Margin = new System.Windows.Forms.Padding(2);
            this.clbTagsGroup1.Name = "clbTagsGroup1";
            this.clbTagsGroup1.Size = new System.Drawing.Size(388, 284);
            this.clbTagsGroup1.TabIndex = 7;
            this.clbTagsGroup1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ValidateInputs);
            // 
            // lblTagsGroup2
            // 
            this.lblTagsGroup2.Location = new System.Drawing.Point(529, 530);
            this.lblTagsGroup2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTagsGroup2.Name = "lblTagsGroup2";
            this.lblTagsGroup2.Size = new System.Drawing.Size(76, 58);
            this.lblTagsGroup2.TabIndex = 8;
            this.lblTagsGroup2.Text = "Tags (Group 2):";
            // 
            // clbTagsGroup2
            // 
            this.clbTagsGroup2.Location = new System.Drawing.Point(609, 530);
            this.clbTagsGroup2.Margin = new System.Windows.Forms.Padding(2);
            this.clbTagsGroup2.Name = "clbTagsGroup2";
            this.clbTagsGroup2.Size = new System.Drawing.Size(425, 284);
            this.clbTagsGroup2.TabIndex = 9;
            this.clbTagsGroup2.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ValidateInputs);
            // 
            // btnGo
            // 
            this.btnGo.Enabled = false;
            this.btnGo.Location = new System.Drawing.Point(1091, 774);
            this.btnGo.Margin = new System.Windows.Forms.Padding(2);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(112, 40);
            this.btnGo.TabIndex = 10;
            this.btnGo.Text = "GO";
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(865, 721);
            this.btnStop.Margin = new System.Windows.Forms.Padding(2);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(112, 40);
            this.btnStop.TabIndex = 11;
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point(0, 760);
            this.progressBar.Margin = new System.Windows.Forms.Padding(2);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(2280, 169);
            this.progressBar.TabIndex = 12;
            this.progressBar.Click += new System.EventHandler(this.progressBar_Click);
            // 
            // lblError
            // 
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(56, 790);
            this.lblError.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(465, 24);
            this.lblError.TabIndex = 13;
            this.lblError.Visible = false;
            // 
            // txtFilename
            // 
            this.txtFilename.Location = new System.Drawing.Point(1242, 54);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.Size = new System.Drawing.Size(417, 31);
            this.txtFilename.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(1072, 46);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 28);
            this.label1.TabIndex = 15;
            this.label1.Text = "Filename:";
            // 
            // htnRefresh
            // 
            this.htnRefresh.Location = new System.Drawing.Point(1677, 49);
            this.htnRefresh.Margin = new System.Windows.Forms.Padding(2);
            this.htnRefresh.Name = "htnRefresh";
            this.htnRefresh.Size = new System.Drawing.Size(112, 40);
            this.htnRefresh.TabIndex = 16;
            this.htnRefresh.Text = "Refresh";
            this.htnRefresh.Click += new System.EventHandler(this.htnRefresh_Click);
            // 
            // clbActors
            // 
            this.clbActors.Location = new System.Drawing.Point(1242, 103);
            this.clbActors.Margin = new System.Windows.Forms.Padding(2);
            this.clbActors.Name = "clbActors";
            this.clbActors.Size = new System.Drawing.Size(388, 284);
            this.clbActors.TabIndex = 17;
            // 
            // btnAddActor
            // 
            this.btnAddActor.Location = new System.Drawing.Point(1648, 406);
            this.btnAddActor.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddActor.Name = "btnAddActor";
            this.btnAddActor.Size = new System.Drawing.Size(203, 40);
            this.btnAddActor.TabIndex = 19;
            this.btnAddActor.Text = "Add Actor";
            this.btnAddActor.Click += new System.EventHandler(this.btnAddActor_Click);
            // 
            // txtAddActor
            // 
            this.txtAddActor.Location = new System.Drawing.Point(1213, 411);
            this.txtAddActor.Name = "txtAddActor";
            this.txtAddActor.Size = new System.Drawing.Size(417, 31);
            this.txtAddActor.TabIndex = 18;
            // 
            // btnAddVideo
            // 
            this.btnAddVideo.Location = new System.Drawing.Point(1677, 103);
            this.btnAddVideo.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddVideo.Name = "btnAddVideo";
            this.btnAddVideo.Size = new System.Drawing.Size(203, 40);
            this.btnAddVideo.TabIndex = 20;
            this.btnAddVideo.Text = "Add Video";
            this.btnAddVideo.Click += new System.EventHandler(this.btnAddVideo_Click);
            // 
            // btnStopRun
            // 
            this.btnStopRun.Location = new System.Drawing.Point(1894, 103);
            this.btnStopRun.Margin = new System.Windows.Forms.Padding(2);
            this.btnStopRun.Name = "btnStopRun";
            this.btnStopRun.Size = new System.Drawing.Size(203, 40);
            this.btnStopRun.TabIndex = 21;
            this.btnStopRun.Text = "Stop Run";
            this.btnStopRun.Click += new System.EventHandler(this.btnStopRun_Click);
            // 
            // labRecordExists
            // 
            this.labRecordExists.AutoSize = true;
            this.labRecordExists.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labRecordExists.ForeColor = System.Drawing.Color.Crimson;
            this.labRecordExists.Location = new System.Drawing.Point(1072, 130);
            this.labRecordExists.Name = "labRecordExists";
            this.labRecordExists.Size = new System.Drawing.Size(103, 75);
            this.labRecordExists.TabIndex = 22;
            this.labRecordExists.Text = "Has \r\nExisting \r\nRecord";
            // 
            // VideoOnboardingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(2280, 929);
            this.Controls.Add(this.labRecordExists);
            this.Controls.Add(this.btnStopRun);
            this.Controls.Add(this.btnAddVideo);
            this.Controls.Add(this.btnAddActor);
            this.Controls.Add(this.txtAddActor);
            this.Controls.Add(this.clbActors);
            this.Controls.Add(this.htnRefresh);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFilename);
            this.Controls.Add(this.lblDirectory);
            this.Controls.Add(this.txtDirectory);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.dgvVideos);
            this.Controls.Add(this.lblGenre);
            this.Controls.Add(this.cmbGenre);
            this.Controls.Add(this.lblTagsGroup1);
            this.Controls.Add(this.clbTagsGroup1);
            this.Controls.Add(this.lblTagsGroup2);
            this.Controls.Add(this.clbTagsGroup2);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblError);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "VideoOnboardingForm";
            this.Text = "Onboard Videos";
            this.Load += new System.EventHandler(this.VideoOnboardingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVideos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.TextBox txtFilename;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button htnRefresh;
        private System.Windows.Forms.CheckedListBox clbActors;
        private System.Windows.Forms.Button btnAddActor;
        private System.Windows.Forms.TextBox txtAddActor;
        private System.Windows.Forms.Button btnAddVideo;
        private System.Windows.Forms.Button btnStopRun;
        private System.Windows.Forms.Label labRecordExists;
    }
}