namespace BlogGPT.Application.Common.Interfaces.Services
{
	public interface IChatbot
	{
		IList<float[]> GetEmbeddings(IList<string> texts);
		List<List<float[]>> GetEmbeddingsList(List<List<string>> textsList);
		IAsyncEnumerable<string> GetAnswerAsync(string question, string chatContext);
	}
}