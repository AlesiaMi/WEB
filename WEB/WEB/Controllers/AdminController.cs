using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WEB.Data;
using WEB.Models;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Users()
    {
        var users = _userManager.Users.ToList();
        var userViewModels = new List<UserViewModel>();

        foreach (var user in users)
        {
            userViewModels.Add(new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user)
            });
        }

        return View(userViewModels);
    }
}