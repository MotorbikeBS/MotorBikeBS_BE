namespace Core.Models
{
    public partial class BillConfirm
    {
        public int BillConfirmId { get; set; }
        public int MotorId { get; set; }
        public int UserId { get; set; }
        public int StoreId { get; set; }
        public decimal? Price { get; set; }
        public DateTime? CreateAt { get; set; }
        public string? Status { get; set; }
        public int RequestId { get; set; }

        public virtual Request Request { get; set; } = null!;
    }
}
