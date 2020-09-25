using News.Data;
using News.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.Services
{
    public interface IArticleService
    {
        IQueryable<Article> SortArticles(IQueryable<Article> articles, Sort sort);
    }
}
