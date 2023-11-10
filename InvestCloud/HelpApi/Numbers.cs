using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InvestCloud.HelpApi
{
    public partial class Numbers
    {
        const string ApiNumbers = @"https://recruitment-test.investcloud.com/api/numbers/";
        const string ValidationStringFormat = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<string>{0}</string>";
        //const string ValidationStringFormat = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<string xmlns=\"http://schemas.microsoft.com/2003/10/Serialization/\">{0}</string>";
        // const string ValidationStringFormat = "{ \"string\": \"{0}\"}";
        public async Task InitDatasets(int size = 1000)
        {
            using var apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(ApiNumbers);
            using var response = await apiClient.GetAsync($"init/{size}");
        }

        public async Task<Matrix> GetMatrix(string datasetName, int size = 1000)
        {
            var result = new Matrix();
            result.Init(size);
            using var apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(ApiNumbers);

            for (int idx = 0; idx < size; ++idx)
            {
                using var response = await apiClient.GetAsync($"{datasetName}/row/{idx}");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var apiResp = JsonSerializer.Deserialize<ApiResponse>(jsonResponse);
                result.Array2D[idx] = apiResp.Value;
            }
            return result;
        }
        
        public async Task<string> ValidateHash(string hash)
        {
            using var apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(ApiNumbers);
            var content = new StringContent(hash, Encoding.UTF8, "application/xml");

            using var response = await apiClient.PostAsync($"validate", content);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return jsonResponse;
        }
        
        public async Task<string> ValidateHash(byte[] hash)
        {
            using var apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(ApiNumbers);
            ByteArrayContent content = new ByteArrayContent(hash);
            using var response = await apiClient.PostAsync($"validate", content);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return jsonResponse;
        }
    }
}
