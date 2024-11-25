using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using TradingCmpn_WEB.Controllers;
using TradingCmpn_WEB.Models;
using AutoMapper;
using NUnit.Framework.Legacy;

namespace TradingCmpnWeb_Tests
{
    [TestFixture]
    public class ProductControllerTests
    {
        private Mock<IProductService> _mockProductService;
        private Mock<IMapper> _mockMapper;
        private ProductController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockProductService = new Mock<IProductService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new ProductController(_mockProductService.Object, _mockMapper.Object);
        }

        [Test]
        public async Task Index_ReturnsViewWithProducts()
        {
            var productsDto = new List<ProductDto>
            {
                new ProductDto { ProductId = 1, ProductName = "Product1", Price = 100 },
                new ProductDto { ProductId = 2, ProductName = "Product2", Price = 200 }
            };
            _mockProductService.Setup(s => s.GetAllProductsAsync()).ReturnsAsync(productsDto);

            var productsModel = new List<ProductModel>
            {
                new ProductModel { ProductId = 1, ProductName = "Product1", Price = 100 },
                new ProductModel { ProductId = 2, ProductName = "Product2", Price = 200 }
            };
            _mockMapper.Setup(m => m.Map<IEnumerable<ProductModel>>(productsDto)).Returns(productsModel);

            var result = await _controller.Index();

            var viewResult = result as ViewResult;
            ClassicAssert.IsNotNull(viewResult);
            var model = viewResult.Model as IEnumerable<ProductModel>;
            ClassicAssert.AreEqual(2, model.Count());
        }

        [Test]
        public async Task Create_ValidModel_RedirectsToIndex()
        {
            var model = new ProductModel { ProductName = "New Product", Price = 300 };
            var productDto = new ProductDto { ProductName = "New Product", Price = 300 };
            _mockMapper.Setup(m => m.Map<ProductDto>(model)).Returns(productDto);

            var result = await _controller.Create(model);

            var redirectToActionResult = result as RedirectToActionResult;
            ClassicAssert.IsNotNull(redirectToActionResult);
            ClassicAssert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [Test]
        public async Task Details_ValidId_ReturnsViewWithProduct()
        {
            var productDto = new ProductDto { ProductId = 1, ProductName = "Product1", Price = 100 };
            _mockProductService.Setup(s => s.GetProductByIdAsync(1)).ReturnsAsync(productDto);
            var productModel = new ProductModel { ProductId = 1, ProductName = "Product1", Price = 100 };
            _mockMapper.Setup(m => m.Map<ProductModel>(productDto)).Returns(productModel);

            var result = await _controller.Details(1);

            var viewResult = result as ViewResult;
            ClassicAssert.IsNotNull(viewResult);
            var model = viewResult.Model as ProductModel;
            ClassicAssert.AreEqual("Product1", model.ProductName);
        }

        [Test]
        public async Task Edit_ValidId_ReturnsViewWithProduct()
        {
            var productDto = new ProductDto { ProductId = 1, ProductName = "Product1", Price = 100 };
            _mockProductService.Setup(s => s.GetProductByIdAsync(1)).ReturnsAsync(productDto);
            var productModel = new ProductModel { ProductId = 1, ProductName = "Product1", Price = 100 };
            _mockMapper.Setup(m => m.Map<ProductModel>(productDto)).Returns(productModel);

            var result = await _controller.Edit(1);

            var viewResult = result as ViewResult;
            ClassicAssert.IsNotNull(viewResult);
            var model = viewResult.Model as ProductModel;
            ClassicAssert.AreEqual("Product1", model.ProductName);
        }

        
        [Test]
        public async Task Delete_ValidId_ReturnsViewWithProduct()
        {
            var productDto = new ProductDto { ProductId = 1, ProductName = "Product1", Price = 100 };
            _mockProductService.Setup(s => s.GetProductByIdAsync(1)).ReturnsAsync(productDto);
            var productModel = new ProductModel { ProductId = 1, ProductName = "Product1", Price = 100 };
            _mockMapper.Setup(m => m.Map<ProductModel>(productDto)).Returns(productModel);

            var result = await _controller.Delete(1);

            var viewResult = result as ViewResult;
            ClassicAssert.IsNotNull(viewResult);
            var model = viewResult.Model as ProductModel;
            ClassicAssert.AreEqual("Product1", model.ProductName);
        }

        [Test]
        public async Task DeleteConfirmed_ValidId_RedirectsToIndex()
        {
            var productId = 1;
            _mockProductService.Setup(s => s.DeleteProductAsync(productId)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteConfirmed(productId);

            var redirectToActionResult = result as RedirectToActionResult;
            ClassicAssert.IsNotNull(redirectToActionResult);
            ClassicAssert.AreEqual("Index", redirectToActionResult.ActionName);
        }
    }
}
