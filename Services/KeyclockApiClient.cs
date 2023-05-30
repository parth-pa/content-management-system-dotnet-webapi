

using System.Net.Http.Headers;
using Keycloak.Net.Models.Users;
using keyclock_Authentication.Model;
using Newtonsoft.Json;


namespace keyclock_Authentication.Services
{
    public class KeyclockApiClient
    {

        private readonly HttpClient _httpClient;

        public KeyclockApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<HttpResponseMessage> GetAccessToken(UserLogin users)
        {

            var content = new FormUrlEncodedContent(new[]
              {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", "CMS-client"),
            new KeyValuePair<string, string>("client_secret","FEWocGIdK336CVHtaD3NpNSuSDfg8Vsz"),
            new KeyValuePair<string, string>("username", users.userName),
            new KeyValuePair<string, string>("password", users.password)
        });

            var request = new HttpRequestMessage(HttpMethod.Post, "http://10.195.81.19:8080/realms/CMS1/protocol/openid-connect/token");
            request.Content = content;

            var response = await _httpClient.SendAsync(request);

            return response;

        }

        public Userinformation getUserData(string userId, string token)
        {

            _httpClient.DefaultRequestHeaders.Authorization
             = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage registerRequet = _httpClient.GetAsync($"http://10.195.81.19:8080/admin/realms/CMS1/users/{userId}").Result;
            var responseContent = registerRequet.Content.ReadAsStringAsync().Result;
            Userinformation responseData = JsonConvert.DeserializeObject<Userinformation>(responseContent);
            return responseData;

        }

        public async Task<string> getAdminToken()
        {

            var bodyContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", "admin-cli"),
            new KeyValuePair<string, string>("username", "admin"),
            new KeyValuePair<string, string>("password", "admin@123")
        });

            var requestForToken = new HttpRequestMessage(HttpMethod.Post, "http://10.195.81.19:8080/realms/master/protocol/openid-connect/token");
            requestForToken.Content = bodyContent;

            var responseToken = await _httpClient.SendAsync(requestForToken);
            var responseJson = await responseToken.Content.ReadAsStringAsync();
            LoginResponse decodedJson = JsonConvert.DeserializeObject<LoginResponse>(responseJson);

            return decodedJson?.access_token;


        }

        public async Task<string> getAdminToken1()
        {

            var bodyContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("client_id", "admin-cli"),
            new KeyValuePair<string, string>("username", "admin"),
            new KeyValuePair<string, string>("password", "admin@123")
        });

            var requestForToken = new HttpRequestMessage(HttpMethod.Post, "http://10.195.81.19:8080/realms/master/protocol/openid-connect/token");
            requestForToken.Content = bodyContent;

            var responseToken = await _httpClient.SendAsync(requestForToken);
            var responseJson = await responseToken.Content.ReadAsStringAsync();
            LoginResponse decodedJson = JsonConvert.DeserializeObject<LoginResponse>(responseJson);

            return decodedJson?.access_token;


        }



    }
}
