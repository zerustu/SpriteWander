
namespace Walking_pokemon
{
    partial class controls
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
            this.AddButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.PokemonList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(15, 332);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(119, 55);
            this.AddButton.TabIndex = 0;
            this.AddButton.Text = "Add 1";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Location = new System.Drawing.Point(140, 332);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(127, 54);
            this.CloseButton.TabIndex = 1;
            this.CloseButton.Text = "Close app";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // PokemonList
            // 
            this.PokemonList.FormattingEnabled = true;
            this.PokemonList.Location = new System.Drawing.Point(9, 5);
            this.PokemonList.Name = "PokemonList";
            this.PokemonList.Size = new System.Drawing.Size(257, 303);
            this.PokemonList.TabIndex = 2;
            // 
            // controls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 395);
            this.Controls.Add(this.PokemonList);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.AddButton);
            this.Name = "controls";
            this.Text = "controls";
            this.Load += new System.EventHandler(this.controls_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.ListBox PokemonList;
    }
}