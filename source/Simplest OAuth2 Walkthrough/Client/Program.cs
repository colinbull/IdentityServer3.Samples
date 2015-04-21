using System;
using System.Net.Http;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Client;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
           // var response = GetClientToken();
           // CallApi(response);

            var response = GetUserToken();
            CallApi(response);

            Console.ReadLine();
        }

        static void CallApi(TokenResponse response)
        {
            var client = new HttpClient();
            client.SetBearerToken(response.AccessToken);

            Console.WriteLine(client.GetStringAsync("http://localhost:14869/test").Result);
        }

        static TokenResponse GetClientToken()
        {
            var client = new OAuth2Client(
                new Uri("http://localhost:44333/connect/token"),
                "silicon",
                "F621F470-9731-4A25-80EF-67A6F7C5F4B8");

            return client.RequestClientCredentialsAsync("api1").Result;
        }

        static async Task<UserInfoResponse> GetUserInfo(TokenResponse response)
        {
            var userInfo = new UserInfoClient(
                new Uri("http://localhost:44333/connect/userinfo"),
                response.AccessToken);

            return await userInfo.GetAsync();
        }

        static TokenResponse GetUserToken()
        {
            var client = new OAuth2Client(
                new Uri("http://localhost:44333/connect/token"),
                "carbon",
                "21B5F798-BE55-42BC-8AA8-0025B903DC3B");

            return client.RequestResourceOwnerPasswordAsync(Environment.UserName, "secret", "openid profile roles api1").Result;
        }
    }
}