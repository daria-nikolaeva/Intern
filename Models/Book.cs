using System;
using System.Collections.Generic;

namespace Internship
{
    public partial class Book
    {
        public short Id { get; set; }
        public string BookName { get; set; }
        public short AuthorId { get; set; }
        public short GenreId { get; set; }
        public decimal Year { get; set; }
        public DateTime DateOfPurchase { get; set; }
        

        public virtual Authors Author { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
