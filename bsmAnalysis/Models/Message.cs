using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;


namespace bsmAnalysis.Models
{
    public class Message:Text
    {
        public int MessageId { get; set; }


        
        public virtual UserProfile writer { get; set; }
       
        public virtual UserProfile sender { get; set; }

        public int? GroupId { get; set; }
        public virtual ICollection<UserProfile> usersShared { get; set; }//*to*
        public virtual ICollection<UserProfile> usersLiked { get; set; }
        public virtual Group messageGroup { get; set; }
       
        public virtual ICollection<Comment> Comments { get; set; }//1..*
       
      
       
       

        public virtual ICollection<Picture> MessagePicture { get; set; }

       
        //methods
        public static object addMessage(string body = "" )
        {
            UsersContext db = new UsersContext();
            UserProfile user = db.UserProfiles.Find(System.Web.Security.Membership.GetUser().ProviderUserKey);

            var g = new Message { writer = user, body = Message.linkyPictureInBox(body), timeAdded = DateTime.Now };
            db.Messages.Add(g);
            db.SaveChanges();
            var myMessage = new { MessageId = g.MessageId, body = g.body, timeAdded = g.timeAdded.ToString("g"), writer = new { UserId = g.writer.UserId, UserName = g.writer.UserName, profilePic = g.writer.profilepic }, usersLiked = g.usersLiked, usersShared = g.usersShared, Comments = g.Comments};

            //var context = GlobalHost.ConnectionManager.GetHubContext("Notifiy"); //GetHubContext<Notifiy>();
            //context.Clients.All().addMessage(myMessage);
           
           
            return myMessage;
        }
        public static object addMessageInGroup(int groupId, string body = "")
        {
            UsersContext db = new UsersContext();
            UserProfile user = db.UserProfiles.Find(System.Web.Security.Membership.GetUser().ProviderUserKey);
            
            var group = db.Groups.Find(groupId);
            var g = new Message { writer = user, body = Message.linkyPictureInBox(body), timeAdded = DateTime.Now, messageGroup = group };
            object myMessage = new { };
            if (group.members.Contains(user)||group.Admin==user)
            {
                db.Messages.Add(g);
                db.SaveChanges();
                myMessage = new { MessageId = g.MessageId, body = g.body, timeAdded = g.timeAdded.ToString("g"), writer = new { UserId = g.writer.UserId, UserName = g.writer.UserName, profilePic = g.writer.profilepic }, usersLiked = g.usersLiked, usersShared = g.usersShared, Comments = g.Comments };
            }

            //var context = GlobalHost.ConnectionManager.GetHubContext("Notifiy"); //GetHubContext<Notifiy>();
            //context.Clients.All().addMessage(myMessage);
            
            return myMessage;
        }
        public static object deleteMessage(int id)
        {
            UsersContext db = new UsersContext();
            UserProfile user = db.UserProfiles.Find(System.Web.Security.Membership.GetUser().ProviderUserKey);

            if (db.Messages.Find(id).writer == user||db.Messages.Find(id).messageGroup.Admin==user)
            {
                db.Messages.Remove(db.Messages.Find(id));
                db.SaveChanges();
                return true;
            }
            return false;
        }
        public static object likeMessage(int id)
        {
            UsersContext db = new UsersContext();
            UserProfile user = db.UserProfiles.Find(System.Web.Security.Membership.GetUser().ProviderUserKey);
        
            if (!db.Messages.Find(id).usersLiked.Contains(user))
            {
                db.Messages.Find(id).usersLiked.Add(user);
            
            }
            else
            {
                db.Messages.Find(id).usersLiked.Remove(user);
               
            }
            db.SaveChanges();

               var ouruser =db.Messages.Find(id).usersLiked.Select(guser=> new { UserId = guser.UserId, UserName = guser.UserName, profilePic = guser.profilepic });
            return ouruser;
        }
      
        public static object shareMessage(int id)
        {
            UsersContext db = new UsersContext();
            UserProfile user = db.UserProfiles.Find(System.Web.Security.Membership.GetUser().ProviderUserKey);
            if (!db.Messages.Find(id).usersShared.Contains(user))
            {
                db.Messages.Find(id).usersShared.Add(user);

            }
            else
            {
                db.Messages.Find(id).usersShared.Remove(user);

            }          
            db.SaveChanges();
            var ouruser = db.Messages.Find(id).usersShared.Select(guser => new { UserId = guser.UserId, UserName = guser.UserName, profilePic = guser.profilepic });
            return ouruser;
        }
        public static string removeShare(int id)
        {
            UsersContext db = new UsersContext();
            UserProfile user = db.UserProfiles.Find(System.Web.Security.Membership.GetUser().ProviderUserKey);

            db.Messages.Find(id).usersShared.Remove(user);
            db.SaveChanges();
            return "";
        }
      
        public static object sendMessage(string body, int userid)
        {
            UsersContext db = new UsersContext();
            UserProfile user = db.UserProfiles.Find(System.Web.Security.Membership.GetUser().ProviderUserKey);
            var sendedmessage = new { writer = new { UserId = user.UserId, UserName = user.UserName, profilePic = user.profilepic }, body = body, timeAdded = DateTime.Now };
            user.conversations.Add(new Message { writer = user, body = body, timeAdded = DateTime.Now });
            db.UserProfiles.Find(userid).conversations.Add(new Message { writer = user, body = body, timeAdded = DateTime.Now });
            db.SaveChanges();
            return sendedmessage;
        }
       
    }
}