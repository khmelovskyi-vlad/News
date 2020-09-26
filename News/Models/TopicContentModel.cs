using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Models
{
    public class TopicContentModel
    {
        public List<Topic> Topics { get; set; }
        public List<Article> Articles { get; set; }
        public string FieldName { get; set; }
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        public Guid? TopicId { get; set; }
        public Guid? SubTopicId { get; set; }
        public bool IsDescending { get; set; }
    }
}
