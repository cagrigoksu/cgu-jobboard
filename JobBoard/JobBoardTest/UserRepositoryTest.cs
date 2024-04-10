using JobBoard.Models.Data;
using JobBoard.Repositories.Interfaces;
using Moq;
using JobBoard.Services;

namespace JobBoardTest
{
    public class UserRepositoryTest
    {

        private readonly UserService _userService;
        private readonly Mock<IUserRepository> _userRepositoryMock = new();

        public UserRepositoryTest()
        {
            _userService = new UserService(_userRepositoryMock.Object);
        }


        [Fact]
        public void UserLoginTest()
        {
            var email = "user@test.com";
            var password = "11";

            var user = new UserDataModel()
            {
                Id = 2,
                Email = email,
                Password = password
            };

            _userRepositoryMock.Setup(x => x.GetUser(email, password))
                .Returns(user);

            var result = _userService.GetUser(email,password);
            
            Assert.Equal(user.Id, result.Id);
        }

    }
}