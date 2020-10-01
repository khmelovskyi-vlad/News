using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Models
{
    public class ArticleCreateModel
    {
        public Article Article { get; set; }
        public List<SelectListItem> SubTopics { get; set; }
        public List<SelectListItem> Admins { get; set; }
        public List<SelectListItem> Authors { get; set; }
        public List<Guid> ArticleAuthors { get; set; }
        public IFormFile ArticleImage { get; set; }
    }
}
