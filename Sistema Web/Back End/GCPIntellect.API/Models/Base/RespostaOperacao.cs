namespace GCPIntellect.API.Models.Base
{
    public class RespostaOperacao<T>
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; } = string.Empty;
        public T? Dados { get; set; }
        public List<string> Detalhes { get; set; } = new();

        public static RespostaOperacao<T> CriarSucesso(string mensagem, T? dados = default)
        {
            return new()
            {
                Sucesso = true,
                Mensagem = mensagem,
                Dados = dados
            };
        }

        public static RespostaOperacao<T> CriarErro(string mensagem)
        {
            return new()
            {
                Sucesso = false,
                Mensagem = mensagem
            };
        }

        public RespostaOperacao<T> AdicionarDetalhe(string detalhe)
        {
            Detalhes.Add(detalhe);
            return this;
        }
    }
}