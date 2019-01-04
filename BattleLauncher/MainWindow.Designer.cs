namespace BattleLauncher
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.serverBrowserView = new System.Windows.Forms.DataGridView();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.map = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Gamemode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Players = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ping = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.FilterGame = new System.Windows.Forms.GroupBox();
            this.FilterGM = new System.Windows.Forms.GroupBox();
            this.FilterGM131072 = new System.Windows.Forms.CheckBox();
            this.FilterGM256 = new System.Windows.Forms.CheckBox();
            this.FilterGM2 = new System.Windows.Forms.CheckBox();
            this.FilterGM8 = new System.Windows.Forms.CheckBox();
            this.FilterGM128 = new System.Windows.Forms.CheckBox();
            this.FilterGM64 = new System.Windows.Forms.CheckBox();
            this.FilterGM8388608 = new System.Windows.Forms.CheckBox();
            this.FilterGM1024 = new System.Windows.Forms.CheckBox();
            this.FilterGM4194304 = new System.Windows.Forms.CheckBox();
            this.FilterGM2048 = new System.Windows.Forms.CheckBox();
            this.FilterGM32 = new System.Windows.Forms.CheckBox();
            this.FilterGM524288 = new System.Windows.Forms.CheckBox();
            this.FilterGM512 = new System.Windows.Forms.CheckBox();
            this.FilterGM4 = new System.Windows.Forms.CheckBox();
            this.FilterGM1 = new System.Windows.Forms.CheckBox();
            this.FilterGameEG = new System.Windows.Forms.CheckBox();
            this.FilterGameAM = new System.Windows.Forms.CheckBox();
            this.FilterGameAK = new System.Windows.Forms.CheckBox();
            this.FilterGameCQ = new System.Windows.Forms.CheckBox();
            this.FilterGameB2K = new System.Windows.Forms.CheckBox();
            this.FilterGameBF3 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.serverBrowserView)).BeginInit();
            this.statusBar.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.FilterGame.SuspendLayout();
            this.FilterGM.SuspendLayout();
            this.SuspendLayout();
            // 
            // serverBrowserView
            // 
            this.serverBrowserView.AllowUserToAddRows = false;
            this.serverBrowserView.AllowUserToDeleteRows = false;
            this.serverBrowserView.AllowUserToResizeRows = false;
            this.serverBrowserView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.serverBrowserView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.serverBrowserView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.serverBrowserView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Index,
            this.map,
            this.name,
            this.Gamemode,
            this.Players,
            this.ping});
            this.serverBrowserView.Cursor = System.Windows.Forms.Cursors.Default;
            this.serverBrowserView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.serverBrowserView.Location = new System.Drawing.Point(320, 12);
            this.serverBrowserView.MultiSelect = false;
            this.serverBrowserView.Name = "serverBrowserView";
            this.serverBrowserView.ReadOnly = true;
            this.serverBrowserView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.serverBrowserView.RowHeadersVisible = false;
            this.serverBrowserView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.serverBrowserView.Size = new System.Drawing.Size(437, 432);
            this.serverBrowserView.TabIndex = 0;
            this.serverBrowserView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.serverBrowserView_CellMouseDoubleClick);
            this.serverBrowserView.Scroll += new System.Windows.Forms.ScrollEventHandler(this.serverBrowserView_Scroll);
            this.serverBrowserView.SelectionChanged += new System.EventHandler(this.serverBrowserView_SelectionChanged);
            // 
            // Index
            // 
            this.Index.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Index.HeaderText = "Index";
            this.Index.Name = "Index";
            this.Index.ReadOnly = true;
            this.Index.Visible = false;
            // 
            // map
            // 
            this.map.HeaderText = "Map";
            this.map.Name = "map";
            this.map.ReadOnly = true;
            // 
            // name
            // 
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // Gamemode
            // 
            this.Gamemode.HeaderText = "Game mode";
            this.Gamemode.Name = "Gamemode";
            this.Gamemode.ReadOnly = true;
            // 
            // Players
            // 
            this.Players.HeaderText = "Players";
            this.Players.Name = "Players";
            this.Players.ReadOnly = true;
            // 
            // ping
            // 
            this.ping.HeaderText = "Ping";
            this.ping.Name = "ping";
            this.ping.ReadOnly = true;
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripDropDownButton1});
            this.statusBar.Location = new System.Drawing.Point(0, 525);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(1223, 22);
            this.statusBar.SizingGrip = false;
            this.statusBar.TabIndex = 1;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(500, 17);
            this.toolStripStatusLabel1.Text = "Idle";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.Enabled = false;
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.ShowDropDownArrow = false;
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(20, 20);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Click += new System.EventHandler(this.ToolStripDropDownButton1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(51, 296);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(199, 20);
            this.textBox1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.FilterGM);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.FilterGame);
            this.groupBox1.Location = new System.Drawing.Point(813, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(401, 510);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // FilterGame
            // 
            this.FilterGame.AutoSize = true;
            this.FilterGame.Controls.Add(this.FilterGameEG);
            this.FilterGame.Controls.Add(this.FilterGameAM);
            this.FilterGame.Controls.Add(this.FilterGameAK);
            this.FilterGame.Controls.Add(this.FilterGameCQ);
            this.FilterGame.Controls.Add(this.FilterGameB2K);
            this.FilterGame.Controls.Add(this.FilterGameBF3);
            this.FilterGame.Location = new System.Drawing.Point(6, 19);
            this.FilterGame.Name = "FilterGame";
            this.FilterGame.Size = new System.Drawing.Size(118, 170);
            this.FilterGame.TabIndex = 0;
            this.FilterGame.TabStop = false;
            this.FilterGame.Text = "Game";
            // 
            // FilterGM
            // 
            this.FilterGM.Controls.Add(this.FilterGM131072);
            this.FilterGM.Controls.Add(this.FilterGM256);
            this.FilterGM.Controls.Add(this.FilterGM2);
            this.FilterGM.Controls.Add(this.FilterGM2048);
            this.FilterGM.Controls.Add(this.FilterGM8);
            this.FilterGM.Controls.Add(this.FilterGM128);
            this.FilterGM.Controls.Add(this.FilterGM64);
            this.FilterGM.Controls.Add(this.FilterGM8388608);
            this.FilterGM.Controls.Add(this.FilterGM1024);
            this.FilterGM.Controls.Add(this.FilterGM4194304);
            this.FilterGM.Controls.Add(this.FilterGM32);
            this.FilterGM.Controls.Add(this.FilterGM524288);
            this.FilterGM.Controls.Add(this.FilterGM512);
            this.FilterGM.Controls.Add(this.FilterGM4);
            this.FilterGM.Controls.Add(this.FilterGM1);
            this.FilterGM.Location = new System.Drawing.Point(6, 195);
            this.FilterGM.Name = "FilterGM";
            this.FilterGM.Size = new System.Drawing.Size(162, 328);
            this.FilterGM.TabIndex = 1;
            this.FilterGM.TabStop = false;
            this.FilterGM.Text = "Game mode";
            // 
            // FilterGM131072
            // 
            this.FilterGM131072.AutoSize = true;
            this.FilterGM131072.Location = new System.Drawing.Point(6, 239);
            this.FilterGM131072.Name = "FilterGM131072";
            this.FilterGM131072.Size = new System.Drawing.Size(15, 14);
            this.FilterGM131072.TabIndex = 15;
            this.FilterGM131072.UseVisualStyleBackColor = true;
            // 
            // FilterGM256
            // 
            this.FilterGM256.AutoSize = true;
            this.FilterGM256.Location = new System.Drawing.Point(6, 159);
            this.FilterGM256.Name = "FilterGM256";
            this.FilterGM256.Size = new System.Drawing.Size(15, 14);
            this.FilterGM256.TabIndex = 14;
            this.FilterGM256.UseVisualStyleBackColor = true;
            // 
            // FilterGM2
            // 
            this.FilterGM2.AutoSize = true;
            this.FilterGM2.Location = new System.Drawing.Point(6, 39);
            this.FilterGM2.Name = "FilterGM2";
            this.FilterGM2.Size = new System.Drawing.Size(15, 14);
            this.FilterGM2.TabIndex = 13;
            this.FilterGM2.UseVisualStyleBackColor = true;
            // 
            // FilterGM8
            // 
            this.FilterGM8.AutoSize = true;
            this.FilterGM8.Location = new System.Drawing.Point(6, 79);
            this.FilterGM8.Name = "FilterGM8";
            this.FilterGM8.Size = new System.Drawing.Size(15, 14);
            this.FilterGM8.TabIndex = 12;
            this.FilterGM8.UseVisualStyleBackColor = true;
            // 
            // FilterGM128
            // 
            this.FilterGM128.AutoSize = true;
            this.FilterGM128.Location = new System.Drawing.Point(6, 139);
            this.FilterGM128.Name = "FilterGM128";
            this.FilterGM128.Size = new System.Drawing.Size(15, 14);
            this.FilterGM128.TabIndex = 11;
            this.FilterGM128.UseVisualStyleBackColor = true;
            // 
            // FilterGM64
            // 
            this.FilterGM64.AutoSize = true;
            this.FilterGM64.Location = new System.Drawing.Point(6, 119);
            this.FilterGM64.Name = "FilterGM64";
            this.FilterGM64.Size = new System.Drawing.Size(15, 14);
            this.FilterGM64.TabIndex = 10;
            this.FilterGM64.UseVisualStyleBackColor = true;
            // 
            // FilterGM8388608
            // 
            this.FilterGM8388608.AutoSize = true;
            this.FilterGM8388608.Location = new System.Drawing.Point(6, 299);
            this.FilterGM8388608.Name = "FilterGM8388608";
            this.FilterGM8388608.Size = new System.Drawing.Size(15, 14);
            this.FilterGM8388608.TabIndex = 9;
            this.FilterGM8388608.UseVisualStyleBackColor = true;
            // 
            // FilterGM1024
            // 
            this.FilterGM1024.AutoSize = true;
            this.FilterGM1024.Location = new System.Drawing.Point(6, 199);
            this.FilterGM1024.Name = "FilterGM1024";
            this.FilterGM1024.Size = new System.Drawing.Size(15, 14);
            this.FilterGM1024.TabIndex = 5;
            this.FilterGM1024.UseVisualStyleBackColor = true;
            // 
            // FilterGM4194304
            // 
            this.FilterGM4194304.AutoSize = true;
            this.FilterGM4194304.Location = new System.Drawing.Point(6, 279);
            this.FilterGM4194304.Name = "FilterGM4194304";
            this.FilterGM4194304.Size = new System.Drawing.Size(15, 14);
            this.FilterGM4194304.TabIndex = 8;
            this.FilterGM4194304.UseVisualStyleBackColor = true;
            // 
            // FilterGM2048
            // 
            this.FilterGM2048.AutoSize = true;
            this.FilterGM2048.Location = new System.Drawing.Point(6, 219);
            this.FilterGM2048.Name = "FilterGM2048";
            this.FilterGM2048.Size = new System.Drawing.Size(15, 14);
            this.FilterGM2048.TabIndex = 7;
            this.FilterGM2048.UseVisualStyleBackColor = true;
            // 
            // FilterGM32
            // 
            this.FilterGM32.AutoSize = true;
            this.FilterGM32.Location = new System.Drawing.Point(6, 99);
            this.FilterGM32.Name = "FilterGM32";
            this.FilterGM32.Size = new System.Drawing.Size(15, 14);
            this.FilterGM32.TabIndex = 6;
            this.FilterGM32.UseVisualStyleBackColor = true;
            // 
            // FilterGM524288
            // 
            this.FilterGM524288.AutoSize = true;
            this.FilterGM524288.Location = new System.Drawing.Point(6, 259);
            this.FilterGM524288.Name = "FilterGM524288";
            this.FilterGM524288.Size = new System.Drawing.Size(15, 14);
            this.FilterGM524288.TabIndex = 4;
            this.FilterGM524288.UseVisualStyleBackColor = true;
            // 
            // FilterGM512
            // 
            this.FilterGM512.AutoSize = true;
            this.FilterGM512.Location = new System.Drawing.Point(6, 179);
            this.FilterGM512.Name = "FilterGM512";
            this.FilterGM512.Size = new System.Drawing.Size(15, 14);
            this.FilterGM512.TabIndex = 3;
            this.FilterGM512.UseVisualStyleBackColor = true;
            // 
            // FilterGM4
            // 
            this.FilterGM4.AutoSize = true;
            this.FilterGM4.Location = new System.Drawing.Point(6, 59);
            this.FilterGM4.Name = "FilterGM4";
            this.FilterGM4.Size = new System.Drawing.Size(15, 14);
            this.FilterGM4.TabIndex = 2;
            this.FilterGM4.UseVisualStyleBackColor = true;
            // 
            // FilterGM1
            // 
            this.FilterGM1.AutoSize = true;
            this.FilterGM1.Location = new System.Drawing.Point(6, 19);
            this.FilterGM1.Name = "FilterGM1";
            this.FilterGM1.Size = new System.Drawing.Size(15, 14);
            this.FilterGM1.TabIndex = 1;
            this.FilterGM1.UseVisualStyleBackColor = true;
            // 
            // FilterGameEG
            // 
            this.FilterGameEG.AutoSize = true;
            this.FilterGameEG.Location = new System.Drawing.Point(6, 134);
            this.FilterGameEG.Name = "FilterGameEG";
            this.FilterGameEG.Size = new System.Drawing.Size(76, 17);
            this.FilterGameEG.TabIndex = 5;
            this.FilterGameEG.Text = "End Game";
            this.FilterGameEG.UseVisualStyleBackColor = true;
            // 
            // FilterGameAM
            // 
            this.FilterGameAM.AutoSize = true;
            this.FilterGameAM.Location = new System.Drawing.Point(6, 111);
            this.FilterGameAM.Name = "FilterGameAM";
            this.FilterGameAM.Size = new System.Drawing.Size(71, 17);
            this.FilterGameAM.TabIndex = 4;
            this.FilterGameAM.Text = "Aftermath";
            this.FilterGameAM.UseVisualStyleBackColor = true;
            // 
            // FilterGameAK
            // 
            this.FilterGameAK.AutoSize = true;
            this.FilterGameAK.Location = new System.Drawing.Point(6, 88);
            this.FilterGameAK.Name = "FilterGameAK";
            this.FilterGameAK.Size = new System.Drawing.Size(81, 17);
            this.FilterGameAK.TabIndex = 3;
            this.FilterGameAK.Text = "Armored Kill";
            this.FilterGameAK.UseVisualStyleBackColor = true;
            // 
            // FilterGameCQ
            // 
            this.FilterGameCQ.AutoSize = true;
            this.FilterGameCQ.Location = new System.Drawing.Point(6, 65);
            this.FilterGameCQ.Name = "FilterGameCQ";
            this.FilterGameCQ.Size = new System.Drawing.Size(95, 17);
            this.FilterGameCQ.TabIndex = 2;
            this.FilterGameCQ.Text = "Close Quarters";
            this.FilterGameCQ.UseVisualStyleBackColor = true;
            // 
            // FilterGameB2K
            // 
            this.FilterGameB2K.AutoSize = true;
            this.FilterGameB2K.Location = new System.Drawing.Point(6, 42);
            this.FilterGameB2K.Name = "FilterGameB2K";
            this.FilterGameB2K.Size = new System.Drawing.Size(106, 17);
            this.FilterGameB2K.TabIndex = 1;
            this.FilterGameB2K.Text = "Back to Karkand";
            this.FilterGameB2K.UseVisualStyleBackColor = true;
            // 
            // FilterGameBF3
            // 
            this.FilterGameBF3.AutoSize = true;
            this.FilterGameBF3.Location = new System.Drawing.Point(6, 19);
            this.FilterGameBF3.Name = "FilterGameBF3";
            this.FilterGameBF3.Size = new System.Drawing.Size(81, 17);
            this.FilterGameBF3.TabIndex = 0;
            this.FilterGameBF3.Text = "Battlefield 3";
            this.FilterGameBF3.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(174, 294);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(137, 44);
            this.button1.TabIndex = 4;
            this.button1.Text = "Apply filter";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1223, 547);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.serverBrowserView);
            this.Name = "MainWindow";
            this.Text = "BattleLauncher";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.serverBrowserView)).EndInit();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.FilterGame.ResumeLayout(false);
            this.FilterGame.PerformLayout();
            this.FilterGM.ResumeLayout(false);
            this.FilterGM.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView serverBrowserView;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn map;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Gamemode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Players;
        private System.Windows.Forms.DataGridViewTextBoxColumn ping;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox FilterGame;
        private System.Windows.Forms.CheckBox FilterGameEG;
        private System.Windows.Forms.CheckBox FilterGameAM;
        private System.Windows.Forms.CheckBox FilterGameAK;
        private System.Windows.Forms.CheckBox FilterGameCQ;
        private System.Windows.Forms.CheckBox FilterGameB2K;
        private System.Windows.Forms.CheckBox FilterGameBF3;
        private System.Windows.Forms.GroupBox FilterGM;
        private System.Windows.Forms.CheckBox FilterGM131072;
        private System.Windows.Forms.CheckBox FilterGM256;
        private System.Windows.Forms.CheckBox FilterGM2;
        private System.Windows.Forms.CheckBox FilterGM8;
        private System.Windows.Forms.CheckBox FilterGM128;
        private System.Windows.Forms.CheckBox FilterGM64;
        private System.Windows.Forms.CheckBox FilterGM8388608;
        private System.Windows.Forms.CheckBox FilterGM1024;
        private System.Windows.Forms.CheckBox FilterGM4194304;
        private System.Windows.Forms.CheckBox FilterGM2048;
        private System.Windows.Forms.CheckBox FilterGM32;
        private System.Windows.Forms.CheckBox FilterGM524288;
        private System.Windows.Forms.CheckBox FilterGM512;
        private System.Windows.Forms.CheckBox FilterGM4;
        private System.Windows.Forms.CheckBox FilterGM1;
        private System.Windows.Forms.Button button1;
    }
}