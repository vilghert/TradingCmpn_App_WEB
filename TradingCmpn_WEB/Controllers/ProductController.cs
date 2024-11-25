using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using TradingCmpn_WEB.Models;

namespace TradingCmpn_WEB.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var productsDto = await _productService.GetAllProductsAsync();
            var products = _mapper.Map<IEnumerable<ProductModel>>(productsDto);
            return View(products);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(ProductModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var productDto = _mapper.Map<ProductDto>(model);
            await _productService.CreateProductAsync(productDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var productDto = await _productService.GetProductByIdAsync(id);
            if (productDto == null)
                return NotFound();

            var model = _mapper.Map<ProductModel>(productDto);
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var productDto = await _productService.GetProductByIdAsync(id);
            if (productDto == null)
                return NotFound();

            var model = _mapper.Map<ProductModel>(productDto);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var productDto = _mapper.Map<ProductDto>(model);

            var existingProduct = await _productService.GetProductByIdAsync(productDto.ProductId);
            if (existingProduct == null)
                return NotFound();

            await _productService.UpdateProductAsync(productDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var productDto = await _productService.GetProductByIdAsync(id);
            if (productDto == null)
                return NotFound();

            var model = _mapper.Map<ProductModel>(productDto);
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
    }
}