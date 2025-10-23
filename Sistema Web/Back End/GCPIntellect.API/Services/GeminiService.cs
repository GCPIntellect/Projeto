using Google.Cloud.AIPlatform.V1;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace GCPIntellect.API.Services
{
    public class GeminiService
    {
        private readonly string _projectId;
        private readonly string _location;
        private readonly PredictionServiceClient _client;

        public GeminiService(IConfiguration configuration)
        {
            _projectId = configuration["GoogleCloudProjectId"] ?? throw new ArgumentNullException("GoogleCloudProjectId não encontrado.");
            _location = configuration["GoogleCloudProjectLocation"] ?? "us-central1";

            var clientBuilder = new PredictionServiceClientBuilder
            {
                Endpoint = $"{_location}-aiplatform.googleapis.com"
            };
            _client = clientBuilder.Build();
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
                        new Value
                        {
                            StructValue = new Struct
                            {
                                Fields = { { "prompt", Value.ForString(prompt) } }
                            }
                        }
                    }
                };

                var response = await _client.PredictAsync(request);
                var prediction = response.Predictions[0];
                var content = prediction.StructValue.Fields["content"].StringValue;

                return content;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao chamar a API Vertex AI (Gemini): {ex.Message}");
                return "Ocorreu um erro ao contatar o assistente de IA. Por favor, abra um chamado.";
            }
        }
    }
}