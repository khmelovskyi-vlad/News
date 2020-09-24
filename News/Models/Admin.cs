using System;
using System.Collections.Generic;

namespace News.Models
{
    public class Admin
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateCreationAccount { get; set; }
        public List<Article> Articles { get; set; }
    }
}