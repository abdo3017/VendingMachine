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
    public class UsersControllerTest : IntegrationTest
    {
        public UsersControllerTest(ApiWebApplicationFactory<Program> fixture) : base(fixture)
        {
            url = "/Users";
            Helper.Configure("seller", "8d5b4e83-9436-4f96-80ce-d42f3816a538", "maherdghazi");
        }



        [Theory, ClassData(typeof(CreateSuccessUserModelData))]
        public async Task Create_User_Success_Async(UserModel user)
        {
            //Arrange
            string inputJson = JsonSerializer.Serialize(user);
            var inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync(url, inputContent);

            //Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response);
        }

        [Theory, ClassData(typeof(CreateFailedUserModelData))]
        public async Task Create_User_Exist_UserName_Async(UserModel user)
        {
            //Arrange
            string inputJson = JsonSerializer.Serialize(user);
            var inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync(url, inputContent);

            //Assert
            Assert.True(response.StatusCode == HttpStatusCode.InternalServerError);
        }


        [Theory, MemberData(nameof(UpdateSuccessUserModelData))]
        public async Task Update_User_Success_Async(string userName, UserModel user)
        {
            //Arrange
            string inputJson = JsonSerializer.Serialize(user);
            var inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

            //Act
            url += "?userName=" + userName;
            var response = await _client.PutAsync(url, inputContent);

            //Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response);
        }

        [Theory, MemberData(nameof(UpdateFailedUserModelData))]
        public async Task Update_User_Fail_Async(string userName, UserModel user)
        {
            //Arrange
            string inputJson = JsonSerializer.Serialize(user);
            var inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");

            //Act
            url += "?userName=" + userName;
            var response = await _client.PutAsync(url, inputContent);

            //Assert
            Assert.True(response.StatusCode == HttpStatusCode.InternalServerError);
            Assert.NotNull(response);
        }

        [Theory, MemberData(nameof(DeleteSuccessUserModelData))]
        public async Task Delete_User_Success_Async(string userName)
        {
            //Arrange
            url += "?userName=" + userName;

            //Act
            var response = await _client.DeleteAsync(url);

            //Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(response);
        }

        [Theory, MemberData(nameof(DeleteFailUserModelData))]
        public async Task delete_User_Fail_Async(string userName)
        {
            //Arrange
            url += "?userName=" + userName;

            //Act
            var response = await _client.DeleteAsync(url);

            //Assert
            Assert.True(response.StatusCode == HttpStatusCode.NotFound);
            Assert.NotNull(response);
        }

        /*data for testing user APIs*/
        public static IEnumerable<object[]> DeleteSuccessUserModelData =>
        new List<object[]>
        {
            new object[] {
                "maherdghazi"
            }
        };
        public static IEnumerable<object[]> DeleteFailUserModelData =>
        new List<object[]>
        {
            new object[] {
                "maherghazi"
            }
        };


        public static IEnumerable<object[]> UpdateSuccessUserModelData =>
        new List<object[]>
        {
            new object[] {
                "abdoghazi",
                new UserModel
                   {

                        FirstName="maherrr",
                        LastName="ghazi",
                        UserName="abdoghazi",
                        Password="123456Ab@",
                        Role="buyer",
                        Deposit=100
                   }
            }
        };

        public static IEnumerable<object[]> UpdateFailedUserModelData =>
        new List<object[]>
        {
            new object[] {
                "abdoghazi",
                new UserModel
                   {

                        FirstName="maherrr",
                        LastName="ghazi",
                        UserName="abdoghazi",
                        Password="123456A",
                        Role="buyer",
                        Deposit=100
                   }
            }
        };

        public class CreateSuccessUserModelData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                   new UserModel
                   {

                        FirstName="maherrr",
                        LastName="ghazi",
                        UserName="abdoghazi",
                        Password="123456Ab@",
                        Role="buyer",
                        Deposit=100
                   }

                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public class CreateFailedUserModelData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                   new UserModel
                   {

                        FirstName="maher",
                        LastName="ghazi",
                        UserName="maherdghazi",
                        Password="123456Ab@",
                        Role="seller",
                        Deposit=100
                   }

                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }


    }
}
