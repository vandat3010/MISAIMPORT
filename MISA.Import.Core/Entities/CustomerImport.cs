using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Import.Core.Entities
{
    /// <summary>
    /// thông tin phản hồi trả về clients
    /// </summary>
    /// createdBy: NVDAT(06/05/2021)
    public class CustomerImport
    {
        /// <summary>
        /// Dữ liệu thông tin một khách hàng
        /// </summary>
        public Customer Data { get; set; }
        /// <summary>
        /// danh dách lỗi thông tin của một khách hàng
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();
    }
}
