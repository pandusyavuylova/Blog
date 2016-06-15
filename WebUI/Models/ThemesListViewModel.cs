using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebUI.HTMLHelpers;

namespace WebUI.Models
{
    public class ThemesListViewModel
    {
        public IEnumerable<Theme> Themes { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}