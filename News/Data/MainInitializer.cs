using Microsoft.EntityFrameworkCore;
using News.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News.Data
{
    public class MainInitializer : IInitializer
    {
        public void Run(ModelBuilder modelBuilder)
        {
            Random random = new Random();
            var authors = GetAuthors(random);
            var users = GetUsers(random);
            var topics = GetTopics(random);
            var admins = GetAdmins(random);
            var subTopics = GetSubTopics(random, topics);
            var articles = GetArticles(random, admins, subTopics);
            var comments = GetComments(random, articles, users);
            var articleAuthors = GetArticleAuthors(random, articles, authors);


            modelBuilder.Entity<Author>().HasData(authors);
            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<Topic>().HasData(topics);
            modelBuilder.Entity<Admin>().HasData(admins);
            modelBuilder.Entity<SubTopic>().HasData(subTopics);
            modelBuilder.Entity<Article>().HasData(articles);
            modelBuilder.Entity<Comment>().HasData(comments);
            modelBuilder.Entity<ArticleAuthor>().HasData(articleAuthors);
        }
        private List<Author> GetAuthors(Random random)
        {
            List<Author> authors = new List<Author>();
            for (int i = 0; i < 10; i++)
            {
                authors.Add(new Author()
                {
                    Id = Guid.NewGuid(),
                    FirstName = CreateRandomString(1, 50, "abcdefghijklmnopqrstuvwxyz", random),
                    LastName = CreateRandomString(1, 50, "abcdefghijklmnopqrstuvwxyz", random),
                    DateOfBirth = CreateRandomData(1900, 2020, random),
                    Information = CreateRandomString(1, 500, "abcdefghijklmnopqrstuvwxyz", random),
                });
            }
            return authors;
        }
        private List<User> GetUsers(Random random)
        {
            List<User> users = new List<User>();
            for (int i = 0; i < 15; i++)
            {
                users.Add(new User()
                {
                    Id = Guid.NewGuid(),
                    Login = CreateRandomString(1, 50, "abcdefghijklmnopqrstuvwxyz", random),
                    Password = CreateRandomString(1, 50, "abcdefghijklmnopqrstuvwxyz", random),
                    Email = CreateRandomString(1, 50, "abcdefghijklmnopqrstuvwxyz", random),
                });
            }
            return users;
        }
        private List<Topic> GetTopics(Random random)
        {
            List<Topic> topics = new List<Topic>();
            for (int i = 0; i < 10; i++)
            {
                topics.Add(new Topic()
                {
                    Id = Guid.NewGuid(),
                    Value = CreateRandomString(1, 50, "abcdefghijklmnopqrstuvwxyz", random)
                });
            }
            return topics;
        }
        private List<SubTopic> GetSubTopics(Random random, List<Topic> topics)
        {
            List<SubTopic> subTopics = new List<SubTopic>();
            foreach (var topic in topics)
            {
                for (int i = 0; i < 15; i++)
                {
                    if (CreateRandomBool(random, 50))
                    {
                        subTopics.Add(new SubTopic()
                        {
                            Id = Guid.NewGuid(),
                            Value = CreateRandomString(1, 50, "abcdefghijklmnopqrstuvwxyz", random),
                            TopicId = topic.Id
                        });
                    }
                }
            }
            return subTopics;
        }
        private List<Admin> GetAdmins(Random random)
        {
            List<Admin> admins = new List<Admin>();
            for (int i = 0; i < 5; i++)
            {
                admins.Add(new Admin()
                {
                    Id = Guid.NewGuid(),
                    FirstName = CreateRandomString(1, 50, "abcdefghijklmnopqrstuvwxyz", random),
                    LastName = CreateRandomString(1, 50, "abcdefghijklmnopqrstuvwxyz", random),
                    DateOfBirth = CreateRandomData(1900, 2020, random),
                    DateCreationAccount = DateTime.Now
                });
            }
            return admins;
        }
        private List<Article> GetArticles(Random random, List<Admin> admins, List<SubTopic> subTopics)
        {
            List<Article> articles = new List<Article>();
            foreach (var admin in admins)
            {
                foreach (var subTopic in subTopics)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        if (CreateRandomBool(random, 20))
                        {
                            articles.Add(new Article()
                            {
                                Id = Guid.NewGuid(),
                                Title = CreateRandomString(1, 50, "abcdefghijklmnopqrstuvwxyz", random),
                                DateOfCreation = CreateRandomData(1900, 2020, random),
                                DateOfPublication = DateTime.Now,
                                IsPublish = CreateRandomBool(random, 50),
                                PhotoName = Guid.NewGuid(),
                                Content = CreateRandomString(1, 50000, "abcdefghijklmnopqrstuvwxyz", random),
                                AdminId = admin.Id,
                                SubTopicId = subTopic.Id
                            });
                        }
                    }
                }
            }
            return articles;
        }
        private List<Comment> GetComments(Random random, List<Article> articles, List<User> users)
        {
            List<Comment> comments = new List<Comment>();
            foreach (var user in users)
            {
                foreach (var article in articles)
                {
                    if (CreateRandomBool(random, 20))
                    {
                        comments.Add(new Comment()
                        {
                            Id = Guid.NewGuid(),
                            Value = CreateRandomString(1, 200, "abcdefghijklmnopqrstuvwxyz", random),
                            UserId = user.Id,
                            ArticleId = article.Id
                        });
                    }
                }
            }
            return comments;
        }
        private List<ArticleAuthor> GetArticleAuthors(Random random, List<Article> articles, List<Author> authors)
        {
            List<ArticleAuthor> articleAuthors = new List<ArticleAuthor>();
            foreach (var article in articles)
            {
                foreach (var author in authors)
                {
                    if (CreateRandomBool(random, 20))
                    {
                        articleAuthors.Add(new ArticleAuthor()
                        {
                            ArticleId = article.Id,
                            AuthorId = author.Id
                        });
                    }
                }
            }
            return articleAuthors;
        }







        private DateTime CreateRandomData(int minYear, int maxYear, Random random)
        {
            var year = random.Next(minYear, maxYear);
            var month = random.Next(1, 13);
            int day;
            if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
            {
                day = random.Next(1, 32);
            }
            else if (month == 4 || month == 6 || month == 9 || month == 11)
            {
                day = random.Next(1, 31);
            }
            else // month == 2
            {
                if (year % 4 == 0)
                {
                    day = random.Next(1, 30);
                }
                else
                {
                    day = random.Next(1, 29);
                }
            }
            return new DateTime(year, month, day);
        }


        private string CreateRandomString(int minLength, int maxLenght, string chars, Random random)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < random.Next(minLength, maxLenght); i++)
            {
                stringBuilder.Append(chars[random.Next(0, chars.Length)]);
            }
            return stringBuilder.ToString();
        }
        private bool CreateRandomBool(Random random, int percent)
        {
            if (percent > random.Next(1, 101))
            {
                return true;
            }
            return false;
        }
    }
}
