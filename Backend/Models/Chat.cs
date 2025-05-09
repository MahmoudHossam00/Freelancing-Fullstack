using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Chat
{
    public int Id { get; set; }


    [ForeignKey("Sender")]
    public string SenderId { get; set; }
    public AppUser Sender { get; set; }


	[ForeignKey("Receiver")]
	public string ReceiverId { get; set; }
    public AppUser Receiver { get; set; }

    [MaxLength(2048)]
    public string? ImageUrl { get; set; }
    public string? Message { get; set; } //contains(".com") 
    public DateTime SentAt { get; set; }
    public bool isRead { set; get; }

    public bool IsEdited { get; set; } = false;
}