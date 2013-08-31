using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using System.Linq;
using Newtonsoft.Json;

namespace bsmAnalysis.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            // modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
            // modelBuilder.Entity<Instructor>()
            //   .HasOptional(p => p.OfficeAssignment).WithRequired(p => p.Instructor);
            //unary many to many
            modelBuilder.Entity<UserProfile>()
              .HasMany(g => g.friends)
              .WithMany()
              .Map(t => t.MapLeftKey("userId")
                 .MapRightKey("friendID")
                 .ToTable("friends"));
            //unary many to many
            modelBuilder.Entity<UserProfile>()
              .HasMany(g => g.friendshipRequests)
              .WithMany()
              .Map(t => t.MapLeftKey("userId")
                 .MapRightKey("friendID")
                 .ToTable("friendshiprequest"));
            //unary many to many
            modelBuilder.Entity<UserProfile>()
              .HasMany(g => g.onlineFriends)
              .WithMany()
              .Map(t => t.MapLeftKey("userId")
                 .MapRightKey("friendID")
                 .ToTable("friendsOnline"));
            // many to many
            modelBuilder.Entity<UserProfile>()
              .HasMany(u =>u.subscribedGroups)
              .WithMany(g=>g.members)
              .Map(t => t.MapLeftKey("groupId")
                 .MapRightKey("userID")
                 .ToTable("subscribedGroups"));
            // many to many
            modelBuilder.Entity<UserProfile>()
              .HasMany(g => g.likedComments)
              .WithMany(c => c.usersLiked)
              .Map(t => t.MapLeftKey("commentId")
                 .MapRightKey("userID")
                 .ToTable("commentsLikes"));
            // many to many
            modelBuilder.Entity<UserProfile>()
              .HasMany(g => g.likedMessages)
              .WithMany(c => c.usersLiked)
              .Map(t => t.MapLeftKey("messageId")
                 .MapRightKey("userID")
                 .ToTable("messagesLikes"));
            // many to many
            modelBuilder.Entity<UserProfile>()
              .HasMany(g => g.sharedMessages)
              .WithMany(c => c.usersShared)
              .Map(t => t.MapLeftKey("messageId")
                 .MapRightKey("userID")
                 .ToTable("messagesShares"));
            // many to many
            modelBuilder.Entity<Group>()
              .HasMany(g => g.requestingMembers)
              .WithMany()
              .Map(t => t.MapLeftKey("userId")
                 .MapRightKey("groupID")
                 .ToTable("groupsRequests"));


            modelBuilder.Entity<Message>()
            .HasOptional(u => u.writer)
            .WithMany(h=>h.writtenMessages);

            modelBuilder.Entity<Message>()
                        .HasOptional(u => u.sender)
                        .WithMany(h => h.conversations);
            modelBuilder.Entity<Comment>()
                .HasRequired(u => u.ParentMessage)
                .WithMany(h => h.Comments)
                .WillCascadeOnDelete(true);

            ////modelBuilder.Entity<UserProfile>()
            //// .HasMany(c => c.friends)
            //// .WithMany(i => i.friends)
            //// .Map(t => t.MapLeftKey("userId")
            ////     .MapRightKey("friendID")
            ////     .ToTable("MessagesLiked"));
         //   modelBuilder.Entity<Message>()
         //     .HasMany(c => c.usersLiked).WithMany(i => i.MessagesLiked)
         //     .Map(t => t.MapLeftKey("MessageId")
         //         .MapRightKey("UserID")
         //         .ToTable("MessagesLiked"));
         //   modelBuilder.Entity<Comment>()
         //       .HasMany(c => c.UsersLiked).WithMany(i => i.likedComments)
         //       .Map(t => t.MapLeftKey("CommentId")
         //           .MapRightKey("UserID")
         //           .ToTable("CommentsLiked"));
         //   modelBuilder.Entity<Message>()
         //.HasMany(c => c.UsersShared).WithMany(i => i.MessagesShared)
         //.Map(t => t.MapLeftKey("MessageId")
         //    .MapRightKey("UserID")
         //    .ToTable("MessagesShared"));
         //   modelBuilder.Entity<Group>()
         //       .HasMany(c => c.members).WithMany(i => i.subscribedGroups)
         //       .Map(t => t.MapLeftKey("GroupId")
         //           .MapRightKey("UserID")
         //           .ToTable("GroupsSubscribtion"));
            //  modelBuilder.Entity<Department>()
            // .HasOptional(x => x.Administrator);
        }
    }

    [Table("UserProfile")]
    public class UserProfile:Notify
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public string UserName { get; set; }
        public string email { get; set; }
        public string profilepic { get; set; }
        public string describsion { get; set; }       
        public bool? online { get; set; }
        public bool? appear { get; set; }

        public virtual ICollection<UserProfile> friends { get; set; }//*to*.
        public virtual ICollection<UserProfile> friendshipRequests { get; set; }//*to*
        public virtual ICollection<UserProfile> onlineFriends { get; set; }//*to*

        public virtual ICollection<Message> writtenMessages { get; set; }//1 to *
        public virtual ICollection<Message> conversations { get; set; }//1to*

        public virtual ICollection<Message> likedMessages { get; set; }//*to*
        public virtual ICollection<Message> sharedMessages { get; set; }//*to*
        

        public virtual ICollection<Comment> writtenComments { get; set; }//1 to *

        public virtual ICollection<Comment> likedComments { get; set; }//*to*        
        public virtual ICollection<Picture> Pictures { get; set; }//1 to *
        public virtual ICollection<Group> ownedGroups { get; set; }//1 to *
        public virtual ICollection<Group> subscribedGroups { get; set; }//*to*
        


        //methods
        
        public static string setProfilePic(int id)
        { 
            UsersContext  db = new UsersContext();

            var user = db.UserProfiles.Find(Membership.GetUser().ProviderUserKey);
       user.profilepic = (from pic in user.Pictures
                          where pic.PictureId == id
                          select pic.pictureUrl).First();       
                          
        db.SaveChanges();            
        return "";
        }
        public static string requestFriendship(int id)
        {
            UsersContext  db = new UsersContext();

            var requester = db.UserProfiles.Find(Membership.GetUser().ProviderUserKey);
            var requested = db.UserProfiles.Find(id);
            if (requester != requested&&!requester.friends.Contains(requested))
            {
                requested.friendshipRequests.Add(requester);
                db.SaveChanges();
            }
            
            return "";
        }
        public static string removeRequestFriendship(int id)
        {
            UsersContext  db = new UsersContext();

            var requester = db.UserProfiles.Find(Membership.GetUser().ProviderUserKey);
            var requested = db.UserProfiles.Find(id);
            requested.friendshipRequests.Remove(requester);
            db.SaveChanges();
            return "";
        }
        public static string acceptFriendship(int id)
        {UsersContext  db = new UsersContext();
           
            var user = db.UserProfiles.Find(Membership.GetUser().ProviderUserKey);
            var userfriend = db.UserProfiles.Find(id);
            user.friendshipRequests.Remove(userfriend);
            user.friends.Add(userfriend);
            userfriend.friends.Add(user);
            db.SaveChanges();
            return "";
        }
        public static string disagreeFriendship(int id)
        {
            UsersContext  db = new UsersContext();
            var requester = db.UserProfiles.Find(id);
            var requested = db.UserProfiles.Find(Membership.GetUser().ProviderUserKey);
            requested.friendshipRequests.Remove(requester);
            db.SaveChanges();
            return "";
        }    
        public static string deleteFriend(int id)
        {UsersContext  db = new UsersContext();
            var user = db.UserProfiles.Find(Membership.GetUser().ProviderUserKey);
            var userfriend = db.UserProfiles.Find(id);
            user.friends.Remove(userfriend);
            userfriend.friends.Remove(user);
            db.SaveChanges();
            return "";
        }
        public static object getOnlineFriends()
        {
            //UsersContext  db = new UsersContext();
            //var user = db.UserProfiles.Find(Membership.GetUser().ProviderUserKey);

           
            //    var result = from y in user.friends
            //                 where y.online == true
            //                 select new { UserId = y.UserId, UserName = y.UserName };
            //    if (user.appear==false)
            //    {
            //       return result = null;
            //    }

            return Notifiy.onlineUsers.Select(g=>g.Value).Select(d=>new {UserId=d.UserId,UserName=d.UserName,ProfilePic=d.profilepic});
        }
        public static string setOnlineStatus(bool onlineornot)
        {UsersContext  db = new UsersContext();
            var user = db.UserProfiles.Find(Membership.GetUser().ProviderUserKey);
            user.appear = onlineornot;            
            db.SaveChanges();
            return user.online.ToString();
        }

        public static string onOffStatus(bool onlineornot)
        {UsersContext  db = new UsersContext();
            var user = db.UserProfiles.Find(Membership.GetUser().ProviderUserKey);
            if (onlineornot==true)
            {
                if (user.appear!=false)
                {
                    user.online = onlineornot;
                }
            }
            else
            {
                user.online = false;
            }

            db.SaveChanges();
            return user.online.ToString();
        }

        public static object getRequests()
        {UsersContext  db = new UsersContext();
        try
        {

       
            var user = db.UserProfiles.Find(Membership.GetUser().ProviderUserKey);
            var result = from g in user.friendshipRequests
                         select new {UserName=g.UserName,UserId=g.UserId,profilepic=g.profilepic };

            
         
            return result;
        }
        catch (Exception)
        {

            return null;
        }
        }
        public static object getConversations()
        {UsersContext  db = new UsersContext();
        try
        {
            var user = db.UserProfiles.Find(Membership.GetUser().ProviderUserKey);
            var result = from g in user.conversations
                         select new { body = g.body, timeAdded = g.timeAdded.ToString("g"), writer = new { UserId = g.writer.UserId, UserName = g.writer.UserName, profilePic = g.writer.profilepic } };



            return result.OrderByDescending(f => f.timeAdded);
        }
        catch (Exception)
        {
            return null;
        }
        }
        public static object getHome()
        {
            UsersContext db = new UsersContext();
            try{
            var user = db.UserProfiles.Find(Membership.GetUser().ProviderUserKey);//messageGroup = new { name = g.messageGroup.name, GroupId = g.messageGroup.GroupId }

            var myMesseages = from g in user.writtenMessages
                              where g.messageGroup == null && g.sender == null
                              select new { sharer = new { UserId = -1, UserName = "-1", profilePic = "" }, MessageId = g.MessageId, body = g.body, timeAdded = g.timeAdded.ToString("g"), writer = new { UserId = g.writer.UserId, UserName = g.writer.UserName, profilePic = g.writer.profilepic }, usersLiked = g.usersLiked.Select(c => new { UserId = c.UserId, UserName = c.UserName, profilePic = c.profilepic }), usersShared = g.usersShared.Select(h => new { UserId = h.UserId, UserName = h.UserName, profilePic = h.profilepic }), messagePicture = g.MessagePicture.Select(f => f.pictureUrl), Comments = g.Comments.Select(fg => new { CommentId = fg.CommentId, body = fg.body, timeAdded = fg.timeAdded.ToString("g"), writer = new { UserId = fg.writer.UserId, UserName = fg.writer.UserName, profilePic = fg.writer.profilepic }, usersLiked = fg.usersLiked.Select(c => new { UserId = c.UserId, UserName = c.UserName, profilePic = c.profilepic }) }) };
            var sharedMesseages = from g in user.sharedMessages
                              where g.messageGroup == null && g.sender == null
                                  select new { sharer = new { UserId = user.UserId, UserName = user.UserName, profilePic = user.profilepic }, MessageId = g.MessageId, body = g.body, timeAdded = g.timeAdded.ToString("g"), writer = new { UserId = g.writer.UserId, UserName = g.writer.UserName, profilePic = g.writer.profilepic }, usersLiked = g.usersLiked.Select(c => new { UserId = c.UserId, UserName = c.UserName, profilePic = c.profilepic }), usersShared = g.usersShared.Select(h => new { UserId = h.UserId, UserName = h.UserName, profilePic = h.profilepic }), messagePicture = g.MessagePicture.Select(f => f.pictureUrl), Comments = g.Comments.Select(fg => new { CommentId = fg.CommentId, body = fg.body, timeAdded = fg.timeAdded.ToString("g"), writer = new { UserId = fg.writer.UserId, UserName = fg.writer.UserName, profilePic = fg.writer.profilepic }, usersLiked = fg.usersLiked.Select(c => new { UserId = c.UserId, UserName = c.UserName, profilePic = c.profilepic }) }) };
            var friendsMessages = from g in user.friends.Select(o => o.writtenMessages.Where(h=>h.messageGroup==null&&h.sender==null))//.Where(f=>f.Select(h=>h.messageGroup)==null)                                 

                                  select new { H = g.Select(hty => new { sharer = new { UserId = -1, UserName = "-1", profilePic = "" }, MessageId = hty.MessageId, body = hty.body, timeAdded = hty.timeAdded.ToString("g"), writer = new { UserId = hty.writer.UserId, UserName = hty.writer.UserName, profilePic = hty.writer.profilepic }, usersLiked = hty.usersLiked.Select(c => new { UserId = c.UserId, UserName = c.UserName, profilePic = c.profilepic }), usersShared = hty.usersShared.Select(h => new { UserId = h.UserId, UserName = h.UserName, profilePic = h.profilepic }), messagePicture = hty.MessagePicture.Select(f => f.pictureUrl), Comments = hty.Comments.Select(fg => new { CommentId = fg.CommentId, body = fg.body, timeAdded = fg.timeAdded.ToString("g"), writer = new { UserId = fg.writer.UserId, UserName = fg.writer.UserName, profilePic = fg.writer.profilepic }, usersLiked = fg.usersLiked.Select(c => new { UserId = c.UserId, UserName = c.UserName, profilePic = c.profilepic }) }) }) };
            var friendsSharedMessages = from friend in user.friends
                                        from g in friend.sharedMessages
                                        where g.messageGroup == null && g.sender == null//.Where(f=>f.Select(h=>h.messageGroup)==null)                                 

                                        select new { sharer = new { UserId = friend.UserId, UserName = friend.UserName, profilePic = friend.profilepic }, MessageId = g.MessageId, body = g.body, timeAdded = g.timeAdded.ToString("g"), writer = new { UserId = g.writer.UserId, UserName = g.writer.UserName, profilePic = g.writer.profilepic }, usersLiked = g.usersLiked.Select(c => new { UserId = c.UserId, UserName = c.UserName, profilePic = c.profilepic }), usersShared = g.usersShared.Select(h => new { UserId = h.UserId, UserName = h.UserName, profilePic = h.profilepic }), messagePicture = g.MessagePicture.Select(f => f.pictureUrl), Comments = g.Comments.Select(fg => new { CommentId = fg.CommentId, body = fg.body, timeAdded = fg.timeAdded.ToString("g"), writer = new { UserId = fg.writer.UserId, UserName = fg.writer.UserName, profilePic = fg.writer.profilepic }, usersLiked = fg.usersLiked.Select(c => new { UserId = c.UserId, UserName = c.UserName, profilePic = c.profilepic }) }) };
          
          
         var allMessages =myMesseages.OrderByDescending(c => c.timeAdded);
         if (friendsMessages.Count() != 0)
         {
             allMessages = friendsMessages.Select(b => b.H).FirstOrDefault().Union(myMesseages).OrderByDescending(c => c.timeAdded);
         }
         if(sharedMesseages.Count()!=0){
             try
             {
                 allMessages = friendsMessages.Select(b => b.H).FirstOrDefault().Union(myMesseages).Union(sharedMesseages).Union(friendsSharedMessages).OrderByDescending(c => c.timeAdded);
             }
             catch (Exception) {
                 allMessages = myMesseages.Union(sharedMesseages).Union(friendsSharedMessages).OrderByDescending(c => c.timeAdded);
             }
         }
        
            var home = new { messages = allMessages, groups = user.subscribedGroups.Select(j => new { GroupId = j.GroupId, name = j.name }).Union(user.ownedGroups.Select(j => new { GroupId = j.GroupId, name = j.name })) };

            return home;
              }
        catch (Exception)
        {
            return null;
        }
        }
        public static object getGroupMessages(int groupId)
        {
            UsersContext db = new UsersContext();
            var user = db.UserProfiles.Find(Membership.GetUser().ProviderUserKey);//messageGroup = new { name = g.messageGroup.name, GroupId = g.messageGroup.GroupId }
            var usergroups=user.subscribedGroups.Union(user.ownedGroups);
            var neededGroup = usergroups.FirstOrDefault(g => g.GroupId == groupId);
            var group = new { groups = usergroups.Select(t => new { GroupId=t.GroupId,name=t.name}), messages = neededGroup.messages.Select(hty => new { MessageId = hty.MessageId, body = hty.body, timeAdded = hty.timeAdded.ToString("g"), writer = new { UserId = hty.writer.UserId, UserName = hty.writer.UserName, profilePic = hty.writer.profilepic }, usersLiked = hty.usersLiked.Select(c => new { UserId = c.UserId, UserName = c.UserName, profilePic = c.profilepic }), usersShared = hty.usersShared.Select(h => new { UserId = h.UserId, UserName = h.UserName, profilePic = h.profilepic }), messagePicture = hty.MessagePicture.Select(f => f.pictureUrl), Comments = hty.Comments.Select(fg => new { CommentId = fg.CommentId, body = fg.body, timeAdded = fg.timeAdded.ToString("g"), writer = new { UserId = fg.writer.UserId, UserName = fg.writer.UserName, profilePic = fg.writer.profilepic }, usersLiked = fg.usersLiked.Select(c => new { UserId = c.UserId, UserName = c.UserName, profilePic = c.profilepic }) }) }), members = neededGroup.members.Select(g => new { UserId = g.UserId, UserName = g.UserName, profilePic = g.profilepic }), name = neededGroup.name, GroupId = neededGroup.GroupId, requestingMembers = neededGroup.requestingMembers.Select(g => new { UserId = g.UserId, UserName = g.UserName, profilePic = g.profilepic }) };

            return group;
        }
        public static object getProfileData(int userId)
        {
            UsersContext db = new UsersContext();
            var user = db.UserProfiles.Find(userId);

            var userMessages = from g in user.writtenMessages
                               where g.messageGroup == null && g.sender == null
                               select new { sharer = new { UserId = -1, UserName = "-1", profilePic = "" }, MessageId = g.MessageId, body = g.body, timeAdded = g.timeAdded.ToString("g"), writer = new { UserId = g.writer.UserId, UserName = g.writer.UserName, profilePic = g.writer.profilepic }, usersLiked = g.usersLiked.Select(c => new { UserId = c.UserId, UserName = c.UserName, profilePic = c.profilepic }), usersShared = g.usersShared.Select(h => new { UserId = h.UserId, UserName = h.UserName, profilePic = h.profilepic }), messagePicture = g.MessagePicture.Select(f => f.pictureUrl), Comments = g.Comments.Select(fg => new { CommentId = fg.CommentId, body = fg.body, timeAdded = fg.timeAdded.ToString("g"), writer = new { UserId = fg.writer.UserId, UserName = fg.writer.UserName, profilePic = fg.writer.profilepic }, usersLiked = fg.usersLiked.Select(c => new { UserId = c.UserId, UserName = c.UserName, profilePic = c.profilepic }) }) };
            var sharedMesseages = from g in user.sharedMessages
                                  where g.messageGroup == null && g.sender == null
                                  select new { sharer = new { UserId = user.UserId, UserName = user.UserName, profilePic = user.profilepic }, MessageId = g.MessageId, body = g.body, timeAdded = g.timeAdded.ToString("g"), writer = new { UserId = g.writer.UserId, UserName = g.writer.UserName, profilePic = g.writer.profilepic }, usersLiked = g.usersLiked.Select(c => new { UserId = c.UserId, UserName = c.UserName, profilePic = c.profilepic }), usersShared = g.usersShared.Select(h => new { UserId = h.UserId, UserName = h.UserName, profilePic = h.profilepic }), messagePicture = g.MessagePicture.Select(f => f.pictureUrl), Comments = g.Comments.Select(fg => new { CommentId = fg.CommentId, body = fg.body, timeAdded = fg.timeAdded.ToString("g"), writer = new { UserId = fg.writer.UserId, UserName = fg.writer.UserName, profilePic = fg.writer.profilepic }, usersLiked = fg.usersLiked.Select(c => new { UserId = c.UserId, UserName = c.UserName, profilePic = c.profilepic }) }) };
            var photos= from g in user.Pictures
                        select new{pictureUrl=g.pictureUrl};
            var friends=from friend in user.friends
                        select new{UserId=friend.UserId,UserName=friend.UserName,profilePic=friend.profilepic};
         
            var profile = new {friends=friends,describsion=user.describsion,profilePic=user.profilepic, messages = userMessages.OrderBy(c=>c.timeAdded).Union(sharedMesseages), groups = user.subscribedGroups.Select(j => new { GroupId = j.GroupId, name = j.name }).Union(user.ownedGroups.Select(j => new { GroupId = j.GroupId, name = j.name })),photos= photos};
           
            return profile;
        }

        public static object search(string query)
        {
            UsersContext db = new UsersContext();
            object result = new { };
            if (query.Trim().Length != 0)
            {
                var groups = from groupy in db.Groups
                             where groupy.name.Contains(query)
                             select groupy;
                var users = from usery in db.UserProfiles
                            where usery.UserName.Contains(query) || usery.email == query
                            select usery;
                 result = new { groups = groups.Select(j => new { GroupId = j.GroupId, name = j.name }), users = users.Select(g => new { UserId = g.UserId, UserName = g.UserName, profilePic = g.profilepic }) };
            }

            return result;
        }

       


    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
