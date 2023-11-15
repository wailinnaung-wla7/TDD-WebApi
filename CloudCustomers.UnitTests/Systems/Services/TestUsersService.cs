using CloudCustomers.API.Config;
using CloudCustomers.API.Models;
using CloudCustomers.API.Services;
using CloudCustomers.UnitTests.Fixtures;
using CloudCustomers.UnitTests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CloudCustomers.UnitTests.Systems.Services
{
    public class TestUsersService
    {
        [Fact]
        public async Task GetAllUsers_WhenCalled_InvokesHttpGetRequest()
        {
            // Arrange
            var expectedResponse = UserFixture.GetTestUsers();
            var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourceList(expectedResponse);
            var httpClient = new HttpClient(handlerMock.Object);
            var endpoint = "https://example.com";
            var config = Options.Create(new UserApiOptions
            {
                Endpoint = endpoint
            });
            var sut = new UserService(httpClient,config);

            // Act
            await sut.GetAllUsers();
            // Assert
            handlerMock
                .Protected()
                .Verify("SendAsync",
                    Times.Exactly(1),
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>()
                    );
        }

        [Fact]
        public async Task GetAllUsers_WhenHits404_ReturnEmptyListOfUsers() 
        {
            // Arrange
            var handlerMock = MockHttpMessageHandler<User>.SetupReturn404();
            var httpClient = new HttpClient(handlerMock.Object);
            var endpoint = "https://example.com";
            var config = Options.Create(new UserApiOptions
            {
                Endpoint = endpoint
            });
            var sut = new UserService(httpClient,config);

            // Act
            var result = await sut.GetAllUsers();
            // Assert
            result.Count.Should().Be(0);    
        }

        //[Fact]
        //public async Task GetAllUsers_WhenCalled_ReturnsListOfExpectedSize()
        //{
        //    // Arrange
        //    var expectedResponse = UserFixture.GetTestUsers();           
        //    var endpoint = "https://example.com";
        //    var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourceList(expectedResponse, endpoint);
        //    var httpClient = new HttpClient(handlerMock.Object);
        //    var config = Options.Create(new UserApiOptions
        //    {
        //        Endpoint = endpoint
        //    });
        //    var sut = new UserService(httpClient, config);

        //    // Act
        //    var result = await sut.GetAllUsers();
        //    // Assert
        //    result.Count.Should().Be(expectedResponse.Count);
        //}

        [Fact]
        public async Task GetAllUsers_WhenCalled_InvokeConfiguredExternalUrl()
        {
            // Arrange
            var expectedResponse = UserFixture.GetTestUsers();
            var endpoint = "https://example.com/users";
            var handlerMock = MockHttpMessageHandler<User>.SetupBasicGetResourceList(expectedResponse,endpoint);
            var httpClient = new HttpClient(handlerMock.Object);
            var config = Options.Create(new UserApiOptions
            {
                Endpoint = endpoint
            });
            var sut = new UserService(httpClient,config);

            // Act
            var result = await sut.GetAllUsers();
            var uri = new Uri(endpoint);
            // Assert
            handlerMock
              .Protected()
              .Verify("SendAsync",
                  Times.Exactly(1),
                  ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == uri),
                  ItExpr.IsAny<CancellationToken>()
                  );
        }
    }
}
