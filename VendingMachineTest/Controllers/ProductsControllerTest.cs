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
using Azure;
using Microsoft.AspNetCore.Mvc;

namespace VendingMachineTest.Controllers
{
    public class ProductsControllerTest : IntegrationTest
    {
        public ProductsControllerTest(ApiWebApplicationFactory<Program> fixture) : base(fixture)
        {
            url = "/Products";
            Helper.Configure("seller", "993e042c-66be-476d-8e4b-a8d4706af871", "mohamedghazi");
        }

        [Theory, ClassData(typeof(CreateSuccessProductModelData))]
        public async Task Create_Product_Success_Async(ProductModel product)
        {
            //Arrange
            string inputJson = JsonSerializer.Serialize(product);
            var inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync(url, inputContent);

            //Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response);
        }

        [Theory, ClassData(typeof(CreateFailedProductModelData))]
        public async Task Create_Product_Fail_Async(object product)
        {
            //Arrange
            string inputJson = JsonSerializer.Serialize(product);
            var inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync(url, inputContent);

            //Assert
            Assert.True(response.StatusCode == HttpStatusCode.InternalServerError);
        }


        [Theory, MemberData(nameof(UpdateSuccessProductModelData))]
        public async Task Update_Product_Success_Async(int productId, ProductModel product)
        {
            //Arrange
            string inputJson = JsonSerializer.Serialize(product);
            var inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

            //Act
            url += "?id=" + productId;
            var response = await _client.PutAsync(url, inputContent);

            //Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response);
        }

        [Theory, MemberData(nameof(UpdateFailedProductModelData))]
        public async Task Update_Product_Fail_Async(int productId, UserModel product)
        {
            //Arrange
            string inputJson = JsonSerializer.Serialize(product);
            var inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

            //Act
            url += "?id=" + productId;
            var response = await _client.PutAsync(url, inputContent);

            //Assert
            Assert.True(response.StatusCode == HttpStatusCode.InternalServerError);
            Assert.NotNull(response);
        }

        [Theory, MemberData(nameof(DeleteSuccessProductModelData))]
        public async Task Delete_User_Success_Async(int productId)
        {
            //Arrange
            url += "?id=" + productId;

            //Act
            var response = await _client.DeleteAsync(url);

            //Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response);
        }

        [Theory, MemberData(nameof(DeleteFailedProductModelData))]
        public async Task delete_User_Fail_Async(int productId)
        {
            //Arrange
            url += "?id=" + productId;

            //Act
            var response = await _client.DeleteAsync(url);

            //Assert
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
            Assert.NotNull(response);
        }

        /*data for testing user APIs*/
        public static IEnumerable<object[]> DeleteSuccessProductModelData =>
        new List<object[]>
        {
            new object[] {
                2
            }
        };
        public static IEnumerable<object[]> DeleteFailedProductModelData =>
        new List<object[]>
        {
            new object[] {
                2
            }
        };


        public static IEnumerable<object[]> UpdateSuccessProductModelData =>
        new List<object[]>
        {
            new object[] {
                2,
                 new ProductModel
                   {
                       Id = 2,
                        Name="shepsi",
                        AvailableAmount=100,
                        Cost=100
                   }
            }
        };

        public static IEnumerable<object[]> UpdateFailedProductModelData =>
        new List<object[]>
        {
            new object[] {
                2,
                 new ProductModel
                   {
                       Id = 1,
                        Name="shepsi",
                        AvailableAmount=100,
                        Cost=100
                   }
            }
        };

        public class CreateSuccessProductModelData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                   new ProductModel
                   {
                        Name="shepsi",
                        AvailableAmount=100,
                        Cost=100
                   }

                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class CreateFailedProductModelData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                   new 
                   {
                        Name="shepsi",
                        AvailableAmount=100,
                        Cost="2000"
                   }

                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }


    }
}
