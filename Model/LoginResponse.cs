

using Newtonsoft.Json;
// LoginResponse myDeserializedClass = JsonConvert.DeserializeObject<LoginResponse>(myJsonResponse);
public class LoginResponse
{
    public string? access_token { get; set; }
    public int expires_in { get; set; }
    public int refresh_expires_in { get; set; }
    public string? refresh_token { get; set; }
    public string? token_type { get; set; }

    [JsonProperty("not-before-policy")]
    public int notbeforepolicy { get; set; }
    public string? session_state { get; set; }
    public string? scope { get; set; }
}

