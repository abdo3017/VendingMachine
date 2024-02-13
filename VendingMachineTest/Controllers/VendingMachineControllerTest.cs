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
using VendingMachineAPI.InfraStructure.Entity;

namespace VendingMachineTest.Controllers
{
    public class VendingMachineControllerTest : IntegrationTest
    {
        public VendingMachineControllerTest(ApiWebApplicationFactory<Program> fixture) : base(fixture)
        {
            url = "/VendingMachine/";

        }



        [Theory]
        [InlineData(2,5)]

        public async Task Buy_Async(int productId, int amount)
        {
            //Arrange
            url += $"Buy?productId={productId}&amount={amount}";
            
            //Act
            var response = await _client.PostAsync(url,null);

            //Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response);
        }

        [Theory, MemberData(nameof(SuccessDepositModelData))]

        public async Task Deposit_Async(List<Coins> coins)
        {
            //Arrange
            url += "Deposit";
            string inputJson = JsonSerializer.Serialize(coins);
            var inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync(url, inputContent);

            //Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response);
        }
        [Fact]

        public async Task Reset_Async()
        {
            //Arrange
            url += "Reset";

            //Act
            var response = await _client.PostAsync(url, null);

            //Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response);
        }

        public static IEnumerable<object[]> SuccessDepositModelData =>
           new List<object[]>
           {
                new object[] {
                    2,
                     new List<Coins>
                       {
                         new Coins
                         {
                             Cent=50,
                             Amount=3
                         }
                       }
                }
           };

    }
}
