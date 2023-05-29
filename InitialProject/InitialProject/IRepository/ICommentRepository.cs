using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface ICommentRepository
    {
        List<OwnerComment> FindAllOwnerComments();

        List<Guest1Comment> FindAllGuestComments();

        List<OwnerComment> FindOwnerComments(int forumId);

        List<Guest1Comment> FindGuestComments(int forumId);
    }
}
