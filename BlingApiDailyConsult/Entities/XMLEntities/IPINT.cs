using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "IPINT")]
    public class IPINT
    {

        [XmlElement(ElementName = "CST")]
        public int CST { get; set; }
    }
}
