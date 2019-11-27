using System;
using System.Collections.Generic;

namespace Internship
{
    public partial class Genre
    {
        public Genre()
        {
            Book = new HashSet<Book>();
        }

        public short Id { get; set; }
        public string GenreName { get; set; }

        public virtual ICollection<Book> Book { get; set; }
    }
}
