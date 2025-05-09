public class SubscriptionPlan
{
    public int Id { get; set; }
    public string name { set; get; }
    public string Description { get; set; }
    public decimal Price { get; set; }
	public bool isDeleted { get; set; } = false;


    public int? DurationInDays { get; set; } //nullable 34n lw fi plans mlha4 duration
    public int TotalNumber { get; set; } = 6; //of bids for freelancer - of prsting projects for client
    

}