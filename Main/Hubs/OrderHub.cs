using System;
using System.CodeDom;
using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using WebShop.Main.Conext;
using WebShop.Main.Context;
using WebShop.Models;

namespace WebShop.Main.Hubs
{
	public class OrderHub:Hub
    {
        public async Task MakeOrder(Order orderModel)
        {
            await Clients.All.SendAsync("MakeOrder", orderModel);
        }
    }
}

