using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Domain.Abstract;
using Domain;
using System.Collections.Generic;
using WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models;
using WebUI.HTMLHelpers;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //органызація
            Mock<IThemeRepository> mock = new Mock<IThemeRepository>();
            mock.Setup(m => m.Themes).Returns(new List<Theme>
                {
                    new Theme {Id = 1, Title = "theme 1", Description = "Good", Teacher = ""},
                    new Theme {Id = 2, Title = "theme 2", Description = "Good", Teacher = ""},
                    new Theme {Id = 3, Title = "theme 3", Description = "Good", Teacher = ""},
                    new Theme {Id = 4, Title = "theme 4", Description = "Good", Teacher = ""},
                    new Theme {Id = 5, Title = "theme 5", Description = "Good", Teacher = ""},
                    new Theme {Id = 6, Title = "theme 6", Description = "Good", Teacher = ""},
                    new Theme {Id = 7, Title = "theme 7", Description = "Good", Teacher = ""},
                    new Theme {Id = 8, Title = "theme 8", Description = "Good", Teacher = ""}
                });

            ThemesController controller = new ThemesController(mock.Object);
            controller.pageSize = 3;

            //Дія
            ThemesListViewModel result = (ThemesListViewModel)controller.List(3).Model;
            
            //Ствердження
            List<Theme> themes = result.Themes.ToList();

            Assert.IsTrue(themes.Count == 2);
            Assert.AreEqual(themes[0].Title, "theme 7");
            Assert.AreEqual(themes[1].Title, "theme 8");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //Організація
            HtmlHelper myHelper = null;
            PagingInfo pagInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            //Дія
            MvcHtmlString result = myHelper.PageLinks(pagInfo, pageUrlDelegate);

            //Ствердження
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IThemeRepository> mock = new Mock<IThemeRepository>();
            mock.Setup(m => m.Themes).Returns(new List<Theme>
                {
                    new Theme {Id = 1, Title = "theme 1", Description = "Good", Teacher = ""},
                    new Theme {Id = 2, Title = "theme 2", Description = "Good", Teacher = ""},
                    new Theme {Id = 3, Title = "theme 3", Description = "Good", Teacher = ""},
                    new Theme {Id = 4, Title = "theme 4", Description = "Good", Teacher = ""},
                    new Theme {Id = 5, Title = "theme 5", Description = "Good", Teacher = ""},
                });

            ThemesController controller = new ThemesController(mock.Object);
            controller.pageSize = 3;

            ThemesListViewModel result = (ThemesListViewModel)controller.List(2).Model;
            PagingInfo pagInfo = result.PagingInfo;
            Assert.AreEqual(pagInfo.CurrentPage, 2);
            Assert.AreEqual(pagInfo.ItemsPerPage, 3);
            Assert.AreEqual(pagInfo.TotalItems, 5);
            Assert.AreEqual(pagInfo.TotalPages, 2);
        }
    }
}
