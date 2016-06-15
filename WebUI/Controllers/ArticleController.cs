using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using WebUI.Models;
using Domain;
using Domain.Concrete;

namespace WebUI.Controllers
{
    public class ArticleController : Controller
    {
        private IArticleRepository repository;
        public int pageSize = 4;

        public ArticleController(IArticleRepository repo)
        {
            repository = repo;
        }

        [Authorize(Roles="Admin, Student, Teacher")]
        public ViewResult List(int page = 1)
        {
            ArticleListViewModel model = new ArticleListViewModel
            {
                Articles = repository.Articles
                .OrderBy(theme => theme.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = repository.Articles.Count()
                }
            };
            
            return View(model);
        }

        [Authorize(Roles="Admin, Teacher, Student")]
        public ViewResult Watch(int id)
        {
            ArticleListViewModel model = new ArticleListViewModel
            {
                Articles = repository.Articles
            };
            string title = "";
            string text = "";
            int note = 0;
            string teacher = "";
            string student = "";
            foreach (var t in model.Articles)
            {
                if (t.Id == id)
                {
                    title = t.Title;
                    text = t.Text;
                    note = t.Note;
                    teacher = t.Teacher;
                    student = t.Student;
                }
            }
            Article modelone = new Article
            {
                Id = id,
                Title = title,
                Text = text,
                Note = note,
                Teacher = teacher,
                Student = student
            };
            return View(modelone);
        }

        [Authorize(Roles="Teacher, Admin")]
        public ActionResult Note(int id, int note)
        {
            EFDbContext context = new EFDbContext();
            foreach (var t in context.Articles)
            {
                if (t.Id == id)
                {
                    t.Note = note;
                }
            }
            context.SaveChanges();
            return RedirectToAction("Watch", new { id = id});
        }
    }
}