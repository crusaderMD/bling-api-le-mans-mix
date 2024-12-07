using System.Xml.Serialization;

namespace BlingApiDailyConsult.Entities.XMLEntities
{

    [XmlRoot(ElementName = "ide")]
    public class Ide
    {

        [XmlElement(ElementName = "cUF")]
        public int CUF { get; set; }

        [XmlElement(ElementName = "cNF")]
        public int CNF { get; set; }

        [XmlElement(ElementName = "natOp")]
        public string? NatOp { get; set; }

        [XmlElement(ElementName = "mod")]
        public int Mod { get; set; }

        [XmlElement(ElementName = "serie")]
        public int Serie { get; set; }

        [XmlElement(ElementName = "nNF")]
        public int NNF { get; set; }

        [XmlElement(ElementName = "dhEmi")]
        public DateTime DhEmi { get; set; }

        [XmlElement(ElementName = "dhSaiEnt")]
        public DateTime DhSaiEnt { get; set; }

        [XmlElement(ElementName = "tpNF")]
        public int TpNF { get; set; }

        [XmlElement(ElementName = "idDest")]
        public int IdDest { get; set; }

        [XmlElement(ElementName = "cMunFG")]
        public int CMunFG { get; set; }

        [XmlElement(ElementName = "tpImp")]
        public int TpImp { get; set; }

        [XmlElement(ElementName = "tpEmis")]
        public int TpEmis { get; set; }

        [XmlElement(ElementName = "cDV")]
        public int CDV { get; set; }

        [XmlElement(ElementName = "tpAmb")]
        public int TpAmb { get; set; }

        [XmlElement(ElementName = "finNFe")]
        public int FinNFe { get; set; }

        [XmlElement(ElementName = "indFinal")]
        public int IndFinal { get; set; }

        [XmlElement(ElementName = "indPres")]
        public int IndPres { get; set; }

        [XmlElement(ElementName = "indIntermed")]
        public int IndIntermed { get; set; }

        [XmlElement(ElementName = "procEmi")]
        public int ProcEmi { get; set; }

        [XmlElement(ElementName = "verProc")]
        public string? VerProc { get; set; }
    }
}