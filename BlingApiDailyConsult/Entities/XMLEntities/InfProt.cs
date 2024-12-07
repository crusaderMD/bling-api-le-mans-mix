using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{
    [XmlRoot(ElementName = "infProt")]
    public class InfProt
    {

        [XmlElement(ElementName = "tpAmb")]
        public int TpAmb { get; set; }

        [XmlElement(ElementName = "verAplic")]
        public string? VerAplic { get; set; }

        [XmlElement(ElementName = "chNFe")]
        public double ChNFe { get; set; }

        [XmlElement(ElementName = "dhRecbto")]
        public DateTime DhRecbto { get; set; }

        [XmlElement(ElementName = "nProt")]
        public double NProt { get; set; }

        [XmlElement(ElementName = "digVal")]
        public string? DigVal { get; set; }

        [XmlElement(ElementName = "cStat")]
        public int CStat { get; set; }

        [XmlElement(ElementName = "xMotivo")]
        public string? XMotivo { get; set; }
    }
}
