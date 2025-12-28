namespace TCSA.SmartBudget.WebAPI.Options;

public sealed class Auth0Settings
{
    public string Domain { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}

