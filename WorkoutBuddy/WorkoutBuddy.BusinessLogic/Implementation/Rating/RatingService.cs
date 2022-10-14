using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutBuddy.BusinessLogic.Base;
using WorkoutBuddy.Common.Exceptions;
using WorkoutBuddy.Entities;
using WorkoutBuddy.Entities.Enums;

namespace WorkoutBuddy.BusinessLogic.Implementation.Rating
{
    public class RatingService : BaseService
    {
        public RatingService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
        }

        public bool PostRating(int rating, Guid splitId, Guid userId, int multiplier)
        {
            var isValid = true;
            ExecuteInTransaction(uow =>
            {
                var split = uow.Splits.Get()
                            .Include(s => s.IdcreatorNavigation)
                                .ThenInclude(c => c.UserPointsHistories)
                            .FirstOrDefault(s => s.Idsplit == splitId);
                if (split == null)
                {
                    throw new NotFoundErrorException("the split does not exist!");
                }

                var userSplit = uow.UserSplits.Get()
                            .Include(s => s.IduserNavigation)
                            .Include(s => s.IdsplitNavigation)
                                .ThenInclude(s => s.IdcreatorNavigation)
                                    .ThenInclude(c => c.UserPointsHistories)
                            .FirstOrDefault(s => s.Idsplit == splitId && s.IduserNavigation.Iduser == userId);

                if (userSplit == null)
                {
                    throw new NotFoundErrorException("you do not have this split in your colection");
                }
                if (userSplit.Rating.HasValue)
                {
                    split.IdcreatorNavigation.UserPointsHistories.Add(new UserPointsHistory()
                    {
                        Iduser = split.IdcreatorNavigation.Iduser,
                        ObtainDate = DateTime.Now.AddMilliseconds(-10),
                        PointsNo = -(userSplit.Rating.Value * multiplier),
                        Reasonid = (int)Reasons.SplitRating
                    });
                }

                userSplit.Rating = rating;

                split = userSplit.IdsplitNavigation;
                //update pointsNo
                split.IdcreatorNavigation.UserPointsHistories.Add(new UserPointsHistory()
                {
                    Iduser = split.IdcreatorNavigation.Iduser,
                    ObtainDate = DateTime.Now,
                    PointsNo = rating * multiplier,
                    Reasonid = (int)Reasons.SplitRating
                });

                try
                {
                    uow.UserSplits.Update(userSplit);
                    uow.Splits.Update(split);
                    uow.SaveChanges();
                }
                catch (Exception e)
                {
                    isValid = false;
                }
            });
            return isValid;
        }

        public List<RankingListItem> GetRanking()
        {
            var listOfUsers = UnitOfWork.Users.Get()
                                .Include(u => u.UserPointsHistories)
                                .ToList();

            var rankingList = new List<RankingListItem>();

            var startDate = StartOfWeek(DateTimeOffset.Now, DayOfWeek.Monday);
            var endDate = EndOfWeek(DateTimeOffset.Now, DayOfWeek.Sunday).AddHours(24).AddSeconds(-1);

            foreach (var user in listOfUsers)
            {
                var listItem = Mapper.Map<User, RankingListItem>(user);
                listItem.PointsNo = user.UserPointsHistories
                    .Where(p => DateTime.Compare(p.ObtainDate, startDate) >= 0 && DateTime.Compare(p.ObtainDate, endDate) <= 0)
                    .Sum(u => u.PointsNo);

                rankingList.Add(listItem);
            }

            return rankingList.OrderByDescending(s => s.PointsNo).Take(10).ToList();
        }

        public DateTime StartOfWeek(DateTimeOffset dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;

            return dt.AddDays(-1 * diff).Date;
        }
        public DateTime EndOfWeek(DateTimeOffset dt, DayOfWeek endOfWeek)
        {
            int diff = (7 + (endOfWeek - dt.DayOfWeek)) % 7;

            return dt.AddDays(diff).Date;
        }
    }
}
