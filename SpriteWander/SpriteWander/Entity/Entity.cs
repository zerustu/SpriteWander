using Newtonsoft.Json;
using OpenTK.Graphics.OpenGL4;
using SpriteWander.textures;

namespace SpriteWander.Entity
{
    public class Entity
    {
        // Position limits
        private readonly DrawPark Park;
        protected  float MIN_X { get =>  0.5f; }
        protected float MAX_X { get => Park.max_X - 0.5f; }
        protected float MIN_Y { get => 0.5f; }
        protected float MAX_Y { get => Park.max_Y - 0.5f; }

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
            Rectangle draw = Texture.Getcoord(state, subState, animTimer, out int Width, out int Height, out Event);
            float[] center = GetCenter();
            float cx = center[0];
            float cy = center[1];
            float[] vertices = {
                    cx * 2f - 1f - draw.Width * Scale / Park.Width, //bas gauche
                    cy * 2f - 1f - draw.Height * Scale / Park.Height,
                    ((float)draw.X) / Width,
                    1-((float) draw.Y + draw.Height) / Height,
                    cx * 2f - 1f + draw.Width * Scale / Park.Width, // bas droite
                    cy * 2f - 1f - draw.Height * Scale / Park.Height,
                    ((float) draw.X + draw.Width) / Width,
                    1-((float) draw.Y + draw.Height) / Height,
                    cx * 2f - 1f - draw.Width * Scale / Park.Width, // haut gauche
                    cy * 2f - 1f + draw.Height * Scale / Park.Height,
                    ((float) draw.X) / Width,
                    1-((float) draw.Y) / Height,
                    cx * 2f - 1f + draw.Width * Scale / Park.Width, //hautdroite
                    cy * 2f - 1f + draw.Height * Scale / Park.Height,
                    ((float) draw.X +(float) draw.Width) / Width,
                    1-((float) draw.Y) / Height
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
        public int cycle = 0;
        protected Direction subState = 0;

        protected readonly float scale = 0;

        public float Scale => scale;

        protected static readonly Random rng = new();



        public Entity(DrawPark Park, textures.Texture texture, float scale = 1)
        {
            state = textures.Animation.Default;
            subState = 0;
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
            animTimer++;
            if (cycle < 0) cycle = 0;
            switch (Event)
            {
                case AnimEvent.End:
                    NextAnim();
                    turn();
                    break;
                case AnimEvent.Reset:
                    turn();
                    if (cycle == 0) NextAnim();
                    else
                    {
                        Event = AnimEvent.Nothing;
                        animTimer = 0;
                        cycle--;
                    }
                    break;
                case AnimEvent.Nothing:
                default: 
                    break;
            }
        }

        protected void NextAnim()
        {
            animTimer = 0;
            double rngvalue = rng.NextDouble();
            switch (state)
            {
                case textures.Animation.Sleep:
                case textures.Animation.Eventsleep:
                case textures.Animation.Laying:
                    state = textures.Animation.Wake;
                    break;
                default:
                case textures.Animation.Default:
                case textures.Animation.Idle:
                    if (rngvalue < 0.5) state = textures.Animation.Walk;
                    else if (rngvalue < 0.55) state = textures.Animation.LeapForth;
                    else if (rngvalue < 0.90) state = textures.Animation.Sleep;
                    else if (rngvalue < 0.95) state = textures.Animation.Pose;
                    else state = textures.Animation.Eat;
                    break;
                case textures.Animation.Trip:
                case textures.Animation.LostBalance:
                case textures.Animation.Faint:
                case textures.Animation.HitGround:
                    state = textures.Animation.Laying;
                    break;
                case textures.Animation.Walk:
                case textures.Animation.Attack:
                case textures.Animation.Attack1:
                case textures.Animation.Attack2:
                case textures.Animation.Attack3:
                case textures.Animation.Double:
                case textures.Animation.Hurt:
                case textures.Animation.Bump:
                case textures.Animation.Swing:
                case textures.Animation.Hop:
                case textures.Animation.Charge:
                case textures.Animation.Rotate:
                case textures.Animation.Eat:
                case textures.Animation.Tumble:
                case textures.Animation.TumbleBack:
                case textures.Animation.Wake:
                case textures.Animation.Pose:
                case textures.Animation.Pull:
                case textures.Animation.Pain:
                case textures.Animation.Float:
                case textures.Animation.DeepBreath:
                case textures.Animation.Nod:
                case textures.Animation.Sit:
                case textures.Animation.LookUp:
                case textures.Animation.Sink:
                case textures.Animation.LeapForth:
                case textures.Animation.Head:
                case textures.Animation.Cringe:
                    state = textures.Animation.Idle;
                    break;
            }
            state = Texture.Normalise(state, out _);
            if (Texture.EndBehaviour(state) == AnimEvent.Reset)
            {
                cycle = rng.Next(5, 20);
            }
        }

        protected void turn()
        {
            double rngvalue = rng.NextDouble();
            int dir = (int)subState;
            if (rngvalue < 0.2) dir++;
            else if (rngvalue < 0.4) dir--;
            else if (rngvalue < 0.5) dir += 2;
            else if (rngvalue < 0.6) dir -= 2;
            else if (rngvalue < 0.7) dir += 4;
            dir %= 8;
            subState = (Direction)dir;
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
