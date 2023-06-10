using Newtonsoft.Json;
using OpenTK.Graphics.OpenGL4;
using SpriteWander.textures;

namespace SpriteWander.Entity
{
    public class Entity
    {
        // Position limits
        private readonly DrawPark Park;
        protected  float MIN_X { get => Park.Left + 0.5f; }
        protected float MAX_X { get => Park.Right - 0.5f; }
        protected float MIN_Y { get => Park.Top + 0.5f; }
        protected float MAX_Y { get => Park.Bottom - 0.5f; }

        //position
        protected float x = 20;
        protected float y = 20;

        public float X
        {
            get
            {
                return x;
            }
            set
            {
                if (value < MIN_X)
                {
                    x = MIN_X;
                    state = textures.Animation.Bump;
                }
                else if (value > MAX_X)
                {
                    x = MAX_X;
                    state = textures.Animation.Bump;
                }
                else
                {
                    x = value;
                }
            }
        }

        public float Y
        {
            get
            {
                return y;
            }
            set
            {
                if (value < MIN_Y)
                {
                    y = MIN_Y;
                    state = textures.Animation.Bump;
                }
                else if (value > MAX_Y)
                {
                    y = MAX_Y;
                    state = textures.Animation.Bump;
                }
                else
                {
                    y = value;
                }
            }
        }

        private float[] GetCenter()
        {
            return new float[] { (X - MIN_X) / (MAX_X - MIN_X), (Y - MIN_Y) / (MAX_Y - MIN_Y) };
        }

        public float[] GetPos()
        {
            Rectangle draw = Texture.Getcoord(state, subState, animTimer, out Event);
            float[] center = GetCenter();
            float cx = center[0];
            float cy = center[1];
            float[] vertices = {
                    cx * 2f - 1f - draw.Width * Scale / Park.Width, //bas gauche
                    cy * 2f - 1f - draw.Height * Scale / Park.Height,
                    ((float)draw.X),
                    1-((float) draw.Y + draw.Height),
                    cx * 2f - 1f + draw.Width * Scale / Park.Width, // bas droite
                    cy * 2f - 1f - draw.Height * Scale / Park.Height,
                    ((float) draw.X + draw.Width),
                    1-((float) draw.Y + draw.Height),
                    cx * 2f - 1f - draw.Width * Scale / Park.Width, // haut gauche
                    cy * 2f - 1f + draw.Height * Scale / Park.Height,
                    ((float) draw.X),
                    1-((float) draw.Y),
                    cx * 2f - 1f + draw.Width * Scale / Park.Width, //hautdroite
                    cy * 2f - 1f + draw.Height * Scale / Park.Height,
                    ((float) draw.X +(float) draw.Width),
                    1-((float) draw.Y)
                };
            return vertices;
        }

        private static readonly uint[] INDICES = {  // note that we start from 0!
            0, 1, 2,   // first triangle
            1, 2, 3    // second triangle
        };
        public readonly textures.Texture Texture;
        readonly int VertexBufferObject;
        readonly int VertexArrayObject;
        readonly int ElementBufferObject;


        //animations variable
        protected int animTimer = 0;
        protected textures.AnimEvent Event = AnimEvent.Nothing;


        //Pokemon state
        public SpriteWander.textures.Animation state = SpriteWander.textures.Animation.Default;
        public int timer = 0;
        protected Direction subState = 0;

        protected readonly float scale = 0;

        public float Scale => scale;

        protected static readonly Random rng = new();



        public Entity(DrawPark Park, textures.Texture texture, float scale = 1)
        {
            this.Park = Park;
            this.scale = scale < 0 ? (float)(rng.NextDouble() + 2.5) : scale;
            X = (float)(rng.NextDouble() * (MAX_X - MIN_X) + MIN_X);
            Y = (float)(rng.NextDouble() * (MAX_Y - MIN_Y) + MIN_Y);
            Texture = texture;
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, GetPos().Length * sizeof(float), GetPos(), BufferUsageHint.StreamDraw);

