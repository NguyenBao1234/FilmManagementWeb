using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FilmManagementWeb.Pages.Warehouses
{
    public class IndexModel : PageModel
    {
        public List<WarehouseInfo> Warehouses { get; set; } = new();

        public void OnGet()
        {
            string connectionString = "Data Source=localhost;Initial Catalog=WebFilmDB;Integrated Security=True;TrustServerCertificate=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT WarehouseId, Location, Capacity FROM Warehouses";
                using (SqlCommand command = new SqlCommand(sql, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Warehouses.Add(new WarehouseInfo
                        {
                            WarehouseId = reader.GetInt32(0),
                            Location = reader.GetString(1),
                            Capacity = reader.GetInt32(2).ToString()
                        });
                    }
                }
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
