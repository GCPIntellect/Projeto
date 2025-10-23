using System.ComponentModel.DataAnnotations;
using System.Collections.Generic; // Necessário para ICollection

namespace GCPIntellect.API.Models
{
    public class PalavraChave
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Texto { get; set; } = string.Empty;

        // --- CORREÇÃO ADICIONADA ---
        // Lista que representa a relação com a tabela de junção
        public virtual ICollection<BaseConhecimento_PalavraChave> BaseConhecimentos { get; set; } = new List<BaseConhecimento_PalavraChave>();
    }
}