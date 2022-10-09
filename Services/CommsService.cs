using ProductManagementAPI.Data.ViewModel;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProductManagementAPI.Services
{
    public interface ICommsService
    {

        Task<bool> SendEmailViaMailjet(SendEmailDto model);
        //Task<Email> GetEmail(string id, CancellationToken token);
        //Task<List<Email>> GetAll(CancellationToken token);


    }
    public class CommsService : ICommsService
    {
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true
        };

        private readonly string BearerToken;
        private HttpClient _httpClient;

        public CommsService(IConfiguration configuration)
        {
            BearerToken = configuration.GetSection("CommsApi:bearerToken").Value;
        }


        public async Task<bool> SendEmailViaMailjet(SendEmailDto model)
        {
            using (_httpClient = new HttpClient())
            {
                var baseUrl = new Uri($"https://fusion-comms.fusionintel.io/api/EMails/send-via-mailjet");

                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                _httpClient.DefaultRequestHeaders.Add("Authorization", BearerToken);

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(baseUrl, model, default);

                var responseString = await response.Content.ReadAsStringAsync();

                //var deserializedResponse = JsonSerializer.Deserialize<EmailResult>(responseString, options);

                if (response.StatusCode == HttpStatusCode.OK)
                    return true;

                return false;
            }
        }
    }
}
