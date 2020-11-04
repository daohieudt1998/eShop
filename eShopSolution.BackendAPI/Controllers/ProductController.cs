using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShopSolution.Application.Catalog.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using eShopSolution.ViewModel.Catalog.Products;

namespace eShopSolution.BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPublicProductService _publicProductService;
        private readonly IManageProductService _manageProductService;
        public ProductController(IPublicProductService publicProductService, IManageProductService manageProductService)
        {
            _publicProductService = publicProductService;
            _manageProductService = manageProductService;
        }

        [HttpGet("{languageId}")]
        public async Task<IActionResult> Get(string languageId)
        {
            var product = await _publicProductService.GetAll(languageId);
            return Ok(product);
        }

        [HttpGet("public-paging")]
        public async Task<IActionResult> GetAllByCategoryId([FromQuery]GetPublicProductPagingRequest request)
        {
            var products = await _publicProductService.GetAllByCategoryId(request);
            return Ok(products);
        }

        [HttpGet("{id}/{languageId}")]
        public async Task<IActionResult> GetById([FromQuery]int id, string languageId)
        {
            var product = await _manageProductService.GetById(id, languageId);
            if (product == null)
                return BadRequest("Cannot find product");
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]ProductCreateRequest request)
        {
            var result = await _manageProductService.Create(request);
            if (result == 0)
                return BadRequest();

            var product = await _manageProductService.GetById(result, request.LanguageId);

            return CreatedAtAction(nameof(GetById), new { productId = result }, product);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody]ProductUpdateRequest request)
        {
            var affectedResult = await _manageProductService.Update(request);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }

        [HttpPut("UpdatePrice/{id}/{newPrice}")]
        public async Task<IActionResult> UpdatePrice([FromQuery]int id, decimal newPrice)
        {
            var isSuceessfull = await _manageProductService.UpdatePrice(id, newPrice);
            if (!isSuceessfull)
                return BadRequest();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var affectedResult = await _manageProductService.Delete(id);
            if (affectedResult == 0)
                return BadRequest();
            return Ok();
        }

    }
}
