using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace FilmManagementWeb.Pages.Warehouses
{
    [Authorize(Policy = "IsStaff")]
    public class CreateModel : PageModel
    {
        [BindProperty]
        public WarehouseInfo Warehouse { get; set; } = new WarehouseInfo();
        public string Message { get; set; } = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (string.IsNullOrEmpty(Warehouse.Capacity) || string.IsNullOrEmpty(Warehouse.Location))
            {
                Message = "Vui lòng nhập đầy đủ thông tin.";
                return;
            }

            try
            {
                string connectionString = "Data Source=localhost;Initial Catalog=WebFilmDB;Integrated Security=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Warehouses (Location,Capacity) VALUES (@Location,@Capacity)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Location", Warehouse.Location);
                        command.Parameters.AddWithValue("@Capacity", Warehouse.Capacity);
                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            Message = "Thêm kho thành công.";
                            Warehouse.Location = "";
                            Warehouse.Capacity = "";
                        }
                        else
                        {
                            Message = "Thêm kho thất bại.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Message = "Lỗi khi thêm kho: " + ex.Message;
            }
        }

        public class WarehouseInfo
        {
            public string Location { get; set; } = "";
            public string Capacity { get; set; } = "";
        }
    }
}
