using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Internship
{
    public partial class Book
    {
        public short Id { get; set; }
        public string BookName { get; set; }
        public short AuthorId { get; set; }
        public short GenreId { get; set; }
        public decimal Year { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DateOfPurchase { get; set; }
        


        public virtual Authors Author { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
