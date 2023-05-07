using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Walking_pokemon.Pokemon
{
    public class Pokepark : GameWindow
    {
        public List<Pokemon> Pokemons;
        protected bool Gravity;


        int VertexBufferObject;
        int ElementBufferObject;
        int VertexArrayObject;
        Shader shader;

        float[] vertices = {
             0.5f,  0.5f, 0.0f,  // top right
             0.5f, -0.5f, 0.0f,  // bottom right
            -0.5f, -0.5f, 0.0f,  // bottom left
            -0.5f,  0.5f, 0.0f   // top left
        }; 
        uint[] indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };


        protected Brush invisible = new SolidBrush(Color.FromArgb(0, 127, 151));

        static SolidBrush transparentBrush = new SolidBrush(Color.FromArgb(0, 127, 151));

        public Pokepark(int heigh, int width, bool gravity) : base(width, heigh, GraphicsMode.Default, "Pokepark")
        {
            Pokemons = new List<Pokemon>();
            Gravity = gravity;
        }

        public Image DrawPark(Image result)
        {
            Graphics land = Graphics.FromImage(result);
            //land.FillRectangle(transparentBrush, new Rectangle(0, 0, Size.Width, Size.Height));
            land.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            land.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            foreach (Pokemon pokemon1 in Pokemons)
            {
                Point pos = pokemon1.oldPos;
                Rectangle Rect = pokemon1.oldRect;
                int width = (int)((float)Rect.Width * pokemon1.Scale);
                int height = (int)((float)Rect.Height * pokemon1.Scale);
                Rectangle destRect = new Rectangle(pos.X - width / 2, pos.Y - height / 2, width, height);
                land.FillRectangle(transparentBrush, destRect);
            }
            foreach (Pokemon pokemon in Pokemons)
            {
                Image sprite = pokemon.Spritesheet;
                Point pos = pokemon.Pos;
                Rectangle Rect = pokemon.SpriteRect;
                int width = (int)((float)Rect.Width * pokemon.Scale);
                int height = (int)((float)Rect.Height * pokemon.Scale);
                Rectangle destRect = new Rectangle(pos.X - width / 2, pos.Y - height / 2, width, height);
                land.DrawImage(sprite, destRect, Rect, GraphicsUnit.Pixel);
                //land.DrawLine(Pens.Red, pos, new Point(0,0));
                //land.DrawString($"{pokemon.timer}, {pokemon.state}", SystemFonts.DefaultFont, Brushes.Red, pos.X, pos.Y + 90);
            }
            return result;
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
            GL.ClearColor(0.2f, 0.3f, 0f, 1f);
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw); 

            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);


            shader = new Shader("shader.vert", "shader.frag");

            shader.Use();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            shader.Use();
            GL.BindVertexArray(VertexArrayObject);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            Context.SwapBuffers();
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

            // Delete all the resources.
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteVertexArray(VertexArrayObject);
            GL.DeleteBuffer(VertexBufferObject);

            GL.DeleteProgram(shader.Handle);
            shader.Dispose();
            base.OnUnload(e);
        }

        public void AddPokemon(string specie)
        {
            PokemonInfo info;
            if (!Program.pokedex.TryGetValue(specie, out info)) Debug.WriteLine("can't find ifo for pokemon " + specie);
            else Pokemons.Add(new Pokemon(info, this));
        }
    }
}
