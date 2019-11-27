using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internship.Models
{
    public class BooksListViewModel
    {
       
        public IEnumerable<Book> Books  { get; set; }
        public string Name { get; set; }
        public Authors Author { get; set; }
        public SelectList Genres { get; set; }
          
        
    }
}
