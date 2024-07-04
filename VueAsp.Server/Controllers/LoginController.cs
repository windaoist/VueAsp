using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using VueAsp.Server.Models;
using VueAsp.Server.Services;

namespace VueAsp.Server.Controllers
{
    [ApiController]
    [Route("api")]
    public class LoginController : Controller
    {
        private readonly IFormDataService _formDataService;
        public LoginController(FormDataService formDataService)
        {
            _formDataService = formDataService;
        }
        [HttpPost("login")]
        public async Task<ActionResult<bool>> Login([FromBody] FormData formData)
        {
            if (formData == null)
            {
                return BadRequest("Invalid data.");
            }
            var exists = await _formDataService.CheckNameExistsAsync(formData.Account);
            if (exists)
            {
                var dat = _formDataService.GetFormDataAsync(formData.Account);
                if (formData.Password == dat.Result.Password)
                {
                    HttpContext.Session.SetString("Account", formData.Account);
                    return Ok(new { message = "Login successful", session = formData.Account });
                }
                else
                {
                    return Ok(new { message = "Account or password incorrect" });
                }
            }
            else
            {
                return Ok(new {message="Account does not exist"});
            }
            }
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // 清除Session
            HttpContext.Session.Clear();
            return Ok(new { message = "Logout successful" });
        }
    }
}
