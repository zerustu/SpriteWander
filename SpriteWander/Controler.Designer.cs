namespace SpriteWander
{
    partial class Controler
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
            tableLayoutPanel1 = new TableLayoutPanel();
            LeftB = new Button();
            RightB = new Button();
            UpB = new Button();
            DownB = new Button();
            Close = new Button();
            URB = new Button();
            DRB = new Button();
            ULB = new Button();
            DLB = new Button();
            CCWB = new Button();
            CWB = new Button();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 6;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Controls.Add(LeftB, 1, 1);
            tableLayoutPanel1.Controls.Add(RightB, 3, 1);
            tableLayoutPanel1.Controls.Add(UpB, 2, 0);
            tableLayoutPanel1.Controls.Add(DownB, 2, 2);
            tableLayoutPanel1.Controls.Add(Close, 5, 3);
            tableLayoutPanel1.Controls.Add(URB, 4, 0);
            tableLayoutPanel1.Controls.Add(DRB, 4, 2);
            tableLayoutPanel1.Controls.Add(ULB, 1, 0);
            tableLayoutPanel1.Controls.Add(DLB, 1, 2);
            tableLayoutPanel1.Controls.Add(CCWB, 0, 0);
            tableLayoutPanel1.Controls.Add(CWB, 5, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 28F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 28F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 28F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 16F));
            tableLayoutPanel1.Size = new Size(523, 190);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // LeftB
            // 
            tableLayoutPanel1.SetColumnSpan(LeftB, 2);
            LeftB.Dock = DockStyle.Fill;
            LeftB.Location = new Point(107, 56);
            LeftB.Name = "LeftB";
            LeftB.Size = new Size(150, 47);
            LeftB.TabIndex = 0;
            LeftB.Text = "Left";
            LeftB.UseVisualStyleBackColor = true;
            // 
            // RightB
            // 
            tableLayoutPanel1.SetColumnSpan(RightB, 2);
            RightB.Dock = DockStyle.Fill;
            RightB.Location = new Point(263, 56);
            RightB.Name = "RightB";
            RightB.Size = new Size(150, 47);
            RightB.TabIndex = 1;
            RightB.Text = "right";
            RightB.UseVisualStyleBackColor = true;
            // 
            // UpB
            // 
            tableLayoutPanel1.SetColumnSpan(UpB, 2);
            UpB.Dock = DockStyle.Fill;
            UpB.Location = new Point(185, 3);
            UpB.Name = "UpB";
            UpB.Size = new Size(150, 47);
            UpB.TabIndex = 2;
            UpB.Text = "Up";
            UpB.UseVisualStyleBackColor = true;
            // 
            // DownB
            // 
            tableLayoutPanel1.SetColumnSpan(DownB, 2);
            DownB.Dock = DockStyle.Fill;
            DownB.Location = new Point(185, 109);
            DownB.Name = "DownB";
            DownB.Size = new Size(150, 47);
            DownB.TabIndex = 3;
            DownB.Text = "down";
            DownB.UseVisualStyleBackColor = true;
            // 
            // Close
            // 
            Close.Dock = DockStyle.Fill;
            Close.Location = new Point(419, 162);
            Close.Name = "Close";
            Close.Size = new Size(101, 25);
            Close.TabIndex = 4;
            Close.Text = "exit";
            Close.UseVisualStyleBackColor = true;
            Close.Click += Close_Click;
            // 
            // URB
            // 
            URB.Dock = DockStyle.Fill;
            URB.Location = new Point(341, 3);
            URB.Name = "URB";
            URB.Size = new Size(72, 47);
            URB.TabIndex = 5;
            URB.Text = "UR";
            URB.UseVisualStyleBackColor = true;
            // 
            // DRB
            // 
            DRB.Dock = DockStyle.Fill;
            DRB.Location = new Point(341, 109);
            DRB.Name = "DRB";
            DRB.Size = new Size(72, 47);
            DRB.TabIndex = 6;
            DRB.Text = "DR";
            DRB.UseVisualStyleBackColor = true;
            // 
            // ULB
            // 
            ULB.Dock = DockStyle.Fill;
            ULB.Location = new Point(107, 3);
            ULB.Name = "ULB";
            ULB.Size = new Size(72, 47);
            ULB.TabIndex = 7;
            ULB.Text = "UL";
            ULB.UseVisualStyleBackColor = true;
            // 
            // DLB
            // 
            DLB.Dock = DockStyle.Fill;
            DLB.Location = new Point(107, 109);
            DLB.Name = "DLB";
            DLB.Size = new Size(72, 47);
            DLB.TabIndex = 8;
            DLB.Text = "DL";
            DLB.UseVisualStyleBackColor = true;
            // 
            // CCWB
            // 
            CCWB.Dock = DockStyle.Fill;
            CCWB.Location = new Point(3, 3);
            CCWB.Name = "CCWB";
            CCWB.Size = new Size(98, 47);
            CCWB.TabIndex = 9;
            CCWB.Text = "CCW";
            CCWB.UseVisualStyleBackColor = true;
            // 
            // CWB
            // 
            CWB.Dock = DockStyle.Fill;
            CWB.Location = new Point(419, 3);
            CWB.Name = "CWB";
            CWB.Size = new Size(101, 47);
            CWB.TabIndex = 10;
            CWB.Text = "CW";
            CWB.UseVisualStyleBackColor = true;
            // 
            // Controler
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(523, 190);
            Controls.Add(tableLayoutPanel1);
            Name = "Controler";
            Text = "Controler";
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button LeftB;
        private Button RightB;
        private Button UpB;
        private Button DownB;
        private Button Close;
        private Button URB;
        private Button DRB;
        private Button ULB;
        private Button DLB;
        private Button CCWB;
        private Button CWB;
    }
}