namespace PayCoreFinalProject.Data.Model;

public class Email
{
    public virtual int Id { get; set; }
    public virtual string EmailAdress { get; set; }
    public virtual string EmailTitle { get; set; }
    public virtual string EmailMessage { get; set; }
    public virtual bool IsSent { get; set; } = false;
    public virtual DateTime CreatedTime { get; set; } = DateTime.Now;
    public virtual DateTime SendTime { get; set; }
}