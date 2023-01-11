using Microsoft.EntityFrameworkCore;
using server.Models;

namespace server
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleComment> ArticleComments { get; set; }
        public DbSet<ArticleKeyWord> ArticleKeyWords { get; set; }
        public DbSet<ArticleLike> ArticleLikes { get; set; }
        public DbSet<ArticlePage> ArticlePages { get; set; }
        public DbSet<ArticlePageComment> ArticlePageComments { get; set; }
        public DbSet<ArticlePageLike> ArticlePageLikes { get; set; }
        public DbSet<BlockList> BlockList { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentAttachment> CommentAttachments { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Favorite> Favorites{ get; set; }
        public DbSet<FavoriteArticle> FavoriteArticles { get; set; }
        public DbSet<FavoritePost> FavoritePosts { get; set; }
        public DbSet<FavoriteGroup> FavoriteGroups { get; set; }
        public DbSet<Models.File> Files { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupFile> GroupFiles { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<GroupMemberRole> GroupMemberRoles { get; set; }
        public DbSet<GroupPost> GroupPosts { get; set; }
        public DbSet<KeyWord> KeyWords { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostAttachment> PostAttachments { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<ReplyComment> ReplyComments { get; set; }
        public DbSet<RequestToGroup> RequestToGroup { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserFile> UserFiles { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<UserInfoPrivacyType> UserInfoPrivacyTypes { get; set; }
        public DbSet<UserInterest> UserInterests { get; set; }
        public DbSet<UserNote> UserNotes { get; set; }
        public DbSet<UserPost> UserPosts { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserStatus> UserStatuses { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public ApplicationContext (DbContextOptions options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Article>().HasIndex(e => e.Title).IsUnique().HasDatabaseName("Articles_Title_Index");
            builder.Entity<Article>().Property(x=>x.PublicationDate).HasDefaultValueSql("getdate()");
            builder.Entity<ArticlePage>().Property(x => x.PublicationDate).HasDefaultValueSql("getdate()");
            builder.Entity<BlockList>().Property(x => x.DateFrom).HasDefaultValue(DateTime.Now);
            builder.Entity<Comment>().Property(x => x.IsReply).HasDefaultValue(false);
            //builder.Entity<Models.File>().HasIndex(e => e.).IsUnique();
            builder.Entity<Country>().HasIndex(x => x.CountryNameRu).IsUnique().HasDatabaseName("Countries_CountryNameRu_Index");
            builder.Entity<Country>().HasIndex(x => x.CountryNameEn).IsUnique().HasDatabaseName("Countries_CountryNameEn_Index");
            builder.Entity<Comment>().HasMany(x => x.ReplyComments).WithOne(x => x.MajorComment).HasForeignKey(x => x.MajorCommentId);
            //builder.Entity<ReplyComment>().HasOne(x => x.RepliedComment).WithOne(x => x.).HasForeignKey(x => x.C);
            //builder.Entity<Models.File>().HasIndex(x => x.FileLink).IsUnique().HasDatabaseName("Files_FileLink_Index");
            builder.Entity<Friend>().HasOne(x => x.User1).WithMany(x => x.Friends1).HasForeignKey(x => x.User1Id);
            builder.Entity<Friend>().HasOne(x => x.User2).WithMany(x => x.Friends2).HasForeignKey(x => x.User2Id);
            builder.Entity<FriendRequest>().HasOne(x => x.Sender).WithMany(x => x.FriendRequestsSenders).HasForeignKey(x => x.SenderId);
            builder.Entity<FriendRequest>().HasOne(x => x.User).WithMany(x => x.FriendRequestsReceivers).HasForeignKey(x => x.UserId);
            builder.Entity<FriendRequest>().Property(x => x.Message).HasDefaultValue("Добрый день, я бы хотел добавить вас в друзья :)");
            builder.Entity<Group>().HasIndex(x => x.GroupName).IsUnique().HasDatabaseName("Groups_GroupName_Index");
            builder.Entity<KeyWord>().HasIndex(x => x.KeyWordRu).IsUnique().HasDatabaseName("KeyWords_KeyWordRu_Index");
            builder.Entity<KeyWord>().HasIndex(x => x.KeyWordEn).IsUnique().HasDatabaseName("KeyWords_KeyWordEn_Index");
            builder.Entity<Notification>().Property(x => x.IsViewed).HasDefaultValue(false);
            builder.Entity<Post>().Property(x => x.PublicationDate).HasDefaultValueSql("getdate()");
            builder.Entity<Post>().Property(x => x.Title).HasDefaultValue("");
            builder.Entity<Subscribe>().HasOne(x => x.Sub).WithMany(x => x.SubSub).HasForeignKey(x => x.SubId);
            builder.Entity<Subscribe>().HasOne(x => x.User).WithMany(x => x.SubUser).HasForeignKey(x => x.UserId);
            builder.Entity<User>().HasIndex(x => x.Email).IsUnique().HasDatabaseName("User_Email_Index");
            builder.Entity<User>().HasIndex(x => x.Login).IsUnique().HasDatabaseName("User_Login_Index");
            builder.Entity<User>().HasIndex(x => x.Nickname).IsUnique().HasDatabaseName("User_Nickname_Index");
            //builder.Entity<User>().HasOne(x => x.Image).WithOne(x => x.UsersImage).HasForeignKey<User>(x => x.ImageId);
            builder.Entity<User>().Property(x => x.RegistrationDate).HasDefaultValueSql("getdate()");
            builder.Entity<User>().Property(x => x.IsVerified).HasDefaultValue(false);
            builder.Entity<UserInfo>().Property(x => x.Status).HasDefaultValue("");
            builder.Entity<UserNote>().Property(x => x.Title).HasDefaultValue("");
            builder.Entity<UserRole>().Property(x => x.RoleName).HasDefaultValue("USER");
            builder.Entity<UserStatus>().Property(x => x.StatusFrom).HasDefaultValueSql("getdate()");
            builder.Entity<UserStatus>().Property(x => x.StatusName).HasDefaultValue("NORMAL");
            //создание стандартных значений

            Country country = new Country { CountryID = 1, CountryNameRu = "Казахстан", CountryNameEn = "Kazakhstan" };
            builder.Entity<Country>().HasData(
                country,
                new Country { CountryID = 2, CountryNameRu = "Россия", CountryNameEn = "Russia" },
                new Country { CountryID = 3, CountryNameRu = "Узбекистан", CountryNameEn = "Uzbekistan" }
            );
            builder.Entity<KeyWord>().HasData(
                new KeyWord { KeyWordId = 1 ,KeyWordRu = "информационные системы", KeyWordEn = "information systems" },
                new KeyWord { KeyWordId = 2, KeyWordRu = "веб-технологии", KeyWordEn = "web technologies" },
                new KeyWord { KeyWordId = 3, KeyWordRu = "информационные технологии", KeyWordEn = "information technology" }
            );

            //добавление политик для информации пользователя
            UserInfoPrivacyType privacyPublic = new UserInfoPrivacyType { UserInfoPrivacyTypeId = 1, UserInfoPrivacyTypeName = "PublicPage" };
            UserInfoPrivacyType privacyPrivate = new UserInfoPrivacyType { UserInfoPrivacyTypeId = 2, UserInfoPrivacyTypeName = "PrivatePage" };
            UserInfoPrivacyType privacyFriendsOnly = new UserInfoPrivacyType { UserInfoPrivacyTypeId = 3, UserInfoPrivacyTypeName = "FriendsOnlyPage" };
            builder.Entity<UserInfoPrivacyType>().HasData(privacyPublic, privacyPrivate, privacyFriendsOnly);
            UserInfo ui_def = new UserInfo {
                UserInfoId = 1,
                Age = 20,
                DateOfBirth = new DateTime(2001, 12, 26),
                CountryId = 1,
                //Country = country,
                City = "Астана",
                Status = "Главные разработчик",
                Education = "ЕНУ им. Л.Н. Гуимлева",
                UserInfoPrivacyTypeId = 1,
                //UserInfoPrivacyType = privacyPublic
            };


            builder.Entity<UserInfo>().HasData(ui_def);
            UserToken token = new UserToken
            {
                TokenId = 1,
                RefreshToken = "some token"
            };

            builder.Entity<UserToken>().HasData(token);


            //добавление дефолтных картинок на сервер
            Models.File default_image = new Models.File { 
                FileId = 1,
                LogicalName = "default_avatar.png",
                FileType = "IMAGE",
                PublicationDate = DateTime.Now,
                FileLink = "/files/images/default_avatar.png",
                PhysicalName = "default_avatar.png",
            };
            Models.File default_group_image = new Models.File
            {
                FileId = 2,
                LogicalName = "default_group_image.png",
                FileType = "IMAGE",
                PublicationDate = DateTime.Now,
                FileLink = "/files/images/default_group_image.png",
                PhysicalName = "default_group_image.png"
            };
            //добавление дефолтных картинок на сервер
            builder.Entity<Models.File>().HasData(default_image, default_group_image);

            ////добавление ролей 
            UserRole admin_role = new UserRole { UserRoleId = 1, RoleName = "ADMIN" };
            UserRole user_role = new UserRole { UserRoleId = 2, RoleName = "USER" };
            builder.Entity<UserRole>().HasData(admin_role, user_role);

            //добавление статуса первого пользователя
            UserStatus st = new UserStatus { StatusFrom = DateTime.Now, UserStatusId = 1, StatusName = "NORMAL" };
            builder.Entity<UserStatus>().HasData(st);


            //добавление первого пользователя
            builder.Entity<User>().HasData(
                new User{
                     UserId = 1,
                    Login = "vecktor_21",
                    Password = BCrypt.Net.BCrypt.HashPassword("root"),
                    Email = "denis@mail.ru",
                    IsVerified = true,
                    Nickname = "vecktor_21",
                    Name = "Денис",
                    Surname = "Одноуров",
                    RegistrationDate = DateTime.Now,
                    TokenId = 1,
                    //Token = token,
                    ImageId = 1,
                    //Image = default_image,
                    UserInfoId = 1,
                    //UserInfo = ui_def,
                    RoleId = 1,
                    //Role = admin_role,
                    UserStatusId = 1,
                    //UserStatus = st
                }
            );


            //создание ролей пользователей групп
            GroupMemberRole adminRole = new GroupMemberRole
            {
                GroupMemberRoleId = 1,
                Name = "Admin",
                NameRu = "Администратор"
            };
            GroupMemberRole moderatorRole = new GroupMemberRole
            {
                GroupMemberRoleId = 2,
                Name = "Moderator",
                NameRu = "Модератор"
            };
            GroupMemberRole userRole = new GroupMemberRole
            {
                GroupMemberRoleId = 3,
                Name = "User",
                NameRu = "Пользователь"
            };

            builder.Entity<GroupMemberRole>().HasData(moderatorRole, adminRole, userRole);
        }
    }
}
