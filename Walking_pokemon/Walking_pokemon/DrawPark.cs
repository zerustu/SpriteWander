using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Walking_pokemon.Pokemon;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Walking_pokemon
{
    public partial class DrawPark : Form
    {
        BackgroundWorker bw = new BackgroundWorker();
        public List<Walking_pokemon.Pokemon.Pokemon> Pokemons;
        Dictionary<string, Texture> Textures;
        public Shader shader;
        private bool _loaded;

        private System.Windows.Forms.Timer _timer = null!;
        private float _angle = 0.0f;

        public DrawPark()
        {
            Pokemons = new List<Walking_pokemon.Pokemon.Pokemon>();
            Textures = new Dictionary<string, Texture>();
            controls ControlForm = new controls();
            ControlForm.Show();
            InitializeComponent();
        }

        private void DrawPark_Load(object sender, EventArgs e)
        {
            //BackgroundWorker tmpBw = new BackgroundWorker();
            //tmpBw.DoWork += new DoWorkEventHandler(bw_DoWork);
            //
            //this.bw = tmpBw;
            //
            //this.bw.RunWorkerAsync();
        }

        private void GLControl_Load(object sender, EventArgs e)
        {
            gLControl.Resize += GLControl_Resize;
            gLControl.Paint += GLControl_Paint;

            // Redraw the screen every 1/20 of a second.
            _timer = new System.Windows.Forms.Timer();
            _timer.Tick += (sender, e) =>
            {
                _angle += 0.5f;
                foreach (Pokemon.Pokemon pokemon in Pokemons)
                {
                    pokemon.Tick(_timer.Interval);
                }
                Render();
            };
            _timer.Interval = 50;   // 1000 ms per sec / 50 ms per frame = 20 FPS
            _timer.Start();

            // Ensure that the viewport and projection matrix are set correctly initially.
            GLControl_Resize(gLControl, EventArgs.Empty);


            gLControl.MakeCurrent();
            GL.ClearColor(this.BackColor);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
            GL.Viewport(0, 0, gLControl.Width, gLControl.Height);
            shader = new Shader("shader.vert", "shader.frag");
            shader.Use();
            _loaded = true;
            gLControl.Invalidate();
            int texture0 = GL.GetUniformLocation(shader.Handle, "texture0");
            GL.Uniform1(texture0, 0);
        }

        private void GLControl_Resize(object sender, EventArgs e)
        {
            gLControl.MakeCurrent();

            if (gLControl.ClientSize.Height == 0)
                gLControl.ClientSize = new System.Drawing.Size(gLControl.ClientSize.Width, 1);

            GL.Viewport(0, 0, gLControl.ClientSize.Width, gLControl.ClientSize.Height);


            //if (!_loaded)
            //    return;
            //GL.Viewport(0, 0, gLControl.Width, gLControl.Height);
            //gLControl.Invalidate();
        }

        public void GLControl_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }
        public void AddPokemon(string specie)
        {
            PokemonInfo info;
            if (!Program.pokedex.TryGetValue(specie, out info)) Debug.WriteLine("can't find ifo for pokemon " + specie);
            else
            {
                Texture texture;
                if (!Textures.TryGetValue(specie, out texture))
                {
                    texture = new Texture(info.imagePath);
                    Textures.Add(specie, texture);
                }
                Pokemons.Add(new Walking_pokemon.Pokemon.Pokemon(info, this, texture, texture.Width, texture.Height, shader.Handle));
            }
        }

        private void Render()
        {
            gLControl.MakeCurrent();

            GL.ClearColor(BackColor);
            GL.Enable(EnableCap.DepthTest);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            foreach (Pokemon.Pokemon pokemon in Pokemons)
            {
                shader.Use();
                pokemon.Bind();
                pokemon.Render();
            }

            gLControl.SwapBuffers();
        }

        public void Exit()
        {
            _loaded = false;
            bw.Dispose();
            foreach (var pokemon in Pokemons)
            {
                pokemon.Dispose();
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteProgram(shader.Handle);
            shader.Dispose();
            this.Close();
        }
    }
}
