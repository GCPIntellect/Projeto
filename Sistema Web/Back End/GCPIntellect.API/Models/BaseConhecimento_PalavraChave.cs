namespace GCPIntellect.API.Models
{
    // Tabela de Junção.
    public class BaseConhecimento_PalavraChave
    {
        public int IdBaseConhecimento { get; set; }
        public int IdPalavraChave { get; set; }

        // Propriedades de Navegação
        public virtual BaseConhecimento? BaseConhecimento { get; set; }
        public virtual PalavraChave? PalavraChave { get; set; }
    }
}