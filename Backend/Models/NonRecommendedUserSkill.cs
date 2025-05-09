namespace Freelancing.Models
{
    public class NonRecommendedUserSkill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;

        public virtual List<Freelancer> Freelancers { get; set; }
    }
}
