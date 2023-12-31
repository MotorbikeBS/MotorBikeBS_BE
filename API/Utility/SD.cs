﻿namespace API.Utility
{
    public static class SD
	{        
        public const int AdminID = 1;

        public const int Role_Admin_Id = 1;
		public const int Role_Store_Id = 2;
		public const int Role_Owner_Id = 3;
		public const int Role_Customer_Id = 4;

        public static readonly int[] Posting_MotorStatusArray = new int[] { 1, 4 };
        public const int Status_Posting = 1;
		public const int Status_Storage = 3;
		public const int Status_Consignment = 4;
		public const int Status_nonConsignment = 5;

        public static readonly int[] RequestPostingTypeArray = new int[] { 2, 3, 4}; //Posting - Consignment - nonConsignment
        public static readonly int[] Owner_RequestPostingTypeArray = new int[] { 3, 4 };
        public const int Request_Motor_Register = 1;
		public const int Request_Motor_Posting = 2;
        public const int Request_Motor_Consignment = 3;
		public const int Request_Motor_nonConsignment = 4;
		public const int Request_Booking_Id = 6;
		public const int Request_Negotiation_Id = 8;
        public const int Request_MotorTranfer_Id = 9;
        public const int Request_Add_Point_Id = 10;
        public const int Request_Post_Boosting_Id = 11;
        public const int Request_Report_Id = 12;

        public const int NotificationType_TranferOwnership = 1;
        public const int NotificationType_OwnerSoldOut = 2;
        public const int NotificationType_NegotiationExpired = 3;
        public const int NotificationType_Valuation = 4;
        public const int NotificationType_Negotiation = 5;
        public const int NotificationType_Payment = 6;
        public const int NotificationType_StoreRegister = 7;

        public const string Request_Pending = "PENDING";
		public const string Request_Accept = "ACCEPT";
		public const string Request_Reject = "REJECT";
		public const string Request_Cancel = "CANCEL";
        public const string Request_Done = "DONE";
        public const string Request_Expired = "EXPIRED";

        public const string Payment_Unpaid = "UNPAID";
        public const string Payment_Error= "ERROR";
        public const string Payment_Paid = "PAID";

        public const string not_verify = "NOT VERIFY";
		public const string active = "ACTIVE";
		public const string in_active = "INACTIVE";
		public const string refuse = "REFUSE";
        public const string pending = "PENDING";

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
        public static readonly string[] GetBillArray = new string[]
        {
            "Request",
            "Request.Motor",
            "Request.Motor.Model.Brand",
            "Request.Motor.MotorbikeImages",
            "Request.Sender",
            "Request.Sender.StoreDescriptions",
            "Request.Receiver",
            "Request.Receiver.StoreDescriptions"
        };
        public static readonly string[] GetRequestArray = new string[]
        {
            "Receiver",
            "Sender",
            "RequestType"
        };
        public static readonly string[] GetCommentArray = new string[]
        {
            "Request",
            "Request.Motor",
            "Request.Motor.Model.Brand",
            "Request.Motor.MotorbikeImages",
            "Request.Sender",
            "Request.Receiver",
            "Request.RequestType",
            "InverseReply"
        };
        public static readonly string[] GetRequestWithStoreArray = new string[]
        {
            "Motor",
            "Motor.Model.Brand",
            "Motor.MotorbikeImages",
            "Sender",
            "Sender.StoreDescriptions",
            "Receiver",
            "Receiver.StoreDescriptions",
            "RequestType"
        };
    }
}
