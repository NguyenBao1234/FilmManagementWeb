using System.ComponentModel.DataAnnotations;//chu thich du lieu
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmManagementWeb.Pages.Identity.Account;

public class Register : PageModel
{
    public void OnGet()
    {
        
    }
    
    private readonly UserManager<IdentityUser> _userManager;
    public Register(UserManager<IdentityUser> userManager) => _userManager = userManager;//Register thuoc Dependency Injection, de DI kiem soat. => 1

    [BindProperty]
    public InputModel Input { get; set; }
    public string Message { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Email là bắt buộc."), EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "{0} phải có ít nhất {2} ký tự và có chỉ có thể dài tối đa {1} ký tự.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,100}$")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Nhập lại Mật khẩu là bắt buộc."), Compare("Password",ErrorMessage = "Mật khẩu xác nhận không khớp."), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                Message = "Đăng ký thành công!";
                // du dinh them "Pending"
                return RedirectToPage("Login");
            }
            else
            {
                Message = "Error: " + string.Join(", ", result.Errors);
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return Page();
    }
}
