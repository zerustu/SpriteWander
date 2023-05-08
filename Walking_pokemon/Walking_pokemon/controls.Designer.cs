
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
            AddButton = new Button();
            CloseButton = new Button();
            PokemonList = new ListBox();
            SuspendLayout();
            // 
            // AddButton
            // 
            AddButton.Location = new Point(18, 383);
            AddButton.Margin = new Padding(4, 3, 4, 3);
            AddButton.Name = "AddButton";
            AddButton.Size = new Size(139, 63);
            AddButton.TabIndex = 0;
            AddButton.Text = "Add 1";
            AddButton.UseVisualStyleBackColor = true;
            AddButton.Click += AddButton_Click;
            // 
            // CloseButton
            // 
            CloseButton.Location = new Point(163, 383);
            CloseButton.Margin = new Padding(4, 3, 4, 3);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(148, 62);
            CloseButton.TabIndex = 1;
            CloseButton.Text = "Close app";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // PokemonList
            // 
            PokemonList.FormattingEnabled = true;
            PokemonList.ItemHeight = 15;
            PokemonList.Location = new Point(10, 6);
            PokemonList.Margin = new Padding(4, 3, 4, 3);
            PokemonList.Name = "PokemonList";
            PokemonList.Size = new Size(299, 349);
            PokemonList.TabIndex = 2;
            // 
            // controls
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(328, 456);
            Controls.Add(PokemonList);
            Controls.Add(CloseButton);
            Controls.Add(AddButton);
            Margin = new Padding(4, 3, 4, 3);
            Name = "controls";
            Text = "controls";
            Load += controls_Load;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.ListBox PokemonList;
    }
}