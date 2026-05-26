using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalkHomeSafeAPI.Extensions;
using WalkHomeSafeAPI.Services;

namespace WalkHomeSafeAPI.Controllers;


[ApiController]
[Route("api/users")]
[Produces("application/json")]
public class UserController(IUserService userService) : Controller
{
    /// <summary>
    /// Deletes an user.
    /// </summary>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    public ActionResult Delete()
    {
        var user = User.GetUserInfo();
        if (user == null)
        {
            return Unauthorized();
        }

        var result = userService.DeleteUser(user);
        return result ? Ok() : BadRequest();
    }
}
