namespace BlogGPT.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string name, object key)
            : base($"The \"{name}\" with the identifier {key} was not found.")
        {
        }
    }
}
