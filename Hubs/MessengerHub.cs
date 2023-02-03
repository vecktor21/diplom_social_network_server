using Microsoft.AspNetCore.SignalR;
using server.Models;
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
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                try
                {
                    Message newMessage = await _messageService.AddMessage(message);
                    if (newMessage != null)
                    {
                        MessageViewModel messageViewModel = new MessageViewModel(newMessage);
                        await Clients.Groups(groupName).SendAsync("Receive", messageViewModel);
                        //await Clients.All.SendAsync("Test", messageViewModel);
                    }
                }
                catch (Exception ex)
                {
                    await Clients.Groups(groupName).SendAsync("Error", ex.Message);
                }
            }
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Notify",$"{Context.ConnectionId}: connected");
        }
    }
}
