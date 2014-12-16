namespace FileExchange.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExchangeFiles",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        FileCategoryId = c.Int(nullable: false),
                        UniqFileName = c.String(maxLength: 56),
                        OrigFileName = c.String(maxLength: 128),
                        Tags = c.String(maxLength: 1024),
                        CreateDate = c.DateTime(nullable: false),
                        ModifyDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FileId)
                .ForeignKey("dbo.FileCategories", t => t.FileCategoryId)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.FileCategoryId);
            
            CreateTable(
                "dbo.FileCategories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.FileComments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        FileId = c.Int(nullable: false),
                        Comment = c.String(maxLength: 1024),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.ExchangeFiles", t => t.FileId)
                .Index(t => t.FileId);
            
            CreateTable(
                "dbo.FileNotificationSubscribers",
                c => new
                    {
                        Subscriberid = c.String(nullable: false, maxLength: 128),
                        FileId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Subscriberid)
                .ForeignKey("dbo.ExchangeFiles", t => t.FileId)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .Index(t => t.FileId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        UserEmail = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.webpages_Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.webpages_UsersInRoles",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.webpages_Roles", t => t.RoleId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.webpages_Membership",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        CreateDate = c.DateTime(),
                        ConfirmationToken = c.String(maxLength: 128),
                        IsConfirmed = c.Boolean(),
                        LastPasswordFailureDate = c.DateTime(),
                        PasswordFailuresSinceLastSuccess = c.Int(nullable: false),
                        Password = c.String(nullable: false, maxLength: 128),
                        PasswordChangedDate = c.DateTime(),
                        PasswordSalt = c.String(nullable: false, maxLength: 128),
                        PasswordVerificationToken = c.String(maxLength: 128),
                        PasswordVerificationTokenExpirationDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.UserProfile", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        NewsId = c.Int(nullable: false, identity: true),
                        Header = c.String(maxLength: 255),
                        Text = c.String(),
                        UniqImageName = c.String(maxLength: 56),
                        OrigImageName = c.String(maxLength: 128),
                        CreateDate = c.DateTime(nullable: false),
                        ModifyDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.NewsId);
            
            CreateTable(
                "dbo.UserRolesUserProfiles",
                c => new
                    {
                        UserRoles_RoleId = c.Int(nullable: false),
                        UserProfile_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserRoles_RoleId, t.UserProfile_UserId })
                .ForeignKey("dbo.webpages_Roles", t => t.UserRoles_RoleId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserProfile_UserId, cascadeDelete: true)
                .Index(t => t.UserRoles_RoleId)
                .Index(t => t.UserProfile_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.webpages_Membership", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.ExchangeFiles", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.FileNotificationSubscribers", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.UserRolesUserProfiles", "UserProfile_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.UserRolesUserProfiles", "UserRoles_RoleId", "dbo.webpages_Roles");
            DropForeignKey("dbo.webpages_UsersInRoles", "RoleId", "dbo.webpages_Roles");
            DropForeignKey("dbo.FileNotificationSubscribers", "FileId", "dbo.ExchangeFiles");
            DropForeignKey("dbo.FileComments", "FileId", "dbo.ExchangeFiles");
            DropForeignKey("dbo.ExchangeFiles", "FileCategoryId", "dbo.FileCategories");
            DropIndex("dbo.UserRolesUserProfiles", new[] { "UserProfile_UserId" });
            DropIndex("dbo.UserRolesUserProfiles", new[] { "UserRoles_RoleId" });
            DropIndex("dbo.webpages_Membership", new[] { "UserId" });
            DropIndex("dbo.webpages_UsersInRoles", new[] { "RoleId" });
            DropIndex("dbo.FileNotificationSubscribers", new[] { "UserId" });
            DropIndex("dbo.FileNotificationSubscribers", new[] { "FileId" });
            DropIndex("dbo.FileComments", new[] { "FileId" });
            DropIndex("dbo.ExchangeFiles", new[] { "FileCategoryId" });
            DropIndex("dbo.ExchangeFiles", new[] { "UserId" });
            DropTable("dbo.UserRolesUserProfiles");
            DropTable("dbo.News");
            DropTable("dbo.webpages_Membership");
            DropTable("dbo.webpages_UsersInRoles");
            DropTable("dbo.webpages_Roles");
            DropTable("dbo.UserProfile");
            DropTable("dbo.FileNotificationSubscribers");
            DropTable("dbo.FileComments");
            DropTable("dbo.FileCategories");
            DropTable("dbo.ExchangeFiles");
        }
    }
}
