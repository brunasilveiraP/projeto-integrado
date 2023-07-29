using System.Threading.Tasks;
using BBL.Models.TokenAuth;
using BBL.Web.Controllers;
using Shouldly;
using Xunit;

namespace BBL.Web.Tests.Controllers
{
    public class HomeController_Tests: BBLWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}