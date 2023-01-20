using Microsoft.AspNetCore.SignalR;
using server.Services;
using server.ViewModels;
namespace server.Hubs
{
    public class MessengerHub : Hub
    {
        private readonly ApplicationContext db;
        private readonly MessageService _messageService;
        public MessengerHub(ApplicationContext db, MessageService messageService)
        {
            this.db = db;
            _messageService = messageService;
        }


        public async Task Send(MessageCreateViewModel message)
        {
            string groupName = message.ChatRoomId.ToString();
            if (groupName != null)
            {
                await _messageService.AddMessage(message);
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                await Clients.Groups(groupName).SendAsync("Receive", "test");
            }
            
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Notify",$"{Context.ConnectionId}: connected");
        }
    }
}
