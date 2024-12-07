using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "infNFe")]
    public class InfNFe
    {

        [XmlElement(ElementName = "ide")]
        public Ide Ide { get; set; }

        [XmlElement(ElementName = "emit")]
        public Emit? Emit { get; set; }

        [XmlElement(ElementName = "dest")]
        public Dest? Dest { get; set; }

        [XmlElement(ElementName = "det")]
        public List<Det>? Det { get; set; }

        [XmlElement(ElementName = "total")]
        public Total? Total { get; set; }

        [XmlElement(ElementName = "transp")]
        public Transp? Transp { get; set; }

        [XmlElement(ElementName = "cobr")]
        public Cobr? Cobr { get; set; }

        [XmlElement(ElementName = "pag")]
        public Pag? Pag { get; set; }

        [XmlElement(ElementName = "infAdic")]
        public InfAdic? InfAdic { get; set; }

        [XmlAttribute(AttributeName = "Id")]
        public string? Id { get; set; }

        [XmlAttribute(AttributeName = "versao")]
        public double Versao { get; set; }

        [XmlText]
        public string? Text { get; set; }
    }
}
