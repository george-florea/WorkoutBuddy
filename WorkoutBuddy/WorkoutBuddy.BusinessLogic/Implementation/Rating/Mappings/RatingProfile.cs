using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutBuddy.Entities;

namespace WorkoutBuddy.BusinessLogic.Implementation.Rating
{
    public class RatingProfile : Profile
    {
        public RatingProfile()
        {
            CreateMap<User, RankingListItem>()
                .ForMember(s => s.PointsNo, s => s.MapFrom(s => 0))
                .ForMember(s => s.UserId, s => s.MapFrom(s => s.Iduser))
                .ForMember(s => s.UserName, s => s.MapFrom(s => s.Username))
                ;
        }
    }
}
