using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using OpenAI_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Collections.Concurrent;
using FIT5032_Assignment.Models.Entites;

namespace FIT5032_Assignment.Hubs
{
    public class ChatHub : Hub
    {

        public void Send(string name, string message)
        {
            Clients.All.addNewMessageToPage(name, message);
        }




    }

}