using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shopping.Web.Pages;

public class OrderListModel(IOrderingService _orderingService, ILogger<OrderListModel> _logger)
    : PageModel
{
    public IEnumerable<OrderModel> Orders { get; set; } = default!;
    public async Task<IActionResult> OnGetAsync()
    {
        var customerId = new Guid("2629cf76-2a44-49eb-845d-2d63a1abc5eb");
        var response = await _orderingService.GetOrdersByCustomer(customerId);
        Orders = response.Orders;

        return Page();
    }
}