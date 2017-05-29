using NewsWebSite.Models.SignalrModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsWebSite.Models.Repository
{
    public interface ICommentsRepository
    {
        Comment GetById(int id);
        int Save(Comment comment);
        int GetArticleId(int id);
        int GetReplyCommentId(int id);
        IList<Comment> GetList(int articleId);
        CommentInfo GetCommentInfo(int id);
    }
}
