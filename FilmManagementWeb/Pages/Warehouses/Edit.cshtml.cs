using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FilmManagementWeb.Pages.Warehouses
{
    public class EditModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty]
        public WarehouseInfo Warehouse { get; set; } = new WarehouseInfo();

        public string Message { get; set; } = "";

        public void OnGet(int id)
        {
            Id = id;
            string connectionString = "Data Source=localhost;Initial Catalog=WebFilmDB;Integrated Security=True;TrustServerCertificate=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT Location, Capacity FROM Warehouses WHERE WarehouseId = @WarehouseId";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@WarehouseId", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Warehouse.WarehouseId = id;
                            Warehouse.Location = reader.GetString(0);
                            Warehouse.Capacity = reader.GetInt32(1).ToString();
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
            if (string.IsNullOrEmpty(Warehouse.Location) || string.IsNullOrEmpty(Warehouse.Capacity))
            {
                Message = "Vui lòng nhập đầy đủ thông tin.";
                return Page();
            }

            try
            {
                string connectionString = "Data Source=localhost;Initial Catalog=WebFilmDB;Integrated Security=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Warehouses SET Location = @Location, Capacity = @Capacity WHERE WarehouseId = @WarehouseId";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Location", Warehouse.Location);
                        command.Parameters.AddWithValue("@Capacity", Warehouse.Capacity);
                        command.Parameters.AddWithValue("@WarehouseId", Warehouse.WarehouseId);
                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            return RedirectToPage("/Warehouses/Index");
                        }
                        else
                        {
                            Message = "Cập nhật kho thất bại.";
                            return Page();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Lỗi khi cập nhật kho: " + ex.Message;
                return Page();
            }
        }

        public class WarehouseInfo
        {
            public int WarehouseId { get; set; }
            public string Location { get; set; } = "";
            public string Capacity { get; set; } = "";
        }
    }
}
