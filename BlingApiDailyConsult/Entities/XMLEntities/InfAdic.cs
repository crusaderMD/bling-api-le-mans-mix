using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "infAdic")]
    public class InfAdic
    {

        [XmlElement(ElementName = "infCpl")]
        public string? InfCpl { get; set; }
    }
}
