// ARQUIVO: ViewModels/ChamadoModels.cs (NOVO ARQUIVO)

using System.ComponentModel.DataAnnotations;

namespace GCPIntellect.API.ViewModels
{
    // --- Modelos de ENTRADA (DTOs) ---

    public class ChamadoCriacaoModel
    {
        [Required] public string Titulo { get; set; } = string.Empty;
        [Required] public string Descricao { get; set; } = string.Empty;
        [Required] public int IdTipo { get; set; }
        [Required] public int IdCategoria { get; set; }
        public int? IdPrioridade { get; set; }
        public int? IdStatus { get; set; }
    }

    public class ChamadoAtualizacaoModel
    {
        [Required] public string Titulo { get; set; } = string.Empty;
        [Required] public string Descricao { get; set; } = string.Empty;
        [Required] public int IdTipo { get; set; }
        [Required] public int IdCategoria { get; set; }
        public int? IdPrioridade { get; set; }
        public int? IdStatus { get; set; }
    }

    public class MensagemCriacaoModel
    {
        [Required]
        public string Conteudo { get; set; } = string.Empty;
    }

    // --- Modelos de SAÍDA (ViewModels) ---

    public class UsuarioVisaoModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
    }

    public class TabelaAuxiliarVisaoModel // Para Status, Categoria, Tipo, Prioridade
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
    }

    public class AnexoVisaoModel
    {
        public int Id { get; set; }
        public string NomeArquivo { get; set; } = string.Empty;
        public string CaminhoArquivo { get; set; } = string.Empty;
        public long TamanhoBytes { get; set; }
    }

    public class MensagemVisaoModel
    {
        public int Id { get; set; }
        public string Conteudo { get; set; } = string.Empty;
        public DateTime DataEnvio { get; set; }
        public UsuarioVisaoModel? Autor { get; set; }
    }

    // O ViewModel principal para a página de detalhes
    public class ChamadoDetalheVisaoModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataAbertura { get; set; }
        public int IdUsuario { get; set; }

        public UsuarioVisaoModel? Usuario { get; set; }
        public TabelaAuxiliarVisaoModel? StatusChamado { get; set; }
        public TabelaAuxiliarVisaoModel? Categoria { get; set; }
        public TabelaAuxiliarVisaoModel? Tipo { get; set; }
        public TabelaAuxiliarVisaoModel? Prioridade { get; set; }

        public ICollection<AnexoVisaoModel> Anexos { get; set; } = new List<AnexoVisaoModel>();
        public ICollection<MensagemVisaoModel> Mensagens { get; set; } = new List<MensagemVisaoModel>();
        public ICollection<UsuarioVisaoModel> TecnicosAtribuidos { get; set; } = new List<UsuarioVisaoModel>();

        public int IdStatus { get; set; }
        public int IdCategoria { get; set; }
        public int IdTipo { get; set; }
        public int IdPrioridade { get; set; }
    }
}