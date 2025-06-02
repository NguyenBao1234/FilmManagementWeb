using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FilmManagementWeb.Pages.Movies
{
    public class IndexModel : PageModel
    {
        public List<MoviesInfo> ListMovies { get; set; } = new List<MoviesInfo>();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=localhost;Initial Catalog=WebFilmDB;Integrated Security=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Movies";
                    if (!string.IsNullOrEmpty(SearchTerm))
                    {
                        sql += " WHERE Title LIKE @Search";
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
                                MoviesInfo movieInfo = new MoviesInfo
                                {
                                    MovieId = reader.GetInt32(0),
                                    Title = reader.GetString(1),
                                    Description = reader.GetString(2),
                                    ReleaseYear = reader.GetValue(3)?.ToString(),
                                    ImageUrl = !reader.IsDBNull(4) ? reader.GetString(4) : "https://www.freeiconspng.com/uploads/no-image-icon-13.png",
                                    GenreId = reader.GetValue(5)?.ToString()
                                };
                                ListMovies.Add(movieInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

public class MoviesInfo
{
    public int MovieId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ReleaseYear { get; set; }
    public string? GenreId { get; set; }
    public string? ImageUrl { get; set; }
}

