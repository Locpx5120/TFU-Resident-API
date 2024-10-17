using System.Text;
using System.Text.Json;

namespace fake_tool.Helpers
{
    public class Utill
    {
        public static string GenerateRandomString(int length)
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            //const string koreanChars = "ㄱㄴㄷㄹㅁㅂㅅㅇㅈㅊㅋㅌㅍㅎ가나다라마바사아자차카타파하";
            //const string japaneseChars = "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめも";
            //const string russianChars = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            //string allChars = koreanChars + japaneseChars + russianChars;

            StringBuilder result = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }

            return result.ToString();
        }

        public static string GenerateRandomInt(int length)
        {
            const string characters = "0123456789";

            StringBuilder result = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }

            return result.ToString();
        }

        public static string FastJsonSerialize<T>(T obj)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = false, // Không thêm khoảng trắng vào JSON
                IncludeFields = true, // Bao gồm các trường (fields) trong serialization
            };

            return JsonSerializer.Serialize(obj, options);
        }

        public static string GeneratePassword(int length = 12, bool useUppercase = true, bool useNumbers = true, bool useSpecialChars = true)
        {
            const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
            const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numberChars = "0123456789";
            const string specialChars = "!@#$%^&*()_-+=<>?";

            var random = new Random();
            var password = new StringBuilder();
            var charSet = lowercaseChars;

            if (useUppercase)
                charSet += uppercaseChars;
            if (useNumbers)
                charSet += numberChars;
            if (useSpecialChars)
                charSet += specialChars;

            // Đảm bảo mật khẩu chứa ít nhất một ký tự từ mỗi loại được yêu cầu
            if (useUppercase)
                password.Append(uppercaseChars[random.Next(uppercaseChars.Length)]);
            if (useNumbers)
                password.Append(numberChars[random.Next(numberChars.Length)]);
            if (useSpecialChars)
                password.Append(specialChars[random.Next(specialChars.Length)]);

            // Điền các ký tự còn lại
            for (int i = password.Length; i < length; i++)
            {
                password.Append(charSet[random.Next(charSet.Length)]);
            }

            // Xáo trộn chuỗi mật khẩu
            return new string(password.ToString().ToCharArray().OrderBy(x => random.Next()).ToArray());
        }

        public static (bool isValid, string errorMessage) ValidatePassword(string password)
        {
            // Kiểm tra độ dài tối thiểu
            if (string.IsNullOrEmpty(password) || password.Length < 8)
            {
                return (false, "Mật khẩu phải có ít nhất 8 ký tự.");
            }

            // Kiểm tra có ít nhất một chữ hoa
            if (!password.Any(char.IsUpper))
            {
                return (false, "Mật khẩu phải chứa ít nhất một chữ cái viết hoa.");
            }

            // Kiểm tra có ít nhất một chữ thường
            if (!password.Any(char.IsLower))
            {
                return (false, "Mật khẩu phải chứa ít nhất một chữ cái viết thường.");
            }

            // Kiểm tra có ít nhất một chữ số
            if (!password.Any(char.IsDigit))
            {
                return (false, "Mật khẩu phải chứa ít nhất một chữ số.");
            }

            // Kiểm tra có ít nhất một ký tự đặc biệt
            string specialCharacters = @"!@#$%^&*()_+-=[]{}|;:,.<>?";
            if (!password.Any(c => specialCharacters.Contains(c)))
            {
                return (false, "Mật khẩu phải chứa ít nhất một ký tự đặc biệt (!@#$%^&*()_+-=[]{}|;:,.<>?)");
            }

            // Kiểm tra không chứa khoảng trắng
            if (password.Contains(" "))
            {
                return (false, "Mật khẩu không được chứa khoảng trắng.");
            }

            return (true, "Mật khẩu hợp lệ.");
        }
    }
}
