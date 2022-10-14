using AutoMapper;
using System;
using WorkoutBuddy.BusinessLogic.Implementation.UserSplitColection.Models;
using WorkoutBuddy.Entities;

namespace WorkoutBuddy.BusinessLogic.Implementation.UserSplitColection
{
    public class UserSplitProfile : Profile
    {
        public UserSplitProfile()
        {
            CreateMap<UserSplit, UserSplitListItem>()
                .ForMember(a => a.SplitId, a => a.MapFrom(s => s.Idsplit))
                .ForMember(a => a.Name, a => a.MapFrom(s => s.IdsplitNavigation.Name))
                .ForMember(a => a.Description, a => a.MapFrom(s => s.IdsplitNavigation.Description))
                .ForMember(a => a.WorkoutsNo, a => a.MapFrom(s => s.IdsplitNavigation.Workouts.Count));

            CreateMap<UserSplit, UserSplitModel>()
                .ForMember(a => a.Idsplit, a => a.MapFrom(s => s.Idsplit))
                .ForMember(a => a.SplitName, a => a.MapFrom(s => s.IdsplitNavigation.Name))
                .ForMember(a => a.Description, a => a.MapFrom(s => s.IdsplitNavigation.Description))
                .ForMember(a => a.Rating, a => a.MapFrom(s => s.Rating))
                .ForMember(a => a.Iduser, a => a.MapFrom(s => s.Iduser))
                .ForMember(a => a.Workouts, a => a.MapFrom(s => new List<WorkoutsListModel>()));

            CreateMap<Workout, WorkoutsListModel>()
                .ForMember(a => a.WorkoutId, a => a.MapFrom(s => s.Idworkout))
                .ForMember(a => a.WorkoutName, a => a.MapFrom(s => s.Name));

            CreateMap<Exercise, UserExerciseModel>()
                 .ForMember(a => a.ExerciseId, a => a.MapFrom(s => s.Idexercise))
                 .ForMember(a => a.ExerciseName, a => a.MapFrom(s => s.Name))
                 .ForMember(a => a.ExerciseType, a => a.MapFrom(s => s.IdtypeNavigation.Idtype))
                 .ForMember(a => a.Sets, a => a.Ignore());

            CreateMap<UserWorkoutModel, UserWorkout>()
                .ForMember(s => s.Idworkout, s => s.MapFrom(s => s.WorkoutId))
                .ForMember(a => a.Date, a => a.MapFrom(s => s.Date))
                .ForMember(a => a.UserExercises, a => a.Ignore());

            CreateMap<UserExercise, ExerciseHistoryModel>()
                .ForMember(a => a.ExerciseId, a => a.MapFrom(s => s.Idworkout))
                .ForMember(a => a.ExerciseName, a => a.MapFrom(s => s.Id.IdexerciseNavigation.Name))
                .ForMember(a => a.ExerciseType, a => a.MapFrom(s => s.Id.IdexerciseNavigation.Idtype))
                .ForMember(a => a.IsPr, a => a.MapFrom(s => s.EffortCoefficient > 1 ? true : false))
                .ForMember(a => a.Sets, a => a.Ignore());

            CreateMap<UserExerciseSet, SetModel>()
                .ForMember(a => a.Reps, a => a.MapFrom(s => s.RepsNo))
                .ForMember(a => a.Weight, a => a.MapFrom(s => s.Weight))
                .ForMember(a => a.Duration, a => a.MapFrom(s => s.Duration))
                .ForMember(a => a.Distance, a => a.MapFrom(s => s.Distance));

            CreateMap<UserExercise, ExercisesProgressModel>()
                .ForMember(a => a.ExerciseName, a => a.MapFrom(s => s.Id.IdexerciseNavigation.Name))
                .ForMember(a => a.WorkoutId, a => a.MapFrom(s => s.Idworkout))
                .ForMember(a => a.Days, a => a.MapFrom(s => new List<DateProgressModel>()))
                .ForMember(a => a.MaxSets, a => a.MapFrom(s => 0));

            CreateMap<UserExercise, DateProgressModel>()
                .ForMember(a => a.Date, a => a.MapFrom(s => s.Date))
                .ForMember(a => a.SetsNo, a => a.MapFrom(s => s.SetsNo))
                .ForMember(a => a.Sets, a => a.MapFrom(s => new List<SetModel>()))
                .ForMember(a => a.ExerciseCoef, a => a.MapFrom(s => s.EffortCoefficient));

            CreateMap<UserSplit, UserWorkout>()
                .ForMember(a => a.Idsplit, a => a.MapFrom(s => s.Idsplit))
                .ForMember(a => a.Id, a => a.MapFrom(s => s));

            CreateMap<UserWorkout, UserExercise>()
                .ForMember(a => a.Iduser, a => a.MapFrom(s => s.Iduser))
                .ForMember(a => a.Idworkout, a => a.MapFrom(s => s.Idworkout))
                .ForMember(a => a.Date, a => a.MapFrom(s => s.Date))
                .ForMember(a => a.UserWorkout, a => a.MapFrom(s => s))
                .ForMember(a => a.Id, a => a.Ignore());

            CreateMap<UserExerciseModel, UserExercise>()
                .ForMember(a => a.Idexercise, a => a.MapFrom(s => s.ExerciseId))
                .ForMember(a => a.SetsNo, a => a.MapFrom(s => s.SetsNo))
                .ForMember(a => a.Id, a => a.Ignore())
                .ForMember(a => a.UserWorkout, a => a.Ignore());

            CreateMap<UserExercise, UserExerciseSet>()
                .ForMember(a => a.UserExercise, a => a.MapFrom(s => s))
                .ForMember(a => a.Idset, a => a.MapFrom(s => Guid.NewGuid()));

            CreateMap<UserWorkout, UserExerciseSet>()
                .ForMember(a => a.Iduser, a => a.MapFrom(s => s.Iduser))
                .ForMember(a => a.Idworkout, a => a.MapFrom(s => s.Idworkout))
                .ForMember(a => a.Date, a => a.MapFrom(s => s.Date))
                .ForMember(a => a.UserExercise, a => a.Ignore());

            CreateMap<SetModel, UserExerciseSet>()
                .ForMember(a => a.Distance, a => a.MapFrom(s => s.Distance))
                .ForMember(a => a.Duration, a => a.MapFrom(s => s.Duration))
                .ForMember(a => a.RepsNo, a => a.MapFrom(s => s.Reps))
                .ForMember(a => a.Weight, a => a.MapFrom(s => s.Weight))
                .ForMember(a => a.UserExercise, a => a.Ignore());

            CreateMap<UserExercise, UserExercisePr>()
                .ForMember(a => a.Idexercise, a => a.MapFrom(s => s.Idexercise))
                .ForMember(a => a.IdexerciseNavigation, a => a.MapFrom(s => s.Id.IdexerciseNavigation))
                .ForMember(a => a.Idpr, a => a.MapFrom(s => Guid.NewGuid()))
                .ForMember(a => a.IduserNavigation, a => a.Ignore());
        }
    }
}
