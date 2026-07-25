using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SchoolERP.Application.DTOs.Auth;
using SchoolERP.Application.Interfaces;
using SchoolERP.Application.Interfaces.Repositories;
using SchoolERP.Application.Services;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;
using SchoolERP.Tests.Helpers;
using Xunit;

namespace SchoolERP.Tests.UnitTests.Application.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<ILogger<AuthService>> _loggerMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _configurationMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<AuthService>>();

            _unitOfWorkMock.Setup(u => u.Users).Returns(_userRepositoryMock.Object);

            // إعداد JWT Configuration
            _configurationMock.Setup(c => c["Jwt:Key"]).Returns("TestSecretKey12345678901234567890");
            _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("SchoolERPAPI");
            _configurationMock.Setup(c => c["Jwt:Audience"]).Returns("SchoolERPClient");
            _configurationMock.Setup(c => c["Jwt:ExpiryMinutes"]).Returns("60");

            _authService = new AuthService(
                _unitOfWorkMock.Object,
                null!, // IMapper مش محتاجينه هنا
                _configurationMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ShouldReturnToken()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "admin",
                Password = "Admin@123"
            };

            var user = TestDataHelper.CreateTestUser(1, "admin", "Admin User");
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123");
            user.UserRoles.Add(new UserRole { RoleType = UserType.Admin, IsPrimary = true });

            _userRepositoryMock.Setup(r => r.GetUserByUsernameAsync(loginDto.Username))
                .ReturnsAsync(user);

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Token.Should().NotBeNullOrEmpty();
            result.Data.Username.Should().Be("admin");
        }

        [Fact]
        public async Task LoginAsync_WithInvalidUsername_ShouldReturnUnauthorized()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "wronguser",
                Password = "WrongPassword"
            };

            _userRepositoryMock.Setup(r => r.GetUserByUsernameAsync(loginDto.Username))
                .ReturnsAsync((User?)null);

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(401);
            result.Message.Should().Contain("غير صحيحة");
        }

        [Fact]
        public async Task LoginAsync_WithWrongPassword_ShouldReturnUnauthorized()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "admin",
                Password = "WrongPassword"
            };

            var user = TestDataHelper.CreateTestUser(1, "admin", "Admin User");
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123");

            _userRepositoryMock.Setup(r => r.GetUserByUsernameAsync(loginDto.Username))
                .ReturnsAsync(user);

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(401);
            result.Message.Should().Contain("غير صحيحة");
        }

        [Fact]
        public async Task LoginAsync_WithPendingUser_ShouldReturnForbidden()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "pendinguser",
                Password = "Password@123"
            };

            var user = TestDataHelper.CreateTestUser(1, "pendinguser", "Pending User");
            user.Status = UserStatus.Pending;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password@123");

            _userRepositoryMock.Setup(r => r.GetUserByUsernameAsync(loginDto.Username))
                .ReturnsAsync(user);

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.StatusCode.Should().Be(403);
            result.Message.Should().Contain("انتظار التفعيل");
        }
    }
}