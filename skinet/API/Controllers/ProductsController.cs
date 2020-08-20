﻿using Core.Entites;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Specifications;
using AutoMapper;
using API.Dtos;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductBrand> _productBrandRepository;
        private readonly IGenericRepository<ProductType> _productTypeRepository;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepository, IGenericRepository<ProductBrand> productBrandRepository, IGenericRepository<ProductType> productTypeRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _productBrandRepository = productBrandRepository;
            _productTypeRepository = productTypeRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();

            var product = await _productRepository.ListAsync(spec);
            var mappedDto = _mapper.Map<IReadOnlyList<ProductToReturnDto>>(product);
            if (mappedDto != null)
                return Ok(mappedDto);
            else
                return NotFound("Product not found");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _productRepository.GetEntityWithSpec(spec);

            var mappedDto = _mapper.Map<ProductToReturnDto>(product);

            if (mappedDto != null)
                return Ok(mappedDto);
            else
                return NotFound("Product not found");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var productBrand = await _productBrandRepository.GetListAllAsync();
            if (productBrand != null)
                return Ok(productBrand);
            else
                return NotFound("ProductBrand not found");
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var productType = await _productTypeRepository.GetListAllAsync();
            if (productType != null)
                return Ok(productType);
            else
                return NotFound("ProductType not found");
        }
    }
}