using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace InterviewConsole
{
    class Program
    {
        const string BaseUrl = "http://localhost:64014/EmployeeService.svc";

        static async Task Main(string[] args)
        {
            await GetEmployeeAsync(id: 1);
            await GetEnabledEmployeeAsync(id: 1);
            await EnableEmployeeAsync(id: 1, enable: true);
        }

        private static async Task GetEmployeeAsync(int id)
        {
            await SendGetAsync($"{BaseUrl}/GetEmployeeById?id={id}");
        }

        private static async Task GetEnabledEmployeeAsync(int id)
        {
            await SendGetAsync($"{BaseUrl}/GetEnabledEmployeeById?id={id}");
        }

        private static async Task EnableEmployeeAsync(int id, bool enable)
        {
            await SendPutAsync($"{BaseUrl}/EnableEmployee?id={id}&enable={enable}");
        }

        private static async Task SendGetAsync(string url)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private static async Task SendPutAsync(string url)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.PutAsync(url, null);
                    response.EnsureSuccessStatusCode();
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}
