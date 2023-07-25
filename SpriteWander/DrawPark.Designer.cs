namespace SpriteWander
{
    partial class DrawPark
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
            gLControl = new OpenTK.GLControl();
            SuspendLayout();
            // 
            // gLControl
            // 
            gLControl.AutoSize = true;
            gLControl.BackColor = Color.Black;
            gLControl.Location = new Point(0, 0);
            gLControl.Margin = new Padding(4, 3, 4, 3);
            gLControl.Name = "gLControl";
            gLControl.Size = new Size(800, 450);
            gLControl.TabIndex = 0;
            gLControl.VSync = false;
            gLControl.Load += GLControl_Load;
            // 
            // DrawPark
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(800, 450);
            Controls.Add(gLControl);
            FormBorderStyle = FormBorderStyle.None;
            Name = "DrawPark";
            ShowInTaskbar = false;
            Text = "DrawPark";
            TopMost = true;
            TransparencyKey = SystemColors.Control;
            Load += DrawPark_Load;
            Activated += DrawPark_GotFocus;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private OpenTK.GLControl gLControl;
    }
}