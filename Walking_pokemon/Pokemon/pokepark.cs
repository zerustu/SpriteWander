using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace Walking_pokemon.Pokemon
{
    public class Pokepark : GameWindow
    {
        public List<Pokemon> Pokemons;
        protected bool Gravity;
        Dictionary<string, Texture> Textures;


        public Shader shader;



        public Pokepark(int heigh, int width, bool gravity) : base(width, heigh, GraphicsMode.Default, "Pokepark")
        {
            Pokemons = new List<Pokemon>();
            Gravity = gravity;
            Textures = new Dictionary<string, Texture>();
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            foreach (Pokemon pokemon in Pokemons)
            {
                pokemon.Tick(e.Time);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            controls ControlForm = new controls();
            ControlForm.Show();

            GL.ClearColor(0.2f, 0.3f, 0f, 1f);

            shader = new Shader("shader.vert", "shader.frag");

            shader.Use();
            int texture0 = GL.GetUniformLocation(shader.Handle, "texture0");
            GL.Uniform1(texture0, 0);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {

            GL.Clear(ClearBufferMask.ColorBufferBit);

            foreach (Pokemon pokemon in Pokemons)
            {
                shader.Use();
                pokemon.Bind();
                pokemon.Render();
            }

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);
        }

        protected override void OnUnload(EventArgs e)
        {
            // Unbind all the resources by binding the targets to 0/null.
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteProgram(shader.Handle);
            shader.Dispose();
            base.OnUnload(e);
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
                Pokemons.Add(new Pokemon(info, this, texture, texture.Width, texture.Height, shader.Handle));
            }
        }
    }
}
