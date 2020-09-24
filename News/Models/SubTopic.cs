using System;
using System.Collections.Generic;

namespace News.Models
{
    public class SubTopic
    {
        public Guid Id { get; set; }
        public string Value { get; set; }

        public Topic Topic { get; set; }
        public Guid TopicId { get; set; }
        public List<Article> Articles { get; set; }
    }
}