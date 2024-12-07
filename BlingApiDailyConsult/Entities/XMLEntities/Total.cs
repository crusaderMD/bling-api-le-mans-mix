using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "total")]
    public class Total
    {

        [XmlElement(ElementName = "ICMSTot")]
        public ICMSTot? ICMSTot { get; set; }
    }
}
