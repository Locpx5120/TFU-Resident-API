namespace Constant
{
    public class MessConstant
    {
        public const string Successfully = "Thành công!";
        public const string UpdateSuccessfully = "Cập nhật thành công!";
        public const string Failed = "Thất bại!";
        public const string FindSuccessfully = "Tìm kiếm thông tin thành công.";

        #region căn hộ
        public const string UserNotApartment = "Tài khoản không sở hữu căn hộ nào.";
        public const string ApartmentFoundZero = "Không tìm thấy căn hộ.";
        #endregion

        #region toà nhà
        public const string BuildingUpdateSuccessfully = "Cập nhật chủ toà nhà thành công";
        public const string BuildingFoundZero = "Không tìm thấy thông tin toà nhà.";
        public const string BuildingHasExist = "Tên toà nhà đã tồn tại.";
        public const string BuildingOverFloor = "Số tầng không tồn tại";
        #endregion

        #region cư dân
        public const string ResidentFoundZero = "Không tìm thấy cư dân";
        public const string ResidentFoundZeroLiving = "Không tìm thấy cư dân ở căn hộ.";
        public const string ResidentHaveExist = "Thành viên đã có trong phòng này rồi";
        #endregion

        #region chủ căn hộ
        public const string OwnerShipFoundZero = "Không tìm thấy chủ căn hộ";
        public const string OwnerShipHaveExist = "Cư dân đã có căn hộ";
        public const string OwnerShipUpdateSuccessfully = "Cập nhật chủ căn hộ thành công";
        #endregion

        #region bản tin
        public const string NotifyCreateSuccessfully = "Bản tin đã được tạo thành công.";
        public const string NotifyFoundZero = "Không tìm thấy bản tin.";
        public const string NotifyUpdateSuccessfully = "Cập nhật bản tin thành công.";
        #endregion

        #region thanh toán
        public const string PaymentFoundZero = "Không tìm thấy thanh toán.";
        public const string PaymentSuccessfully = "Thanh toán thành công";
        #endregion

        #region Hợp đồng
        public const string ServiceContractFoundZero = "Không tìm thấy dịch vụ.";
        public const string ThirdPartyContractFoundZero = "Không tìm thấy hợp đồng.";
        #endregion

        #region Gói dịch vụ
        public const string PackageServiceFoundZero = "Không tìm thấy gói dịch vụ.";
        #endregion

        #region Nhân viên
        public const string StaffHasEmail = "Đã có nhân viên có email này.";
        public const string StaffFoundZero = "Không tìm thấy cán bộ nhân viên.";
        #endregion
    }
}
