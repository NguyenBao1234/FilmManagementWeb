using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;

namespace FilmManagementWeb.Pages.Genres
{
    [Authorize(Policy = "IsStaff")]
    public class CreateModel : PageModel
    {
        public GenreInfo genreInfo = new GenreInfo();
        public string errorMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {

            genreInfo.Name = Request.Form["Name"];
            genreInfo.ImageUrl = Request.Form["ImageUrl"];
            if (string.IsNullOrEmpty(genreInfo.Name) || string.IsNullOrEmpty(genreInfo.ImageUrl))
            {
                errorMessage = " Tất cả các trường là bắt buộc";
                return;
            }
            try
            {
                string connectionString = "Data Source =  localhost; Initial Catalog = WebFilmDB;" + "Integrated Security = True; Pooling = False; TrustServerCertificate = True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Genres" + "(Name, ImageUrl) VALUES" + "(@Name, @ImageUrl);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.Parameters.AddWithValue("@Name", genreInfo.Name);
                        command.Parameters.AddWithValue("@ImageUrl", genreInfo.ImageUrl);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            genreInfo.GenreId = "";
            genreInfo.Name = "";
            genreInfo.ImageUrl = "";
            Response.Redirect("/Genres/");
        }
    }
}
