using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Internship;
using Internship.Models;
using Microsoft.AspNetCore.Authorization;

namespace Internship.Controllers
{
  
    public class HomeController : Controller
    {
        private readonly booksDBContext _context;
        public HomeController(booksDBContext context)
        {
            _context = context;
        }



        // GET: Home
        [Authorize]
        public async Task<IActionResult> Index(int? genre,string name,string author,int page=1)
        {
            int pageSize = 3;

            IQueryable<Book> books= _context.Book.Include(b => b.Author).Include(b => b.Genre);

            // var booksDBContext = _context.Book.Include(b => b.Author).Include(b => b.Genre);
           //фильтрация
            if (genre != null && genre != 0)
            {
                books = books.Where(b => b.Genre.Id == genre);
            }
            if (!String.IsNullOrEmpty(name))
            {
                books = books.Where(b => b.BookName.Contains(name));
            }
            if (!String.IsNullOrEmpty(author))
            {
                books = books.Where(b => b.Author.AuthorName.Contains(author));
            }

            //пангинация
            var count = await books.CountAsync();
            var items = await books.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();


          
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                FilterViewModel = new FilterViewModel(_context.Genre.ToList(), genre, name, author),
                Books = items
            };
            return View(viewModel);
        }

        // GET: Home/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Home/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "AuthorName");
            ViewData["GenreId"] = new SelectList(_context.Genre, "Id", "GenreName");
            
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel model )
        {
            if (ModelState.IsValid)
            {
                if (!_context.Authors.Any(a => a.AuthorName == model.Author.AuthorName))
                {
                    _context.Authors.Add(model.Author);
                    await _context.SaveChangesAsync();
                }


                var auth = await _context.Authors.FirstOrDefaultAsync(a => a.AuthorName == model.Author.AuthorName);
                model.Book.AuthorId = auth.Id;

                _context.Add(model.Book);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            //ModelState.AddModelError("", model.Author.AuthorName.ToString());

            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "AuthorName", model.Book.AuthorId);
            ViewData["GenreId"] = new SelectList(_context.Genre, "Id", "GenreName", model.Book.GenreId);
            return View(model);
        }

        // GET: Home/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "AuthorName", book.AuthorId);
            ViewData["GenreId"] = new SelectList(_context.Genre, "Id", "GenreName", book.GenreId);
            return View(book);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, [Bind("Id,BookName,AuthorId,GenreId,Year,DateOfPurchase")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "AuthorName", book.AuthorId);
            ViewData["GenreId"] = new SelectList(_context.Genre, "Id", "GenreName", book.GenreId);
            return View(book);
        }

        // GET: Home/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.Author)
                .Include(b => b.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var book = await _context.Book.FindAsync(id);
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(short id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
