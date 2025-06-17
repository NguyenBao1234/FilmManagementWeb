using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace FilmManagementWeb.Pages
{
    public class HomepageModel : PageModel
    {
        public List<FilmInfo> FeaturedFilms { get; set; } = new();
        public List<FilmInfo> Films { get; set; } = new();

        public void OnGet()
        {
            // Example data, replace with your actual data source
            FeaturedFilms = new List<FilmInfo>
            {
                new FilmInfo { Title = "Inception", ImageUrl = "https://content.triethocduongpho.net/wp-content/uploads/2018/04/Inception.jpg", Description = "Bộ phim cực hay của Christopher Nolan." },
                new FilmInfo { Title = "Interstellar", ImageUrl = "https://anhtuanptnk.wordpress.com/wp-content/uploads/2014/11/interstellar_banner.jpg?w=810", Description = "Bộ phim khoa học viễn tưởng kinh điển." },
                new FilmInfo { Title = "The Dark Knight", ImageUrl = "https://egreg.io/wp-content/uploads/bat.jpg", Description = "Chiến binh bóng tối." }
            };

            Films = new List<FilmInfo>
            {
                new FilmInfo { Title = "Inception", ImageUrl = "https://content.triethocduongpho.net/wp-content/uploads/2018/04/Inception.jpg", Description = "Bộ phim cực hay của Christopher Nolan." },
                new FilmInfo { Title = "Interstellar", ImageUrl = "https://anhtuanptnk.wordpress.com/wp-content/uploads/2014/11/interstellar_banner.jpg?w=810", Description = "Bộ phim khoa học viễn tưởng kinh điển." },
                new FilmInfo { Title = "The Dark Knight", ImageUrl = "https://egreg.io/wp-content/uploads/bat.jpg", Description = "Chiến binh bóng tối." },
                new FilmInfo { Title = "Avatar", ImageUrl = "https://media.vietnamplus.vn/images/7255a701687d11cb8c6bbc58a6c80785f55546d9a543a98ddde2bb5de35ebb6467411fc21acf3831be8049bda16827e6/avatar.JPG", Description = "Thế giới Avatar." },
                new FilmInfo { Title = "Titanic", ImageUrl = "https://cdnmedia.baotintuc.vn/Upload/XmrgEWAN1PzjhSWqVO54A/files/2018/10/2510/titanic1.jpg", Description = "A timeless love story." },
                new FilmInfo { Title = "The Matrix", ImageUrl = "https://static2.vieon.vn/vieplay-image/carousel_web_v4/2022/01/04/qs45wvy5_1920x1080-matran.jpg", Description = "A revolutionary sci-fi action film." }
            };
        }
    }

    public class FilmInfo
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
    }
}
