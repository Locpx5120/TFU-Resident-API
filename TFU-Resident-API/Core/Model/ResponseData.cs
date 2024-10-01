using Core.Enums;
using System.ComponentModel;
using System.Reflection;

namespace Core.Model
{
    public class ResponseData<T> where T : class
    {
        public ResponseData()
        {
            Success = false;
            Message = string.Empty;
            Code = (int)ErrorCodeAPI.OK;
            Data = default(T);
        }

        public ResponseData(ErrorCodeAPI errorCodes)
        {
            Success = false;
            Code = (int)errorCodes;
            Message = errorCodes.GetDescription();
            Data = default(T);
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public int Code { get; set; }
        public T? Data { get; set; }
    }

    public static class EnumExtensions
    {
        public static string GetDescription(this System.Enum enumValue, string msg = null)
        {
            return msg ?? enumValue.GetType()
                       .GetMember(enumValue.ToString())
                       .First()
                       .GetCustomAttribute<DescriptionAttribute>()?
                       .Description ?? string.Empty;
        }
    }
}
