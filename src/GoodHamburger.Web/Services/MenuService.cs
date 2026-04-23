using GoodHamburger.Web.Models;
using System.Net.Http.Json;

namespace GoodHamburger.Web.Services
{
    public class MenuService
    {
        private readonly HttpClient _httpClient;

        public MenuService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<MenuItemDto>> GetMenuAsync()
        {
            var response = await _httpClient.GetAsync("api/menu");
            response.EnsureSuccessStatusCode();
            
            var menu = await response.Content.ReadFromJsonAsync<List<MenuItemDto>>();
            return menu ?? new List<MenuItemDto>();
        }
    }
}
