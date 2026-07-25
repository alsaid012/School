using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using SchoolERP.Application.DTOs.Auth;
using SchoolERP.Application.DTOs.Common;
using System.Net.Http.Json;
using Xunit;

namespace SchoolERP.Tests.IntegrationTests.Controllers
{
    public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public AuthControllerTests(WebApplicationFactory<Program> factory)
        {
            // ✅ إنشاء client مع إعدادات صحيحة
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost:44316"),
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Login_WithValidCredentials_ShouldReturnToken()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "admin",
                Password = "Admin@123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Auth/login", loginDto);

            // Assert
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadFromJsonAsync<ResponseDto<TokenDto>>();
            result.Should().NotBeNull();
            result!.Success.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Data!.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ShouldReturnUnauthorized()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "wronguser",
                Password = "WrongPassword"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/Auth/login", loginDto);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }
    }
}