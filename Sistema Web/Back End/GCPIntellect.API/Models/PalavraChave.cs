using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace GCPIntellect.API.Models
{
    public class PalavraChave
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Texto { get; set; } = string.Empty;

        // Propriedade de navegação para a tabela de junção
        public virtual ICollection<BaseConhecimentoPalavraChave> BaseConhecimentos { get; set; } = new List<BaseConhecimentoPalavraChave>();
    }
}