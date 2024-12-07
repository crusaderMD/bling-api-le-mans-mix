using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "pag")]
    public class Pag
    {

        [XmlElement(ElementName = "detPag")]
        public DetPag? DetPag { get; set; }
    }
}
