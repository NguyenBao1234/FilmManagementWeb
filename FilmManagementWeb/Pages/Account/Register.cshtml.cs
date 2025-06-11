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

    public class InputModel
    {
        [Required(ErrorMessage = "Email là bắt buộc."), EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "{0} phải có ít nhất {2} ký tự và có chỉ có thể dài tối đa {1} ký tự.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,100}$",
            ErrorMessage = "Mật khẩu phải chứa ít nhất 1 chữ thường, 1 chữ hoa, 1 chữ số, 1 ký tự đặc biệt và dài từ 6 đến 100 ký tự.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Nhập lại Mật khẩu là bắt buộc."), Compare("Password",ErrorMessage = "Mật khẩu xác nhận không khớp."), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Where(kvp => kvp.Value.Errors.Count > 0).ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );

            return new JsonResult(new { success = false, errors });
        }
        var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
        var result = await _userManager.CreateAsync(user, Input.Password);
        
        if (result.Succeeded)
        {
            //confirm email manually
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _userManager.ConfirmEmailAsync(user, token);
            
            return new JsonResult(new { success = true, redirectUrl = Url.Page("/Account/Login") });
        }
        return new JsonResult(new { success = false });
    }
}
