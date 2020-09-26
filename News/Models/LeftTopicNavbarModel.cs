using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Models
{
    public class LeftTopicNavbarModel
    {
        public List<Topic> Topics { get; set; }
        public Guid? SubTopicId { get; set; }
    }
}
