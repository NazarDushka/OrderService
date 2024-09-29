﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedModels;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private static readonly List<Product> Products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 1500 },
            new Product { Id = 2, Name = "Phone", Price = 800 }
        };

        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return Ok(Products);
        }

        // Додати новий продукт
        [HttpPost]
        public ActionResult<Product> AddProduct([FromBody] Product newProduct)
        {
            newProduct.Id = Products.Max(p => p.Id) + 1; // Генерація нового ID
            Products.Add(newProduct);
            return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, newProduct);
        }

        // Оновити існуючий продукт
        [HttpPut("{id}")]
        public ActionResult UpdateProduct(int id, [FromBody] Product updatedProduct)
        {
            var existingProduct = Products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
                return NotFound();

            // Оновлення полів продукту
            existingProduct.Name = updatedProduct.Name;
            existingProduct.Price = updatedProduct.Price;

            return Ok(existingProduct);
        }
    }
}

