using System;
using Microsoft.AspNetCore.SignalR;                                         
using WebShop.Main.Context;

namespace WebShop.Main.Hubs
{
    public class ChatHub:Hub                                              
    {
        public Task SendMessage1(string productId, string message)         
        {
            return Clients.All.SendAsync("ReceiveOne", productId, message); 
        }
    }
}