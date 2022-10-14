using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutBuddy.BusinessLogic.Base;
using WorkoutBuddy.BusinessLogic.Implementation.UserSplitColection.Models;
using WorkoutBuddy.Common.Exceptions;
using WorkoutBuddy.Common.Extensions;
using WorkoutBuddy.Entities;
using WorkoutBuddy.Entities.Enums;

namespace WorkoutBuddy.BusinessLogic.Implementation.UserSplitColection
{
    public class UserSplitService : BaseService
    {
        private readonly AddProgressValidator addProgressValidator;
        public UserSplitService(ServiceDependencies serviceDependencies) : base(serviceDependencies)
        {
            this.addProgressValidator = new AddProgressValidator(UnitOfWork);
        }

        public List<UserSplitListItem> GetListOfSplits(Guid userId)
        {
            var userSplitsList = new List<UserSplitListItem>();
            var user = UnitOfWork.Users.Get()
                .Include(u => u.UserSplits)
                    .ThenInclude(us => us.UserWorkouts)
                .Include(u => u.UserSplits)
                    .ThenInclude(us => us.IdsplitNavigation)
                        .ThenInclude(s => s.Workouts)
                .SingleOrDefault(u => u.Iduser == userId);

            var splits = user.UserSplits.ToList();
            foreach (var split in splits)
            {
                var userSplit = Mapper.Map<UserSplit, UserSplitListItem>(split);
                userSplitsList.Add(userSplit);
            }

            return userSplitsList;
        }

        public UserSplitModel GetUserSplit(Guid splitId, Guid userId)
        {
            var split = UnitOfWork.UserSplits.Get()
                        .Include(us => us.IdsplitNavigation)
                            .ThenInclude(s => s.Workouts)
                        .FirstOrDefault(us => us.Idsplit == splitId && us.Iduser == userId);

            if (split == null)
            {
                throw new NotFoundErrorException("this split does not exist!");
            }

            var userSplit = Mapper.Map<UserSplit, UserSplitModel>(split);

            foreach (var workout in split.IdsplitNavigation.Workouts)
            {
                var workoutModel = Mapper.Map<Workout, WorkoutsListModel>(workout);
                userSplit.Workouts.Add(workoutModel);
            }

            return userSplit;
        }

        public UserWorkoutModel PopulateUserWorkoutModel(Guid id)
        {
            var workout = UnitOfWork.Workouts.Get()
                .Include(w => w.WorkoutExercises)
                    .ThenInclude(we => we.IdexerciseNavigation)
                        .ThenInclude(e => e.IdtypeNavigation)
                .FirstOrDefault(w => w.Idworkout == id);

            if (workout == null)
            {
                throw new NotFoundErrorException("this workout does not exist!");
            }

            var userExercises = new List<UserExerciseModel>();
            foreach (var ex in workout.WorkoutExercises.Select(we => we.IdexerciseNavigation))
            {
                var userExercise = Mapper.Map<Exercise, UserExerciseModel>(ex);
                userExercises.Add(userExercise);
            }

            var userWorkout = new UserWorkoutModel()
            {
                SplitId = workout.Idsplit,
                WorkoutId = workout.Idworkout,
                Exercises = userExercises
            };
            return userWorkout;
        }

        public List<DateTime> GetDates(int index, Guid workoutId, Guid userId, int noOfPages)
        {
            var userWorkouts = UnitOfWork.UserWorkouts.Get()
                                .Where(us => us.Idworkout == workoutId && us.Iduser == userId);
            var dates = userWorkouts.Select(us => us.Date)
                        .Skip(index * noOfPages)
                        .Take(noOfPages)
                        .ToList();
            return dates;
        }

        public int ComputeNoOfPages(Guid workoutId, Guid userId, int noOfPages)
        {
            var res = UnitOfWork.UserWorkouts
                            .Get()
                            .Include(us => us.UserExercises)
                                .ThenInclude(ue => ue.Id)
                                    .ThenInclude(we => we.IdexerciseNavigation)
                            .Include(us => us.UserExercises)
                                .ThenInclude(ue => ue.UserExerciseSets)
                            .Where(us => us.Idworkout == workoutId && us.Iduser == userId)
                            .ToList()
                            .Count / (float)noOfPages;
            if (res * 10 % 10 == 0)
            {
                return (int)res;
            }
            return (int)res + 1;
        }

