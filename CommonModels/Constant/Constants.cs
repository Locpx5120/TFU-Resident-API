namespace Constant
{
    public class Constants
    {
        public static readonly Guid SYSTEM_USER_ID = Guid.Empty;
        public const string SYSTEM_USER_NAME = "System";
        public const string SYSTEM_USER_ROLE_NAME = "Customer";

        //Role
        public const string ADMIN_ROLE_NAME = "Admin";
        public const string SALE_ROLE_NAME = "Staff";

        //DB
        public const string SCHEMA_NAME = "software";

        //Role Web 2
        public const string ROLE_KE_TOAN = "KeToan";
        public const string ROLE_HANH_CHINH = "HanhChinh";
        public const string ROLE_KI_THUAT = "KiThuat";
        public const string ROLE_LE_TAN = "LeTan";
        public const string ROLE_BAN_QUAN_LY = "BanQuanLy";
        public const string ROLE_BEN_THU_BA = "BenThuBa";
        public const string ROLE_Resident = "BenThuBa";

        //trạng thái bản tin
        public const string NOTY_PENDING_APPROVAL = "PENDING_APPROVAL";
        public const string NOTY_APPLYING = "APPLYING";
        public const string NOTY_PENDING_APPLY = "PENDING_APPLY";
        public const string NOTY_REJECT = "REJECT";
        public const string NOTY_DRAFT = "DRAFT";
        public const string NOTY_EXPIRE = "EXPIRE";

        // thể loại tiền đầu vào
        public const string TRANS_SERVICE_INVOICE_ALL = "TRSCINA"; // Thanh toán dịch vụ phòng theo căn hộ danh sách id voince



        // trạng thái giao dịch
        public const string TRANS_STATUS_LOG_INIT = "LOG_INIT";
        public const string TRANS_STATUS_LOG_DONE = "LOG_DONE";
        public const string TRANS_STATUS_LOG_REJECT = "LOG_REJECT";
    }
}
