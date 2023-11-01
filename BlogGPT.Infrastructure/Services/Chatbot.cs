using BlogGPT.Application.Common.Interfaces.Services;
using LLama;
using LLama.Common;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace BlogGPT.Infrastructure.Services
{
    public class Chatbot : IChatbot
    {
        private readonly string _modelPath;
        private readonly string _embeddingModelPath;

        public Chatbot(IConfiguration configuration)
        {
            _modelPath = configuration.GetSection("Chatbot:ModelPath").Value ?? throw new Exception("Model path not found");
            _embeddingModelPath = configuration.GetSection("Chatbot:EmbeddingModelPath").Value ?? throw new Exception("Model path not found");
        }

        public float[] GetEmbedding(string text)
        {
            var embeddingModelParams = new ModelParams(_embeddingModelPath)
            {
                GpuLayerCount = 35,
                EmbeddingMode = true,
                ContextSize = 10000,
            };

            using var embeddingWeights = LLamaWeights.LoadFromFile(embeddingModelParams);
            var embedder = new LLamaEmbedder(embeddingWeights, embeddingModelParams);

            var embedding = embedder.GetEmbeddings(text);
            embedder.Dispose();
            embeddingWeights.Dispose();

            return embedding;
        }

        public async IAsyncEnumerable<string> GetAnswerAsync(string question, string chatContext)
        {
            var modelParams = new ModelParams(_modelPath)
            {
                ContextSize = 1024,
                Seed = unchecked(RandomNumberGenerator.GetInt32(int.MaxValue)),
                GpuLayerCount = 35,
            };
            using var modelWeights = LLamaWeights.LoadFromFile(modelParams);
            var modelContext = new LLamaContext(modelWeights, modelParams);
            var modelExecutor = new InstructExecutor(modelContext);

            StringBuilder prompt = new("Transcript of a dialog, where the User interacts with an Assistant. Assistant is helpful, kind, honest, good at writing, and never fails to answer the User's requests immediately and with precision.");

            if (true)
            {
                var userHistory = "Hello, Assistant.";
                var answerHistory = "Hello. How may I help you today?";
                prompt.AppendLine($"User: {userHistory}");
                prompt.AppendLine($"Assistant: {answerHistory}");
            }

            if (chatContext.Length > 0)
            {
                prompt.AppendLine("Please read the following articles then answer User based on that.");
                prompt.AppendLine($"{chatContext}");
            }

            prompt.Append($"User: {question}");

            var inferenceParams = new InferenceParams() { Temperature = 0.6f, AntiPrompts = new string[] { "User:", ">" }, MaxTokens = 800 };
            Console.WriteLine(_modelPath);
            await foreach (var output in modelExecutor.InferAsync(prompt.ToString(), inferenceParams))
            {
                if (output.Contains("User")) break;
                Console.Write(output);
                yield return output;
            }
            modelContext.Dispose();
            modelWeights.Dispose();
        }
    }
}