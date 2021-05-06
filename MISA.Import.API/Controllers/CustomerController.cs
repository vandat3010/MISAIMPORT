using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Import.Core.Entities;
using MISA.Import.Core.Interfaces.Repositories;
using MISA.Import.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.Import.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        #region property
        private ICustomerService _customerService;
        private ICustomerRepository _customerRepository;
        #endregion

        #region Constructor

        public CustomerController(ICustomerService customerService, ICustomerRepository customerRepository)
        {
            _customerService = customerService;
            _customerRepository = customerRepository;
        }

        #endregion

        #region Method

        /// <summary>
        /// đọc dữ liệu từ file excel và đưa ra lỗi của từng khách hàng.
        /// </summary>
        /// <param name="formFile">file excel</param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        ///  <response code="200">Có dữ liệu trả về.</response>
        ///  <response code="400">Lỗi client.</response>
        ///  <response code="500">Lỗi server</response>
        /// </returns>

        [HttpPost("Reader")]

        public async Task<IActionResult> Post(IFormFile formFile, CancellationToken cancellationToken)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return BadRequest();
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest();
            }

            var res = await _customerService.ReadFromExcel(formFile, cancellationToken);
            return Ok(res);
        }
        /// <summary>
        /// Thêm danh sách khách hàng không có lỗi vào db
        /// </summary>
        /// <param name="customersImport">Danh sách khách hàng và lỗi thông tin cảu khách hàng</param>
        /// <returns>
        /// <response code="200">Thêm thành công.</response>
        /// <response code="500">Lỗi server.</response>
        /// </returns>
        [HttpPost("Import")]
        public IActionResult InsertCustomers(List<CustomerImport> customersImport)
        {
            int success = _customerService.InsertCustomers(customersImport);
            return Ok(new
            {
                totalRecord = customersImport.Count(),
                success = success,
                customers = customersImport
            });
        }

        #endregion
    }
}
