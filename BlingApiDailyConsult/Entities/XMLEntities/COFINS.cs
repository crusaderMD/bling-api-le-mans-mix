using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "COFINS")]
    public class COFINS
    {

        [XmlElement(ElementName = "COFINSOutr")]
        public COFINSOutr? COFINSOutr { get; set; }
    }
}
