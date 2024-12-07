using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "Transform")]
    public class Transform
    {

        [XmlAttribute(AttributeName = "Algorithm")]
        public string? Algorithm { get; set; }
    }
}
