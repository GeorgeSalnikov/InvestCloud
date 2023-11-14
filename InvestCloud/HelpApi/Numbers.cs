using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace InvestCloud.HelpApi
{
    public class Numbers
    {
        const string ApiNumbers = @"https://recruitment-test.investcloud.com/api/numbers/";
        const string ValidationStringFormat = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<string>{0}</string>";

        public async Task InitDatasets(int size = 1000)
        {
            using var apiClient = new HttpClient();
            apiClient.BaseAddress = new Uri(ApiNumbers);
            using var response = await apiClient.GetAsync($"init/{size}");
            response.EnsureSuccessStatusCode();
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
            var res = await Task.WhenAll(taskGetRow);

            var mat = new Matrix().Init(size);
            for (int idx = 0; idx < size; ++idx)
            {
                mat[idx] = taskGetRow[idx].Result;
            }
            return mat;
        }

        private async Task<int[]> GetMatrixRowAsync(string datasetName, int idx, HttpClient apiClient)
        {
            using var response = await apiClient.GetAsync($"{datasetName}/row/{idx}");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
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
    }
}
