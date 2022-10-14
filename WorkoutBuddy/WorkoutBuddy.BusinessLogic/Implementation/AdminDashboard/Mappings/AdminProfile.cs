using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutBuddy.Entities;

namespace WorkoutBuddy.BusinessLogic.Implementation.AdminDashboard
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<User, UserModel>()
                .ForMember(s => s.UserId, s => s.MapFrom(s => s.Iduser))
                .ForMember(s => s.Email, s => s.MapFrom(s => s.Email))
                .ForMember(s => s.Username, s => s.MapFrom(s => s.Username))
                .ForMember(s => s.Name, s => s.MapFrom(s => s.Name))
                ;
        }

        
    }
}
