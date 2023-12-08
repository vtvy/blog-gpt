using BlogGPT.Application.Common.Interfaces.Services;
using LLama;
using LLama.Common;
using Python.Runtime;
using System.Security.Cryptography;
using System.Text.Json;

namespace BlogGPT.Infrastructure.Services
{
    public class Chatbot : IChatbot
    {

        public Chatbot()
        {
        }

        public List<float[]> GetEmbeddings(List<string> texts)
        {
            List<float[]> embeddings;

            PythonEngine.Initialize();
            using (Py.GIL())
            {
                PythonEngine.Initialize();
                dynamic _module = Py.Import("sentence_transformers");
                dynamic np = Py.Import("numpy");
                dynamic model = _module.SentenceTransformer("sentence-transformers/all-MiniLM-L6-v2");
                dynamic arr = np.array(texts);
                var em = model.encode(arr).tolist().ToString();
                embeddings = JsonSerializer.Deserialize<List<float[]>>(em);
            }
            return embeddings;
        }

        public List<List<float[]>> GetEmbeddingsList(List<List<string>> textsList)
        {
            using (Py.GIL())
            {
                dynamic _module = Py.Import("sentence_transformers");
                dynamic np = Py.Import("numpy");
                dynamic model = _module.SentenceTransformer("sentence-transformers/all-MiniLM-L6-v2");

                var embeddingsList = textsList.Select(texts =>
                {
                    dynamic arr = np.array(texts);
                    var em = model.encode(arr).tolist().ToString();
                    List<float[]> embeddings = JsonSerializer.Deserialize<List<float[]>>(em);
                    return embeddings;
                }).ToList();

                return embeddingsList;
            }
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