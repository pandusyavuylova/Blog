using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class EFThemeRepository: IThemeRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<Theme> Themes
        {
            get { return context.Themes; }
        }
    }
}
