using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Models
{
    public class ArticleAuthor
    {
        public Article Article { get; set; }
        public Guid ArticleId { get; set; }
        public Author Author { get; set; }
        public Guid AuthorId { get; set; }
    }
}
