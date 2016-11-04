using HKSJ.WBVV.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Entity.Tables;

namespace HKSJ.WBVV.Model.Context
{
    public partial class DataContext
    {
        /// <summary>
        /// API授权验证表
        /// </summary>
        public DbSet<AuthKeys> AuthKeys { get; set; }
        /// <summary>
        /// 头部推荐
        /// </summary>
        public DbSet<BannerVideo> BannerVideo { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public DbSet<Category> Category { get; set; }
        /// <summary>
        /// 评论表
        /// </summary>
        public DbSet<Comments> Comments { get; set; }
        /// <summary>
        /// 字典
        /// </summary>
        public DbSet<Dictionary> Dictionary { get; set; }
        /// <summary>
        /// 字典节点
        /// </summary>
        public DbSet<DictionaryItem> DictionaryItem { get; set; }
        /// <summary>
        /// 搜索关键词表
        /// </summary>
        public DbSet<KeyWords> KeyWords { get; set; }
        /// <summary>
        /// 语言表
        /// </summary>
        public DbSet<Language> Language { get; set; }
        /// <summary>
        /// 用户等级表
        /// </summary>
        public DbSet<Level> Level { get; set; }
        /// <summary>
        /// 管理员表
        /// </summary>
        public DbSet<Manage> Manage { get; set; }
        /// <summary>
        /// 管理员日志表
        /// </summary>
        public DbSet<ManageLog> ManageLog { get; set; }
        /// <summary>
        /// 管理菜单
        /// </summary>
        public DbSet<ManageMenu> ManageMenu { get; set; }
        /// <summary>
        /// 读消息表
        /// </summary>
        public DbSet<MessageRead> MessageRead { get; set; }
        /// <summary>
        /// 后台权限表
        /// </summary>
        public DbSet<Permission> Permission { get; set; }
        /// <summary>
        /// 板块
        /// </summary>
        public DbSet<Plate> Plate { get; set; }
        /// <summary>
        /// 视频和板块中间表
        /// </summary>
        public DbSet<PlateVideo> PlateVideo { get; set; }
        /// <summary>
        /// 点赞表
        /// </summary>
        public DbSet<Praises> Praises { get; set; }
        /// <summary>
        /// 七牛预处理日志
        /// </summary>
        public DbSet<QiniuFopLog> QiniuFopLog { get; set; }
        /// <summary>
        /// 角色表
        /// </summary>
        public DbSet<Role> Role { get; set; }
        /// <summary>
        /// 角色管理员菜单表
        /// </summary>
        public DbSet<RoleMenu> RoleMenu { get; set; }
        /// <summary>
        /// 短信验证记录表
        /// </summary>
        public DbSet<SmsRecord> SmsRecord { get; set; }
        /// <summary>
        /// 系统消息表
        /// </summary>
        public DbSet<SysMessage> SysMessage { get; set; }
        /// <summary>
        /// 标签表
        /// </summary>
        public DbSet<Tags> Tags { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        public DbSet<User> User { get; set; }
        /// <summary>
        /// 用户关注表
        /// </summary>
        public DbSet<UserAttention> UserAttention { get; set; }
        /// <summary>
        /// 用户第三方绑定表
        /// </summary>
        public DbSet<UserBind> UserBind { get; set; }
        /// <summary>
        /// 用户收藏表
        /// </summary>
        public DbSet<UserCollect> UserCollect { get; set; }
        /// <summary>
        /// 用户粉丝
        /// </summary>
        public DbSet<UserFans> UserFans { get; set; }
        /// <summary>
        /// 用户观看历史表
        /// </summary>
        public DbSet<UserHistory> UserHistory { get; set; }
        /// <summary>
        /// 用户等级表
        /// </summary>
        public DbSet<UserLevel> UserLevel { get; set; }
        /// <summary>
        /// 用户日志表
        /// </summary>
        public DbSet<UserLog> UserLog { get; set; }
        /// <summary>
        /// 用户推荐表
        /// </summary>
        public DbSet<UserRecommend> UserRecommend { get; set; }
        /// <summary>
        /// 举报表
        /// </summary>
        public DbSet<UserReport> UserReport { get; set; }
        /// <summary>
        /// 用户空间专辑、视频映射
        /// </summary>
        public DbSet<UserRoomChoose> UserRoomChoose { get; set; }
        /// <summary>
        /// 用户积分表
        /// </summary>
        public DbSet<UserScore> UserScore { get; set; }
        /// <summary>
        /// 用户积分规则表
        /// </summary>
        public DbSet<UserScoreRule> UserScoreRule { get; set; }
        /// <summary>
        /// 用户分享视频
        /// </summary>
        public DbSet<UserShare> UserShare { get; set; }
        /// <summary>
        /// 用户空间皮肤
        /// </summary>
        public DbSet<UserSkin> UserSkin { get; set; }
        /// <summary>
        /// 用户专辑
        /// </summary>
        public DbSet<UserSpecial> UserSpecial { get; set; }
        /// <summary>
        /// 用户专辑子表
        /// </summary>
        public DbSet<UserSpecialSon> UserSpecialSon { get; set; }
        /// <summary>
        /// 用户上传视频日志表
        /// </summary>
        public DbSet<UserUploadVedioLog> UserUploadVedioLog { get; set; }
        /// <summary>
        /// 用户访问日志
        /// </summary>
        public DbSet<UserVisitLog> UserVisitLog { get; set; }
        /// <summary>
        /// 视频
        /// </summary>
        public DbSet<Video> Video { get; set; }
        /// <summary>
        /// 视频审核信息表
        /// </summary>
        public DbSet<VideoApprove> VideoApprove { get; set; }
        /// <summary>
        /// 视频播放记录表
        /// </summary>
        public DbSet<VideoPlayRecord> VideoPlayRecord { get; set; }
        /// <summary>
        /// 审片
        /// </summary>
        public DbSet<VideoPrereview> VideoPrereview { get; set; }
    }
}