        public HistoryModel GetHistory(Guid id, Guid userId)
        {
            var userWorkouts = UnitOfWork.UserWorkouts.Get()
                                .Include(us => us.IdNavigation)
                                .Include(us => us.UserExercises)
                                    .ThenInclude(e => e.Id)
                                        .ThenInclude(we => we.IdexerciseNavigation)
                                .Include(us => us.UserExercises)
                                    .ThenInclude(ue => ue.UserExerciseSets)
                                .Where(us => us.Idworkout == id && us.Iduser == userId)
                                .ToList();

            if (userWorkouts == null)
            {
                throw new NotFoundErrorException("there is no recored progress for this workout!");
            }

            if (userWorkouts.Count == 0)
            {
                return new HistoryModel()
                {
                    WorkoutId = id
                };
            }

            var workoutHistory = new WorkoutHistoryModel()
            {
                WorkoutId = userWorkouts[0].Idworkout,
                Date = userWorkouts[0].Date,
            };

            var exercises = new List<ExerciseHistoryModel>();
            foreach (var ex in userWorkouts[0].UserExercises)
            {
                var exHistory = Mapper.Map<UserExercise, ExerciseHistoryModel>(ex);

                var sets = new List<SetModel>();

                foreach (var set in ex.UserExerciseSets)
                {
                    sets.Add(Mapper.Map<UserExerciseSet, SetModel>(set));
                }

                exHistory.Sets = sets;
                exercises.Add(exHistory);
            }
            workoutHistory.Exercises = exercises;

            return new HistoryModel()
            {
                WorkoutId = userWorkouts[0].Idworkout,
                Dates = userWorkouts.Select(us => us.Date).ToList(),
                CoefList = userWorkouts.Select(us => us.WorkoutEffortCoefficient).ToList(),
                FirstWorkout = workoutHistory
            };
        }

        public WorkoutHistoryModel GetHistoryFor(Guid id, DateTime date, Guid userId)
        {
            var userWorkout = UnitOfWork.UserWorkouts.Get()
                            .Include(us => us.IdNavigation)
                            .Include(us => us.UserExercises)
                                .ThenInclude(e => e.Id)
                                    .ThenInclude(we => we.IdexerciseNavigation)
                            .Include(us => us.UserExercises)
                                .ThenInclude(ue => ue.UserExerciseSets)
                            .FirstOrDefault(us => us.Idworkout == id && us.Date == date);
            if (userWorkout == null)
            {
                throw new NotFoundErrorException("there is no recored progress for this workout!");
            }

            var workoutHistory = new WorkoutHistoryModel()
            {
                WorkoutId = id,
                Date = date,
            };

            var exercises = new List<ExerciseHistoryModel>();
            foreach (var ex in userWorkout.UserExercises)
            {
                var exHistory = Mapper.Map<UserExercise, ExerciseHistoryModel>(ex);

                var sets = new List<SetModel>();
                foreach (var set in ex.UserExerciseSets)
                {
                    sets.Add(Mapper.Map<UserExerciseSet, SetModel>(set));
                }

                exHistory.Sets = sets;
                exercises.Add(exHistory);

            }

            workoutHistory.Exercises = exercises;
            return workoutHistory;
        }

        public List<ExercisesProgressModel> GetProgress(Guid id, Guid userId, int pageIndex, int weekDays)
        {
            var workouts = UnitOfWork.UserWorkouts
                            .Get()
                            .Include(us => us.UserExercises)
                                .ThenInclude(ue => ue.Id)
                                    .ThenInclude(we => we.IdexerciseNavigation)
                            .Include(us => us.UserExercises)
                                .ThenInclude(ue => ue.UserExerciseSets)
                            .Where(us => us.Idworkout == id && us.Iduser == userId)
                            .Skip(pageIndex * weekDays)
                            .Take(weekDays)
                            .ToList();

            if (workouts == null)
            {
                throw new NotFoundErrorException("there is no recored progress for this workout!");
            }

            var exercises = new List<ExercisesProgressModel>();

            if (workouts.Count == 0)
            {
                exercises.Add(new ExercisesProgressModel() { WorkoutId = id });
                return exercises;
            }

            foreach (var ex in workouts[0].UserExercises)
            {
                exercises.Add(Mapper.Map<UserExercise, ExercisesProgressModel>(ex));
            }

            for (var index = 0; index < workouts.Count; index++)
            {
                for (var i = 0; i < workouts[index].UserExercises.Count; i++)
                {
                    var userEx = workouts[index].UserExercises.ToList()[i];

                    var day = Mapper.Map<UserExercise, DateProgressModel>(userEx);

                    if (day.SetsNo > exercises[i].MaxSets)
                    {
                        exercises[i].MaxSets = day.SetsNo;
                    }
                    foreach (var set in userEx.UserExerciseSets)
                    {
                        day.Sets.Add(Mapper.Map<UserExerciseSet, SetModel>(set));
                    }
                    exercises[i].Days.Add(day);
                }
            }

            return exercises;
        }

