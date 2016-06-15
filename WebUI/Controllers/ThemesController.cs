﻿using Domain;
using Domain.Abstract;
using Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebUI.Filters;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class ThemesController : Controller
    {
        private IThemeRepository repository;
        public int pageSize = 4;

        public ThemesController ( IThemeRepository repo)
        {
            repository = repo;
        }

        [Authorize(Roles="Admin, Student, Teacher")]
        public ViewResult List(int page = 1)
        {
            ThemesListViewModel model = new ThemesListViewModel
            {
                Themes = repository.Themes
                .OrderBy(theme => theme.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = repository.Themes.Count()
                }
            };
            
            return View(model);
        }

        [Authorize(Roles="Student, Admin")]
        public ViewResult Take(int id)
        {
            ThemesListViewModel model = new ThemesListViewModel
            {
                Themes = repository.Themes
            };
            string title = "";
            foreach (var t in model.Themes)
            {
                if (id == t.Id)
                {
                    title = t.Title;
                }
            }
            //Theme modelone = new Theme
            //{
            //    Title = title
            //};
            Article modelone = new Article
            {
                Title = title
            };
            return View(modelone);
        }

        [Authorize(Roles = "Student, Admin")]
        [HttpPost]
        public ActionResult Take(int id, string tit, Article model1)
        {
            string title = "";
            using (var context = new EFDbContext())
            {
                if (id != null && id != 0)
                {
                    foreach (var t in context.Themes)
                    {
                        if (t.Id == id)
                        {
                            title = t.Title;
                        }
                    }
                    Article art = context.Articles
                       .Where(c => c.Title == title)
                       .FirstOrDefault();
                    art.Text = model1.Text;
                    if (art.Student == null)
                    {
                        art = new Article();
                        int iD = 0;
                        try
                        {
                            iD = context.Themes.Max(x => x.Id);
                        }
                        catch
                        { }
                        iD++;
                        foreach (var t in context.Themes)
                        {
                            if (t.Id == id)
                            {
                                art.Title = t.Title;
                                art.Teacher = t.Teacher;
                            }
                        }
                        art.Student = "Default Student";
                        //context.Articles.Add(art);
                        //context.SaveChanges();
                    }
                    context.Articles.Add(art);
                    context.SaveChanges();
                }
                
            }


            return RedirectToAction("List", "Themes");
        }

        
	}
}