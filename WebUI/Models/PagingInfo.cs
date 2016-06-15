using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class PagingInfo
    {
        public int TotalItems { get; set; }     //Загальна кількість елементів
        public int ItemsPerPage { get; set; }   //Кількість елементів на сторінці
        public int CurrentPage { get; set; }    //Номер даної сторінки
        public int TotalPages                   //Загальна кількість сторінок
        {
            get
            {
                return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
            }
        }
    }
}