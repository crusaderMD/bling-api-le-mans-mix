using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "cobr")]
    public class Cobr
    {

        [XmlElement(ElementName = "fat")]
        public Fat? Fat { get; set; }

        [XmlElement(ElementName = "dup")]
        public Dup? Dup { get; set; }
    }
}
