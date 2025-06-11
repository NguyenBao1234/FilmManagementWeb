using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc; //chu thich du lieu
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmManagementWeb.Pages.Identity.Account;
//page target : Gen reset password link
public class ForgotPassword : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<ForgotPasswordModel> _logger;

    public ForgotPassword(UserManager<IdentityUser> userManager, ILogger<ForgotPasswordModel> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }
    
    [BindProperty]
    [Required(ErrorMessage = "Email là bắt buộc."), EmailAddress]
    public string Email { get; set; }
    
    public bool bSuccess { get; set; }
    public string Message { get; set; }

    public void OnGet()
    {
        bSuccess = false;
        Message = null;
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var user = await _userManager.FindByEmailAsync(Email);
        
        if (user == null)
        {
            Message = "Không tìm thấy tài khoản với email này. Vui lòng kiểm tra lại hoặc đăng ký tài khoản mới.";
            bSuccess = false;
            return Page();
        }
        //gen link
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var callbackUrl = Url.Page(
            "/Account/ResetPasswordFromForgot",
            pageHandler: null, 
            values: new { userId = user.Id, token = token },
            protocol: Request.Scheme);

        //minh hoa email nhan
        Console.WriteLine($"Link đặt lại mật khẩu cho {Email}: {callbackUrl}");

        // Thông báo cho người dùng
        Message = "Một liên kết đặt lại mật khẩu đã được gửi qua email của bạn ( demo log in ra link rồi )";
        bSuccess = true;
        return Page();
    }
}