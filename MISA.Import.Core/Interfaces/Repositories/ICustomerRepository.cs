using MISA.Import.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Import.Core.Interfaces.Repositories
{
    /// <summary>
    ///Inteface repository của khách hàng
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Kiểm tra mã khách hàng có tồn tại trên db
        /// </summary>
        /// <param name="customerCode">Mã khách hàng cần kiểm tra</param>
        /// <returns>tòn tại hay không tồn tại</returns>
        public bool CheckCustomerCodeExists(string customerCode);

        /// <summary>
        ///  Kiểm tra SĐT có tồn tại trên db
        /// </summary>
        /// <param name="phoneNumber">SĐT cần check</param>
        /// <returns>tồn tại hay không tồn tại</returns>
        public bool CheckPhoneNumberExists(string phoneNumber);
        /// <summary>
        /// Lấy thông tin một  nhóm khách hàng trên db
        /// </summary>
        /// <param name="customerGroupName">Tên nhóm khách cần lấy</param>
        /// <returns>Thông tin một nhóm khách hàng.</returns>
        public CustomerGroup GetCustomerGroup(string customerGroupName);

        /// <summary>
        /// Thêm một khách hàng vào db
        /// </summary>
        /// <param name="customer">Thông tin khách hàng</param>
        /// <returns>Số khách hàng thêm thành công</returns>

        public int InsertCustomer(Customer customer);
    }
}
