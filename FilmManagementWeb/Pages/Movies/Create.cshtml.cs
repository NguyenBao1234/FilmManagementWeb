using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FilmManagementWeb.Pages.Movies
{
    public class CreateModel : PageModel
    {
        public MoviesInfo MovieInfo = new MoviesInfo();
        public string Message = "";

        public void OnGet()
        {

        }

        public void OnPost()
        {
            // Safely parse MovieId and handle possible nulls for all fields
            var movieIdValue = Request.Form["MovieId"].ToString();
       
         
            MovieInfo.MovieId = string.IsNullOrEmpty(movieIdValue) ? 0 : int.Parse(movieIdValue);
            MovieInfo.Title = Request.Form["Title"].ToString() ?? string.Empty;
            MovieInfo.Description = Request.Form["Description"].ToString() ?? string.Empty;
            MovieInfo.ReleaseYear = Request.Form["ReleaseYear"].ToString() ?? string.Empty;
           
            MovieInfo.ImageUrl = Request.Form["ImageUrl"].ToString() ?? string.Empty;

            if (string.IsNullOrEmpty(MovieInfo.Title) ||
                string.IsNullOrEmpty(MovieInfo.Description) ||
              
                string.IsNullOrEmpty(MovieInfo.ReleaseYear))
            {
                Message = "Cac thong tin can phai duoc dien nhe.";
                return;
            }
            try
            {
                string connectionString = "Data Source = localhost;Initial Catalog = WebFilmDB;" +
                    "Integrated Security = True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "Insert into Movies" +
                        "(Title, Description, ReleaseYear, ImageUrl) " +
                        "Values(@Title, @Description, @ReleaseYear, @ImageUrl)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Title", MovieInfo.Title);
                        command.Parameters.AddWithValue("@Description", MovieInfo.Description);
                        command.Parameters.AddWithValue("@ReleaseYear", MovieInfo.ReleaseYear);
                        
                        command.Parameters.AddWithValue("@ImageUrl", MovieInfo.ImageUrl);
                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            Message = "Them phim thanh cong.";
                        }
                        else
                        {
                            Message = "Them phim that bai.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Loi ket noi toi co so du lieu: " + ex.Message;
                return;
            }
            // Reset all fields for new entry
            
            MovieInfo.Title = "";
            MovieInfo.Description = "";
            MovieInfo.ReleaseYear = "";
            
            MovieInfo.ImageUrl = "";
        }
    }
}
