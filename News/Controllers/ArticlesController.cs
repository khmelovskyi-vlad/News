using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> GetArticlesForTopic(Guid? id)
        {
            return View("GetArticles", await _context
                .Topica
                .Where(topic => topic.Id == id)
                .Include(topic => topic.SubTopics)
                .ThenInclude(subTopic => subTopic.Articles)
                .ToListAsync());
        }
        public async Task<IActionResult> GetArticlesForSubTopic(Guid? id)
        {
            return View("GetArticles", await _context
                .SubTopics
                .Where(subTopic => subTopic.Id == id)
                .Include(subTopic => subTopic.Topic)
                .Include(subTopic => subTopic.Articles)
                .ToListAsync());
        }
        // GET: Articles
        public async Task<IActionResult> Index()
        {
            var newsContext = _context.Articles.Include(a => a.AddedBy).Include(a => a.SubTopic);
            return View(await newsContext.ToListAsync());
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.AddedBy)
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
                .Include(a => a.AddedBy)
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
