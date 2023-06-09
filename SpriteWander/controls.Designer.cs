﻿namespace SpriteWander
{
    partial class Controls
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
            EntityList = new ListBox();
            SuspendLayout();
            // 
            // AddButton
            // 
            AddButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            AddButton.Location = new Point(20, 875);
            AddButton.Margin = new Padding(11, 8, 11, 8);
            AddButton.Name = "AddButton";
            AddButton.Size = new Size(344, 191);
            AddButton.TabIndex = 0;
            AddButton.Text = "Add Sprites";
            AddButton.UseVisualStyleBackColor = true;
            AddButton.Click += AddButton_Click;
            // 
            // CloseButton
            // 
            CloseButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            CloseButton.Location = new Point(401, 875);
            CloseButton.Margin = new Padding(11, 8, 11, 8);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(376, 191);
            CloseButton.TabIndex = 1;
            CloseButton.Text = "Close app";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // EntityList
            // 
            EntityList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            EntityList.FormattingEnabled = true;
            EntityList.ItemHeight = 41;
            EntityList.Location = new Point(23, 16);
            EntityList.Margin = new Padding(11, 8, 11, 8);
            EntityList.Name = "EntityList";
            EntityList.SelectionMode = SelectionMode.MultiExtended;
            EntityList.Size = new Size(735, 824);
            EntityList.TabIndex = 2;
            EntityList.SelectedIndexChanged += EntityList_SelectedIndexChanged;
            // 
            // Controls
            // 
            AutoScaleDimensions = new SizeF(17F, 41F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(797, 1083);
            Controls.Add(EntityList);
            Controls.Add(CloseButton);
            Controls.Add(AddButton);
            Margin = new Padding(11, 8, 11, 8);
            Name = "Controls";
            Text = "controls";
            FormClosing += CloseMainApp;
            Load += Controls_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button AddButton;
        private Button CloseButton;
        private ListBox EntityList;
    }
}