using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedModels;
using System.Text.Json;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private static readonly List<Order> Orders = new List<Order>
    {
        new Order { Id = 1, UserId = 1, ProductId = 1, Quantity = 1 },
        new Order { Id = 2, UserId = 2, ProductId = 2, Quantity = 2 }
    };

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _userServiceUrl = "https://localhost:7083/api/User";
        private readonly string _productServiceUrl = "https://localhost:7123/api/Product";

        public OrderController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
        {
            var user = await GetUserById(order.UserId);
            if (user == null)
                return BadRequest("User not found");

            var product = await GetProductById(order.ProductId);
            if (product == null)
                return BadRequest("Product not found");

            Orders.Add(order);
            return Ok(order);
        }


        [HttpGet("{id}")]
        public ActionResult<Order> GetOrder(int id)
        {
            var Order = Orders.FirstOrDefault(u => u.Id == id);
            if (Order == null)
                return NotFound();
            return Ok(Order);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {
            return Ok(Orders);
        }


        private async Task<User?> GetUserById(int userId)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_userServiceUrl}/{userId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<User>(json);
        }

        private async Task<Product?> GetProductById(int productId)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"{_productServiceUrl}/{productId}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Product>(json);
        }
       
    }
}
