using System;
using System.Data;
using System.Collections.Generic;
using HKSJ.WBVV.Repository.Interface.IBase;
using HKSJ.WBVV.Entity;

namespace HKSJ.WBVV.Repository.Interface
{
    public interface  ICommentsRepository:IBaseAccess<Comments>
    {
        int CreateVideoComment(int loginUserId, int videoId, string commentContent);
        int ReplyVideoComment(int loginUserId, int videoId, int commentId, string commentContent);
        bool DeleteVideoComment(int loginUserId, int videoId, int commentId);
        bool DeleteVideoComment(int loginUserId, int commentId);

        int CreateSpaceComment(int loginUserId, int toUserId, string commentContent);
        int ReplySpaceComment(int loginUserId, int toUserId, int commentId, string commentContent);
        bool DeleteSpaceComment(int loginUserId, int toUserId, int commentId);
        bool DeleteSpaceComment(int loginUserId, int commentId);

    }
}