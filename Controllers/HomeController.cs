using BookStore.Models;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
namespace BookStore.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger<HomeController> _logger;

        public static List<Author> Authors = new List<Author>()
        {
             new Author { Id = 1, Name = "J.K. Rowling", Country = "UK" },
             new Author { Id = 2, Name = "William Shakespeare", Country = "England" }
        };
        public static List<Book> Books = new List<Book>()
        {    new Book { Id = 1, Title = "Harry Potter", Price = 1500, AuthorId = 1 },
             new Book { Id = 2, Title = "Hamlet", Price = 1200, AuthorId = 2 },
             new Book { Id = 3, Title = "Fantastic Beasts", Price = 1800, AuthorId = 1 }
        };

        private int GetNextBookId()
        {
            return Books.Count > 0 ? Books.Max(b => b.Id) + 1 : 1;
        }

        private int GetNextAuthorId()
        {
            return Authors.Count > 0 ? Authors.Max(a => a.Id) + 1 : 1;
        }
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var viewModel = new HomeViewModel
            {
                Books = Books.Select(b => new BookCardViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Price = b.Price,

                    AuthorName = Authors
                        .FirstOrDefault(a => a.Id == b.AuthorId)?.Name
                }).ToList(),

                Authors = Authors
            };

            return View(viewModel);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBook(string title, decimal price, int authorId)
        {
            Book newBook = new Book
            {
                Id = GetNextBookId(),
                Title = title,
                Price = price,
                AuthorId = authorId
            };

            Books.Add(newBook);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveBook(int id)
        {
            var book = Books.FirstOrDefault(b => b.Id == id);

            if (book != null)
            {
                Books.Remove(book);
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult AddAuthor(string name, string country)
        {
            Author newAuthor = new Author
            {
                Id = GetNextAuthorId(),
                Name = name,
                Country = country
            };

            Authors.Add(newAuthor);

            return RedirectToAction("Index");
        }
    }
}