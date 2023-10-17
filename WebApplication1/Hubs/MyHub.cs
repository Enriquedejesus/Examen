using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;



namespace WebApplication1.Hubs
{
    public class MyHub : Hub
    {

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("UsuarioConectado", Context.ConnectionId);
            await base.OnConnectedAsync();

        }
        public override async Task OnDisconnectedAsync(Exception? e)
        {
            await Clients.All.SendAsync("UsuarioDesconectado", Context.ConnectionId);
            await base.OnDisconnectedAsync(e);
        }

 

     



    }
}