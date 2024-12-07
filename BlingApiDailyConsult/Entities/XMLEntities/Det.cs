using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "det")]
    public class Det
    {

        [XmlElement(ElementName = "prod")]
        public Prod? Prod { get; set; }

        [XmlElement(ElementName = "imposto")]
        public Imposto? Imposto { get; set; }

        [XmlElement(ElementName = "infAdProd")]
        public string? InfAdProd { get; set; }

        [XmlAttribute(AttributeName = "nItem")]
        public int NItem { get; set; }

        [XmlText]
        public string? Text { get; set; }
    }

}
