using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HKSJ.WBVV.Business.Interface.Base;
using HKSJ.WBVV.Entity;
using HKSJ.WBVV.Entity.ViewModel;
using HKSJ.WBVV.Entity.ViewModel.Client;

namespace HKSJ.WBVV.Business.Interface
{
    public interface ICommentBusiness : IBaseBusiness
    {

        IQueryable<CommentView> GetParentCommentVideos(int videoId);
        IQueryable<CommentView> GetChildCommentVideos(int videoId, Comments parentComment);
        IQueryable<CommentView> GetChildCommentVideos(int videoId, int pId);
        IList<CommentView> GetVideoComments(int videoId, int pId);
        PageResult GetVideoComments(int videoId, int pId, int size, int index);
        PageResult GetVideoComments(int videoId, int size, int index);
        int CreateVideoComment(int videoId, string commentContent);
        int ReplyVideoComment(int videoId, int commentId, string commentContent);
        bool DeleteVideoComment(int videoId, int commentId);
        bool DeleteVideoComment(int commentId);

        IQueryable<CommentView> GetParentCommentUsers(int ownerUserId);
        IQueryable<CommentView> GetChildCommentUsers(int ownerUserId,Comments parentComment);
        IQueryable<CommentView> GetChildCommentUsers(int videoId, int pId);
        IList<CommentView> GetSpaceComments(int ownerUserId, int pId);
        PageResult GetSpaceComments(int ownerUserId, int pId, int size, int index);
        PageResult GetSpaceComments(int ownerUserId, int size, int index);
        int CreateSpaceComment(int ownerUserId, string commentContent);
        int ReplySpaceComment(int ownerUserId, int commentId, string commentContent);
        bool DeleteSpaceComment(int ownerUserId, int commentId);
        bool DeleteSpaceComment(int commentId);
    }
}
