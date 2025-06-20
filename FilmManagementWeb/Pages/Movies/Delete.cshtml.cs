using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace FilmManagementWeb.Pages.Movies
{
    [Authorize(Policy = "IsStaff")]
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public int MovieId { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }

        public void OnGet(int id)
        {
            // Load movie info for confirmation
            string connectionString = "Data Source=localhost;Initial Catalog=WebFilmDB;Integrated Security=True;Pooling=False;TrustServerCertificate=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT Title FROM Movies WHERE MovieId = @MovieId";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@MovieId", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            MovieId = id;
                            Title = reader.GetString(0);
                        }
                        else
                        {
                            Message = "Không tìm thấy phim.";
                        }
                    }
                }
            }
        }

        public IActionResult OnPost()
        {
            if (MovieId == 0)
            {
                Message = "ID phim không hợp lệ.";
                return Page();
            }

            try
            {
                string connectionString = "Data Source=localhost;Initial Catalog=WebFilmDB;Integrated Security=True;Pooling=False;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM Movies WHERE MovieId = @MovieId";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@MovieId", MovieId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch
            {
                Message = "Lỗi khi xóa phim.";
                return Page();
            }

            return RedirectToPage("/Movies/Index");
        }
    }
}
