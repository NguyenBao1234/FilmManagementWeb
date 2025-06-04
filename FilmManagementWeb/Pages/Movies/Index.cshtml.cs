using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace FilmManagementWeb.Pages.Movies
{
    public class IndexModel : PageModel
    {
        public List<MoviesInfo> ListMovies { get; set; } = new List<MoviesInfo>();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public void OnGet()
        {
            var movieDict = new Dictionary<int, MoviesInfo>();
            try
            {
                string connectionString = "Data Source=localhost;Initial Catalog=WebFilmDB;Integrated Security=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                        SELECT m.MovieId, m.Title, m.Description, m.ReleaseYear, m.ImageUrl, g.Name
                        FROM Movies m
                        LEFT JOIN MovieGenres mg ON m.MovieId = mg.MovieId
                        LEFT JOIN Genres g ON mg.GenreId = g.GenreId";
                    if (!string.IsNullOrEmpty(SearchTerm))
                    {
                        sql += " WHERE m.Title LIKE @Search";
                    }
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        if (!string.IsNullOrEmpty(SearchTerm))
                        {
                            command.Parameters.AddWithValue("@Search", "%" + SearchTerm + "%");
                        }
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int movieId = reader.GetInt32(0);
                                if (!movieDict.ContainsKey(movieId))
                                {
                                    movieDict[movieId] = new MoviesInfo
                                    {
                                        MovieId = movieId,
                                        Title = reader.GetString(1),
                                        Description = reader.GetString(2),
                                        ReleaseYear = reader.GetValue(3)?.ToString(),
                                        ImageUrl = !reader.IsDBNull(4) ? reader.GetString(4) : "https://www.freeiconspng.com/uploads/no-image-icon-13.png",
                                        GenreNames = new List<string>()
                                    };
                                }
                                if (!reader.IsDBNull(5))
                                {
                                    movieDict[movieId].GenreNames.Add(reader.GetString(5));
                                }
                            }
                        }
                    }
                }
                ListMovies = movieDict.Values.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class MoviesInfo
    {
        public int MovieId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ReleaseYear { get; set; }
        public string? ImageUrl { get; set; }
        public List<string> GenreNames { get; set; } = new List<string>();
    }
}
