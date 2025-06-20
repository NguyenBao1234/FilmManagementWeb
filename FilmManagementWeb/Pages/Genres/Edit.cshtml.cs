using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace FilmManagementWeb.Pages.Genres
{
    [Authorize(Policy = "IsStaff")]
    public class EditModel : PageModel
    {
        public GenreInfo genreInfo = new GenreInfo();
        public string errorMessage = "";
        public void OnGet()
        {
            String id = Request.Query["id"];
            try
            {
                string connectionString = "Data Source = localhost; Initial Catalog = WebFilmDB;" + "Integrated Security = True; Pooling = False; TrustServerCertificate = True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "Select * from Genres where GenreID = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                genreInfo.GenreId = "" + reader.GetInt32(0);
                                genreInfo.Name = reader.GetString(1);
                                genreInfo.ImageUrl = reader.GetString(2); 
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
        public void OnPost()
        {
            genreInfo.GenreId = Request.Form["GenreId"];
            genreInfo.Name = Request.Form["Name"];
            genreInfo.ImageUrl = Request.Form["ImageUrl"];
            if (string.IsNullOrEmpty(genreInfo.Name) || string.IsNullOrEmpty(genreInfo.ImageUrl))
            {
                errorMessage = " Tất cả các trường là bắt buộc";
                return;
            }
            try
            {
                string connectionString = "Data Source = localhost\\sqlexpress; Initial Catalog = WebFilmDB;" + "Integrated Security = True; Pooling = False; TrustServerCertificate = True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Genres" + " SET Name=@Name, ImageUrl=@ImageUrl " + "Where Genreid= @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.Parameters.AddWithValue("@Name", genreInfo.Name);
                        command.Parameters.AddWithValue("@ImageUrl", genreInfo.ImageUrl);
                        command.Parameters.AddWithValue("@id", genreInfo.GenreId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            Response.Redirect("/Genres");
        }
    }
}
