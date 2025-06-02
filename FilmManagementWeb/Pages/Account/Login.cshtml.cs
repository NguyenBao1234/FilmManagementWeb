using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmManagementWeb.Pages.Identity.Account;

public class Login : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;
    public Login(SignInManager<IdentityUser> signInManager) => _signInManager = signInManager; //construct

    [BindProperty] public InputModel Input { get; set; }
    public string Message { get; set; }

    public class InputModel
    {
        //Property email
        [Required(ErrorMessage = "Email là bắt buộc."), EmailAddress]
        public string Email { get; set; }

        //Property password
        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(100, MinimumLength = 6,
            ErrorMessage = "{0} phải có ít nhất {2} ký tự và có chỉ có thể dài tối đa {1} ký tự.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,100}$")]
        public string Password { get; set; }

        //property Remember me
        public bool RememberMe { get; set; }
    }


    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, true);
            if (result.Succeeded) return RedirectToPage("/Index");
            if (result.IsLockedOut)
            {
                Message = "Tài khoản của bạn đã bị khóa. Vui lòng thử lại sau.";
            }
            else
            {
                Message = "Tài khoản hoặc mật khẩu không đúng.";
            }

            Message = "Sai thông tin đăng nhập hoặc tài khoản chưa tồn tại.";

        }
        return Page();
    }
}