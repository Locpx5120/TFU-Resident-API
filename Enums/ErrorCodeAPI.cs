using System.ComponentModel;

namespace Core.Enums
{
    public enum ErrorCodeAPI
    {
        [Description("Internal Error")]
        InternalError = 101,
        [Description("OK")]
        OK = 200,
        [Description("Redirect")]
        Redirect = 302,
        [Description("Bad request")]
        BadRequest = 400,
        [Description("Unauthorized")]
        Unauthorized = 401,
        [Description("Not Found")]
        NotFound = 404,

        //Auth & User mã lỗi bắt đầu từ 2000
        [Description("OKChangePass")]
        OKChangePass = 2000,
        [Description("Nhập lại mật khẩu không trùng khớp!")]
        PasswordNotMatch = 2001,
        [Description("Không tìm thấy quyền!")]
        RoleNotFound = 2002,
        [Description("Email đã được sử dụng!")]
        EmailUsed = 2003,
        [Description("Không tìm thấy tài khoản!")]
        UserNotExit = 2004,
        [Description("Tài khoản đã bị khoá hoặc xoá! Liên hệ quản trị viên biết thêm chi tiết")]
        UserIsBlockOrDelete = 2005,
        [Description("Email không hợp lệ")]
        EmailNotAvailable = 2006,
        [Description("Email chưa có trong hệ thống")]
        EmailNotUse = 2007,
        [Description("Mật khẩu chưa hợp lệ")]
        PasswordIsNotValid = 2008,
        [Description("Mật khẩu mới chưa hợp lệ")]
        NewPasswordIsNotValid = 2009,
        [Description("Mật khẩu cũ chưa chính xác")]
        PasswordIsNotCorrect = 2010,
        [Description("Mã xác nhận chưa tồn tại")]
        OtpNotExit = 2011,

        //mã lỗi chung từ 3001
        [Description("Investor đã được sử dụng!")]
        CustomerUsed = 3001,
        [Description("Không tìm thấy tài khoản Investor!")]
        InvestorNotFound = 3002,
        [Description("Không tìm thấy dự án")]
        ProjectNotFound = 3002,
        [Description("Không tìm thấy user")]
        UserNotFound = 3003,

        // Hệ thống mã lỗi từ 4001
        [Description("Lỗi hệ thống")]
        SystemIsError = 4001,
        InvalidData = 4003,

        // Mã lỗi Duplicate Entry cho trường hợp dữ liệu bị trùng lặp
        [Description("Dữ liệu đã tồn tại")]
        DuplicateEntry = 1001
    }
}
