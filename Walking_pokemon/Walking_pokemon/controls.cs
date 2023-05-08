namespace Walking_pokemon
{
    public partial class controls : Form
    {
        public controls()
        {
            InitializeComponent();
        }

        private void controls_Load(object sender, EventArgs e)
        {
            foreach (string pokemon in Program.pokedex.Keys)
            {
                PokemonList.Items.Add(pokemon);
            }
            PokemonList.SelectedIndex = 0;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Program.Park.Exit();
            this.Close();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            Program.Park.AddPokemon(this.PokemonList.SelectedItem.ToString());
        }
    }
}
