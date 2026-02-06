namespace DigitalLibrary.API.Common;

using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;

public static class UserClaimsHelper
{
    public static bool TryGetUserId(ControllerBase controller, out int userId)
    {
        var userIdFromClaim = controller.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        
        return int.TryParse(userIdFromClaim, out userId);
    }
}