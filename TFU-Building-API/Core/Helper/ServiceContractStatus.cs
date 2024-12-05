namespace TFU_Building_API.Core.Helper
{
    public struct ServiceContractStatus
    {
        public const int Pending = 0;    // Đang xử lý
        public const int Approved = 1;   // Đồng ý ( done )
        public const int Rejected = 2;   // Reject
        public const int Assigment = 3;   // Giao việc và ghi lại thông tin step 3
        public const int ApprovedAssigmentStaff = 4;   // Cư dân xác nhận thông tin ghi chú
        public const int RejectedAssigmentStaff = 5;   // Cư dân 0 xác nhận thông tin ghi chú
        public const int StaffPending = 6;   // Nhân viên xác nhân đang xử lý
        public const int StaffDone = 7;   // Nhân viên đã xử lý xong

    }

}
