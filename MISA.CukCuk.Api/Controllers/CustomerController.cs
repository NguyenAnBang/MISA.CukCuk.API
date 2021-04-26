using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Repositories;
using MISA.CukCuk.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/v2/[controller]s")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        ICustomerRepository _customerRepository;
        ICustomerServices _customerServices;

        public CustomerController(ICustomerRepository customerRepository, ICustomerServices customerServices)
        {
            _customerRepository = customerRepository;
            _customerServices = customerServices;
        }
        
        /// <summary>
        /// Lấy tất cả bản ghi từ database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var customers = _customerServices.GetAll();
            if (customers.Count() > 0) return Ok(customers);
            else return NoContent();
        }

        /// <summary>
        /// Lấy bản ghi từ database theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetGetCustomerById(Guid id)
        {
            var customer = _customerServices.GetCustomerById(id);
            if (customer != null)
            {
                return Ok(customer);
            }
            else
            {
                return NoContent();
            }
        }
        /// <summary>
        /// Lấy bản ghi theo phân trang
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("Paging")]

        public IActionResult GetCustomerPaging(int pageIndex, int pageSize)
        {
            var customers = _customerServices.GetCustomerPaging(pageIndex, pageSize);
            if (customers.Count() > 0)
            {
                return Ok(customers);
            }
            else
            {
                return NoContent();
            }
        }

        /// <summary>
        /// Insert 1 bản ghi vào database 
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>

        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            var rowsAffect = _customerServices.Post(customer);

            if (rowsAffect > 0) return Ok();
            else return NoContent();
        }

        /// <summary>
        /// Update 1 bản ghi vào database (sau khi validate dữ liệu trong service)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put(Guid id, [FromBody] Customer customer)
        {
            var rowsAffect = _customerServices.Put(id, customer);
            if (rowsAffect > 0) return Ok(rowsAffect);
            else return NoContent();
        }

        /// <summary>
        /// Xóa 1 bản ghi trong database 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            var rowsAffect = _customerServices.Delete(id);
            if (rowsAffect > 0) return Ok(rowsAffect);
            else return NoContent();
        }
    }
}
