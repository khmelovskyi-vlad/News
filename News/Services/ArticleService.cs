using Microsoft.EntityFrameworkCore;
using News.Data;
using News.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace News.Services
{
    public class ArticleService : IArticleService
    {
        public IQueryable<Article> SortArticles(IQueryable<Article> articles, Sort sort, int maxArticlesOnPage)
        {
            return OrderBy(articles, sort).Skip((sort.PageNumber - 1) * maxArticlesOnPage).Take(maxArticlesOnPage);
        }
        private IQueryable<Article> OrderBy(IQueryable<Article> articles, Sort sort)
        {
            var fields = sort.FieldName.Split('.');
            var type = typeof(Article);
            var property = type.GetProperty(fields[0]);
            var parameter = Expression.Parameter(type, "parameter");
            var orderByAccess = Expression.MakeMemberAccess(parameter, property);
            for (int i = 1; i < fields.Length; i++)
            {
                property = property.PropertyType.GetProperty(fields[i]);
                orderByAccess = Expression.MakeMemberAccess(orderByAccess, property);
            }
            var orderByExpression = Expression.Lambda(orderByAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable),
                sort.IsDescending ? "OrderByDescending" : "OrderBy",
                //sort.MethodName,
                new Type[] { type, property.PropertyType },
                articles.Expression,
                Expression.Quote(orderByExpression));
            return articles.Provider.CreateQuery<Article>(resultExpression);
        }
        //private void CreateField(Type type, string[] fieldNames)
        //{
        //    var property = type.GetProperty(fieldNames[0]);
        //    var parameter = Expression.Parameter(type, "parameter");
        //    var orderByAccess = Expression.MakeMemberAccess(parameter, property);
        //    for (var i = 1; i < fieldNames.Length; i++)
        //    {
        //        property = property.PropertyType.GetProperty(fieldNames[i]);
        //        orderByAccess = Expression.MakeMemberAccess(orderByAccess, property);
        //    }
        //    return new Field(property.PropertyType, parameter, orderByAccess);
        //}
    }
}
