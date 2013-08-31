namespace bsmAnalysis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aliperfectISAdesigner : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        email = c.String(),
                        profilepic = c.String(),
                        describsion = c.String(),
                        online = c.Boolean(),
                        appear = c.Boolean(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        GroupId = c.Int(),
                        body = c.String(),
                        timeAdded = c.DateTime(nullable: false),
                        writer_UserId = c.Int(),
                        sender_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.MessageId)
                .ForeignKey("dbo.UserProfile", t => t.writer_UserId)
                .ForeignKey("dbo.UserProfile", t => t.sender_UserId)
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .Index(t => t.writer_UserId)
                .Index(t => t.sender_UserId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupId = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        Admin_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.GroupId)
                .ForeignKey("dbo.UserProfile", t => t.Admin_UserId)
                .Index(t => t.Admin_UserId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        body = c.String(),
                        timeAdded = c.DateTime(nullable: false),
                        writer_UserId = c.Int(),
                        ParentMessage_MessageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.UserProfile", t => t.writer_UserId)
                .ForeignKey("dbo.Messages", t => t.ParentMessage_MessageId, cascadeDelete: true)
                .Index(t => t.writer_UserId)
                .Index(t => t.ParentMessage_MessageId);
            
            CreateTable(
                "dbo.Pictures",
                c => new
                    {
                        PictureId = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        pictureUrl = c.String(),
                        owner_UserId = c.Int(),
                        PictureMessage_MessageId = c.Int(),
                    })
                .PrimaryKey(t => t.PictureId)
                .ForeignKey("dbo.UserProfile", t => t.owner_UserId)
                .ForeignKey("dbo.Messages", t => t.PictureMessage_MessageId)
                .Index(t => t.owner_UserId)
                .Index(t => t.PictureMessage_MessageId);
            
            CreateTable(
                "dbo.friends",
                c => new
                    {
                        userId = c.Int(nullable: false),
                        friendID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.userId, t.friendID })
                .ForeignKey("dbo.UserProfile", t => t.userId)
                .ForeignKey("dbo.UserProfile", t => t.friendID)
                .Index(t => t.userId)
                .Index(t => t.friendID);
            
            CreateTable(
                "dbo.friendshiprequest",
                c => new
                    {
                        userId = c.Int(nullable: false),
                        friendID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.userId, t.friendID })
                .ForeignKey("dbo.UserProfile", t => t.userId)
                .ForeignKey("dbo.UserProfile", t => t.friendID)
                .Index(t => t.userId)
                .Index(t => t.friendID);
            
            CreateTable(
                "dbo.friendsOnline",
                c => new
                    {
                        userId = c.Int(nullable: false),
                        friendID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.userId, t.friendID })
                .ForeignKey("dbo.UserProfile", t => t.userId)
                .ForeignKey("dbo.UserProfile", t => t.friendID)
                .Index(t => t.userId)
                .Index(t => t.friendID);
            
            CreateTable(
                "dbo.groupsRequests",
                c => new
                    {
                        userId = c.Int(nullable: false),
                        groupID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.userId, t.groupID })
                .ForeignKey("dbo.Groups", t => t.userId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.groupID, cascadeDelete: true)
                .Index(t => t.userId)
                .Index(t => t.groupID);
            
            CreateTable(
                "dbo.messagesLikes",
                c => new
                    {
                        messageId = c.Int(nullable: false),
                        userID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.messageId, t.userID })
                .ForeignKey("dbo.UserProfile", t => t.messageId, cascadeDelete: true)
                .ForeignKey("dbo.Messages", t => t.userID, cascadeDelete: true)
                .Index(t => t.messageId)
                .Index(t => t.userID);
            
            CreateTable(
                "dbo.messagesShares",
                c => new
                    {
                        messageId = c.Int(nullable: false),
                        userID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.messageId, t.userID })
                .ForeignKey("dbo.UserProfile", t => t.messageId, cascadeDelete: true)
                .ForeignKey("dbo.Messages", t => t.userID, cascadeDelete: true)
                .Index(t => t.messageId)
                .Index(t => t.userID);
            
            CreateTable(
                "dbo.commentsLikes",
                c => new
                    {
                        commentId = c.Int(nullable: false),
                        userID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.commentId, t.userID })
                .ForeignKey("dbo.UserProfile", t => t.commentId, cascadeDelete: true)
                .ForeignKey("dbo.Comments", t => t.userID, cascadeDelete: true)
                .Index(t => t.commentId)
                .Index(t => t.userID);
            
            CreateTable(
                "dbo.subscribedGroups",
                c => new
                    {
                        groupId = c.Int(nullable: false),
                        userID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.groupId, t.userID })
                .ForeignKey("dbo.UserProfile", t => t.groupId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.userID, cascadeDelete: true)
                .Index(t => t.groupId)
                .Index(t => t.userID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.subscribedGroups", new[] { "userID" });
            DropIndex("dbo.subscribedGroups", new[] { "groupId" });
            DropIndex("dbo.commentsLikes", new[] { "userID" });
            DropIndex("dbo.commentsLikes", new[] { "commentId" });
            DropIndex("dbo.messagesShares", new[] { "userID" });
            DropIndex("dbo.messagesShares", new[] { "messageId" });
            DropIndex("dbo.messagesLikes", new[] { "userID" });
            DropIndex("dbo.messagesLikes", new[] { "messageId" });
            DropIndex("dbo.groupsRequests", new[] { "groupID" });
            DropIndex("dbo.groupsRequests", new[] { "userId" });
            DropIndex("dbo.friendsOnline", new[] { "friendID" });
            DropIndex("dbo.friendsOnline", new[] { "userId" });
            DropIndex("dbo.friendshiprequest", new[] { "friendID" });
            DropIndex("dbo.friendshiprequest", new[] { "userId" });
            DropIndex("dbo.friends", new[] { "friendID" });
            DropIndex("dbo.friends", new[] { "userId" });
            DropIndex("dbo.Pictures", new[] { "PictureMessage_MessageId" });
            DropIndex("dbo.Pictures", new[] { "owner_UserId" });
            DropIndex("dbo.Comments", new[] { "ParentMessage_MessageId" });
            DropIndex("dbo.Comments", new[] { "writer_UserId" });
            DropIndex("dbo.Groups", new[] { "Admin_UserId" });
            DropIndex("dbo.Messages", new[] { "GroupId" });
            DropIndex("dbo.Messages", new[] { "sender_UserId" });
            DropIndex("dbo.Messages", new[] { "writer_UserId" });
            DropForeignKey("dbo.subscribedGroups", "userID", "dbo.Groups");
            DropForeignKey("dbo.subscribedGroups", "groupId", "dbo.UserProfile");
            DropForeignKey("dbo.commentsLikes", "userID", "dbo.Comments");
            DropForeignKey("dbo.commentsLikes", "commentId", "dbo.UserProfile");
            DropForeignKey("dbo.messagesShares", "userID", "dbo.Messages");
            DropForeignKey("dbo.messagesShares", "messageId", "dbo.UserProfile");
            DropForeignKey("dbo.messagesLikes", "userID", "dbo.Messages");
            DropForeignKey("dbo.messagesLikes", "messageId", "dbo.UserProfile");
            DropForeignKey("dbo.groupsRequests", "groupID", "dbo.UserProfile");
            DropForeignKey("dbo.groupsRequests", "userId", "dbo.Groups");
            DropForeignKey("dbo.friendsOnline", "friendID", "dbo.UserProfile");
            DropForeignKey("dbo.friendsOnline", "userId", "dbo.UserProfile");
            DropForeignKey("dbo.friendshiprequest", "friendID", "dbo.UserProfile");
            DropForeignKey("dbo.friendshiprequest", "userId", "dbo.UserProfile");
            DropForeignKey("dbo.friends", "friendID", "dbo.UserProfile");
            DropForeignKey("dbo.friends", "userId", "dbo.UserProfile");
            DropForeignKey("dbo.Pictures", "PictureMessage_MessageId", "dbo.Messages");
            DropForeignKey("dbo.Pictures", "owner_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Comments", "ParentMessage_MessageId", "dbo.Messages");
            DropForeignKey("dbo.Comments", "writer_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Groups", "Admin_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Messages", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Messages", "sender_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Messages", "writer_UserId", "dbo.UserProfile");
            DropTable("dbo.subscribedGroups");
            DropTable("dbo.commentsLikes");
            DropTable("dbo.messagesShares");
            DropTable("dbo.messagesLikes");
            DropTable("dbo.groupsRequests");
            DropTable("dbo.friendsOnline");
            DropTable("dbo.friendshiprequest");
            DropTable("dbo.friends");
            DropTable("dbo.Pictures");
            DropTable("dbo.Comments");
            DropTable("dbo.Groups");
            DropTable("dbo.Messages");
            DropTable("dbo.UserProfile");
        }
    }
}
