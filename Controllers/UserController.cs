using System.Security.Claims;
using CustomAuthorize.Data;
using CustomAuthorize.Data.VieModel;
using CustomAuthorize.DataContexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CustomAuthorize.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly DataContext _context;

    public UserController(DataContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<UserModel>> Create([FromBody] CreateUser body)
    {
        if (_context.Users == null) return BadRequest();
        var data = await _context.Users
            .AddAsync(new UserModel(id: 0, name: body.Name, email: body.Email,
                username: body.Username));
        await _context.SaveChangesAsync();
        return Ok(data.Entity);
    }
    
    [HttpGet]
    [Securities.CustomAuthorize("GetUser")]
    public async Task<ActionResult<List<UserModel>>> Get()
    {
        if (_context.Users == null) return BadRequest();
        var data = await _context.Users.Include(i => i.Roles).ToListAsync();
        return Ok(data);
    }
    
    [HttpGet("get-roles")]
    [Securities.CustomAuthorize("GetRoles")]
    public async Task<ActionResult<List<UserModel>>> GetRoles()
    {
        if (_context.Roles == null) return BadRequest();
        var data = await _context.Roles.ToListAsync();
        return Ok(data);
    }

    [HttpPost("add-roles")]
    public async Task<ActionResult<List<RoleModel>>> AddRole([FromBody] List<CreateRole> body)
    {
        if (_context.Roles == null) return BadRequest();
        await _context.Roles
            .AddRangeAsync(body.Select(i => new RoleModel(id: 0, userModelId: i.UserModelId, name: i.Name)).ToList());
        await _context.SaveChangesAsync();

        var data = await _context.Roles
            .Where(i => i.UserModelId == body[0].UserModelId).ToListAsync();
        return Ok(data);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]Login body)
    {
        var usr = await _context.Users?
            .Include(i=> i.Roles)
            .FirstOrDefaultAsync(i => i.Username == body.Username)!;
        if (usr is null) return Unauthorized();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usr.Username),
            new Claim(ClaimTypes.Role, JsonConvert.SerializeObject(usr.Roles.Select(i=> i.Name)) ?? string.Empty)
        };
        var claimsIdentity = new ClaimsIdentity(claims, "Login");
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        return Ok();
    }
}