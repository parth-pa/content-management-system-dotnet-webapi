// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using Newtonsoft.Json;

public class Attributes
{
    public List<string> prefrence_id { get; set; }
}

// public class ClientRoles
// {
//     [JsonProperty("CMS-client")]
//     public List<string> CMSclient { get; set; }
// }

public class Credential
{
    public string type { get; set; }
    public string value { get; set; }
}

public class Register
{
    public string username { get; set; }
    public string email { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public bool enabled { get; set; }
    public Attributes attributes { get; set; }
    public List<Credential> credentials { get; set; }
    // public ClientRoles clientRoles { get; set; }
}

