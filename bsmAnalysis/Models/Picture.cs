using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace bsmAnalysis.Models
{
    public class Picture:Notify
    {
        public int PictureId { get; set; }

        public virtual UserProfile owner { get; set; }
        public string name { get; set; }      
        public string pictureUrl { get; set; }
       


        public virtual Message PictureMessage { get; set; }



     
        
        public static string uploadPic(HttpPostedFileBase photo,bool Isprofile=false,string name="",string body="")
        { UsersContext db = new UsersContext();
            var user=db.UserProfiles.Find(Membership.GetUser().ProviderUserKey);
            //1mega in bytes
            int maxsize = 1048576;
            var TempFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_TEMP");
            Random r = new Random();
            string savepath = TempFolder + DateTime.Now.GetHashCode() + r.Next() + photo.FileName;
            if (photo != null && photo.ContentLength < maxsize && photo.ContentType.Contains("image/"))
            { photo.SaveAs(savepath); 
                 ;
                 var message = new Message {writer=user,body=body,timeAdded=DateTime.Now, MessagePicture = new List<Picture>() };
                 message.MessagePicture.Add(new Picture {name=name,pictureUrl=savepath,owner=user });
                 db.Messages.Add(message);
                 if (Isprofile == true) { user.profilepic = savepath; }
                 db.SaveChanges();
            }
            else
            { return "empty file or heavy one"; }
 
   

            return savepath;
        }
    }
}