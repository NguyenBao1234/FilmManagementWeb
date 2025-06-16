using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FilmManagementWeb.Pages.Warehouses
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public int WarehouseId { get; set; }
        public string? Location { get; set; }
        public string? Message { get; set; }

        public void OnGet(int id)
        {
            WarehouseId = id;
            string connectionString = "Data Source=localhost;Initial Catalog=WebFilmDB;Integrated Security=True;TrustServerCertificate=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT Location FROM Warehouses WHERE WarehouseId = @WarehouseId";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@WarehouseId", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Location = reader.GetString(0);
                        }
                        else
                        {
                            Message = "Không tìm thấy kho.";
                        }
                    }
                }
            }
        }

        public IActionResult OnPost()
        {
            if (WarehouseId == 0)
            {
                Message = "ID kho không hợp lệ.";
                return Page();
            }

            try
            {
                string connectionString = "Data Source=localhost;Initial Catalog=WebFilmDB;Integrated Security=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM Warehouses WHERE WarehouseId = @WarehouseId";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@WarehouseId", WarehouseId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Lỗi khi xóa kho : " + ex.Message;
                return Page();
            }

            return RedirectToPage("/Warehouses/Index");
        }
    }
}
