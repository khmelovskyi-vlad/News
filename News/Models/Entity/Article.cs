using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace News.Models
{
    public class Article
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        [Display(Name = "Date of creation")]
        public DateTime DateOfCreation { get; set; }
        [Display(Name = "Date of publication")]
        public DateTime? DateOfPublication { get; set; }
        public bool IsPublish { get; set; }
        public Guid ImageId { get; set; }
        public string Content { get; set; }


        public Admin Admin { get; set; }
        public Guid AdminId { get; set; }
        public SubTopic SubTopic { get; set; }
        public Guid SubTopicId { get; set; }
        public List<Comment> Comments { get; set; }
        public List<ArticleAuthor> ArticleAuthors { get; set; }
    }
}
