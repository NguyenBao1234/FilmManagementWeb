using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FilmManagementWeb.Pages.Genres
{
    public class ConfirmDeleteModel : PageModel
    {
        public string GenreId { get; set; } = "";
        public string genreName = "";
        public string errorMessage = "";

        public void OnGet()
        {
            GenreId = Request.Query["id"];
            if (string.IsNullOrEmpty(GenreId)) return;

            try
            {
                string connectionString = "Data Source=  localhost\\sqlexpress;Initial Catalog=WebFilmDB;Integrated Security=True;Pooling=False;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT Name FROM Genres WHERE GenreId = @GenreId";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@GenreId", GenreId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                genreName = reader.GetString(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Lỗi: " + ex.Message;
            }
        }

        public IActionResult OnPost()
        {
            GenreId = Request.Form["GenreId"];
            if (string.IsNullOrEmpty(GenreId))
            {
                errorMessage = "Không có ID để xóa.";
                return Page();
            }

            try
            {
                string connectionString = "Data Source=  localhost\\sqlexpress;Initial Catalog=WebFilmDB;Integrated Security=True;Pooling=False;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Xóa trong bảng phụ trước
                    string sql1 = "DELETE FROM MovieGenres WHERE GenreId = @GenreId";
                    using (SqlCommand cmd1 = new SqlCommand(sql1, connection))
                    {
                        cmd1.Parameters.AddWithValue("@GenreId", GenreId);
                        cmd1.ExecuteNonQuery();
                    }

                    // Sau đó xóa chính nó
                    string sql2 = "DELETE FROM Genres WHERE GenreId = @GenreId";
                    using (SqlCommand cmd2 = new SqlCommand(sql2, connection))
                    {
                        cmd2.Parameters.AddWithValue("@GenreId", GenreId);
                        cmd2.ExecuteNonQuery();
                    }
                }

                return RedirectToPage("/Genres/Index");
            }
            catch (Exception ex)
            {
                errorMessage = "Lỗi khi xóa: " + ex.Message;
                return Page();
            }
        }
    }
}
