using OpenTK.Graphics.OpenGL4;
using SpriteWander.textures;

namespace SpriteWander.Entity
{
    public class Entity
    {
        // Position limits
        private readonly DrawPark Park;
        static readonly float borderMargin = 0.5f;
        protected float MIN_X = borderMargin;
        protected float MAX_X { get => Park.max_X - borderMargin; }
        protected float MIN_Y = borderMargin;
        protected float MAX_Y { get => Park.max_Y - borderMargin; }

        //position
        protected float x = 20;
        protected float y = 20;

        float targetX, targetY;

        public float X
        {
            get => x;
            set
            {
                x = Math.Clamp(value, MIN_X, MAX_X);
                if (x != value) state = Animation.Bump;
            }
        }

        public float Y
        {
            get => y;
            set
            {
                y = Math.Clamp(value, MIN_Y, MAX_Y);
                if (y != value) state = Animation.Bump;
            }
        }

        private (float, float) GetCenter() => (X / Park.max_X, Y / Park.max_Y);

        public float[] GetPos()
        {
            Rectangle draw = Texture.Getcoord(state, subState, animTimer, out int Width, out int Height, out Event);
            (float cx, float cy) = GetCenter();
            return new[] {
                cx * 2f - 1f - Scale * draw.Width / (Park.max_X * 20), // bas gauche
                cy * 2f - 1f - Scale * draw.Height / (Park.max_Y * 20),
                (float)draw.X / Width,
                1 - (float)(draw.Y + draw.Height) / Height,
                cx * 2f - 1f + Scale * draw.Width / (Park.max_X * 20), // bas droite
                cy * 2f - 1f - Scale * draw.Height / (Park.max_Y * 20),
                (float)(draw.X + draw.Width) / Width,
                1 - (float)(draw.Y + draw.Height) / Height,
                cx * 2f - 1f - Scale * draw.Width / (Park.max_X * 20), // haut gauche
                cy * 2f - 1f + Scale * draw.Height / (Park.max_Y * 20),
                (float)draw.X / Width,
                1 - (float)(draw.Y) / Height,
                cx * 2f - 1f + Scale * draw.Width / (Park.max_X * 20), // haut droite
                cy * 2f - 1f + Scale * draw.Height / (Park.max_Y * 20),
                (float)(draw.X + draw.Width) / Width,
                1 - (float)(draw.Y) / Height
            };
        }

        private static readonly uint[] INDICES = {  // note that we start from 0!
            0, 1, 2,   // first triangle
            1, 2, 3    // second triangle
        };
        public readonly Texture Texture;
        readonly int VertexBufferObject;
        readonly int VertexArrayObject;
        readonly int ElementBufferObject;


        //animations variable
        protected int animTimer = 0;
        protected AnimEvent Event = AnimEvent.Nothing;


        //Pokemon state
        public Animation state = Animation.Default;
        public int cycle = 0;
        protected Direction subState = 0;

        protected readonly float scale = 0;

        public float Scale => scale;

        protected static readonly Random rng = new();

        public Entity(DrawPark Park, Texture texture, float scale = 1)
        {
            state = Animation.Default;
            subState = (Direction)rng.Next(0, 8);
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
                    turn(0.7);
                    break;
                case AnimEvent.Reset:
                    turn(0.1);
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
            int length = Texture.Length(state);
            switch (state)
            {
                case Animation.Walk:
                case Animation.LeapForth:
                    Move(1f / length);
                    break;
                case Animation.TumbleBack:
                    Move(-1f / length);
                    break;
                default:
                    break;
            }
        }

        protected void Move(float speed)
        {
            (int vecX, int vecY) = Texture.DirToVect(subState);
            X += vecX * speed;
            Y += vecY * speed;
        }

        protected void NextAnim()
        {
            animTimer = 0;
            double rngvalue = rng.NextDouble();
            switch (state)
            {
                case Animation.Sleep:
                case Animation.Eventsleep:
                case Animation.Laying:
                    state = Animation.Wake;
                    break;
                default:
                case Animation.Default:
                case Animation.Idle:
                    if (rngvalue < 0.5) state = Animation.Walk;
                    else if (rngvalue < 0.55) state = Animation.LeapForth;
                    else if (rngvalue < 0.90) state = Animation.Sleep;
                    else if (rngvalue < 0.95) state = Animation.Pose;
                    else state = Animation.Eat;
                    break;
                case Animation.Trip:
                case Animation.LostBalance:
                case Animation.Faint:
                case Animation.HitGround:
                    state = Animation.Laying;
                    break;
                case Animation.Walk:
                case Animation.Attack:
                case Animation.Attack1:
                case Animation.Attack2:
                case Animation.Attack3:
                case Animation.Double:
                case Animation.Hurt:
                case Animation.Bump:
                case Animation.Swing:
                case Animation.Hop:
                case Animation.Charge:
                case Animation.Rotate:
                case Animation.Eat:
                case Animation.Tumble:
                case Animation.TumbleBack:
                case Animation.Wake:
                case Animation.Pose:
                case Animation.Pull:
                case Animation.Pain:
                case Animation.Float:
                case Animation.DeepBreath:
                case Animation.Nod:
                case Animation.Sit:
                case Animation.LookUp:
                case Animation.Sink:
                case Animation.LeapForth:
                case Animation.Head:
                case Animation.Cringe:
                    state = Animation.Idle;
                    break;
            }
            state = Texture.Normalise(state, out _);
            if (Texture.EndBehaviour(state) == AnimEvent.Reset)
            {
                cycle = rng.Next(5, 20);
            }
        }

        protected void turn(double p)
        {
            p /= 0.7;
            double rngvalue = rng.NextDouble();
            int dir = (int)subState;
            if (rngvalue < 0.2 * p) dir++;
            else if (rngvalue < 0.4 * p) dir--;
            else if (rngvalue < 0.5 * p) dir += 2;
            else if (rngvalue < 0.6 * p) dir -= 2;
            else if (rngvalue < 0.7 * p) dir += 4;
            dir = (dir + 8) % 8;
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
