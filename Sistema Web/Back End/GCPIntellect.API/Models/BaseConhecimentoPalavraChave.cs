// ARQUIVO: Models/BaseConhecimentoPalavraChave.cs (Antigo BaseConhecimento_PalavraChave.cs)

using System.ComponentModel.DataAnnotations.Schema;

namespace GCPIntellect.API.Models
{
    public class BaseConhecimentoPalavraChave
    {
        [ForeignKey("BaseConhecimento")]
        public int IdBaseConhecimento { get; set; }

        [ForeignKey("PalavraChave")]
        public int IdPalavraChave { get; set; }
        
        public virtual BaseConhecimento? BaseConhecimento { get; set; }
        public virtual PalavraChave? PalavraChave { get; set; }
    }
}