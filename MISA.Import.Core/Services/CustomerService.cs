using Microsoft.AspNetCore.Http;
using MISA.Import.Core.Entities;
using MISA.Import.Core.Interfaces.Repositories;
using MISA.Import.Core.Interfaces.Services;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MISA.Import.Core.Services
{
    public class CustomerService : ICustomerService
    {
        #region Property

        private ICustomerRepository _customerRepository;

        #endregion

        #region Constructor

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        #endregion

        #region Method: INSET, READ file
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerImports"></param>
        /// <returns></returns>
        public int InsertCustomers(List<CustomerImport> customerImports)
        {
            int i = 0;
            foreach (var j in customerImports)
            {
                if (!j.Errors.Any())
                {
                    i++;
                    _customerRepository.InsertCustomer(j.Data);
                }
            }
            return i;
        }
        /// <summary>
        /// Đọc thông tin khách hàng
        /// </summary>
        /// <param name="file">file excel</param>
        /// <param name="cancellationToken">Token Hủy</param>
        /// <returns>Danh sách khách hàng và lỗi của từng khách hàng</returns>
        public async Task<List<CustomerImport>> ReadFromExcel(IFormFile formFile, CancellationToken cancellationToken)
        {
            var customersImport = new List<CustomerImport>();
            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream, cancellationToken);
                using var package = new ExcelPackage(stream);
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int rowNumber = 3; rowNumber <= rowCount; rowNumber++)
                {
                    var customer = new Customer()
                    {
                        CustomerCode = GetValue(worksheet.Cells[rowNumber, 1].Value),
                        FullName = GetValue(worksheet.Cells[rowNumber, 2].Value),
                        MemberCardCode = GetValue(worksheet.Cells[rowNumber, 3].Value),
                        PhoneNumber = GetValue(worksheet.Cells[rowNumber, 4].Value),
                        DateOfBirth = ParseDate(worksheet.Cells[rowNumber, 6].Value),
                        CompanyName = GetValue(worksheet.Cells[rowNumber, 7].Value),
                        CompanyTaxCode = GetValue(worksheet.Cells[rowNumber, 8].Value),
                        Email = GetValue(worksheet.Cells[rowNumber, 9].Value),
                        Address = GetValue(worksheet.Cells[rowNumber, 10].Value),
                        Note = GetValue(worksheet.Cells[rowNumber, 11].Value)
                    };

                    var customerImport = new CustomerImport();

                    if (customersImport.Any())
                    {
                        // check valid.

                        // check customerCode exists on import.
                        var customerCodeExistsOnExcel = customersImport.Where(c => c.Data.CustomerCode == customer.CustomerCode).Any();

                        if (customerCodeExistsOnExcel == true)
                        {
                            customerImport.Errors.Add(Properties.Resources.MsgErrorCustomerCodeExistsOnImport);
                        }

                        // check phoneNumber exists on import.
                        var phoneNumberExistsOnExcel = customersImport.Where(c => c.Data.PhoneNumber == customer.PhoneNumber).Any();


                        if (phoneNumberExistsOnExcel == true)
                        {
                            customerImport.Errors.Add(Properties.Resources.MsgErrorPhoneNumberExistsOnImport);
                        }
                    }

                    // check customerCode tồn tại trên hệ thống.
                    var customerCodeExists = _customerRepository.CheckCustomerCodeExists(customer.CustomerCode);
                    if (customerCodeExists == true)
                    {
                        customerImport.Errors.Add(Properties.Resources.MsgErrorCustomerCodeExists);
                    }

                    // check phoneNumber tồn tại trên hệ thống.
                    var phoneNumberExists = _customerRepository.CheckPhoneNumberExists(customer.PhoneNumber);
                    if (phoneNumberExists == true)
                    {
                        customerImport.Errors.Add(Properties.Resources.MsgErrorPhoneNumberExists);
                    }

                    // check nhóm khách hàng có tồn tại trên hệ thống.
                    string customerGroupName = GetValue(worksheet.Cells[rowNumber, 5].Value);

                    var customerGroup = _customerRepository.GetCustomerGroup(customerGroupName);
                    if (customerGroup == null)
                    {
                        customerImport.Errors.Add(Properties.Resources.MsgErrorCustomerGroupNotExists);
                    }
                    else
                    {
                        customer.CustomerGroupId = customerGroup.CustomerGroupId;
                        customer.CustomerGroupName = customerGroupName;
                    }

                    customerImport.Data = customer;
                    customersImport.Add(customerImport);
                }
            }
            return customersImport;
        }

        /// <summary>
        ///chuyển giá trị object từ excel thành kiểu string.
        /// </summary>
        /// <param name="valueObj">Giá trị cần chuyển</param>
        /// <returns>Chuỗi string.</returns>
        private string GetValue(object valueObject)
        {
            if (valueObject is null)
            {
                return null;
            }
            return valueObject.ToString().Trim();
        }

        /// <summary>
        ///parse date string thành kiểu DateTime.
        /// </summary>
        /// <param name="valueObj">dateString</param>
        /// <returns>DateTime</returns>
        private DateTime? ParseDate(object valueObject)
        {
            string valueStr = GetValue(valueObject);
            if (valueObject is null)
            {
                return null;
            }
            return DateTime.ParseExact(s: valueStr, formats: new string[] { "dd/MM/yyyy", "MM/yyyy", "yyyy" }, provider: null);
        }
        #endregion
    }
}
