using Google.Api.Gax.Grpc;
using Google.Apis.Auth.OAuth2; // Para GoogleCredential
using Google.Cloud.AIPlatform.V1;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using System;
using System.IO; // Necessário para Path e File
using System.Threading.Tasks;

namespace GCPIntellect.API.Services
{
    public class ServicoGemini
    {
        private readonly string _projectId;
        private readonly string _location;
        private readonly PredictionServiceClient _cliente;

        public ServicoGemini(IConfiguration configuracao)
        {
            _projectId = configuracao["GoogleCloudProjectId"] ?? throw new ArgumentNullException("GoogleCloudProjectId não encontrado.");
            _location = configuracao["GoogleCloudProjectLocation"] ?? "us-central1";

            string credentialFileName = "gen-lang-client-0785267136-2869ec974d08.json";
            string credentialPath = Path.Combine(AppContext.BaseDirectory, credentialFileName);

            if (!File.Exists(credentialPath))
            {
                throw new FileNotFoundException($"Arquivo de credencial não encontrado. Verifique se '{credentialFileName}' está na pasta raiz do projeto e se o .csproj está configurado para 'CopyToOutputDirectory'.");
            }

            // Usando CredentialFactory para criar a credencial de forma segura
            var credentialJson = File.ReadAllText(credentialPath);
            var credential = Google.Apis.Auth.OAuth2.CredentialFactory.FromJson<ServiceAccountCredential>(credentialJson).ToGoogleCredential();
            
            var clientBuilder = new PredictionServiceClientBuilder
            {
                Endpoint = $"{_location}-aiplatform.googleapis.com",
                Credential = credential
            };
            _cliente = clientBuilder.Build();
        }

        public async Task<string> GerarSolucaoAsync(string problemaDoUsuario)
        {
            try
            {
                var endpointName = EndpointName.FromProjectLocationPublisherModel(_projectId, _location, "google", "gemini-1.5-flash-001");
                var prompt = $"Aja como um especialista de suporte técnico de TI para uma empresa interna. Um usuário está com o seguinte problema: '{problemaDoUsuario}'. Forneça uma solução clara e objetiva em formato de passo a passo. Se não souber a resposta, sugira que o usuário abra um chamado formal. Não adicione saudações ou despedidas, vá direto para a solução.";

                var request = new PredictRequest
                {
                    EndpointAsEndpointName = endpointName,
                    Instances =
                    {
                        new Google.Protobuf.WellKnownTypes.Value
                        {
                            StructValue = new Google.Protobuf.WellKnownTypes.Struct
                            {
                                Fields = { { "prompt", Google.Protobuf.WellKnownTypes.Value.ForString(prompt) } }
                            }
                        }
                    }
                };

                var resposta = await _cliente.PredictAsync(request);
                var previsao = resposta.Predictions[0];
                var conteudo = previsao.StructValue.Fields["content"].StringValue;

                return conteudo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao chamar a API Vertex AI (Gemini): {ex.Message}");
                return "Ocorreu um erro ao contatar o assistente de IA. Por favor, abra um chamado.";
            }
        }
    }
}