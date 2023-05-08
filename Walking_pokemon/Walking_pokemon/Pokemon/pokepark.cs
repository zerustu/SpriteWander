//using System;
//using System.Collections.Generic;
//using System.Windows.Forms;
//using System.Text;
//using System.Drawing;
//using System.Diagnostics;
//using OpenTK.Graphics.OpenGL;
//using OpenTK.Windowing.Common;
//using OpenTK.Windowing.Desktop;
//using OpenTK.Windowing.GraphicsLibraryFramework;
//
//using System;
//using System.Runtime.InteropServices;
//using OpenTK;
//using OpenTK.Graphics;
//using OpenTK.Platform;
//
//namespace Walking_pokemon.Pokemon
//{
//    public class Pokepark : GameWindow
//    {
//        public List<Pokemon> Pokemons;
//        protected bool Gravity;
//        Dictionary<string, Texture> Textures;
//
//
//        public Shader shader;
//
//
//
//        public Pokepark(int height, int width, bool gravity) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = ( width, height), Title = "pokepark"})
//        {
//            Pokemons = new List<Pokemon>();
//            Gravity = gravity;
//            Textures = new Dictionary<string, Texture>();
//
//            //this.WindowState = WindowState.Fullscreen;
//            this.WindowBorder = WindowBorder.Hidden;
//        }
//
//
//        protected override void OnUpdateFrame(FrameEventArgs e)
//        {
//            base.OnUpdateFrame(e);
//            foreach (Pokemon pokemon in Pokemons)
//            {
//                pokemon.Tick(e.Time);
//            }
//        }
//
//        protected override void OnLoad()
//        {
//            base.OnLoad();
//
//            GL.ClearColor(Color.Transparent);
//            GL.Enable(EnableCap.Blend);
//            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
//            GL.Enable(EnableCap.AlphaTest);
//            GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
//
//        shader = new Shader("shader.vert", "shader.frag");
//
//            shader.Use();
//            int texture0 = GL.GetUniformLocation(shader.Handle, "texture0");
//            GL.Uniform1(texture0, 0);
//
//
//            controls ControlForm = new controls();
//            ControlForm.Show();
//        }
//
//        protected override void OnRenderFrame(FrameEventArgs e)
//        {
//            base.OnRenderFrame(e);
//            GL.Clear(ClearBufferMask.ColorBufferBit);
//
//            foreach (Pokemon pokemon in Pokemons)
//            {
//                shader.Use();
//                pokemon.Bind();
//                pokemon.Render();
//            }
//
//            Context.SwapBuffers();
//        }
//
//        protected override void OnResize(ResizeEventArgs e)
//        {
//            base.OnResize(e);
//
//            GL.Viewport(0, 0, e.Width, e.Height);
//        }
//
//        protected override void OnUnload()
//        {
//            // Unbind all the resources by binding the targets to 0/null.
//            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
//            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
//            GL.BindVertexArray(0);
//            GL.UseProgram(0);
//
//            GL.DeleteProgram(shader.Handle);
//            shader.Dispose();
//            base.OnUnload();
//        }
//        protected override void Dispose(bool disposing)
//        {
//            base.Dispose(disposing);
//        }
//
//        public void AddPokemon(string specie)
//        {
//            PokemonInfo info;
//            if (!Program.pokedex.TryGetValue(specie, out info)) Debug.WriteLine("can't find ifo for pokemon " + specie);
//            else
//            {
//                Texture texture;
//                if (!Textures.TryGetValue(specie, out texture))
//                {
//                    texture = new Texture(info.imagePath);
//                    Textures.Add(specie, texture);
//                }
//                //Pokemons.Add(new Pokemon(info, this, texture, texture.Width, texture.Height, shader.Handle));
//            }
//        }
//
//        internal void Exit()
//        {
//            this.Close();
//        }
//    }
//}
//