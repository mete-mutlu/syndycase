using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product.API.Models;
using Product.Core.Abstract;
using Product.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Product.Core.Enums;

namespace Product.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Core.Entities.Product> repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductController(IRepository<Core.Entities.Product> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        /// <summary>
        ///  Create a new Product
        /// </summary>
        /// <param name="model"></param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] CreateProductModel model)
        {

            repository.Create(mapper.Map<Core.Entities.Product>(model));
            if (await unitOfWork.SaveChangesAsync() > 0)
            {
                return Ok("Product successfully created!");
            }

            return Ok("Product cannot created.");
        }

        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <response code="204">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("Update", Name = "Update Parking Lot")]
        public async Task<ActionResult> Update([FromBody] UpdateProductModel model)
        {

            var product = await repository.GetAsync(model.Id);
            if (product != null)
            {
                mapper.Map(model, product);
                repository.Update(product);
                if (await unitOfWork.SaveChangesAsync() > 0)
                    return NoContent();
                else
                    return Ok("Product could not be updated!");
            }
            else
                return NotFound("Product not found.");


        }


        /// <summary>
        /// Get Product
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns Parking Lot</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> Get(int id)
        {
            if (id < 1)
                return BadRequest("Id must be positive.");

            var entity = await repository.GetAsync(id, "User", "Brand");
            if (entity == null)
                return NotFound();

            return Ok(mapper.Map<ProductDto>(entity));
        }


        /// <summary>
        /// Get User Products
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns User Products</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<ProductDto>> GetByUser(int userId)
        {
            var result = await repository.GetWhereAsync(p=>p.UserId==userId,"User","Brand");
            return Ok(mapper.Map<IEnumerable<ProductDto>>(result));
        }

        /// <summary>
        /// Get Brand Products
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns Brand Products</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("Brand/{brandId}")]
        public async Task<ActionResult<ProductDto>> GetByBrand(int brandId)
        {
            var result = await repository.GetWhereAsync(p => p.UserId == brandId, "User", "Brand");
            return Ok(mapper.Map<IEnumerable<ProductDto>>(result));
        }


        /// <summary>
        /// Archive Product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <response code="204">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPatch("Archive/{id}")]
        public async Task<ActionResult> Archive(int id)
        {
            if (id < 1)
                return BadRequest("Id must be more than ");

            var product = await repository.GetAsync(id);
            if (product !=null)
            {
                product.Status = EntityStatus.Archived;
                return NoContent();
            }
            else
                return NotFound("Product could not be found");

               
        }


    }
}
