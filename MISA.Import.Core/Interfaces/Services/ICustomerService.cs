using Microsoft.AspNetCore.Http;
using MISA.Import.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.Import.Core.Interfaces.Services
{
    /// <summary>
    /// Interface service của khách hàng
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>
        /// Đọc thông tin của file excel
        /// </summary>
        /// <param name="file">file excel</param>
        /// <param name="cancellationToken">token hủy</param>
        /// <returns>Danh sách khách hàng và lỗi của từng khách hàng</returns>
        public Task<List<CustomerImport>> ReadFromExcel(IFormFile file, CancellationToken cancellationToken);

        /// <summary>
        /// Thêm danh sách khách hàng không có lỗi vào DB
        /// </summary>
        /// <param name="customerImports">Danh sách khách hàng và Lỗi của từng thông tin của một khách hàng</param>
        /// <returns>các khách hàng thêm thành công</returns>
        public int InsertCustomers(List<CustomerImport> customerImports);
    }
}
