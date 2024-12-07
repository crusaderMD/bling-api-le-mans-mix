using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "Transforms")]
    public class Transforms
    {

        [XmlElement(ElementName = "Transform")]
        public List<Transform>? Transform { get; set; }
    }
}
