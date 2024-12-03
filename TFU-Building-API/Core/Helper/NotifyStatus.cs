namespace TFU_Building_API.Core.Helper
{
    public struct NotifyStatus
    {
        public const int Draft = 0;
        public const int Pending = 1;    // Đang xử lý
        public const int Approved = 2;   // Đồng ý
        public const int Rejected = 3;   // Reject
    }

}
