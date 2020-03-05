using Server.Databases;
using Server.Databases.Redis.Core;
using Server.Databases.Redis.Models;
using Server.Databases.Sql.Models;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public interface ILeaderboardService
    {
        Task<Leaderboard> GetLeaderboard(string leaderboardId);
        Task<bool> DeleteLeaderboardEntries(string leaderboardId);
        Task<bool> DeleteLeaderboardEntry(string leaderboardId, string member);
        Task<LeaderboardEntry> FindLeaderboardEntry(string leaderboardId, string member);
        Task<IEnumerable<LeaderboardEntry>> FindLeaderboardEntries(string leaderboardId, long start = 0, long stop = -1);
        Task<IEnumerable<LeaderboardEntry>> FindLeaderboardEntries(string leaderboardId, string member, long start = 0, long stop = -1);
        Task<LeaderboardEntry> UpdateLeaderboardEntry(string leaderboardId, string member, double score);
    }

    public class LeaderboardService : ILeaderboardService
    {
        private readonly IServerUnitOfWork _unitOfWork;
        private readonly ICacheClient _cache;
        private readonly IDatabase _database;

        public LeaderboardService(IServerUnitOfWork unitOfWork, IRepository redis)
        {
            _unitOfWork = unitOfWork;
            _cache = redis.GetCacheClient();
            _database = _cache.Database;
        }

        private string ToKey(string leaderboardId)
        {
            return leaderboardId;
        }

        public async Task<Leaderboard> GetLeaderboard(string leaderboardId)
        {
            return await _unitOfWork.Leaderboards.GetAsync(x => x.Id == leaderboardId);
        }

        public async Task<bool> DeleteLeaderboardEntry(string leaderboardId, string member)
        {
            var key = ToKey(leaderboardId);

            return await _database.SortedSetRemoveAsync(key, member);
        }

        public async Task<bool> DeleteLeaderboardEntries(string leaderboardId)
        {
            var key = ToKey(leaderboardId);

            return await _database.KeyDeleteAsync(key);
        }

        public async Task<LeaderboardEntry> FindLeaderboardEntry(string leaderboardId, string member)
        {
            var leaderboardDB = await GetLeaderboard(leaderboardId);
            if (leaderboardDB == null)
                return null;

            var key = ToKey(leaderboardDB.Id);

            double? rscore = await _database.SortedSetScoreAsync(key, member);
            if (!rscore.HasValue)
                return null;    //점수가 없으면 한번도 안한 사람임 점수가 있으면 랭킹이 무조건 있겠지

            double score = rscore.Value;

            long rank = 0;
            long? rrank = await _database.SortedSetRankAsync(key, member, (Order)leaderboardDB.Order);
            if (rrank.HasValue)
                rank = rrank.Value + 1;

            return new LeaderboardEntry()
            {
                LeaderboardId = leaderboardId,
                Member = member,
                Rank = rank,
                Score = score,
            };
        }

        public async Task<IEnumerable<LeaderboardEntry>> FindLeaderboardEntries(string leaderboardId, long start = 0, long stop = -1)
        {
            var leaderboardDB = await GetLeaderboard(leaderboardId);
            if (leaderboardDB == null)
                return null;

            var key = ToKey(leaderboardDB.Id);
            SortedSetEntry[] entries = await _database.SortedSetRangeByRankWithScoresAsync(key, start, stop, (Order)leaderboardDB.Order);

            var leaderboardentries = entries.Select((x, i) => new LeaderboardEntry()
            {
                LeaderboardId = leaderboardId,
                Member = x.Element,
                Rank = (start + i + 1),
                Score = x.Score
            });

            return leaderboardentries;
        }

        public async Task<IEnumerable<LeaderboardEntry>> FindLeaderboardEntries(string leaderboardId, string member, long start = 0, long stop = -1)
        {
            var leaderboardDB = await GetLeaderboard(leaderboardId);
            if (leaderboardDB == null)
                return null;

            var targetentry = await FindLeaderboardEntry(leaderboardId, member);
            if (targetentry == null)
                return null;

            var targetrank = targetentry.Rank;
            var targetstart = targetrank + start;
            var targetstop = targetrank + stop;

            var key = ToKey(leaderboardDB.Id);
            var entries = await _database.SortedSetRangeByRankWithScoresAsync(key, targetstart, targetstop, (Order)leaderboardDB.Order);

            var leaderboardentries = entries.Select((x, i) => new LeaderboardEntry()
            {
                LeaderboardId = leaderboardId,
                Member = x.Element,
                Rank = (targetstart + i + 1),
                Score = x.Score
            });

            return leaderboardentries;
        }

        public async Task<LeaderboardEntry> UpdateLeaderboardEntry(string leaderboardId, string member, double score)
        {
            var leaderboardDB = await GetLeaderboard(leaderboardId);
            if (leaderboardDB == null)
                return null;

            return await UpdateLeaderboardEntry(leaderboardDB.Id, member, score, leaderboardDB.Order, leaderboardDB.ScoreType);
        }

        private async Task<LeaderboardEntry> UpdateLeaderboardEntry(string leaderboardId, string member, double score, LeaderboardOrder order, LeaderboardScoreType scoreType)
        {
            bool result = false;
            double newscore = 0;

            var key = ToKey(leaderboardId);

            switch (scoreType)
            {
                case LeaderboardScoreType.Overwriting:
                    {
                        result = await _database.SortedSetAddAsync(key, member, score);
                        if (result)
                            newscore = score;
                    }
                    break;
                case LeaderboardScoreType.HighScore:
                    {
                        double? oldscore = await _database.SortedSetScoreAsync(key, member);

                        bool isbest = true;

                        if (oldscore.HasValue)
                        {
                            result = true;
                            newscore = oldscore.Value;

                            switch (order)
                            {
                                case LeaderboardOrder.Ascending: isbest = oldscore > score; break;
                                case LeaderboardOrder.Descending: isbest = oldscore < score; break;
                            }
                        }

                        if (isbest)
                        {
                            result = await _database.SortedSetAddAsync(key, member, score);
                            if (result)
                                newscore = score;
                        }
                    }
                    break;
                case LeaderboardScoreType.Increment:
                    {
                        double? rscore = await _database.SortedSetIncrementAsync(key, member, score);
                        if (rscore.HasValue)
                        {
                            newscore = rscore.Value;
                            result = true;
                        }
                    }
                    break;
                case LeaderboardScoreType.Decrement:
                    {
                        double? rscore = await _database.SortedSetDecrementAsync(key, member, score);
                        if (rscore.HasValue)
                        {
                            newscore = rscore.Value;
                            result = true;
                        }
                    }
                    break;
            }

            if (!result)
                return null;

            long newrank = 0;
            long? rrank = await _database.SortedSetRankAsync(key, member, (Order)order);
            if (rrank.HasValue)
                newrank = rrank.Value + 1;

            return new LeaderboardEntry()
            {
                LeaderboardId = leaderboardId,
                Member = member,
                Rank = newrank,
                Score = newscore,
            };
        }
    }
}