        public Guid AddProgress(UserWorkoutModel model, int pointsNo)
        {
            ExecuteInTransaction(uow =>
            {
                var validationRes = addProgressValidator.Validate(model);
                if (!validationRes.IsValid)
                {
                    validationRes.ThenThrow(PopulateUserWorkoutModel(model.WorkoutId));
                }

                var progress = Mapper.Map<UserWorkoutModel, UserWorkout>(model);
                progress.Iduser = model.UserId;


                var split = uow.Splits.Get()
                            .Include(s => s.UserSplits)
                                .ThenInclude(us => us.UserWorkouts)
                            .Include(s => s.UserSplits)
                                .ThenInclude(s => s.IduserNavigation)
                                    .ThenInclude(s => s.UserPointsHistories)
                            .Include(s => s.Workouts)
                                .ThenInclude(w => w.WorkoutExercises)
                                    .ThenInclude(we => we.IdexerciseNavigation)
                                        .ThenInclude(e => e.UserExercisePrs)
                            .FirstOrDefault(s => s.Idsplit == model.SplitId);

                if (split == null)
                {
                    throw new NotFoundErrorException("this split does not exist!");
                }

                var userSplit = split.UserSplits
                               .FirstOrDefault(us => us.Iduser == model.UserId);

                if (userSplit == null)
                {
                    throw new NotFoundErrorException("you do not have this split in your collection!");
                }

                progress.Idsplit = userSplit.Idsplit;
                progress.Id = userSplit;

                var splitWorkout = split.Workouts.FirstOrDefault(w => w.Idworkout == model.WorkoutId);
                if (splitWorkout == null)
                {
                    throw new NotFoundErrorException("this workout does not exist in the split!");
                }

                progress.IdNavigation = splitWorkout;

                var exercises = new List<UserExercise>();
                var workoutCoef = decimal.One - 1;

                foreach (var ex in model.Exercises)
                {
                    var workoutExercise = split.Workouts
                                        .FirstOrDefault(w => w.Idworkout == progress.Idworkout)
                                        .WorkoutExercises
                                        .FirstOrDefault(we => we.Idexercise == ex.ExerciseId);
                    if (workoutExercise == null)
                    {
                        throw new NotFoundErrorException("this exercise does not exist!");
                    }

                    var exercise = Mapper.Map<UserWorkout, UserExercise>(progress);
                    Mapper.Map<UserExerciseModel, UserExercise>(ex, exercise);
                    exercise.Id = workoutExercise;


                    var sets = new List<UserExerciseSet>();
                    foreach (var exSet in ex.Sets)
                    {
                        var set = Mapper.Map<UserExercise, UserExerciseSet>(exercise);
                        Mapper.Map<UserWorkout, UserExerciseSet>(progress, set);
                        Mapper.Map<SetModel, UserExerciseSet>(exSet, set);

                        if (ex.ExerciseType == (short)ExerciseTypes.Calisthenics)
                        {
                            var weightHistory = uow.UserWeightHistorys.Get()
                                        .Where(w => w.Iduser == model.UserId)
                                        .OrderByDescending(w => w.WeighingDate)
                                        .FirstOrDefault();
                            if (weightHistory == null)
                            {
                                throw new NotFoundErrorException("you don't have any recorded weight!");
                            }
                            var weight = weightHistory.Weight;
                            set.Weight = weight;
                        }
                        sets.Add(set);
                    }
                    if (ex.ExerciseType != (short)ExerciseTypes.Cardio)
                    {
                        var user = uow.Users.Get().FirstOrDefault(u => u.Iduser == model.UserId);
                        var coef = ExercisesCoefficientCalculator(sets, user);
                        if (!splitWorkout.UserWorkouts.Any(us => us.Iduser == user.Iduser))
                        {
                            coef = 1;
                        }
                        if (coef > 1)
                        {
                            userSplit.IduserNavigation.UserPointsHistories.Add(new UserPointsHistory()
                            {
                                Iduser = userSplit.Iduser,
                                ObtainDate = progress.Date,
                                PointsNo = pointsNo,
                                Reasonid = (int)Reasons.NewPr
                            });
                        }
                        exercise.EffortCoefficient = coef;
                        workoutCoef += coef;
                    }
                    exercise.UserExerciseSets = sets;
                    exercises.Add(exercise);
                }

                workoutCoef /= model.Exercises
                                .Select(e => e.ExerciseType)
                                .Where(e => e != (short)ExerciseTypes.Cardio)
                                .Count();

                progress.UserExercises = exercises;
                progress.WorkoutEffortCoefficient = workoutCoef;

                userSplit.UserWorkouts.Add(progress);

                uow.UserSplits.Update(userSplit);

                uow.SaveChanges();
            });

            return model.SplitId;
        }

