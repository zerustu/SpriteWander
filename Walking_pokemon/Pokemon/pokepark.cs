using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Runtime.InteropServices;

namespace Walking_pokemon.Pokemon
{
    public class Pokepark : GameWindow
    {
        private const int SDL_WINDOW_OPENGL = 0x00000002;
        private const int SDL_WINDOW_BORDERLESS = 0x00000010;

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GL_GetProcAddress")]
        public static extern IntPtr SDL_GL_GetProcAddress(string proc);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GL_SetAttribute")]
        public static extern int SDL_GL_SetAttribute(int attr, int value);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_CreateWindow")]
        public static extern IntPtr SDL_CreateWindow(string title, int x, int y, int w, int h, int flags);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GL_CreateContext")]
        public static extern IntPtr SDL_GL_CreateContext(IntPtr window);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GL_DeleteContext")]
        public static extern void SDL_GL_DeleteContext(IntPtr context);

        [DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_DestroyWindow")]
        public static extern void SDL_DestroyWindow(IntPtr window);

        private delegate IntPtr SDL_GL_GetProcAddressDelegate(string proc);
        private delegate int SDL_GL_SetAttributeDelegate(int attr, int value);

        private IntPtr SDL_WindowHandle;
        private IntPtr SDL_GLContext;

        private SDL_GL_GetProcAddressDelegate SDL_GL_GetProcAddress;
        private SDL_GL_SetAttributeDelegate SDL_GL_SetAttribute;


        public List<Pokemon> Pokemons;
        protected bool Gravity;
        Dictionary<string, Texture> Textures;


        public Shader shader;



        public Pokepark(int heigh, int width, bool gravity) : base(width, heigh, GraphicsMode.Default, "Pokepark")
        {
            Pokemons = new List<Pokemon>();
            Gravity = gravity;
            Textures = new Dictionary<string, Texture>();

            //this.WindowState = WindowState.Fullscreen;
            this.WindowBorder = WindowBorder.Hidden;
            
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

            // Create an SDL window with OpenGL context
            IntPtr sdlWindow = SDL_CreateWindow(
                Title,
                X, Y,
                Width, Height,
                SDL_WINDOW_OPENGL | SDL_WINDOW_BORDERLESS
            );

            IntPtr sdlContext = SDL_GL_CreateContext(sdlWindow);

            // Initialize OpenTK with the SDL context
            InitializeContext();

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.ClearColor(Color.Transparent);
            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Greater, 0.0f);

            shader = new Shader("shader.vert", "shader.frag");

            shader.Use();
            int texture0 = GL.GetUniformLocation(shader.Handle, "texture0");
            GL.Uniform1(texture0, 0);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            controls ControlForm = new controls();
            ControlForm.Show();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            foreach (Pokemon pokemon in Pokemons)
            {
                shader.Use();
                pokemon.Bind();
                pokemon.Render();
            }

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

            GL.DeleteProgram(shader.Handle);
            shader.Dispose();
            base.OnUnload(e);
        }
        protected override void Dispose(bool disposing)
        {
            // Clean up SDL resources when disposing the window
            SDL_GL_DeleteContext(SDL_GL_GetProcAddress);
            SDL_DestroyWindow(SDL_GetWindowSurface);

            base.Dispose(disposing);
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

        private void InitializeContext()
        {
            // Load SDL2 library
            OpenTK.Toolkit.Init();

            // Set up SDL_GL_GetProcAddress and SDL_GL_SetAttribute
            IntPtr sdlLibraryHandle = OpenTK.Platform.Utilities.LoadLibrary("SDL2.dll");
            SDL_GL_GetProcAddress = Marshal.GetDelegateForFunctionPointer<SDL_GL_GetProcAddressDelegate>(
                SDL_GL_GetProcAddress("SDL_GL_GetProcAddress")
            );

            SDL_GL_SetAttribute = Marshal.GetDelegateForFunctionPointer<SDL_GL_SetAttributeDelegate>(
                SDL_GL_GetProcAddress("SDL_GL_SetAttribute")
            );

            // Set SDL_GL attributes
            SDL_GL_SetAttribute(SDL_GL_CONTEXT_MAJOR_VERSION, 3);
            SDL_GL_SetAttribute(SDL_GL_CONTEXT_MINOR_VERSION, 3);
            SDL_GL_SetAttribute(SDL_GL_CONTEXT_PROFILE_MASK, SDL_GL_CONTEXT_PROFILE_CORE);

            // Initialize SDL
            SDL_Init(SDL_INIT_VIDEO);

            // Set SDL_GL attributes
            SDL_GL_SetAttribute(SDL_GL_RED_SIZE, 8);
            SDL_GL_SetAttribute(SDL_GL_GREEN_SIZE, 8);
            SDL_GL_SetAttribute(SDL_GL_BLUE_SIZE, 8);
            SDL_GL_SetAttribute(SDL_GL_ALPHA_SIZE, 8);
            SDL_GL_SetAttribute(SDL_GL_DEPTH_SIZE, 24);
            SDL_GL_SetAttribute(SDL_GL_STENCIL_SIZE, 8);
            SDL_GL_SetAttribute(SDL_GL_DOUBLEBUFFER, 1);

            // Initialize the SDL window and context
            SDL_WindowHandle = SDL_CreateWindow(
                Title,
                X, Y,
                Width, Height,
                SDL_WINDOW_OPENGL | SDL_WINDOW_BORDERLESS
            );

            SDL_GLContext = SDL_GL_CreateContext(SDL_WindowHandle);
        }
    }
}
