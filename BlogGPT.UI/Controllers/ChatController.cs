using BlogGPT.Application.Chats;
using Microsoft.AspNetCore.Mvc;

namespace BlogGPT.UI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IChatService _chatService;

        public ChatController(IMapper mapper, IChatService chatService)
        {
            _mapper = mapper;
            _chatService = chatService;
        }

        [HttpPost("send")]
        public async Task SendStreamAsync(ChatRequest request, CancellationToken cancellationToken)
        {
            Response.ContentType = "text/event-stream";

            await foreach (var output in _chatService.SendStreamingAsync(request))
            {
                await Response.WriteAsync(output, cancellationToken);
                await Response.Body.FlushAsync(cancellationToken);
            };

            await Response.CompleteAsync();
        }
    }

}