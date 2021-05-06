using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Import.Core.Entities;
using MISA.Import.Core.Interfaces.Repositories;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Import.Infastructure.Repositories
{
    /// <summary>
    /// Repository khách hàng
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        #region property
        private string _connectionString;
        #endregion

        #region Constructor
        public CustomerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionDb");
        }
        #endregion

        #region Method
        /// <summary>
        /// kiểm tra mã khách hàng có bị tồn tại không
        /// </summary>
        /// <param name="customerCode">Mã khách hàng cần check</param>
        /// <returns>Tồn tai hay không tồn tại</returns>
        public bool CheckCustomerCodeExists(string customerCode)
        {
            using var connection = new MySqlConnection(_connectionString);
            var c = new DynamicParameters();
            c.Add("customerCode", customerCode);
            bool iExists = connection.QueryFirstOrDefault<bool>("Proc_CheckCustomerCodeExists", c, commandType: CommandType.StoredProcedure);
            return iExists;
        }

        /// <summary>
        /// kiểm tra SĐT có bị tồn tại không
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns>Tồn tai hay không tồn tại</returns>
        public bool CheckPhoneNumberExists(string phoneNumber)
        {
            using var connection = new MySqlConnection(_connectionString);
            var c = new DynamicParameters();
            c.Add("phoneNumber", phoneNumber);
            bool iExists = connection.QueryFirstOrDefault<bool>("Proc_CheckPhoneNumberExists", c, commandType: CommandType.StoredProcedure);
            return iExists;
        }

        /// <summary>
        /// lấy thông tin tên nhóm khách hàng.
        /// </summary>
        /// <param name="customerGroupName">tên nhóm khách hàng</param>
        /// <returns>tên nhóm khách hàng</returns>
        public CustomerGroup GetCustomerGroup(string customerGroupName)
        {
            using var connection = new MySqlConnection(_connectionString);
            var c = new DynamicParameters();
            c.Add("customerGroupName", customerGroupName);
            var customerGroup = connection.QueryFirstOrDefault<CustomerGroup>("Proc_GetCustomerGroup", c, commandType: CommandType.StoredProcedure);
            return customerGroup;
        }

        /// <summary>
        /// Thêm thồn tin khách hàng vào db
        /// </summary>
        /// <param name="customer">Thông tin khách hàng</param>
        /// <returns>Thông tin khách hàng</returns>
        public int InsertCustomer(Customer customer)
        {
            using var connection = new MySqlConnection(_connectionString);
            var rowsAffect = connection.Execute("Proc_InsertCustomer", customer, commandType: CommandType.StoredProcedure);
            return rowsAffect;
        }
        #endregion
    }
}
