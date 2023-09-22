namespace Core.Models
{
    public partial class MotorbikeStatus
    {
        public MotorbikeStatus()
        {
            Motorbikes = new HashSet<Motorbike>();
        }

        public int MotorStatusId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Motorbike> Motorbikes { get; set; }
    }
}
