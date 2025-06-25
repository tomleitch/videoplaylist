namespace VideoPlaylist
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.axWindowsMediaPlayer2 = new AxWMPLib.AxWindowsMediaPlayer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnSelectPlaylist = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabVideo = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panelPlaylistFilter = new System.Windows.Forms.Panel();
            this.txtfvCurrentMatches = new System.Windows.Forms.TextBox();
            this.labfvCurrentMatches = new System.Windows.Forms.Label();
            this.btnfvReplace = new System.Windows.Forms.Button();
            this.btnfvAppend = new System.Windows.Forms.Button();
            this.clbfvActors = new System.Windows.Forms.CheckedListBox();
            this.txtfvActorSearch = new System.Windows.Forms.TextBox();
            this.lblfvActors = new System.Windows.Forms.Label();
            this.clbfvTagsGroup2 = new System.Windows.Forms.CheckedListBox();
            this.lblfvTagsGroup2 = new System.Windows.Forms.Label();
            this.clbfvTagsGroup1 = new System.Windows.Forms.CheckedListBox();
            this.lblfvTagsGroup1 = new System.Windows.Forms.Label();
            this.clbfvGenre = new System.Windows.Forms.CheckedListBox();
            this.lblfvGenre = new System.Windows.Forms.Label();
            this.panelPlaylist = new System.Windows.Forms.Panel();
            this.btnppCreatePlaylist = new System.Windows.Forms.Button();
            this.btnppDelete = new System.Windows.Forms.Button();
            this.txtppNewPlaylistName = new System.Windows.Forms.TextBox();
            this.labppNewPlaylist = new System.Windows.Forms.Label();
            this.txtppNumEntries = new System.Windows.Forms.TextBox();
            this.labppNumEntries = new System.Windows.Forms.Label();
            this.butppClear = new System.Windows.Forms.Button();
            this.cboppPlaylist = new System.Windows.Forms.ComboBox();
            this.labppPlaylist = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnOnboarding = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer2)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabVideo.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panelPlaylistFilter.SuspendLayout();
            this.panelPlaylist.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(3, 3);
            this.axWindowsMediaPlayer1.Margin = new System.Windows.Forms.Padding(2);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(1910, 1011);
            this.axWindowsMediaPlayer1.TabIndex = 0;
            this.axWindowsMediaPlayer1.KeyDownEvent += new AxWMPLib._WMPOCXEvents_KeyDownEventHandler(this.axWindowsMediaPlayer1_KeyDownEvent);
            this.axWindowsMediaPlayer1.KeyPressEvent += new AxWMPLib._WMPOCXEvents_KeyPressEventHandler(this.axWindowsMediaPlayer1_KeyPressEvent);
            // 
            // axWindowsMediaPlayer2
            // 
            this.axWindowsMediaPlayer2.Enabled = true;
            this.axWindowsMediaPlayer2.Location = new System.Drawing.Point(235, 282);
            this.axWindowsMediaPlayer2.Margin = new System.Windows.Forms.Padding(2);
            this.axWindowsMediaPlayer2.Name = "axWindowsMediaPlayer2";
            this.axWindowsMediaPlayer2.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer2.OcxState")));
            this.axWindowsMediaPlayer2.Size = new System.Drawing.Size(8, 38);
            this.axWindowsMediaPlayer2.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(2629, 36);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(286, 392);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(147, 78);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(286, 58);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(184, 48);
            this.button2.TabIndex = 4;
            this.button2.Text = "Onboarding";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnSelectPlaylist
            // 
            this.btnSelectPlaylist.Location = new System.Drawing.Point(286, 120);
            this.btnSelectPlaylist.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelectPlaylist.Name = "btnSelectPlaylist";
            this.btnSelectPlaylist.Size = new System.Drawing.Size(184, 50);
            this.btnSelectPlaylist.TabIndex = 5;
            this.btnSelectPlaylist.Text = "Select Playlist";
            this.btnSelectPlaylist.UseVisualStyleBackColor = true;
            this.btnSelectPlaylist.Visible = false;
            this.btnSelectPlaylist.Click += new System.EventHandler(this.btnSelectPlaylist_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(286, 183);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(184, 58);
            this.button4.TabIndex = 6;
            this.button4.Text = "Populate Playlist";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabVideo);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1924, 1050);
            this.tabControl1.TabIndex = 7;
            // 
            // tabVideo
            // 
            this.tabVideo.Controls.Add(this.axWindowsMediaPlayer1);
            this.tabVideo.Location = new System.Drawing.Point(4, 29);
            this.tabVideo.Name = "tabVideo";
            this.tabVideo.Padding = new System.Windows.Forms.Padding(3);
            this.tabVideo.Size = new System.Drawing.Size(1916, 1017);
            this.tabVideo.TabIndex = 0;
            this.tabVideo.Text = "Video";
            this.tabVideo.UseVisualStyleBackColor = true;
            this.tabVideo.Click += new System.EventHandler(this.tabVideo_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panelPlaylistFilter);
            this.tabPage2.Controls.Add(this.panelPlaylist);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1916, 1017);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Playlist";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panelPlaylistFilter
            // 
            this.panelPlaylistFilter.Controls.Add(this.txtfvCurrentMatches);
            this.panelPlaylistFilter.Controls.Add(this.labfvCurrentMatches);
            this.panelPlaylistFilter.Controls.Add(this.btnfvReplace);
            this.panelPlaylistFilter.Controls.Add(this.btnfvAppend);
            this.panelPlaylistFilter.Controls.Add(this.clbfvActors);
            this.panelPlaylistFilter.Controls.Add(this.txtfvActorSearch);
            this.panelPlaylistFilter.Controls.Add(this.lblfvActors);
            this.panelPlaylistFilter.Controls.Add(this.clbfvTagsGroup2);
            this.panelPlaylistFilter.Controls.Add(this.lblfvTagsGroup2);
            this.panelPlaylistFilter.Controls.Add(this.clbfvTagsGroup1);
            this.panelPlaylistFilter.Controls.Add(this.lblfvTagsGroup1);
            this.panelPlaylistFilter.Controls.Add(this.clbfvGenre);
            this.panelPlaylistFilter.Controls.Add(this.lblfvGenre);
            this.panelPlaylistFilter.Location = new System.Drawing.Point(29, 189);
            this.panelPlaylistFilter.Name = "panelPlaylistFilter";
            this.panelPlaylistFilter.Size = new System.Drawing.Size(1038, 553);
            this.panelPlaylistFilter.TabIndex = 13;
            // 
            // txtfvCurrentMatches
            // 
            this.txtfvCurrentMatches.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold);
            this.txtfvCurrentMatches.Location = new System.Drawing.Point(275, 409);
            this.txtfvCurrentMatches.Name = "txtfvCurrentMatches";
            this.txtfvCurrentMatches.ReadOnly = true;
            this.txtfvCurrentMatches.Size = new System.Drawing.Size(140, 25);
            this.txtfvCurrentMatches.TabIndex = 12;
            // 
            // labfvCurrentMatches
            // 
            this.labfvCurrentMatches.AutoSize = true;
            this.labfvCurrentMatches.Location = new System.Drawing.Point(106, 414);
            this.labfvCurrentMatches.Name = "labfvCurrentMatches";
            this.labfvCurrentMatches.Size = new System.Drawing.Size(131, 20);
            this.labfvCurrentMatches.TabIndex = 11;
            this.labfvCurrentMatches.Text = "Current Matches:";
            // 
            // btnfvReplace
            // 
            this.btnfvReplace.Location = new System.Drawing.Point(293, 466);
            this.btnfvReplace.Name = "btnfvReplace";
            this.btnfvReplace.Size = new System.Drawing.Size(154, 69);
            this.btnfvReplace.TabIndex = 10;
            this.btnfvReplace.Text = "Replace";
            this.btnfvReplace.UseVisualStyleBackColor = true;
            // 
            // btnfvAppend
            // 
            this.btnfvAppend.Location = new System.Drawing.Point(110, 466);
            this.btnfvAppend.Name = "btnfvAppend";
            this.btnfvAppend.Size = new System.Drawing.Size(154, 69);
            this.btnfvAppend.TabIndex = 9;
            this.btnfvAppend.Text = "Append";
            this.btnfvAppend.UseVisualStyleBackColor = true;
            // 
            // clbfvActors
            // 
            this.clbfvActors.Location = new System.Drawing.Point(110, 149);
            this.clbfvActors.Name = "clbfvActors";
            this.clbfvActors.Size = new System.Drawing.Size(337, 234);
            this.clbfvActors.TabIndex = 8;
            // 
            // txtfvActorSearch
            // 
            this.txtfvActorSearch.Location = new System.Drawing.Point(112, 117);
            this.txtfvActorSearch.Name = "txtfvActorSearch";
            this.txtfvActorSearch.Size = new System.Drawing.Size(337, 26);
            this.txtfvActorSearch.TabIndex = 7;
            // 
            // lblfvActors
            // 
            this.lblfvActors.AutoSize = true;
            this.lblfvActors.Location = new System.Drawing.Point(12, 123);
            this.lblfvActors.Name = "lblfvActors";
            this.lblfvActors.Size = new System.Drawing.Size(59, 20);
            this.lblfvActors.TabIndex = 6;
            this.lblfvActors.Text = "Actors:";
            // 
            // clbfvTagsGroup2
            // 
            this.clbfvTagsGroup2.Location = new System.Drawing.Point(597, 301);
            this.clbfvTagsGroup2.Name = "clbfvTagsGroup2";
            this.clbfvTagsGroup2.Size = new System.Drawing.Size(337, 234);
            this.clbfvTagsGroup2.TabIndex = 5;
            // 
            // lblfvTagsGroup2
            // 
            this.lblfvTagsGroup2.AutoSize = true;
            this.lblfvTagsGroup2.Location = new System.Drawing.Point(493, 301);
            this.lblfvTagsGroup2.Name = "lblfvTagsGroup2";
            this.lblfvTagsGroup2.Size = new System.Drawing.Size(71, 20);
            this.lblfvTagsGroup2.TabIndex = 4;
            this.lblfvTagsGroup2.Text = "Tags (2):";
            // 
            // clbfvTagsGroup1
            // 
            this.clbfvTagsGroup1.Location = new System.Drawing.Point(597, 38);
            this.clbfvTagsGroup1.Name = "clbfvTagsGroup1";
            this.clbfvTagsGroup1.Size = new System.Drawing.Size(337, 257);
            this.clbfvTagsGroup1.TabIndex = 3;
            // 
            // lblfvTagsGroup1
            // 
            this.lblfvTagsGroup1.AutoSize = true;
            this.lblfvTagsGroup1.Location = new System.Drawing.Point(480, 38);
            this.lblfvTagsGroup1.Name = "lblfvTagsGroup1";
            this.lblfvTagsGroup1.Size = new System.Drawing.Size(71, 20);
            this.lblfvTagsGroup1.TabIndex = 2;
            this.lblfvTagsGroup1.Text = "Tags (1):";
            // 
            // clbfvGenre
            // 
            this.clbfvGenre.Location = new System.Drawing.Point(112, 24);
            this.clbfvGenre.Name = "clbfvGenre";
            this.clbfvGenre.Size = new System.Drawing.Size(337, 96);
            this.clbfvGenre.TabIndex = 1;
            // 
            // lblfvGenre
            // 
            this.lblfvGenre.AutoSize = true;
            this.lblfvGenre.Location = new System.Drawing.Point(22, 24);
            this.lblfvGenre.Name = "lblfvGenre";
            this.lblfvGenre.Size = new System.Drawing.Size(58, 20);
            this.lblfvGenre.TabIndex = 0;
            this.lblfvGenre.Text = "Genre:";
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
            this.panelPlaylist.Location = new System.Drawing.Point(29, 6);
            this.panelPlaylist.Name = "panelPlaylist";
            this.panelPlaylist.Size = new System.Drawing.Size(1038, 177);
            this.panelPlaylist.TabIndex = 12;
            // 
            // btnppCreatePlaylist
            // 
            this.btnppCreatePlaylist.Location = new System.Drawing.Point(698, 98);
            this.btnppCreatePlaylist.Name = "btnppCreatePlaylist";
            this.btnppCreatePlaylist.Size = new System.Drawing.Size(154, 47);
            this.btnppCreatePlaylist.TabIndex = 8;
            this.btnppCreatePlaylist.Text = "Create";
            this.btnppCreatePlaylist.UseVisualStyleBackColor = true;
            // 
            // btnppDelete
            // 
            this.btnppDelete.Location = new System.Drawing.Point(858, 19);
            this.btnppDelete.Name = "btnppDelete";
            this.btnppDelete.Size = new System.Drawing.Size(154, 49);
            this.btnppDelete.TabIndex = 7;
            this.btnppDelete.Text = "Delete";
            this.btnppDelete.UseVisualStyleBackColor = true;
            // 
            // txtppNewPlaylistName
            // 
            this.txtppNewPlaylistName.Location = new System.Drawing.Point(192, 119);
            this.txtppNewPlaylistName.Name = "txtppNewPlaylistName";
            this.txtppNewPlaylistName.Size = new System.Drawing.Size(474, 26);
            this.txtppNewPlaylistName.TabIndex = 6;
            // 
            // labppNewPlaylist
            // 
            this.labppNewPlaylist.AutoSize = true;
            this.labppNewPlaylist.Location = new System.Drawing.Point(36, 105);
            this.labppNewPlaylist.Name = "labppNewPlaylist";
            this.labppNewPlaylist.Size = new System.Drawing.Size(92, 40);
            this.labppNewPlaylist.TabIndex = 5;
            this.labppNewPlaylist.Text = "New Playlist\r\nName:";
            // 
            // txtppNumEntries
            // 
            this.txtppNumEntries.Location = new System.Drawing.Point(192, 76);
            this.txtppNumEntries.Name = "txtppNumEntries";
            this.txtppNumEntries.ReadOnly = true;
            this.txtppNumEntries.Size = new System.Drawing.Size(223, 26);
            this.txtppNumEntries.TabIndex = 4;
            // 
            // labppNumEntries
            // 
            this.labppNumEntries.AutoSize = true;
            this.labppNumEntries.Location = new System.Drawing.Point(40, 79);
            this.labppNumEntries.Name = "labppNumEntries";
            this.labppNumEntries.Size = new System.Drawing.Size(100, 20);
            this.labppNumEntries.TabIndex = 3;
            this.labppNumEntries.Text = "Num Entries:";
            // 
            // butppClear
            // 
            this.butppClear.Location = new System.Drawing.Point(698, 14);
            this.butppClear.Name = "butppClear";
            this.butppClear.Size = new System.Drawing.Size(154, 54);
            this.butppClear.TabIndex = 2;
            this.butppClear.Text = "Clear";
            this.butppClear.UseVisualStyleBackColor = true;
            // 
            // cboppPlaylist
            // 
            this.cboppPlaylist.FormattingEnabled = true;
            this.cboppPlaylist.Location = new System.Drawing.Point(192, 28);
            this.cboppPlaylist.Name = "cboppPlaylist";
            this.cboppPlaylist.Size = new System.Drawing.Size(474, 28);
            this.cboppPlaylist.TabIndex = 1;
            // 
            // labppPlaylist
            // 
            this.labppPlaylist.AutoSize = true;
            this.labppPlaylist.Location = new System.Drawing.Point(40, 28);
            this.labppPlaylist.Name = "labppPlaylist";
            this.labppPlaylist.Size = new System.Drawing.Size(61, 20);
            this.labppPlaylist.TabIndex = 0;
            this.labppPlaylist.Text = "Playlist:";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnOnboarding);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(1916, 1017);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Other";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnOnboarding
            // 
            this.btnOnboarding.Location = new System.Drawing.Point(91, 66);
            this.btnOnboarding.Name = "btnOnboarding";
            this.btnOnboarding.Size = new System.Drawing.Size(191, 88);
            this.btnOnboarding.TabIndex = 0;
            this.btnOnboarding.Text = "On Boarding";
            this.btnOnboarding.UseVisualStyleBackColor = true;
            this.btnOnboarding.Click += new System.EventHandler(this.btnOnboarding_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1924, 1050);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.btnSelectPlaylist);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.axWindowsMediaPlayer2);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer2)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabVideo.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panelPlaylistFilter.ResumeLayout(false);
            this.panelPlaylistFilter.PerformLayout();
            this.panelPlaylist.ResumeLayout(false);
            this.panelPlaylist.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnSelectPlaylist;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabVideo;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panelPlaylist;
        private System.Windows.Forms.Button btnppCreatePlaylist;
        private System.Windows.Forms.Button btnppDelete;
        private System.Windows.Forms.TextBox txtppNewPlaylistName;
        private System.Windows.Forms.Label labppNewPlaylist;
        private System.Windows.Forms.TextBox txtppNumEntries;
        private System.Windows.Forms.Label labppNumEntries;
        private System.Windows.Forms.Button butppClear;
        private System.Windows.Forms.ComboBox cboppPlaylist;
        private System.Windows.Forms.Label labppPlaylist;
        private System.Windows.Forms.Panel panelPlaylistFilter;
        private System.Windows.Forms.TextBox txtfvCurrentMatches;
        private System.Windows.Forms.Label labfvCurrentMatches;
        private System.Windows.Forms.Button btnfvReplace;
        private System.Windows.Forms.Button btnfvAppend;
        private System.Windows.Forms.CheckedListBox clbfvActors;
        private System.Windows.Forms.TextBox txtfvActorSearch;
        private System.Windows.Forms.Label lblfvActors;
        private System.Windows.Forms.CheckedListBox clbfvTagsGroup2;
        private System.Windows.Forms.Label lblfvTagsGroup2;
        private System.Windows.Forms.CheckedListBox clbfvTagsGroup1;
        private System.Windows.Forms.Label lblfvTagsGroup1;
        private System.Windows.Forms.CheckedListBox clbfvGenre;
        private System.Windows.Forms.Label lblfvGenre;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnOnboarding;
    }
}

