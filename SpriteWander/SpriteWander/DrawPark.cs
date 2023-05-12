using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.Runtime.InteropServices;
using SpriteWander.Entity;

namespace SpriteWander
{
    public partial class DrawPark : Form
    {
        public List<Entity.Entity> Entities;
        readonly Dictionary<string, Texture> Textures;
        public Shader shader;

        int oldWindowLong;

        [Flags]
        enum WindowStyles : uint
        {
            WS_OVERLAPPED = 0x00000000,
            WS_POPUP = 0x80000000,
            WS_CHILD = 0x40000000,
            WS_MINIMIZE = 0x20000000,
            WS_VISIBLE = 0x10000000,
            WS_DISABLED = 0x08000000,
            WS_CLIPSIBLINGS = 0x04000000,
            WS_CLIPCHILDREN = 0x02000000,
            WS_MAXIMIZE = 0x01000000,
            WS_BORDER = 0x00800000,
            WS_DLGFRAME = 0x00400000,
            WS_VSCROLL = 0x00200000,
            WS_HSCROLL = 0x00100000,
            WS_SYSMENU = 0x00080000,
            WS_THICKFRAME = 0x00040000,
            WS_GROUP = 0x00020000,
            WS_TABSTOP = 0x00010000,

            WS_MINIMIZEBOX = 0x00020000,
            WS_MAXIMIZEBOX = 0x00010000,

            WS_CAPTION = WS_BORDER | WS_DLGFRAME,
            WS_TILED = WS_OVERLAPPED,
            WS_ICONIC = WS_MINIMIZE,
            WS_SIZEBOX = WS_THICKFRAME,
            WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,

            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
            WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
            WS_CHILDWINDOW = WS_CHILD,

            //Extended Window Styles

            WS_EX_DLGMODALFRAME = 0x00000001,
            WS_EX_NOPARENTNOTIFY = 0x00000004,
            WS_EX_TOPMOST = 0x00000008,
            WS_EX_ACCEPTFILES = 0x00000010,
            WS_EX_TRANSPARENT = 0x00000020,

            //#if(WINVER >= 0x0400)

            WS_EX_MDICHILD = 0x00000040,
            WS_EX_TOOLWINDOW = 0x00000080,
            WS_EX_WINDOWEDGE = 0x00000100,
            WS_EX_CLIENTEDGE = 0x00000200,
            WS_EX_CONTEXTHELP = 0x00000400,

            WS_EX_RIGHT = 0x00001000,
            WS_EX_LEFT = 0x00000000,
            WS_EX_RTLREADING = 0x00002000,
            WS_EX_LTRREADING = 0x00000000,
            WS_EX_LEFTSCROLLBAR = 0x00004000,
            WS_EX_RIGHTSCROLLBAR = 0x00000000,

            WS_EX_CONTROLPARENT = 0x00010000,
            WS_EX_STATICEDGE = 0x00020000,
            WS_EX_APPWINDOW = 0x00040000,

            WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE),
            WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST),
            //#endif /* WINVER >= 0x0400 */

            //#if(WIN32WINNT >= 0x0500)

            WS_EX_LAYERED = 0x00080000,
            //#endif /* WIN32WINNT >= 0x0500 */

            //#if(WINVER >= 0x0500)

            WS_EX_NOINHERITLAYOUT = 0x00100000, // Disable inheritence of mirroring by children
            WS_EX_LAYOUTRTL = 0x00400000, // Right to left mirroring
            //#endif /* WINVER >= 0x0500 */

            //#if(WIN32WINNT >= 0x0500)

