using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SpriteWander.textures
{
    [XmlRoot("AnimData")]
    public class MultiTexture : Texture
    {
        [XmlElement("ShadowSize")]
        public int ShadowSize { get; set; }

        [XmlArray("Anims")]
        [XmlArrayItem("Anim")]
        public List<Anim> Anims { get; set; }

        private Dictionary<Animation, Anim> ActiveAnims = new Dictionary<Animation, Anim>();

        public override Rectangle Getcoord(Animation anim, Direction Dir, int Time, out int Width, out int Height, out AnimEvent reset)
        {
            Animation fixAnim = Normalise(anim, out _);
            Anim animData = ActiveAnims[fixAnim];
            Width = animData.fullWidth.Value;
            Height = animData.fullHeight.Value;
            int temp_time = 0;
            int x = 0;
            bool found = false;
            foreach (int frame in animData.Durations)
            {
                if (Time <= frame + temp_time) 
                { 
                    found = true;
                    break; 
                }
                temp_time += frame;
                x++;
            }
            if (!found || (x == animData.Durations.Count - 1 && Time == temp_time + animData.Durations.Last()))
            {
                reset = EndBehaviour(fixAnim);
            }
            else
            {
                reset = AnimEvent.Nothing;
            }
            return new Rectangle(animData.Width * x, (int)Dir * animData.Height, animData.Width, animData.Height);
        }

        public override Animation Normalise(Animation animation, out AnimEvent reset)
        {
            if (ActiveAnims.Count == 0) Load();
            if (ActiveAnims.Count == 0) throw new Exception("No animation in an entity");
            if (ActiveAnims.ContainsKey(animation))
            {
                reset = AnimEvent.Nothing;
                return animation;
            }
            reset = AnimEvent.Reset;
            switch (animation)
            {
                case Animation.Attack1:
                case Animation.Attack2:
                case Animation.Attack3:
                    return Normalise(Animation.Attack, out _);
                case Animation.Eventsleep:
                case Animation.Laying:
                    return Normalise(Animation.Sleep, out _);
                case Animation.Bump:
                    if (ActiveAnims.ContainsKey(Animation.TumbleBack)) return Animation.TumbleBack;
                    if (ActiveAnims.ContainsKey(Animation.HitGround)) return Animation.HitGround;
                    if (ActiveAnims.ContainsKey(Animation.Cringe)) return Animation.Cringe;
                    return Normalise(Animation.Idle, out _);
                default:
                    if (ActiveAnims.ContainsKey(Animation.Idle)) return Animation.Idle;
                    return ActiveAnims.Keys.First();
            }
        }

        public override void Use(Animation n)
        {
            Animation fixAnim = Normalise(n, out _);
            if (ActiveAnims[fixAnim].Handle == null) { throw new Exception("Normalise can returned uninitialised anim"); }
            Use(ActiveAnims[fixAnim].Handle.Value, OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);
        }

        public override void Print()
        {
            Console.WriteLine($"{this.name} : \n" +
                              $" path : {this.path}\n" +
                              $" ready : {this.ready}\n" +
                              $" ShadowSize : {this.ShadowSize}\n" +
                              $" Anims :\n");
            foreach (Anim anim in Anims)
            {
                anim.Print();
            }
        }

        public override void Load()
        {
            if (path  == null)
            {
                throw new InvalidOperationException("no path to files");
            }
            ZipArchive archive = ZipFile.OpenRead(path);
            foreach (Anim anim in Anims)
            {
                ZipArchiveEntry? img = archive.GetEntry(anim.Name + "-Anim.png");
                if (img != null)
                {
                    Stream stream = img.Open();
                    anim.Handle = GenerateHandle();
                    LoadFile(stream, anim.Handle.Value, OpenTK.Graphics.OpenGL4.TextureUnit.Texture0, out int fullWidth, out int fullHeight);
                    anim.fullHeight = fullHeight;
                    anim.fullWidth = fullWidth;
                    ActiveAnims.Add((Animation)anim.Index, anim);
                }
            }
            ready = true;
        }

        public override void Dispose()
        {
            foreach (var item in Anims)
            {
                if (item.Handle != null)
                {
                    Dispose(item.Handle.Value);
                    item.Handle = null;
                }
            }
            ready = false;
        }
    }

    public class Anim
    {
        public int? Handle;

        public int? fullWidth;
        public int? fullHeight;

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Index")]
        public int Index { get; set; }

        [XmlElement("FrameWidth")]
        public int Width { get; set; }

        [XmlElement("FrameHeight")]
        public int Height { get; set; }

        [XmlArray("Durations")]
        [XmlArrayItem("Duration")]
        public List<int> Durations { get; set; }

        [XmlElement("RushFram",IsNullable = true)]
        public int? RushFrame { get; set; }

        [XmlElement("HitFrame", IsNullable = true)]
        public int? HitFrame { get; set; }

        [XmlElement("ReturnFrame", IsNullable = true)]
        public int? ReturnFrame { get; set; }

        public void Print()
        {
            Console.WriteLine($" name : {this.Name}\n" +
                              $"  index : {this.Index}\n" +
                              $"  width : {this.Width} \n" +
                              $"  height : {this.Height} \n" +
                              $"  RushFrame : {this.RushFrame} \n" +
                              $"  HitFrame : {this.HitFrame} \n" +
                              $"  ReturnFrame : {this.ReturnFrame} \n" +
                              $"  durations : {this.Durations} \n");
        }
    }
}
