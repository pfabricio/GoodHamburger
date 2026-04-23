using GoodHamburger.Web.Models;
using System.Net.Http.Json;

namespace GoodHamburger.Web.Services
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagedResult<OrderResponseDto>> GetOrdersAsync(int page = 1, int pageSize = 20)
        {
            var response = await _httpClient.GetAsync($"api/orders?page={page}&pageSize={pageSize}");
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadFromJsonAsync<PagedResult<OrderResponseDto>>();
            return result ?? new PagedResult<OrderResponseDto>();
        }

        public async Task<OrderResponseDto?> GetOrderByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/orders/{id}");
            
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;
            
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<OrderResponseDto>();
        }

        public async Task<OrderResponseDto> CreateOrderAsync(CreateOrderRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/orders", request);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<OrderResponseDto>() 
                ?? throw new InvalidOperationException("Failed to create order");
        }

        public async Task<OrderResponseDto> UpdateOrderAsync(int id, CreateOrderRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/orders/{id}", request);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadFromJsonAsync<OrderResponseDto>() 
                ?? throw new InvalidOperationException("Failed to update order");
        }

        public async Task DeleteOrderAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/orders/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
