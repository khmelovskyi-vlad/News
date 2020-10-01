using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using News.Data;
using News.Models;


namespace News.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly NewsContext _context;

        public ArticlesController(NewsContext context)
        {
            _context = context;
        }
        //public async Task<IActionResult> Redact(Guid? id)
        //{
        //    var article = await _context
        //        .Articles
        //        .Where(art => art.Id == id)
        //        .Include(art => art.Comments)
        //        .FirstOrDefaultAsync();
        //    return View(article);
        //}
        [HttpPost]
        public async Task<IActionResult> ChangeArticle(Guid? id, Article newArticle)
        {
            var article = await _context
                .Articles
                .Where(art => art.Id == id)
                .Include(art => art.Comments)
                .FirstOrDefaultAsync();
            article.Title = newArticle.Title;
            article.DateOfCreation = newArticle.DateOfCreation;
            article.DateOfPublication = newArticle.DateOfPublication;
            article.IsPublish = newArticle.IsPublish;
            article.Content = newArticle.Content;
            await _context.SaveChangesAsync();
            return View("Index", article);
        }
        // GET: Articles
        public async Task<IActionResult> Index(Guid? id)
        {
            var article = await _context
                .Articles
                .Where(art => art.Id == id)
                .Include(art => art.Comments)
                .FirstOrDefaultAsync();
            return View(article);
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Admin)
                .Include(a => a.SubTopic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: Articles/Create
        public IActionResult Create()
        {
            ViewData["AdminId"] = new SelectList(_context.Admins, "Id", "Id");
            ViewData["SubTopicId"] = new SelectList(_context.SubTopics, "Id", "Id");
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,DateOfCreation,DateOfPublication,IsPublish,PhotoName,Content,AdminId,SubTopicId")] Article article)
        {
            if (ModelState.IsValid)
            {
                article.Id = Guid.NewGuid();
                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdminId"] = new SelectList(_context.Admins, "Id", "Id", article.AdminId);
            ViewData["SubTopicId"] = new SelectList(_context.SubTopics, "Id", "Id", article.SubTopicId);
            return View(article);
        }
        public IActionResult MyCreate()
        {
            var articleCreateModel = new ArticleCreateModel();
            var admins = new List<SelectListItem>();
            admins.AddRange(_context.Admins.Select(admin => new SelectListItem
            {
                Value = admin.Id.ToString(),
                Text = $"first name - {admin.FirstName}, last nane - {admin.LastName}, id - {admin.Id}"
            }).ToList());
            articleCreateModel.Admins = admins;

            var subtopics = new List<SelectListItem>();
            subtopics.AddRange(_context.SubTopics.Select(subtopic => new SelectListItem
            {
                Value = subtopic.Id.ToString(),
                Text = $"subtopic - {subtopic.Value}, topic - {subtopic.Topic.Value}"
            }).ToList());
            articleCreateModel.SubTopics = subtopics;

            var authors = new List<SelectListItem>();
            authors.AddRange(_context.Authors.Select(author => new SelectListItem
            {
                Value = author.Id.ToString(),
                Text = $"first name - {author.FirstName}, last name - {author.LastName} id - {author.Id}"
            }).ToList());
            articleCreateModel.Authors = authors;
            articleCreateModel.Article = new Article();

            ViewData["AdminId"] = new SelectList(_context.Admins, "Id", "Id");
            ViewData["SubTopicId"] = new SelectList(_context.SubTopics, "Id", "Id");
            return View(articleCreateModel);
        }
        [HttpPost]
        public async Task<IActionResult> MyCreate(ArticleCreateModel articleCreateModel)
        {
            if (ModelState.IsValid)
            {
                if (articleCreateModel.ArticleImage != null)
                {
                    var imgId = Guid.NewGuid();
                    await DownloadFile(articleCreateModel.ArticleImage, $"{imgId}.jpg");
                    articleCreateModel.Article.ImageId = imgId;

                }
                articleCreateModel.Article.DateOfCreation = DateTime.Now;
                if (articleCreateModel.Article.IsPublish)
                {
                    articleCreateModel.Article.DateOfPublication = DateTime.Now;
                }
                articleCreateModel.Article.Id = Guid.NewGuid();
                _context.Add(articleCreateModel.Article);
                await _context.SaveChangesAsync();
                await AddAuthors(articleCreateModel.ArticleAuthors, articleCreateModel.Article.Id);
                return RedirectToAction(nameof(Index));
            }
            return View(articleCreateModel);
        }
        private async Task AddAuthors(List<Guid> authorIds, Guid articleId)
        {
            if (authorIds != null && articleId != null)
            {
                var articleAuthors = new List<ArticleAuthor>();
                foreach (var authorId in authorIds)
                {
                    articleAuthors.Add(new ArticleAuthor() { ArticleId = articleId, AuthorId = authorId });
                }
                await _context.ArticleAuthors.AddRangeAsync(articleAuthors);
            }
        }
        private async Task DownloadFile(IFormFile postedFile, string fileName)
        {
            var imgPath = @"C:\GIT\News\News\wwwroot\img";
            using (FileStream stream = new FileStream(Path.Combine(imgPath, fileName), FileMode.Create))
            {
                await postedFile.CopyToAsync(stream);
            }
        }
        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            ViewData["AdminId"] = new SelectList(_context.Admins, "Id", "Id", article.AdminId);
            ViewData["SubTopicId"] = new SelectList(_context.SubTopics, "Id", "Id", article.SubTopicId);
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,DateOfCreation,DateOfPublication,IsPublish,PhotoName,Content,AdminId,SubTopicId")] Article article)
        {
            if (id != article.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdminId"] = new SelectList(_context.Admins, "Id", "Id", article.AdminId);
            ViewData["SubTopicId"] = new SelectList(_context.SubTopics, "Id", "Id", article.SubTopicId);
            return View(article);
        }

        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Admin)
                .Include(a => a.SubTopic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var article = await _context.Articles.FindAsync(id);
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(Guid id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}
