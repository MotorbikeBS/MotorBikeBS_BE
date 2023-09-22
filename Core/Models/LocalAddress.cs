namespace Core.Models
{
    public partial class LocalAddress
    {
        public LocalAddress()
        {
            StoreDesciptions = new HashSet<StoreDesciption>();
            Users = new HashSet<User>();
        }

        public int LocalId { get; set; }
        public string WardName { get; set; } = null!;

        public virtual ICollection<StoreDesciption> StoreDesciptions { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
