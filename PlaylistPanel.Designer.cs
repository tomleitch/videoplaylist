namespace VideoPlaylist
{
    partial class PlaylistPanel
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.labppPlaylist = new System.Windows.Forms.Label();
            this.cboppPlaylist = new System.Windows.Forms.ComboBox();
            this.panelPlaylist = new System.Windows.Forms.Panel();
            this.btnppCreatePlaylist = new System.Windows.Forms.Button();
            this.btnppDelete = new System.Windows.Forms.Button();
            this.txtppNewPlaylistName = new System.Windows.Forms.TextBox();
            this.labppNewPlaylist = new System.Windows.Forms.Label();
            this.txtppNumEntries = new System.Windows.Forms.TextBox();
            this.labppNumEntries = new System.Windows.Forms.Label();
            this.butppClear = new System.Windows.Forms.Button();
            this.panelPlaylist.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(769, 97);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(8, 8);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // labppPlaylist
            // 
            this.labppPlaylist.AutoSize = true;
            this.labppPlaylist.Location = new System.Drawing.Point(36, 24);
            this.labppPlaylist.Name = "labppPlaylist";
            this.labppPlaylist.Size = new System.Drawing.Size(87, 25);
            this.labppPlaylist.TabIndex = 0;
            this.labppPlaylist.Text = "Playlist:";
            // 
            // cboppPlaylist
            // 
            this.cboppPlaylist.FormattingEnabled = true;
            this.cboppPlaylist.Location = new System.Drawing.Point(170, 24);
            this.cboppPlaylist.Name = "cboppPlaylist";
            this.cboppPlaylist.Size = new System.Drawing.Size(421, 33);
            this.cboppPlaylist.TabIndex = 1;
            // 
            // panelPlaylist
            // 
            this.panelPlaylist.Controls.Add(this.btnppCreatePlaylist);
            this.panelPlaylist.Controls.Add(this.btnppDelete);
            this.panelPlaylist.Controls.Add(this.txtppNewPlaylistName);
            this.panelPlaylist.Controls.Add(this.labppNewPlaylist);
            this.panelPlaylist.Controls.Add(this.txtppNumEntries);
            this.panelPlaylist.Controls.Add(this.labppNumEntries);
            this.panelPlaylist.Controls.Add(this.butppClear);
            this.panelPlaylist.Controls.Add(this.cboppPlaylist);
            this.panelPlaylist.Controls.Add(this.labppPlaylist);
            this.panelPlaylist.Location = new System.Drawing.Point(12, 12);
            this.panelPlaylist.Name = "panelPlaylist";
            this.panelPlaylist.Size = new System.Drawing.Size(922, 197);
            this.panelPlaylist.TabIndex = 2;
            // 
            // btnppCreatePlaylist
            // 
            this.btnppCreatePlaylist.Location = new System.Drawing.Point(612, 139);
            this.btnppCreatePlaylist.Name = "btnppCreatePlaylist";
            this.btnppCreatePlaylist.Size = new System.Drawing.Size(137, 46);
            this.btnppCreatePlaylist.TabIndex = 8;
            this.btnppCreatePlaylist.Text = "Create";
            this.btnppCreatePlaylist.UseVisualStyleBackColor = true;
            // 
            // btnppDelete
            // 
            this.btnppDelete.Location = new System.Drawing.Point(767, 27);
            this.btnppDelete.Name = "btnppDelete";
            this.btnppDelete.Size = new System.Drawing.Size(137, 46);
            this.btnppDelete.TabIndex = 7;
            this.btnppDelete.Text = "Delete";
            this.btnppDelete.UseVisualStyleBackColor = true;
            // 
            // txtppNewPlaylistName
            // 
            this.txtppNewPlaylistName.Location = new System.Drawing.Point(170, 139);
            this.txtppNewPlaylistName.Name = "txtppNewPlaylistName";
            this.txtppNewPlaylistName.Size = new System.Drawing.Size(421, 31);
            this.txtppNewPlaylistName.TabIndex = 6;
            // 
            // labppNewPlaylist
            // 
            this.labppNewPlaylist.AutoSize = true;
            this.labppNewPlaylist.Location = new System.Drawing.Point(27, 139);
            this.labppNewPlaylist.Name = "labppNewPlaylist";
            this.labppNewPlaylist.Size = new System.Drawing.Size(129, 50);
            this.labppNewPlaylist.TabIndex = 5;
            this.labppNewPlaylist.Text = "New Playlist\r\nName:";
            // 
            // txtppNumEntries
            // 
            this.txtppNumEntries.Location = new System.Drawing.Point(170, 79);
            this.txtppNumEntries.Name = "txtppNumEntries";
            this.txtppNumEntries.ReadOnly = true;
            this.txtppNumEntries.Size = new System.Drawing.Size(125, 31);
            this.txtppNumEntries.TabIndex = 4;
            // 
            // labppNumEntries
            // 
            this.labppNumEntries.AutoSize = true;
            this.labppNumEntries.Location = new System.Drawing.Point(27, 85);
            this.labppNumEntries.Name = "labppNumEntries";
            this.labppNumEntries.Size = new System.Drawing.Size(135, 25);
            this.labppNumEntries.TabIndex = 3;
            this.labppNumEntries.Text = "Num Entries:";
            // 
            // butppClear
            // 
            this.butppClear.Location = new System.Drawing.Point(612, 27);
            this.butppClear.Name = "butppClear";
            this.butppClear.Size = new System.Drawing.Size(137, 46);
            this.butppClear.TabIndex = 2;
            this.butppClear.Text = "Clear";
            this.butppClear.UseVisualStyleBackColor = true;
            // 
            // PlaylistPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1631, 764);
            this.Controls.Add(this.panelPlaylist);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "PlaylistPanel";
            this.Tag = "7";
            this.Text = "PlaylistPanel";
            this.Load += new System.EventHandler(this.PlaylistPanel_Load);
            this.panelPlaylist.ResumeLayout(false);
            this.panelPlaylist.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label labppPlaylist;
        private System.Windows.Forms.ComboBox cboppPlaylist;
        private System.Windows.Forms.Panel panelPlaylist;
        private System.Windows.Forms.Button btnppCreatePlaylist;
        private System.Windows.Forms.Button btnppDelete;
        private System.Windows.Forms.TextBox txtppNewPlaylistName;
        private System.Windows.Forms.Label labppNewPlaylist;
        private System.Windows.Forms.TextBox txtppNumEntries;
        private System.Windows.Forms.Label labppNumEntries;
        private System.Windows.Forms.Button butppClear;
    }
}