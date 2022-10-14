using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutBuddy.BusinessLogic.Base;
using WorkoutBuddy.BusinessLogic.Implementation.Rating;

namespace WorkoutBuddy.BusinessLogic.Implementation.Home
{
    public class HomeService : BaseService
    {
        private readonly RatingService service;
        private readonly SplitService splitService;
        public HomeService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
            service = new RatingService(serviceDependencies);
            splitService = new SplitService(serviceDependencies);
        }

        public HomeModel GetHomeItems()
        {
            var ranking = service.GetRanking();
            var userId = service.GetRanking()[0].UserId;
            var pointsNo = service.GetRanking()[0].PointsNo;

            var user = UnitOfWork.Users.Get()
                        .Include(u => u.Splits)
                        .FirstOrDefault(u => u.Iduser == userId);

            var splitNames = user.Splits.Select(s => s.Name).ToList();

            var userOfTheWeek = new UserOfTheWeekModel()
            {
                Username = user.Username,
                PointsNo = pointsNo,
                SplitsNames = splitNames,
            };


            //check if split exists
            var split = splitService.GetSplits().FirstOrDefault();
            return new HomeModel()
            {
                Split = split,
                User = userOfTheWeek
            };
        }
    }
}
