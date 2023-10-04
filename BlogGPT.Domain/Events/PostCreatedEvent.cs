namespace BlogGPT.Domain.Events
{
    public class PostCreatedEvent : BaseEvent
    {
        public Post Post { get; }

        public PostCreatedEvent(Post post)
        {
            Post = post;
        }
    }
}
