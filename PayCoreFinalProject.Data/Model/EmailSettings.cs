namespace PayCoreFinalProject.Data.Model;

public class EmailSettings
{
    public const string Email = "Email";
    public string Host { get; set; }
    public string From { get; set; }
    public string User { get; set; }
    public int RetryCount { get; set; }
    public int Port { get; set; }
    public string Pass { get; set; }
}