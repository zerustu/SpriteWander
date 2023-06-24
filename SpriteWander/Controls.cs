using System.Configuration;
using System.Diagnostics;

namespace SpriteWander
{
    public partial class Controls : Form
    {
        public Controls()
        {
            InitializeComponent();
        }

        private void Controls_Load(object sender, EventArgs e)
        {
            foreach (string Name in Program.entries.Keys)
            {
                EntityList.Items.Add(Name);
            }
            EntityList.SelectedIndex = 0;
        }

        private void CloseButton_Click(object sender, EventArgs e) => Close();

        private void AddButton_Click(object sender, EventArgs e)
        {
            foreach (string selectedItem in EntityList.SelectedItems)
            {
                Program.Park?.AddEntity(selectedItem);
            }
        }

        private void CloseMainApp(object sender, EventArgs e)
        {
            Program.Park?.Exit();
        }

        private void EntityList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
