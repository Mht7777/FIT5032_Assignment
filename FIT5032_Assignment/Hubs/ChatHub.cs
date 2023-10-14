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

namespace FIT5032_Assignment.Hubs
{
    public class ChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, string> ConnectedUsers = new ConcurrentDictionary<string, string>();

        public void Send(string name, string message)
        {
            Clients.All.addNewMessageToPage(name, message);
        }

        public async Task GetChatGptResponse(string userMessage)
        {
            var authentication = new APIAuthentication("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");
            var api = new OpenAIAPI(authentication);

            var result = await api.Chat.CreateChatCompletionAsync(new ChatRequest()
            {
                Model = Model.ChatGPTTurbo,
                Temperature = 0.5,
                MaxTokens = 250,
                Messages = new ChatMessage[] {
              new ChatMessage(ChatMessageRole.User, userMessage)
            }
            });
            var reply = result.Choices[0].Message;
            Console.WriteLine($"{reply.Role}: {reply.Content.Trim()}");


            Clients.All.addNewMessageToPage("ChatGPT", reply.Content.Trim());

        }






    }

}