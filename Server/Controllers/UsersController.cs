using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Databases;
using Server.Databases.Sql.Models;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Authorize]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IServerUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsersController(
            IServerUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usersDB = await _unitOfWork.Users.GetManyAsync();
                if (usersDB == null)
                    return Payloader.Fail(PayloadCode.DbNull);

                return Payloader.Success(_mapper.Map<IEnumerable<User>>(usersDB));
            }
            catch (Exception ex)
            {
                return Payloader.Error(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]UserViewModel user)
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

                return Payloader.Success(_mapper.Map<UserViewModel>(newUser));
            }
            catch (Exception ex)
            {
                return Payloader.Error(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody]UserViewModel user)
        {
            try
            {
                var userDB = await _unitOfWork.Users.GetAsync(u => u.Id == id, isTracking: true);
                if (userDB == null)
                    return Payloader.Fail(PayloadCode.DbNull);

                userDB.Password = user.Password;

                await _unitOfWork.CommitAsync();

                return Payloader.Success(_mapper.Map<UserViewModel>(userDB));
            }
            catch (Exception ex)
            {
                return Payloader.Error(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var userDB = await _unitOfWork.Users.GetAsync(u => u.Id == id);
                if (userDB == null)
                    return Payloader.Fail(PayloadCode.DbNull);

                await _unitOfWork.Users.DeleteAsync(userDB);
                await _unitOfWork.CommitAsync();

                return Payloader.Success(id);
            }
            catch (Exception ex)
            {
                return Payloader.Error(ex);
            }
        }
    }
}
