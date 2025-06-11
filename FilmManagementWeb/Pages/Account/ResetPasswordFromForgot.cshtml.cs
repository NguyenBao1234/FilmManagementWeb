using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmManagementWeb.Pages.Identity.Account;
public class ResetPasswordFromForgot : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;

    public ResetPasswordFromForgot(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public bool bSuccess { get; set; } = false;
    
    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }
        
        public string Email { get; set; } // just for display from OnGet

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "{0} phải có ít nhất {2} ký tự và có chỉ có thể dài tối đa {1} ký tự.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,100}$",
            ErrorMessage = "Mật khẩu phải chứa ít nhất 1 chữ thường, 1 chữ hoa, 1 chữ số, 1 ký tự đặc biệt và dài từ 6 đến 100 ký tự.")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Xác nhận mật khẩu mới là bắt buộc.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }
    }
    
    public async Task<IActionResult> OnGet(string userId, string token)
    {
        if (userId == null || token == null) return RedirectToPage("/Error");
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return RedirectToPage("Pages/Error", new { message = "Liên kết đặt lại mật khẩu không hợp lệ hoặc đã hết hạn." });
        
        Input = new InputModel 
        { 
            UserId = userId, 
            Token = token,
            Email = user.Email
        };
        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        bSuccess = false;
        if (!ModelState.IsValid) return Page();
        var user = await _userManager.FindByIdAsync(Input.UserId);
        if (user == null)
        {
            Console.WriteLine("Khong Thanh cong tu dau roi");
            return RedirectToPage("Page/Error", new { message = "Liên kết đặt lại mật khẩu không hợp lệ hoặc đã hết hạn." });
        }

        var result = await _userManager.ResetPasswordAsync(user, Input.Token, Input.Password);
        if (result.Succeeded)
        {
            Console.WriteLine("OK Yeah Thanh cong roi");
            bSuccess = true;
            return Page();
        }
        
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        Console.WriteLine("Khong Thanh cong roi");
        Input.Email = user.Email; 
        return Page();
    }
}