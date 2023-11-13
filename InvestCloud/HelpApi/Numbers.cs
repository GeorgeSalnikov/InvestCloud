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
        public async Task InitDatasets(int size = 1000)
        {
            using var apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(ApiNumbers);
            using var response = await apiClient.GetAsync($"init/{size}");
        }

        public async Task<Matrix> GetMatrix(string datasetName, int size = 1000)
        {
            using var apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(ApiNumbers);

            var taskGetRow = new Task<int[]>[size];
            for (int idx = 0; idx < size; ++idx)
            {
                taskGetRow[idx] = GetMatrixRowAsync(datasetName, idx, apiClient);
            }
            await Task.WhenAll(taskGetRow);

            var result = new Matrix().Init(size);
            for (int idx = 0; idx < size; ++idx)
            {
                result.Array2D[idx] = taskGetRow[idx].Result;
            }
            return result;
        }

        private async Task<int[]> GetMatrixRowAsync(string datasetName, int idx, HttpClient apiClient)
        {
            using var response = await apiClient.GetAsync($"{datasetName}/row/{idx}");
            var jsonResponse = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var apiResp = JsonSerializer.Deserialize<ApiResponse<int[]>>(jsonResponse);
            return apiResp.Value;
        }

        public async Task<string> ValidateHash(string hash)
        {
            using var apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(ApiNumbers);
            var content = new StringContent(hash, Encoding.UTF8, "application/xml");

            using var response = await apiClient.PostAsync($"validate", content);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return jsonResponse;
        }
        
        public async Task<string> ValidateHash(byte[] hash)
        {
            using var apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(ApiNumbers);
            ByteArrayContent content = new ByteArrayContent(hash);
            using var response = await apiClient.PostAsync($"validate", content);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var apiResp = JsonSerializer.Deserialize<ApiResponse<string>>(jsonResponse);
            return jsonResponse;
        }
    }
}
