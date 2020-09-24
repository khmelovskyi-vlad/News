using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Models
{
    public class Article
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime DateOfCreation { get; set; }
        public DateTime DateOfPublication { get; set; }
        public bool IsPublish { get; set; }
        public Guid PhotoName { get; set; }
        public string Content { get; set; }


        public Admin AddedBy { get; set; }
        public Guid AdminId { get; set; }
        public SubTopic SubTopic { get; set; }
        public Guid SubTopicId { get; set; }
        public List<Comment> Comments { get; set; }
        public List<ArticleAuthor> ArticleAuthors { get; set; }
    }
}
