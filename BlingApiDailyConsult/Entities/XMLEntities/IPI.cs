using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "IPI")]
    public class IPI
    {

        [XmlElement(ElementName = "cEnq")]
        public int CEnq { get; set; }

        [XmlElement(ElementName = "IPINT")]
        public IPINT? IPINT { get; set; }
    }
}
