
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using StoryStore.DataModels;
using StoryStore.Models;

namespace StoryStore.Controllers
{
    public class HomeController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly StoryStoreDbContext _db;

        public HomeController(SignInManager<AppUser> signInManager,
            StoryStoreDbContext db, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _db = db;

        }

        public IActionResult Login()
        {
       
            return View();
        }
        [Authorize]
        public IActionResult Dashboard()
        {

            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetRoleAndRange()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);
            var ageRange = _db.AgeRanges.Where(x => x.Id == user.AgeRangeId).SingleOrDefault();
            RoleAndRange roleAndRange = new RoleAndRange();
            roleAndRange.Range = ageRange.Range;
            roleAndRange.RoleName = roles.ElementAt(0).ToString();
            return Ok(roleAndRange) ;
        }

        [HttpPost]
        public async Task<IActionResult> AddNewUser([FromBody] AddUserModel addUserModel)
        {

            if (ModelState.IsValid)
            {
                IdentityResult result, result1;
                AppUser findedUser = null;
                var user = new AppUser
                {
                    UserName = addUserModel.Email,
                    PasswordHash = addUserModel.password,
                    Email = addUserModel.Email,
                    EmailConfirmed = true,
                    AgeRangeId = addUserModel.AgeRangeId
                };
                result = await _userManager.CreateAsync(user, user.PasswordHash);
                if (result.Succeeded)
                {
                    findedUser = await _userManager.FindByEmailAsync(user.Email);
                    result1 = await _userManager.AddToRoleAsync(findedUser, "User");
                    await _signInManager.PasswordSignInAsync(addUserModel.Email, addUserModel.password, true, false);
                    return Ok("done");
                }
                return Ok(result);


            }
            return BadRequest("");
        }


        public IActionResult GetAgeRanges()
        {
            return Json(_db.AgeRanges.ToList());
        }
        [HttpPost]

        public async Task<IActionResult> SignIn([FromBody] userLoginViewModel user)
        {

            var result = await _signInManager.PasswordSignInAsync(user.email, user.password, true, false);
            return Json(result);

        }

        [Authorize]
        public IActionResult Stories()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AddNewStory()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteStory()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Home/Login");
        }
    }
}
