using Bogus;
using TaskManager.Core.Application.Helpers;
using TaskManager.Core.Application.ViewModels.Task;
using TaskManager.Core.Application.ViewModels.User;
using TaskManager.Core.Domain.Entities;
using entityTask = TaskManager.Core.Domain.Entities.Task;

namespace TaskManager.Test
{
    public class TestDataGenerator
    {
        #region user
        public static User GenerateUser()
        {
            var userFaker = new Faker<User>()
                .RuleFor(user => user.Id, f => f.Random.Int(1, 999))
                .RuleFor(user => user.Name, f => f.Name.FirstName())
                .RuleFor(user => user.LastName, f => f.Name.LastName())
                .RuleFor(user => user.UserName, f => f.Internet.UserName())
                .RuleFor(user => user.Email, f => f.Internet.Email())
                .RuleFor(user => user.Password, PasswordEncryption.ComputeShad256Hash("test"))
                .RuleFor(user => user.CreatedBy, f => f.Name.FirstName())
                .RuleFor(user => user.Created, f => f.Date.Recent());

            return userFaker.Generate();
        }

        public static List<User> GenerateUserList(int count) 
        {
            List<User> users = new List<User>();
            for (int i = 0; i < count; i++)
            {
                users.Add(GenerateUser());
            }
            return users;
        }

        public static UserViewModel GenerateUserViewModel()
        {
            var userViewModelFaker = new Faker<UserViewModel>()
                .RuleFor(userVm => userVm.Id, f => f.Random.Int(1, 999))
                .RuleFor(userVm => userVm.Name, f => f.Name.FirstName())
                .RuleFor(userVm => userVm.LastName, f => f.Name.LastName())
                .RuleFor(userVm => userVm.UserName, f => f.Internet.UserName())
                .RuleFor(userVm => userVm.Email, f => f.Internet.Email());

            return userViewModelFaker.Generate();
        }

        public static LoginViewModel GenerateLoginViewModel() 
        { 
            var loginViewModelFaker = new Faker<LoginViewModel>()
                .RuleFor(loginVm => loginVm.UserName, f => f.Internet.UserName())
                .RuleFor(loginVm => loginVm.Password, f => f.Internet.Password());

            return loginViewModelFaker.Generate();
        }

        public static LoginSuccessViewModel GenerateLoginSuccessViewModel()
        {
            var userAuthenticatedViewModelFaker = new Faker<UserAuthenticatedViewModel>()
                .RuleFor(userAuth => userAuth.Id, f => f.Random.Int(1, 999))
                .RuleFor(userAuth => userAuth.Name, f => f.Name.FirstName())
                .RuleFor(userAuth => userAuth.LastName, f => f.Name.LastName())
                .RuleFor(userAuth => userAuth.UserName, f => f.Internet.UserName())
                .RuleFor(userAuth => userAuth.Email, f => f.Internet.Email());

            var loginSuccessViewModelFaker = new Faker<LoginSuccessViewModel>()
                .RuleFor(login => login.User, userAuthenticatedViewModelFaker.Generate())
                .RuleFor(login => login.AccessToken, f => f.Random.AlphaNumeric(32));


            return loginSuccessViewModelFaker.Generate();
        }
        #endregion

        #region task
        public static entityTask GenerateTask()
        {
            var taskFaker = new Faker<entityTask>()
                .RuleFor(task => task.Id, f => f.Random.Int(1, 999))
                .RuleFor(task => task.Name, f => f.Name.JobTitle())
                .RuleFor(task => task.SerialNumber, f => f.Random.Int(1000, 99999))
                .RuleFor(task => task.Description, f => f.Lorem.Paragraph())
                .RuleFor(task => task.UserId, f => f.Random.Int(1, 999))
                .RuleFor(task => task.CreatedBy, f => f.Name.FirstName())
                .RuleFor(task => task.Created, f => f.Date.Recent());

            return taskFaker.Generate();
        }

        public static List<entityTask> GenerateTaskList(int count)
        {
            List<entityTask> tasks = new List<entityTask>();
            for (int i = 0; i < count; i++)
            {
                tasks.Add(GenerateTask());
            }
            return tasks;
        }

        public static TaskViewModel GenerateTaskViewModel()
        {
            var taskViewModelFaker = new Faker<TaskViewModel>()
                .RuleFor(taskVm => taskVm.Id, f => f.Random.Int(1, 999))
                .RuleFor(taskVm => taskVm.Name, f => f.Name.JobTitle())
                .RuleFor(taskVm => taskVm.SerialNumber, f => f.Random.Int(1000, 99999))
                .RuleFor(taskVm => taskVm.Description, f => f.Lorem.Paragraph())
                .RuleFor(taskVm => taskVm.UserId, f => f.Random.Int(1, 999));

            return taskViewModelFaker.Generate();
        }

        public static List<TaskViewModel> GenerateTaskViewModelList(int count) 
        {
            List<TaskViewModel> taskViewModels = new List<TaskViewModel>();
            for (int i = 0; i < count; i++)
            {
                taskViewModels.Add(GenerateTaskViewModel());
            }
            return taskViewModels;
        }
        #endregion
    }
}
