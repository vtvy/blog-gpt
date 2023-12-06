using BlogGPT.Application.Common.Interfaces.Services;
using LLama;
using LLama.Common;
using System.Security.Cryptography;

namespace BlogGPT.Infrastructure.Services
{
    public class Chatbot : IChatbot
    {

        public Chatbot()
        {
        }

        public IList<float[]> GetEmbeddings(IList<string> texts)
        {

            var embeddingList = new List<string> {
                @"D:\code\model\beta\zephyr-q3-k_m.gguf",
                @"D:\Downloads\zephyr-7b-beta.Q4_K_M.gguf",
                @"D:\code\model\beta\zephyr-q5-k-m.gguf",
                @"D:\code\model\beta\zephyr-q4-k-m.gguf",
            };

            var embeddingModelParams = new ModelParams(embeddingList[0])
            {
                GpuLayerCount = 35,
                EmbeddingMode = true,
                ContextSize = 8192,
            };

            using var embeddingWeights = LLamaWeights.LoadFromFile(embeddingModelParams);
            var embedder = new LLamaEmbedder(embeddingWeights, embeddingModelParams);

            var embeddings = texts.Select(text => embedder.GetEmbeddings(text)).ToList();
            embedder.Dispose();
            embeddingWeights.Dispose();

            return embeddings;
        }

        public List<List<float[]>> GetEmbeddingsList(List<List<string>> textsList)
        {
            var embeddingList = new List<string> {
                @"D:\code\model\beta\zephyr-q3-k_m.gguf",
                @"D:\Downloads\zephyr-7b-beta.Q4_K_M.gguf",
                @"D:\code\model\beta\zephyr-q5-k-m.gguf",
                @"D:\code\model\beta\zephyr-q4-k-m.gguf",
            };

            var embeddingModelParams = new ModelParams(embeddingList[0])
            {
                GpuLayerCount = 35,
                EmbeddingMode = true,
                ContextSize = 8192,
            };

            using var embeddingWeights = LLamaWeights.LoadFromFile(embeddingModelParams);
            var embedder = new LLamaEmbedder(embeddingWeights, embeddingModelParams);

            var embeddingsList = textsList.Select(texts => texts.Select(text => embedder.GetEmbeddings(text)).ToList()).ToList();
            embedder.Dispose();
            embeddingWeights.Dispose();

            return embeddingsList;
        }


        public async IAsyncEnumerable<string> GetAnswerAsync(string question, string chatContext)
        {
            var modelList = new List<string> {
                @"D:\code\model\beta\zephyr-q3-k_m.gguf",
                @"D:\Downloads\zephyr-7b-beta.Q4_K_M.gguf",
                @"D:\code\model\beta\zephyr-q5-k-m.gguf",
                @"D:\code\model\beta\zephyr-q4-k_m.gguf",
            };

            var modelParams = new ModelParams(modelList[0])
            {
                ContextSize = 8192,
                Seed = unchecked((uint)RandomNumberGenerator.GetInt32(int.MaxValue)),
                GpuLayerCount = 35,
            };
            using var modelWeights = LLamaWeights.LoadFromFile(modelParams);
            var modelContext = new LLamaContext(modelWeights, modelParams);
            var modelExecutor = new InstructExecutor(modelContext);

            //StringBuilder prompt = new("Transcript of a dialog, where the User interacts with an Assistant. Assistant is helpful, kind, honest, good at writing, and never fails to answer the User's requests immediately and with precision.");
            var prompt =
"""
<|system|>
Some articles:
{{$facts}}
======
Given only some articles above, provide a comprehensive and detailed answer.
You don't know where the knowledge comes from, just answer.
If you don't have sufficient information, reply with 'I cannot find relevance information about this question'.</s>
<|user|>
{{$input}}</s>
<|assistant|>

""";

            var promptWithInfor = prompt.Replace("{{$facts}}", chatContext);
            if (question[question.Length - 1] != '?') question += '?';
            var promptWithQuestion = prompt.Replace("{{$input}}", question);

            var inferenceParams = new InferenceParams() { Temperature = 0.6f, AntiPrompts = new string[] { "<|user|>" }, MaxTokens = 1000 };
            Console.WriteLine(modelList[0]);
            await foreach (var output in modelExecutor.InferAsync(promptWithQuestion, inferenceParams))
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