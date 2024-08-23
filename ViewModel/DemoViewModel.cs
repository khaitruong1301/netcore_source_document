
public class DemoViewModel
{
    [EmailAttribute(ErrorMessage = "Lỗi email")]
    public required string  Email {get;set;}
}