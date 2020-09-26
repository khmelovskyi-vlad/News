using System;
using System.Collections.Generic;

namespace News.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public List<Comment> Comments { get; set; }
    }
}