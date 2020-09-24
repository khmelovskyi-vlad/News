using System;
using System.Collections.Generic;

namespace News.Models
{
    public class Author
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Information { get; set; }
        public List<ArticleAuthor> ArticleAuthors { get; set; }
    }
}