namespace API.Utility
{
	public static class SD
	{
		public const int Role_Admin_Id = 1;
		public const int Role_Store_Id = 2;
		public const int Role_Owner_Id = 3;
		public const int Role_Customer_Id = 4;

		public const string not_verify = "NOT VERIFY";
		public const string active = "ACTIVE";
		public const string in_active = "INACTIVE";
		public const string refuse = "REFUSE";

		public const string Storage_Container = "motorbikebs";

		public static readonly string[] GetMotorArray = new string[]
        {
            "Model",
            "Model.Brand",
            "MotorStatus",
            "MotorType",
            "MotorbikeImages",
            "Owner",
            "Owner.Role",
            "Store"
        };
    }
}
