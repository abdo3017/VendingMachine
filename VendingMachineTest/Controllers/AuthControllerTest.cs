using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.Collections;
using Assert = Xunit.Assert;
using VendingMachineTest.Utilts;
using VendingMachine;
using VendingMachineAPI.Presentation.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace VendingMachineTest.Controllers
{
    public class AuthControllerTest : IntegrationTest
    {
        public AuthControllerTest(ApiWebApplicationFactory<Program> fixture) : base(fixture)
        {
            url = "/Auth/Token";

        }



        [Theory]
        [ClassData(typeof(SuccessTokenRequestModelData))]

        public async Task Get_Token_Success_Async(TokenRequestModel token)
        {
            //Arrange
            string inputJson = JsonSerializer.Serialize(token);
            var inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync(url, inputContent);

            //Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response);
        }

        [Theory]
        [ClassData(typeof(FailedTokenRequestModelData))]

        public async Task Get_Token_Fail_Async(TokenRequestModel token)
        {
            //Arrange
            string inputJson = JsonSerializer.Serialize(token);
            var inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync(url, inputContent);

            //Assert
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
            Assert.NotNull(response);
        }

        public class SuccessTokenRequestModelData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                   new TokenRequestModel
                   {
                       UserName="abdoghazi",
                       Password="123456Ab@"
                   }

            };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class FailedTokenRequestModelData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                   new TokenRequestModel
                   {
                       UserName="abdoghazi",
                       Password="123456@Ab"
                   }

            };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

    }
}
