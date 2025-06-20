using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace FilmManagementWeb.Pages.Movies
{
    [Authorize(Policy = "IsStaff")]
    public class GenreID_EditModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int MovieId { get; set; }
        public List<GenreInfo> AllGenres { get; set; } = new();
        [BindProperty]
        public List<int> SelectedGenreIds { get; set; } = new();
        public string? Message { get; set; }

        public void OnGet(int id)
        {
            MovieId = id;
            string connectionString = "Data Source=localhost;Initial Catalog=WebFilmDB;Integrated Security=True;TrustServerCertificate=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Get all genres
                string sqlGenres = "SELECT GenreId, Name FROM Genres";
                using (SqlCommand cmd = new SqlCommand(sqlGenres, connection))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AllGenres.Add(new GenreInfo
                        {
                            GenreId = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }

                // Get selected genres for this movie
                string sqlSelected = "SELECT GenreId FROM MovieGenres WHERE MovieId = @MovieId";
                using (SqlCommand cmd = new SqlCommand(sqlSelected, connection))
                {
                    cmd.Parameters.AddWithValue("@MovieId", MovieId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SelectedGenreIds.Add(reader.GetInt32(0));
                        }
                    }
                }
            }
        }

        public IActionResult OnPost()
        {
            string connectionString = "Data Source=localhost;Initial Catalog=WebFilmDB;Integrated Security=True;TrustServerCertificate=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Remove all current genres for this movie
                string sqlDelete = "DELETE FROM MovieGenres WHERE MovieId = @MovieId";
                using (SqlCommand cmd = new SqlCommand(sqlDelete, connection))
                {
                    cmd.Parameters.AddWithValue("@MovieId", MovieId);
                    cmd.ExecuteNonQuery();
                }

                // Add selected genres
                foreach (var genreId in SelectedGenreIds)
                {
                    string sqlInsert = "INSERT INTO MovieGenres (MovieId, GenreId) VALUES (@MovieId, @GenreId)";
                    using (SqlCommand cmd = new SqlCommand(sqlInsert, connection))
                    {
                        cmd.Parameters.AddWithValue("@MovieId", MovieId);
                        cmd.Parameters.AddWithValue("@GenreId", genreId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            Message = "Cập nhật thể loại thành công.";
            return RedirectToPage("/Movies/Index");
        }

        public class GenreInfo
        {
            public int GenreId { get; set; }
            public string Name { get; set; } = "";
        }
    }
}
