using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace FilmManagementWeb.Pages.Movies
{
    [Authorize(Policy = "IsStaff")]
    public class EditModel : PageModel
    {
        public MoviesInfo MovieInfo = new MoviesInfo();
        public string Message = "";

        public void OnGet()
        {
            string? id = Request.Query["id"].ToString();
            if (string.IsNullOrEmpty(id))
            {
                Message = "Không tìm thấy ID phim.";
                return;
            }
            try
            {
                string connectionString = "Data Source = localhost;Initial Catalog = WebFilmDB;" +
                    "Integrated Security = True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "Select * from Movies where MovieId = @MovieId";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@MovieId", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                MovieInfo.MovieId = reader.GetInt32(0);
                                MovieInfo.Title = reader.GetString(1);
                                MovieInfo.Description = reader.GetString(2);
                                MovieInfo.ReleaseYear = reader.GetValue(3)?.ToString() ?? "";
                                
                                MovieInfo.ImageUrl = !reader.IsDBNull(5)
                                    ? reader.GetString(5)
                                    : "https://www.freeiconspng.com/uploads/no-image-icon-13.png";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Lỗi khi truy vấn dữ liệu: " + ex.Message;
            }
        }

        public void OnPost()
        {
            var movieIdValue = Request.Form["MovieId"].ToString();
            if (!int.TryParse(movieIdValue, out int movieId))
            {
                Message = "ID phim không hợp lệ.";
                return;
            }
            MovieInfo.MovieId = movieId;
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
                    string sql = "Update Movies " +
                        "Set Title = @Title, Description = @Description, ReleaseYear = @ReleaseYear, " +
                        " ImageUrl = @ImageUrl " +
                        "Where MovieId = @MovieId";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Title", MovieInfo.Title);
                        command.Parameters.AddWithValue("@Description", MovieInfo.Description);
                        command.Parameters.AddWithValue("@ReleaseYear", MovieInfo.ReleaseYear);
                        
                        command.Parameters.AddWithValue("@ImageUrl", MovieInfo.ImageUrl);
                        command.Parameters.AddWithValue("@MovieId", MovieInfo.MovieId);
                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            Message = "Cap nhat phim thanh cong.";
                        }
                        else
                        {
                            Message = "Cap nhat phim that bai.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Lỗi khi cập nhật dữ liệu: " + ex.Message;
            }
        }
    }
}
