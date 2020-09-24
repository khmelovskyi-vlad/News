using System;
using System.Collections.Generic;

namespace News.Models
{
    public class Topic
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public List<SubTopic> SubTopics { get; set; }
    }
}