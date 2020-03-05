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
    [Route("api/leaderboards")]
    public class LeaderboardsController : Controller
    {
        private readonly IServerUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LeaderboardsController(
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
                var leaderboardsDB = await _unitOfWork.Leaderboards.GetManyAsync();
                if (leaderboardsDB == null)
                    return Payloader.Fail(PayloadCode.DbNull);

                return Payloader.Success(_mapper.Map<IEnumerable<LeaderboardViewModel>>(leaderboardsDB));
            }
            catch (Exception ex)
            {
                return Payloader.Error(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]LeaderboardViewModel leaderboard)
        {
            try
            {
                var leaderboardId = leaderboard.Id;

                var leaderboardDB = await _unitOfWork.Leaderboards.GetAsync(u => u.Id == leaderboardId);
                if (leaderboardDB != null)
                    return Payloader.Fail(PayloadCode.Duplication);

                var newLeaderboard = new Leaderboard
                {
                    Id = leaderboardId,
                    Order = leaderboard.Order,
                    ScoreType = leaderboard.ScoreType,
                };

                await _unitOfWork.Leaderboards.AddAsync(newLeaderboard);
                await _unitOfWork.CommitAsync();

                return Payloader.Success(_mapper.Map<LeaderboardViewModel>(newLeaderboard));
            }
            catch (Exception ex)
            {
                return Payloader.Error(ex);
            }
        }

        [HttpPut("{leaderboardId}")]
        public async Task<IActionResult> Put(string leaderboardId, [FromBody]LeaderboardViewModel leaderboard)
        {
            try
            {
                var leaderboardDB = await _unitOfWork.Leaderboards.GetAsync(u => u.Id == leaderboardId, isTracking: true);
                if (leaderboardDB == null)
                    return Payloader.Fail(PayloadCode.DbNull);

                leaderboardDB.Order = leaderboard.Order;
                leaderboardDB.ScoreType = leaderboard.ScoreType;

                await _unitOfWork.CommitAsync();

                return Payloader.Success(_mapper.Map<LeaderboardViewModel>(leaderboardDB));
            }
            catch (Exception ex)
            {
                return Payloader.Error(ex);
            }
        }

        [HttpDelete("{leaderboardId}")]
        public async Task<IActionResult> Delete(string leaderboardId)
        {
            try
            {
                var leaderboardDB = await _unitOfWork.Leaderboards.GetAsync(u => u.Id == leaderboardId);
                if (leaderboardDB == null)
                    return Payloader.Fail(PayloadCode.DbNull);

                await _unitOfWork.Leaderboards.DeleteAsync(leaderboardDB);
                await _unitOfWork.CommitAsync();

                return Payloader.Success(leaderboardId);
            }
            catch (Exception ex)
            {
                return Payloader.Error(ex);
            }
        }
    }
}
