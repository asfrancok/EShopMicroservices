using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shopping.Web.Pages;

public class CheckoutModel(IBasketService _basketService, ILogger<CheckoutModel> _logger)
    : PageModel
{
    [BindProperty]
    public BasketCheckoutModel Order { get; set; } = default!;

    public ShoppingCartModel Cart { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        Cart = await _basketService.LoadUserBasket();

        return Page();
    }

    public async Task<IActionResult> OnPostCheckOutAsync()
    {
        _logger.LogInformation("Checkout button clicked");

        Cart = await _basketService.LoadUserBasket();

        if(!ModelState.IsValid)
            return Page();

        Order.CustomerId = new Guid("2629cf76-2a44-49eb-845d-2d63a1abc5eb");
        Order.UserName = Cart.UserName;
        Order.TotalPrice = Cart.TotalPrice;

        await _basketService.CheckoutBasket(new CheckoutBasketRequest(Order));

        return RedirectToPage("Confirmation", "OrderSubmitted");
    }
}