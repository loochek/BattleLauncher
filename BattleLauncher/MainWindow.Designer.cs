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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.FilterGameBF3 = new System.Windows.Forms.CheckBox();
            this.FilterGameB2K = new System.Windows.Forms.CheckBox();
            this.FilterGameCQ = new System.Windows.Forms.CheckBox();
            this.FilterGameAK = new System.Windows.Forms.CheckBox();
            this.FilterGameAM = new System.Windows.Forms.CheckBox();
            this.FilterGameEG = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.statusBar.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Index,
            this.map,
            this.name,
            this.Gamemode,
            this.Players,
            this.ping});
            this.dataGridView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(320, 12);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(606, 432);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridView1_CellMouseDoubleClick);
            this.dataGridView1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridView1_Scroll);
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
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
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(932, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(282, 432);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Filter";
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.FilterGameEG);
            this.groupBox2.Controls.Add(this.FilterGameAM);
            this.groupBox2.Controls.Add(this.FilterGameAK);
            this.groupBox2.Controls.Add(this.FilterGameCQ);
            this.groupBox2.Controls.Add(this.FilterGameB2K);
            this.groupBox2.Controls.Add(this.FilterGameBF3);
            this.groupBox2.Location = new System.Drawing.Point(6, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(118, 170);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Game";
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
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1223, 547);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.dataGridView1);
            this.Name = "MainWindow";
            this.Text = "BattleLauncher";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
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
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox FilterGameEG;
        private System.Windows.Forms.CheckBox FilterGameAM;
        private System.Windows.Forms.CheckBox FilterGameAK;
        private System.Windows.Forms.CheckBox FilterGameCQ;
        private System.Windows.Forms.CheckBox FilterGameB2K;
        private System.Windows.Forms.CheckBox FilterGameBF3;
    }
}