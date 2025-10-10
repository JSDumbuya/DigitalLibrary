using Microsoft.AspNetCore.Mvc;

namespace DigitalLibrary.API.Controllers;

[ApiController]
[Route("api/users/{userId:int}")]
public class UserController : ControllerBase
{ }