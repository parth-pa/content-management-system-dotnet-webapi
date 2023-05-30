using keyclock_Authentication.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

using System.Net;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

using Keycloak.Net.Models.Users;
using System.Security.Claims;
using keyclock_Authentication.Services;

namespace keyclock_Authentication.Controllers
{

    [ApiController]

    public class AuthController : Controller
    {

        private readonly HttpClient _httpClient;
        private readonly KeyclockApiClient _keyCloakApiClient;

        public AuthController(HttpClient httpClient, KeyclockApiClient keyclockApiClient)
        {
            _httpClient = httpClient;
            _keyCloakApiClient = keyclockApiClient;
        }

        [HttpPost]
        [Route("userLogin")]

        public async Task<IActionResult> keyclockLogin(UserLogin users)
        {
            
            var response = await _keyCloakApiClient.GetAccessToken(users);
           
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Unauthorized(new { status = 401, isSuccess = false, message = "Username and password does not match.", });
            }

            var jsons = await response.Content.ReadAsStringAsync();
            
            LoginResponse myDeserializedClass = JsonConvert.DeserializeObject<LoginResponse>(jsons);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken decodedToken = tokenHandler.ReadJwtToken(myDeserializedClass.access_token);
            IEnumerable<Claim> claims = decodedToken.Claims;


            Claim claim = claims.FirstOrDefault(c => c.Type == "roles");
            string? claimValue = claim?.Value;

            Claim cliam1 = claims.FirstOrDefault(c => c.Type == "sub");
            string? userId = cliam1?.Value;

            var adminToken = await _keyCloakApiClient.getAdminToken();
            var responesData = _keyCloakApiClient.getUserData(userId,adminToken);
            var userName = responesData.firstName + " " + responesData.lastName;
            return Ok(new { status = 200, isSuccess = true, token = myDeserializedClass?.access_token, userole = claimValue,username = userName, userpreference = responesData.attributes.prefrence_id.FirstOrDefault() });
        }




        [HttpPost]
        [Route("userRegistration")]

        public async Task<IActionResult> keyclockRegister(Register register)
        {
            var result = await _keyCloakApiClient.getAdminToken();
            _httpClient.DefaultRequestHeaders.Authorization
                                     = new AuthenticationHeaderValue("Bearer", result);

            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage registerRequet = _httpClient.PostAsJsonAsync("http://10.195.81.19:8080/admin/realms/CMS1/users", register).Result;
           
         /*   if (registerRequet.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Unauthorized(new { status = 401, isSuccess = false, message = "Username and password does not match.", });
            }*/
            if (registerRequet.StatusCode == HttpStatusCode.Conflict)
            {
                return Conflict(new { status = 409, isScuccess = false, message = "User exists with same username or email" });
            }
            return Ok(new { status = 201, isSuccess = true, message = "User created sucessfully" });

        }

    }
}
