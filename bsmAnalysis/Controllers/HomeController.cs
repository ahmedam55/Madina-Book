using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using bsmAnalysis.Models;

namespace bsmAnalysis.Controllers
{
    public class HomeController : Controller
    {
        UsersContext db = new UsersContext();
        public ActionResult Index()
        {
            try
            {
                ViewBag.UserId = db.UserProfiles.Find(Membership.GetUser().ProviderUserKey).UserId;
            }
            catch (Exception)
            {
                
               
            }
           
            return View();
        }

        public ActionResult About(int id)
        {
            ViewBag.Message = "Your app description page.";

          

            return View();
        }
       // bool once=false ;
       // [HttpPost]        
        public JsonResult getHome()
        {return Json(UserProfile.getHome(), JsonRequestBehavior.AllowGet);}

        public JsonResult addComment(int id,string body)
        { return Json(Comment.addComment(id, body), JsonRequestBehavior.AllowGet); }

        public JsonResult addMessage(string body)
        {return Json(Message.addMessage(body), JsonRequestBehavior.AllowGet);}

        public JsonResult likeMessage(int id)
        {return Json(Message.likeMessage(id), JsonRequestBehavior.AllowGet);}

        public JsonResult shareMessage(int id)
        { return Json(Message.shareMessage(id), JsonRequestBehavior.AllowGet); }

        public JsonResult likeComment(int id)
        {return Json(Comment.likeComment(id), JsonRequestBehavior.AllowGet);}

        public JsonResult deleteComment(int id)
        {return Json(Comment.deleteComment(id), JsonRequestBehavior.AllowGet);}

        public JsonResult deleteMessage(int id)
        {return Json(Message.deleteMessage(id), JsonRequestBehavior.AllowGet);}

        public JsonResult uploadPic(HttpPostedFileBase photo,bool Isprofile=false)
        {return Json(Picture.uploadPic(photo,Isprofile), JsonRequestBehavior.AllowGet);}

        public JsonResult sendMessage(int userid,string body)
        {return Json(Message.sendMessage(body,userid), JsonRequestBehavior.AllowGet);}

        public JsonResult getConversations()
        {return Json(UserProfile.getConversations(), JsonRequestBehavior.AllowGet);}

        public JsonResult requestFriendship(int userid)
        {return Json(UserProfile.requestFriendship(userid), JsonRequestBehavior.AllowGet);}

        public JsonResult removeRequestFriendship(int userid)
        {return Json(UserProfile.removeRequestFriendship(userid), JsonRequestBehavior.AllowGet);}

        public JsonResult getRequests()
        { return Json(UserProfile.getRequests(), JsonRequestBehavior.AllowGet); }

        public JsonResult acceptFriendship(int userid)
        {return Json(UserProfile.acceptFriendship(userid), JsonRequestBehavior.AllowGet);}

        public JsonResult disagreeFriendship(int userid)
        {return Json(UserProfile.disagreeFriendship(userid), JsonRequestBehavior.AllowGet);}

        public JsonResult deleteFriend(int userid)
        { return Json(UserProfile.deleteFriend(userid), JsonRequestBehavior.AllowGet); }

        public JsonResult makeGroup(string name)
        { return Json(Group.makeGroup(name), JsonRequestBehavior.AllowGet); }

        public JsonResult getGroupMessages(int groupId)
        { return Json(UserProfile.getGroupMessages(groupId), JsonRequestBehavior.AllowGet); }

        public JsonResult addMessageInGroup(int groupId,string body)
        { return Json(Message.addMessageInGroup(groupId,body), JsonRequestBehavior.AllowGet); }

        public JsonResult search(string query)
        { return Json(UserProfile.search(query), JsonRequestBehavior.AllowGet); }
      
        public JsonResult requestSubscribtion(int groupId)
        { return Json(Group.requestSubscribtion(groupId), JsonRequestBehavior.AllowGet); }

        public JsonResult removeRequestSubscribtion(int groupId)
        { return Json(Group.removeRequestSubscribtion(groupId), JsonRequestBehavior.AllowGet); }

        public JsonResult acceptSubscribtion(int userId, int groupId)
        { return Json(Group.acceptSubscribtion(userId,groupId), JsonRequestBehavior.AllowGet); }
        //طرد عضو
        public JsonResult disagreeSubscribtion(int userId,int groupId)
        { return Json(Group.disagreeSubscribtion(userId,groupId), JsonRequestBehavior.AllowGet); }
        
        public JsonResult leaveGroup(int groupId)
        { return Json(Group.leaveGroup(groupId), JsonRequestBehavior.AllowGet); }

        public JsonResult getProfileData(int id)
        { return Json(UserProfile.getProfileData(id), JsonRequestBehavior.AllowGet); }

        public ActionResult ProfileData(int id=0)
        {
            ViewBag.userId = id;
            return View(); 
        }

        public JsonResult getOnlineFriends()
        { return Json(UserProfile.getOnlineFriends(), JsonRequestBehavior.AllowGet); }

    }
}
