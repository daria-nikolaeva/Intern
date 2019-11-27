using System;
using System.Collections.Generic;

namespace Internship
{
    public partial class Authors
    {
        public Authors()
        {
            Book = new HashSet<Book>();
        }

        public short Id { get; set; }
        public string AuthorName { get; set; }

        public virtual ICollection<Book> Book { get; set; }
    }
}
