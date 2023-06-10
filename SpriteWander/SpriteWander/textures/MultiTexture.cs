using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SpriteWander.textures
{
    [XmlRoot("AnimData")]
    public class MultiTexture
    {
        [XmlElement("ShadowSize")]
        public int ShadowSize { get; set; }

        [XmlArray("Anims")]
        [XmlArrayItem("Anim")]
        public List<Anim> Anims { get; set; }

        public bool ready = false;

        public string? path;
    }

    public class Anim
    {
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
    }
}