        public void RemoveSplit(Guid id, Guid userId)
        {
            ExecuteInTransaction(uow =>
            {
                var userSplit = uow.UserSplits.Get()
                                    .FirstOrDefault(u => u.Idsplit == id && u.Iduser == userId);

                if (userSplit == null)
                {
                    throw new NotFoundErrorException("this split does not exist!");
                }

                uow.UserSplits.Delete(userSplit);
                uow.SaveChanges();
            });
        }

        private decimal ExercisesCoefficientCalculator(List<UserExerciseSet> sets, User? user)
        {
            var exerciseCoefficient = decimal.One;
            ExecuteInTransaction(uow =>
            {
                var isFirstTime = false;
                var userExercise = sets[0].UserExercise;
                var prEntity = userExercise
                        .Id
                        .IdexerciseNavigation
                        .UserExercisePrs
                        .FirstOrDefault(e => e.Idexercise == userExercise.Idexercise && e.Iduser == user.Iduser);
                var pr = new decimal?();
                if (prEntity == null)
                {
                    pr = decimal.MinusOne;

                    var userExercisePr = Mapper.Map<UserExercise, UserExercisePr>(userExercise);
                    userExercisePr.OneRepMax = pr;
                    userExercisePr.IduserNavigation = user;
                    uow.UserExercisePrs.Insert(userExercisePr);
                    isFirstTime = true;
                    uow.SaveChanges();
                }
                else
                {
                    pr = prEntity.OneRepMax;
                }

                var listOfCoef = new List<decimal?>();
                var newPr = pr;

                foreach (var set in sets)
                {
                    var orm = OneRepMaxCalculator(set.RepsNo, set.Weight);

                    if (pr == decimal.MinusOne)
                    {
                        pr = orm;
                        newPr = orm;
                    }

                    if (orm > pr)
                    {
                        newPr = orm;
                    }

                    var coef = orm / pr;
                    listOfCoef.Add(coef);
                }

                var exPr = uow.UserExercisePrs.Get()
                            .FirstOrDefault(pr => pr.Idexercise == userExercise.Idexercise);

                exPr.OneRepMax = newPr;
                uow.UserExercisePrs.Update(exPr);
                uow.SaveChanges();

                if (!isFirstTime)
                {
                    listOfCoef.Sort();
                    var exCoef = decimal.One - 1;
                    for (var i = 0; i < listOfCoef.Count; i++)
                    {
                        var res = (listOfCoef[i] * (i + 1) / (decimal.Parse(listOfCoef.Count.ToString()))) ?? 0;
                        exCoef += res;
                    }
                    var algebricSum = (1 / decimal.Parse(listOfCoef.Count.ToString()) + 1) * listOfCoef.Count / 2;
                    exerciseCoefficient = exCoef / algebricSum;
                }
                else
                {
                    exerciseCoefficient = 1;
                }
            });
            return exerciseCoefficient;
        }

        private decimal? OneRepMaxCalculator(int? reps, double? weight)
        {
            return (decimal?)(weight * 36 / (37 - reps));
        }

    }
}
