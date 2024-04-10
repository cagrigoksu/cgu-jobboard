using JobBoard;
using JobBoard.Models.Data;
using JobBoard.Repositories.Interfaces;
using Moq;
using JobBoard.Services;
using JobBoard.Services.Interfaces;

namespace JobBoardTest
{
    public class UserRepositoryTest
    {

        private readonly UserService _userService;
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly Mock<ISecurityService> _securityServiceMock = new();

        public UserRepositoryTest()
        {
            _userService = new UserService(_userRepositoryMock.Object);
        }


        [Fact]
        public void UserLoginTest()
        {
            var email = "user@test.com";
            var password = "Goksu";
            var pwdSalt = "QWd+52BpnVuQxp5o/oJpuA==";
            var hashedPwd = _securityServiceMock.Object.Hasher(password,pwdSalt,Globals.HashIter);

            var user = new UserDataModel()
            {
                Email = email,
                PasswordHash = hashedPwd
            };

            _userRepositoryMock.Setup(x => x.GetUser(email))
                .Returns(user);

            var result = _userService.GetUser(email);
            
            Assert.Equal(user.Id, result.Id);
        }

    }
}