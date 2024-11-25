using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace TradingCompany_WEB.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            var productDtos = _mapper.Map<List<ProductDto>>(products);
            return View(productDtos);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var product = _mapper.Map<ProductDto>(productDto);
                await _productService.CreateProductAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(productDto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var productDto = await _productService.GetProductByIdAsync(id);
            if (productDto == null) return NotFound();

            var product = _mapper.Map<ProductDto>(productDto);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                var product = _mapper.Map<ProductDto>(productDto);
                await _productService.UpdateProductAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(productDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var productDto = await _productService.GetProductByIdAsync(id);
            if (productDto == null) return NotFound();

            return View(productDto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.DeleteProductAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