            Park.shader.Use();
            int CoordLocation = GL.GetAttribLocation(Park.shader.Handle, "aPosition");
            GL.EnableVertexAttribArray(CoordLocation);
            GL.VertexAttribPointer(CoordLocation, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            int textLocation = GL.GetAttribLocation(Park.shader.Handle, "aTexCoord");
            GL.EnableVertexAttribArray(textLocation);
            GL.VertexAttribPointer(textLocation, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));

            ElementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);

            GL.BufferData(BufferTarget.ElementArrayBuffer, INDICES.Length * sizeof(uint), INDICES, BufferUsageHint.StaticDraw);
        }


        public void Tick(double time)
        {
            timer--;
            animTimer++;
            if (timer < 0) timer = 0;
            switch (state)
            {
                case textures.Animation.Default:
                case textures.Animation.Bump:
                    state = Texture.Normalise(state, out Event);
                    break;
                default:
                    break;
            }
            switch (Event)
            {
                case AnimEvent.End:
                    animTimer = 0;
                    NextAnim();
                    Event = AnimEvent.Nothing;
                    break;
                case AnimEvent.Reset:
                    animTimer = 0;
                    Event = AnimEvent.Nothing;
                    break;
                case AnimEvent.Nothing:
                default: 
                    break;
            }
        }

        protected void NextAnim()
        {
            switch (state)
            {
                case textures.Animation.Walk:
                    break;
                case textures.Animation.Attack:
                    break;
                case textures.Animation.Attack1:
                    break;
                case textures.Animation.Attack2:
                    break;
                case textures.Animation.Attack3:
                    break;
                case textures.Animation.Sleep:
                    break;
                case textures.Animation.Hurt:
                    break;
                default:
                case textures.Animation.Default:
                case textures.Animation.Bump:
                case textures.Animation.Idle:
                    break;
                case textures.Animation.Swing:
                    break;
                case textures.Animation.Double:
                    break;
                case textures.Animation.Hop:
                    break;
                case textures.Animation.Charge:
                    break;
                case textures.Animation.Rotate:
                    break;
                case textures.Animation.Eventsleep:
                    break;
                case textures.Animation.Wake:
                    break;
                case textures.Animation.Eat:
                    break;
                case textures.Animation.Tumble:
                    break;
                case textures.Animation.Pose:
                    break;
                case textures.Animation.Pull:
                    break;
                case textures.Animation.Pain:
                    break;
                case textures.Animation.Float:
                    break;
                case textures.Animation.DeepBreath:
                    break;
                case textures.Animation.Nod:
                    break;
                case textures.Animation.Sit:
                    break;
                case textures.Animation.LookUp:
                    break;
                case textures.Animation.Sink:
                    break;
                case textures.Animation.Trip:
                    break;
                case textures.Animation.Laying:
                    break;
                case textures.Animation.LeapForth:
                    break;
                case textures.Animation.Head:
                    break;
                case textures.Animation.Cringe:
                    break;
                case textures.Animation.LostBalance:
                    break;
                case textures.Animation.TumbleBack:
                    break;
                case textures.Animation.Faint:
                    break;
                case textures.Animation.HitGround:
                    break;
            }
        }

        public void Bind()
        {
            GL.BindVertexArray(VertexArrayObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            Texture.Use(state);
        }

        public void Render()
        {
            GL.BufferData(BufferTarget.ArrayBuffer, GetPos().Length * sizeof(float), GetPos(), BufferUsageHint.StreamDraw);
            GL.DrawElements(PrimitiveType.Triangles, INDICES.Length, DrawElementsType.UnsignedInt, 0);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(VertexBufferObject);
            GL.DeleteBuffer(ElementBufferObject);
            GL.DeleteBuffer(VertexArrayObject);
        }
    }
}
