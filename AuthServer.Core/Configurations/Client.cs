namespace AuthServer.Core.Configurations;

public class Client
{
    public string Id { get; set; }

    public string SecretKey { get; set; }

    public List<String> Audiences { get; set; }
}
