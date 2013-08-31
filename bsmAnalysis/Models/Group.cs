using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bsmAnalysis.Models
{
    public class Group:Notify
    {
        public int GroupId { get; set; }

        public string  name { get; set; }
        public virtual UserProfile Admin { get; set; }
        public virtual ICollection<UserProfile> requestingMembers { get; set; }
        public virtual ICollection<UserProfile> members { get; set; }
        public virtual ICollection<Message> messages { get; set; }


        
        //methods
        public static string makeGroup(string groupname )
        {
            UsersContext db = new UsersContext();
            UserProfile user = db.UserProfiles.Find(System.Web.Security.Membership.GetUser().ProviderUserKey);

            db.Groups.Add(new Group {Admin=user,name=groupname });
            db.SaveChanges();
            return "";
        }

        public static string requestSubscribtion(int groupid)
        {
            UsersContext db = new UsersContext();
            UserProfile user = db.UserProfiles.Find(System.Web.Security.Membership.GetUser().ProviderUserKey);

            var group=db.Groups.Find(groupid);
            if(group.Admin!=user)
            {
                group.requestingMembers.Add(user);
                db.SaveChanges();
            }
            return "";
        }

        public static string removeRequestSubscribtion(int groupid)
        {
            UsersContext db = new UsersContext();
            UserProfile user = db.UserProfiles.Find(System.Web.Security.Membership.GetUser().ProviderUserKey);

            var group = db.Groups.Find(groupid);
           
                group.requestingMembers.Remove(user);
                db.SaveChanges();
            
            return "";
        }

        public static string acceptSubscribtion(int userid, int groupid)
        {
            UsersContext db = new UsersContext();
            UserProfile user = db.UserProfiles.Find(System.Web.Security.Membership.GetUser().ProviderUserKey);

            var group = db.Groups.Find(groupid);
            if (group.Admin == user)
            {
                group.requestingMembers.Remove(db.UserProfiles.Find(userid));
                group.members.Add(db.UserProfiles.Find(userid));
                db.SaveChanges();
            }

            return "";
        }

        public static string disagreeSubscribtion(int userid, int groupid)
        {
            UsersContext db = new UsersContext();
            UserProfile user = db.UserProfiles.Find(System.Web.Security.Membership.GetUser().ProviderUserKey);

            var group = db.Groups.Find(groupid);
            if (group.Admin == user)
            {
               
                group.members.Remove(db.UserProfiles.Find(userid));
                db.SaveChanges();
            }
            return "";
        }

        public static string leaveGroup(int groupid)
        {
            UsersContext db = new UsersContext();
            UserProfile user = db.UserProfiles.Find(System.Web.Security.Membership.GetUser().ProviderUserKey);

            var group = db.Groups.Find(groupid);          

                group.members.Remove(user);
                db.SaveChanges();
           
            return "";
        }

        // have been replaced by getGroupMessages that include getting (members,messages,requests)
        //public static object getRequests(int groupId)
        //{
        //    UsersContext db = new UsersContext();
        //    UserProfile user = db.UserProfiles.Find(System.Web.Security.Membership.GetUser().ProviderUserKey);

        //   var group=db.Groups.Find(groupId);
        //    if (user==group.Admin)
        //    {
        //         var groupRequests = group.requestingMembers.Select(g => new { UserName = g.UserName, UserId = g.UserId, profilepic = g.profilepic });
        //         return groupRequests;
        //    }
        //    else
        //    {
        //        var groupRequests = new{};
        //         return groupRequests;
        //    }
           
        //}




    }
}