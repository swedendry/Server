using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Databases;
using Server.Services;
using System;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [Authorize]
    [Route("api/leaderboards/{leaderboardId}/members")]
    public class LeaderboardMembersController : Controller
    {
        private readonly IServerUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardMembersController(
            IServerUnitOfWork unitOfWork,
            IMapper mapper,
            ILeaderboardService leaderboardService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _leaderboardService = leaderboardService;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string leaderboardId, long start = 0, long stop = -1)
        {
            try
            {
                var entries = await _leaderboardService.FindLeaderboardEntries(leaderboardId, start, stop);
                if (entries == null)
                    return Payloader.Fail(PayloadCode.DbNull);

                return Payloader.Success(entries);
            }
            catch (Exception ex)
            {
                return Payloader.Error(ex);
            }
        }

        [HttpPut("{member}/score/{score}")]
        public async Task<IActionResult> Update(string leaderboardId, string member, double score)
        {
            try
            {
                var newentry = await _leaderboardService.UpdateLeaderboardEntry(leaderboardId, member, score);
                if (newentry == null)
                    return Payloader.Fail(PayloadCode.DbNull);

                return Payloader.Success(newentry);
            }
            catch (Exception ex)
            {
                return Payloader.Error(ex);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string leaderboardId)
        {
            try
            {
                var result = await _leaderboardService.DeleteLeaderboardEntries(leaderboardId);

                return Payloader.Success(leaderboardId);
            }
            catch (Exception ex)
            {
                return Payloader.Error(ex);
            }
        }

        [HttpDelete("{member}")]
        public async Task<IActionResult> Delete(string leaderboardId, string member)
        {
            try
            {
                var result = await _leaderboardService.DeleteLeaderboardEntry(leaderboardId, member);

                return Payloader.Success(new 
                {
                    leaderboardId,
                    member,
                });
            }
            catch (Exception ex)
            {
                return Payloader.Error(ex);
            }
        }
    }
}
