using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Server.Databases;
using Server.Databases.Sql.Models;
using Server.Extensions;
using Server.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Authorize]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IServerUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILeaderboardService _leaderboardService;

        public AuthController(
            IServerUnitOfWork unitOfWork,
            IMapper mapper,
            IConfiguration configuration,
            ILeaderboardService leaderboardService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _leaderboardService = leaderboardService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var authToken = Request.Headers["Authorization"].ToString();
                var tokenString = authToken.Substring(7);

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadToken(tokenString);
                var userId = ((JwtSecurityToken)token).Payload["nameid"].ToString();

                var userDB = await _unitOfWork.Users.GetAsync(u => u.Id == userId);
                if (userDB == null)
                    return Payloader.Fail(PayloadCode.DbNull);

                return Payloader.Success(_mapper.Map<UserViewModel>(userDB));
            }
            catch (Exception ex)
            {
                return Payloader.Error(ex);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]UserViewModel user)
        {
            try
            {
                var userDB = await _unitOfWork.Users.GetAsync(u => u.Id == user.Id && u.Password == user.Password);
                if (userDB == null)
                    return Payloader.Fail(PayloadCode.DbNull);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier,userDB.Id.ToString())
                }),
                    Expires = DateTime.Now.AddMinutes(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                var userVM = _mapper.Map<UserViewModel>(userDB);

                await _leaderboardService.UpdateLeaderboardEntry("leaderboard:login", userVM.Id, 1);

                LogService.Log(LogType.Login, Request.ToIP(), userVM.Id, userVM);

                return Payloader.Success(new
                {
                    user = userVM,
                    token = tokenString,
                });
            }
            catch (Exception ex)
            {
                LogService.Log(LogType.Error, Request.ToIP(), user.Id, new
                {
                    method = "Login",
                    message = ex.ToMessage(),
                });

                return Payloader.Error(ex);
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserViewModel user)
        {
            try
            {
                var userDB = await _unitOfWork.Users.GetAsync(u => u.Id == user.Id);
                if (userDB != null)
                    return Payloader.Fail(PayloadCode.Duplication);

                var newUser = new User
                {
                    Id = user.Id,
                    Password = user.Password,
                };

                await _unitOfWork.Users.AddAsync(newUser);
                await _unitOfWork.CommitAsync();

                var userVM = _mapper.Map<UserViewModel>(newUser);

                LogService.Log(LogType.Register, Request.ToIP(), userVM.Id, userVM);

                return Payloader.Success(userVM);
            }
            catch (Exception ex)
            {
                LogService.Log(LogType.Error, Request.ToIP(), user.Id, new
                {
                    method = "Register",
                    message = ex.ToMessage(),
                });

                return Payloader.Error(ex);
            }
        }
    }
}
