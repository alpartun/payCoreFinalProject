namespace PayCoreFinalProject.Dto;

public class EmailDto
{
    public  string EmailAdress { get; set; }
    public  string EmailTitle { get; set; }
    public  string EmailMessage { get; set; }
    public  bool IsSent { get; set; } = false;
    public  DateTime CreatedTime { get; set; } = DateTime.Now;
    public  DateTime SendTime { get; set; }
    
}