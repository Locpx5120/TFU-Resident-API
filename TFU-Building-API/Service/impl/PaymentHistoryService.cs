using Core.Enums;
using Core.Model;
using Newtonsoft.Json.Linq;
using TFU_Building_API.Dto;

namespace TFU_Building_API.Service.impl
{
    public class PaymentHistoryService : IPaymentHistoryService
    {
        private static readonly HttpClient client = new HttpClient();
        private string URL = "ht/transactions"; // Thay bằng URL thật
        private string APIKEY = "AK_CS.f5b8bUewYuYzaA8TGwIyzNBVD1uZvrEz8PqdFas6hPzOC7rHERe7Pm7lygLJJckog0R4IWH";

        public async Task<ResponseData<PaymentHistoryResponseDto>> CheckPayment(string type, Guid id)
        {
            CheckBank("", 2);
            return new ResponseData<PaymentHistoryResponseDto>
            {
                Success = true,
                Message = "Apartment information retrieved successfully.",
                Data = null,
                Code = (int)ErrorCodeAPI.OK
            };
        }

        public async Task<bool> CheckBank(String noiDung, double price)
        {
            try
            {
                // Tạo request với header "Authorization: apikey <apiKey>"
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "apikey " + APIKEY);

                HttpResponseMessage response = await client.GetAsync(URL);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Unexpected code " + response.StatusCode);
                }

                // Đọc dữ liệu từ response
                string responseData = await response.Content.ReadAsStringAsync();

                // Parse JSON response nếu cần
                JObject json = JObject.Parse(responseData);
                if (json["data"]?["records"] != null)
                {
                    var recordsArray = json["data"]["records"];
                    foreach (var recordJson in recordsArray)
                    {
                        int id = recordJson.Value<int>("id");
                        string tid = recordJson.Value<string>("tid");
                        int amount = recordJson.Value<int>("amount");
                        string description = recordJson.Value<string>("description");

                        // Kiểm tra mô tả và giá trị
                        if (!string.IsNullOrEmpty(description) && noiDung.Length > 25)
                        {
                            if (description.ToLower().Contains(noiDung.ToLower()) && price == amount)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log hoặc xử lý exception nếu cần
                //Console.WriteLine(ex.Message);
            }

            return false;
        }
    }
}
