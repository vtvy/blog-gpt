namespace BlogGPT.Domain.Events
{
    public class PostDeletedEvent : BaseEvent
    {
        public Post Post { get; }

        public PostDeletedEvent(Post post)
        {
            Post = post;
        }
    }
}
