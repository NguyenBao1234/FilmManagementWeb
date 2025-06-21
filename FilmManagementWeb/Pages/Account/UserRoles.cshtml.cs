using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FilmManagementWeb.Pages.Account;

[Authorize(Policy = "IsAdmin")]
public class UserRoles : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserRoles(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public List<UserWithRoles> Users { get; set; }

    public class UserWithRoles
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }
    
    [BindProperty(SupportsGet = true)]
    public string SearchTerm { get; set; }
    
    public async Task OnGetAsync()
    {
        var allUsers = _userManager.Users.ToList();

        if (!string.IsNullOrWhiteSpace(SearchTerm))
        {
            allUsers = allUsers
                .Where(u => u.Email.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        Users = new List<UserWithRoles>();
        foreach (var user in allUsers)
        {
            var roles = await _userManager.GetRolesAsync(user);
            Users.Add(new UserWithRoles
            {
                Id = user.Id,
                Email = user.Email,
                Roles = roles
            });
        }
    }

    public async Task<IActionResult> OnPostGrantAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        if (!await _roleManager.RoleExistsAsync(role)) await _roleManager.CreateAsync(new IdentityRole(role));

        if (!await _userManager.IsInRoleAsync(user, role)) await _userManager.AddToRoleAsync(user, role);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRevokeAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        if (await _userManager.IsInRoleAsync(user, role)) await _userManager.RemoveFromRoleAsync(user, role);
        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostDeleteAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Contains("Admin")) return Forbid();
        await _userManager.DeleteAsync(user);
        return RedirectToPage(new { SearchTerm });
    }
}