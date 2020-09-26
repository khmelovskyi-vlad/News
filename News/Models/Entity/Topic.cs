using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace News.Models
{
    public class Topic
    {
        public Guid Id { get; set; }
        [Display(Name = "Topic")]
        public string Value { get; set; }
        public List<SubTopic> SubTopics { get; set; }
    }
}