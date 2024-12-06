using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlingApiDailyConsult.Entities.Enums
{
    public enum SituacaoNotaFiscal : int
    {
        Pendente = 1,
        Cancelada = 2,
        AguardandoRecibo = 3,
        Rejeitada = 4,
        Autorizada = 5,
        EmitidaDANFE = 6,
        Registrada = 7,
        AguardandoProtocolo = 8,
        Denegada = 9,
        ConsultaSituação = 10,
        Bloqueada = 11
    }
}
