using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shopping.Web.Pages;

public class CartModel(IBasketService _basketService, ILogger<CartModel> _logger)
    : PageModel
{
    public ShoppingCartModel Cart { get; set; } = new ShoppingCartModel();
    public async Task<IActionResult> OnGetAsync()
    {
        Cart = await _basketService.LoadUserBasket();

        return Page();
    }

    public async Task<IActionResult> OnPostRemoveToCartAsync(Guid productId)
    {
        _logger.LogInformation("Remove to cart button clicked");
        Cart = await _basketService.LoadUserBasket();

        Cart.Items.RemoveAll(x => x.ProductId == productId);
        await _basketService.StoreBasket(new StoreBasketRequest(Cart));

        return RedirectToPage();
    }
}