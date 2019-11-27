using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Internship.Models
{
    public class FilterViewModel
    {
        public FilterViewModel(List<Genre> genres, int? genre, string name, string author)
        {

            genres.Insert(0, new Genre { GenreName = "All", Id = 0 });
            Genres = new SelectList(genres, "Id", "GenreName", genre);
            SelectedGenre = genre;
            SelectedName = name;
            SelectedAuthor = author;
        }
        public SelectList Genres { get; private set; }
        public int? SelectedGenre { get; private set; }
        public string SelectedName { get; private set; }
        public string SelectedAuthor { get; private set; }
    }
}
    