using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using bsmAnalysis.Models;
using Microsoft.AspNet.SignalR;


namespace bsmAnalysis
{
    public class Notifiy:Hub
    {
        public static Dictionary<string, UserProfile> onlineUsers = new Dictionary<string, UserProfile>();
        public void Send(string chatText, int chatRecieverId)
        {
            // Call the broadcastMessage method to update clients.
            //var context = GlobalHost.ConnectionManager.GetHubContext<Notifiy>();
            //context.Clients.All.send(obj);

            UsersContext db = new UsersContext();
            var user = db.UserProfiles.Find(chatRecieverId); 
           string RecieverConnectionId= onlineUsers.FirstOrDefault(f => f.Value.UserId == chatRecieverId).Key;
           var chat = Message.sendMessage(chatText, chatRecieverId);
            Clients.Client(RecieverConnectionId).recieveChat(chat);
            Clients.Caller.recieveChat(chat);
         
            
        }

        public override Task OnConnected()
        {
           
            UsersContext db = new UsersContext();
            var user = db.UserProfiles.Find(Membership.GetUser().ProviderUserKey);
            if(!onlineUsers.ContainsValue(user)){
            onlineUsers.Add(Context.ConnectionId,user);
            Clients.All.newOnlineFriend(new { UserId=user.UserId,UserName=user.UserName,ProfilePic=user.profilepic,connectionId=Context.ConnectionId});}
            return base.OnConnected();
        }
       
        public override Task OnDisconnected()
        {
           
           // string name = Context.QueryString["name"];
         //   UsersContext db = new UsersContext();
            //var user = db.UserProfiles.Find(Membership.GetUser().ProviderUserKey);
            onlineUsers.Remove(Context.ConnectionId);
            Clients.All.disconnectedFriend(Context.ConnectionId);
            return base.OnDisconnected();
        }
    }
}