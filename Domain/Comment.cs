﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Comment
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public string Text { get; set; }
        public int OwnerId { get; set; }
    }
}
