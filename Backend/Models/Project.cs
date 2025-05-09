using Freelancing.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

public abstract class Project
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }


    //[not in dto]
    public DateTime CreatedAt { get; set; } //Initially the date of the post - once taken by a freelancer add new date of starting this project 
	
	
	public DateTime? EndDate { get; set; } //not in dto


    public Currency currency { set; get; }

	public int ExpectedDuration { get; set; } //in days
	

	[ForeignKey("Client")]
    public string ClientId { get; set; } //not in dto
    public virtual Client Client { get; set; }



	[ForeignKey("Freelancer")]
    public string? FreelancerId { get; set; } //not in dto
	public virtual Freelancer? Freelancer { get; set; }
	



	[NotMapped]
    public projectStatus Status =>  FreelancerId==null? projectStatus.Pending 
		: this.Milestones.All(m=>m.Status==MilestoneStatus.Completed) ? projectStatus.Completed:projectStatus.Working;
	
	
	[ForeignKey("Subcategory")]
    public int SubcategoryId { get; set; }
	public virtual Subcategory Subcategory { set; get; }



	public virtual List<Milestone> Milestones { get; set; } = new List<Milestone>();
	public ExperienceLevel experienceLevel { get; set; }
	public virtual List<Proposal> Proposals { get; set; } = new List<Proposal>();
	public virtual List<ProjectSkill> ProjectSkills { get; set; }
	public virtual List<Review> Reviews { get; set; }

	public bool IsDeleted { get; set; } = false;
}

public enum projectStatus
{
	Pending,
	Working,
	Completed
}

public enum ExperienceLevel
{
	Entry,
	Intermediate,
	Expert
}
public enum Currency
{
	USD,
	EUR,
	GBP,
	JPY,
	AUD,
	CAD,
	CHF,
	CNY,
	SEK,
	NZD
}