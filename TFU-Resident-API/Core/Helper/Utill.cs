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

        public static string FastJsonSerialize<T>(T obj)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = false, // Không thêm khoảng trắng vào JSON
                IncludeFields = true, // Bao gồm các trường (fields) trong serialization
            };

            return JsonSerializer.Serialize(obj, options);
        }
    }
}