            WS_EX_COMPOSITED = 0x02000000,
            WS_EX_NOACTIVATE = 0x08000000
            //#endif /* WIN32WINNT >= 0x0500 */

        }

        public enum GetWindowLongConst
        {
            GWL_WNDPROC = (-4),
            GWL_HINSTANCE = (-6),
            GWL_HWNDPARENT = (-8),
            GWL_STYLE = (-16),
            GWL_EXSTYLE = (-20),
            GWL_USERDATA = (-21),
            GWL_ID = (-12)
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        /// <summary>
        /// Make the form (specified by its handle) a window that supports transparency.
        /// </summary>
        /// <param name="Handle">The window to make transparency supporting</param>
        public void SetFormTransparent(IntPtr Handle)
        {
            oldWindowLong = GetWindowLong(Handle, (int)GetWindowLongConst.GWL_EXSTYLE);
            SetWindowLong(Handle, (int)GetWindowLongConst.GWL_EXSTYLE, Convert.ToInt32(oldWindowLong | (uint)WindowStyles.WS_EX_LAYERED | (uint)WindowStyles.WS_EX_TRANSPARENT));
        }

        private System.Windows.Forms.Timer TickTimer = null!;

        public DrawPark()
        {
            InitializeComponent();

            //MaximizeEverything();
            Opacity = Program._options.Alpha;

            SetFormTransparent(Handle);

            //SetTheLayeredWindowAttribute();
            Rectangle screen = Screen.PrimaryScreen.Bounds;
            Bounds = screen;
            gLControl.Location = screen.Location;
            gLControl.Size = screen.Size;
            Entities = new List<Entity.Entity>();
            Textures = new Dictionary<string, Texture>();
        }

        private void DrawPark_Load(object sender, EventArgs e)
        {
            Controls ControlForm = new Controls();
            ControlForm.Show();
        }

        private void GLControl_Load(object sender, EventArgs e)
        {
            gLControl.Resize += GLControl_Resize;
            gLControl.Paint += GLControl_Paint;

            // Redraw the screen every 1/20 of a second.
            TickTimer = new System.Windows.Forms.Timer();
            TickTimer.Tick += (sender, e) =>
            {
                foreach (Entity.Entity Entity in Entities)
                {
                    Entity.Tick(TickTimer.Interval / 200.0);
                }
                Render();
            };
            TickTimer.Interval = (int)(1000f / Program._options.TickFrequency);   // 1000 ms per sec / 50 ms per frame = 20 FPS
            TickTimer.Start();

            // Ensure that the viewport and projection matrix are set correctly initially.
            GLControl_Resize(gLControl, EventArgs.Empty);


            gLControl.MakeCurrent();
            gLControl.BackColor = BackColor;
            GL.ClearColor(BackColor);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Greater, 0.0f);
            GL.Viewport(0, 0, gLControl.Width, gLControl.Height);
            shader = new Shader("shader.vert", "shader.frag");
            shader.Use();
            gLControl.Invalidate();
            int texture0 = GL.GetUniformLocation(shader.Handle, "texture0");
            GL.Uniform1(texture0, 0);
            //ControlForm.Focus();
        }

        private void GLControl_Resize(object sender, EventArgs e)
        {
            gLControl.MakeCurrent();

            if (gLControl.ClientSize.Height == 0)
                gLControl.ClientSize = new System.Drawing.Size(gLControl.ClientSize.Width, 1);

            GL.Viewport(0, 0, gLControl.ClientSize.Width, gLControl.ClientSize.Height);
        }

        public void GLControl_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }
        public void AddEntity(string specie)
        {
            if (!Program.AllEntities.TryGetValue(specie, out EntityInfo info)) Debug.WriteLine("can't find ifo for pokemon " + specie);
            else
            {
                if (!Textures.TryGetValue(specie, out Texture texture))
                {
                    texture = new Texture(info.imagePath);
                    Textures.Add(specie, texture);
                }
                Entities.Add(new Entity.Entity(info, this, texture));
            }
        }

        private void Render()
        {
            gLControl.MakeCurrent();

            GL.ClearColor(BackColor);
            GL.Enable(EnableCap.DepthTest);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            foreach (Entity.Entity pokemon in Entities)
            {
                shader.Use();
                pokemon.Bind();
                pokemon.Render();
            }

            gLControl.SwapBuffers();
        }

        public void Exit()
        {
            TickTimer.Stop();
            TickTimer.Dispose();
            foreach (var Entity in Entities)
            {
                Entity.Dispose();
            }
            foreach (Texture texture in Textures.Values)
            {
                texture.Dispose();
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteProgram(shader.Handle);
            shader.Dispose();
            Close();
        }
    }
}
