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
            InitEntityList();
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

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

            Process.Start(Application.ExecutablePath, "--help");
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void entityFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            do
            {
                DialogResult result = folderBrowserDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    Program._options.Folder = folderBrowserDialog1.SelectedPath;
                    Program.LoadData(folderBrowserDialog1.SelectedPath);
                    EntityList.Items.Clear();
                    try
                    {
                        InitEntityList();
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        MessageBox.Show("The folder must contains at least one entity");
                    }
                }
            } while (EntityList.Items.Count == 0);
        }

        private void InitEntityList()
        {
            foreach (string Name in Program.entries.Keys)
            {
                EntityList.Items.Add(Name);
            }
            EntityList.SelectedIndex = 0;
        }
    }
}
