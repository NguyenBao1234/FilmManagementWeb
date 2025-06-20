using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace FilmManagementWeb.Pages.Movies
{
    [Authorize(Policy = "IsStaff")]
    public class Warehouses_EditModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int MovieId { get; set; }
        public List<WarehouseInfo> AllWarehouses { get; set; } = new();
        [BindProperty]
        public List<int> SelectedWarehouseIds { get; set; } = new();
        public string? Message { get; set; }

        public void OnGet(int id)
        {
            MovieId = id;
            string connectionString = "Data Source=localhost;Initial Catalog=WebFilmDB;Integrated Security=True;TrustServerCertificate=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Get all warehouses
                string sqlWarehouses = "SELECT WarehouseId, Location, Capacity FROM Warehouses";
                using (SqlCommand cmd = new SqlCommand(sqlWarehouses, connection))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AllWarehouses.Add(new WarehouseInfo
                        {
                            WarehouseId = reader.GetInt32(0),
                            Location = reader.GetString(1),
                            Capacity = reader.GetInt32(2).ToString()    
                        });
                    }
                }

                // Get selected warehouses for this movie
                string sqlSelected = "SELECT WarehouseId FROM WarehouseMovies WHERE MovieId = @MovieId";
                using (SqlCommand cmd = new SqlCommand(sqlSelected, connection))
                {
                    cmd.Parameters.AddWithValue("@MovieId", MovieId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SelectedWarehouseIds.Add(reader.GetInt32(0));
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

                // Remove all current warehouses for this movie
                string sqlDelete = "DELETE FROM WarehouseMovies WHERE MovieId = @MovieId";
                using (SqlCommand cmd = new SqlCommand(sqlDelete, connection))
                {
                    cmd.Parameters.AddWithValue("@MovieId", MovieId);
                    cmd.ExecuteNonQuery();
                }

                // Add selected warehouses
                foreach (var warehouseId in SelectedWarehouseIds)
                {
                    string sqlInsert = "INSERT INTO WarehouseMovies (MovieId, WarehouseId) VALUES (@MovieId, @WarehouseId)";
                    using (SqlCommand cmd = new SqlCommand(sqlInsert, connection))
                    {
                        cmd.Parameters.AddWithValue("@MovieId", MovieId);
                        cmd.Parameters.AddWithValue("@WarehouseId", warehouseId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            Message = "Cập nhật kho thành công.";
            return RedirectToPage("/Movies/Index");
        }

        public class WarehouseInfo
        {
            public int WarehouseId { get; set; }
            public string Location { get; set; } = "";
            public string Capacity { get; set; } = "";
        }
    }
}
