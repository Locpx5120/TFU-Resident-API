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

        //Auth & User mã lỗi bắt đầu từ 2001
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
    }
}
