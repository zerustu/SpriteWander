using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpriteWander
{
    public partial class Controler : Form
    {
        private DrawPark park;

        public Controler(DrawPark Park)
        {
            InitializeComponent();
            park = Park;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            park.NextCommands = Entity.ControlsCom.Disconnect;
        }
    }
}
