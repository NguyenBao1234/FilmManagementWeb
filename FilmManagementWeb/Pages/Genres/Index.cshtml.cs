using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Linq.Expressions;
namespace FilmManagementWeb.Pages.Genres
{
    public class IndexModel : PageModel
    {
        public List<GenreInfo> ListGenre = new List<GenreInfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source = localhost; Initial Catalog = WebFilmDB;" + "Integrated Security = True; Pooling = False; TrustServerCertificate = True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "Select * from Genres";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                GenreInfo genreInfo = new GenreInfo();
                                genreInfo.GenreId = "" + reader.GetInt32(0);
                                genreInfo.Name = reader.GetString(1);
                                genreInfo.ImageUrl = reader.GetString(2);
                                ListGenre.Add(genreInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
    public class GenreInfo
    {
        public string GenreId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }

    }
}

