using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Data
{
    public class Sort
    {
        public string FieldName { get; set; }
        public bool IsDescending { get; set; }
        public int PageNumber { get; set; }
    }
}
