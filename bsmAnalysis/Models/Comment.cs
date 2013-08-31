using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bsmAnalysis.Models
{
    public class Comment:Text
    {
        public int CommentId { get; set; }


        //public int UserProfileId;
        public virtual UserProfile writer { get; set; }
        public virtual Message ParentMessage { get; set; }
        public virtual ICollection<UserProfile> usersLiked { get; set; }


        
       
        //methods
        public static object addComment(int messageId,string body)
        {
            UsersContext db = new UsersContext();
            UserProfile user = db.UserProfiles.Find(System.Web.Security.Membership.GetUser().ProviderUserKey);
           
            Comment myComment = new Comment { writer = user, body = body, timeAdded = DateTime.Now};
            db.Messages.Find(messageId).Comments.Add(myComment );
            db.SaveChanges();
            var myCommentJson = new { CommentId=myComment.CommentId, body = myComment.body, timeAdded = myComment.timeAdded.ToString("g"), writer = new { UserId = myComment.writer.UserId, UserName = myComment.writer.UserName, profilePic = myComment.writer.profilepic }, usersLiked = myComment.usersLiked ,MessageId=myComment.ParentMessage.MessageId};
             
            //var context = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<Notifiy>();
            //context.Clients.All.addComment(myCommentJson);
            return myCommentJson ;
        }
        public static object deleteComment(int commentId)
        {
            UsersContext db = new UsersContext();
            UserProfile user = db.UserProfiles.Find(System.Web.Security.Membership.GetUser().ProviderUserKey);

            var comment=db.Comments.Find(commentId);
            if (comment.writer==user||comment.ParentMessage.writer==user||comment.ParentMessage.messageGroup.Admin==user)
        	{
               // db.Messages.Find(messageId).Comments.Remove(comment);
                db.Comments.Remove(comment);
                db.SaveChanges();
                return true;
	        }           
            return false;
        }
        public static object likeComment(int id)
        {
            UsersContext db = new UsersContext();
            UserProfile user = db.UserProfiles.Find(System.Web.Security.Membership.GetUser().ProviderUserKey);
            if (!db.Comments.Find(id).usersLiked.Contains(user))
            {
                db.Comments.Find(id).usersLiked.Add(user);
            }
            else
            {
                db.Comments.Find(id).usersLiked.Remove(user);
            }
            db.SaveChanges();
            var ouruser = db.Comments.Find(id).usersLiked.Select(guser => new { UserId = guser.UserId, UserName = guser.UserName, profilePic = guser.profilepic });
                   return ouruser;
        }
      
    }
}