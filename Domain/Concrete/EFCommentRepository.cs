using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class EFCommentRepository : ICommentRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<Comment> Comments
        {
            get { return context.Comments; }
        }
    }
}
