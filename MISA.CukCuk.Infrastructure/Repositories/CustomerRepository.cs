using Microsoft.Extensions.Configuration;
using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using Dapper;
using MySqlConnector;
using System.Data;


namespace MISA.CukCuk.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        IConfiguration _configuration;

        //Khởi tạo để sử dụng interface configuration để gọi connection string từ appsettings.json
        public CustomerRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Check trùng mã khách hàng
        /// </summary>
        /// <param name="customerCode"></param>
        /// <returns></returns>
        public bool CheckDuplicateCustomerCode(string customerCode)
        {
            //Kết nối database
            IDbConnection dbConnection = new MySqlConnection(_configuration.GetConnectionString("connectionString"));

            //Check trùng mã khách hàng
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@m_CustomerCode", customerCode);
            var customerCodeExists = dbConnection.QueryFirstOrDefault<bool>("Proc_CheckCustomerCodeExists", param: dynamicParameters, commandType: CommandType.StoredProcedure);

            return customerCodeExists;
        }

        /// <summary>
        /// Delete 1 bản ghi trong database dựa theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(Guid id)
        {
            //Kết nối database
            IDbConnection dbConnection = new MySqlConnection(_configuration.GetConnectionString("connectionString"));

            //Tiến hành xóa dữ liệu

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@CustomerId", id);

            var rowsAffect = dbConnection.Execute("Proc_DeleteCustomer", param: dynamicParameters, commandType: CommandType.StoredProcedure);

            return rowsAffect;
        }

        /// <summary>
        /// Lấy tất cả dữ liệu từ database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetAll()
        {
            //Kết nối database 
            IDbConnection dbConnection = new MySqlConnection(_configuration.GetConnectionString("connectionString"));
            //Validate dữ liệu

            //Return dữ liệu
            var customers = dbConnection.Query<Customer>("Proc_GetCustomers", commandType: CommandType.StoredProcedure);
            return customers;
        }

        /// <summary>
        /// Lấy 1 bản ghi từ database dựa theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Customer GetCustomerById(Guid id)
        {
            //Kết nối database
            IDbConnection dbConnection = new MySqlConnection(_configuration.GetConnectionString("connectionString"));

            //Validate dữ liệu

            //Return dữ liệu
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@CustomerId", id);

            var customer = dbConnection.QueryFirstOrDefault<Customer>("Proc_GetCustomerById", param: dynamicParameters, commandType: CommandType.StoredProcedure);
            return customer;
        }

        public IEnumerable<Customer> GetCustomerPaging(int pageIndex, int pageSize)
        {
            //Kết nối database
            IDbConnection dbConnection = new MySqlConnection(_configuration.GetConnectionString("connectionString"));

            //validate dữ liệu

            //Return dữ liệu
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@m_PageIndex", pageIndex);
            dynamicParameters.Add("@m_PageSize", pageSize);

            var customers = dbConnection.Query<Customer>("Proc_GetCustomerPaging", param: dynamicParameters, commandType: CommandType.StoredProcedure);
            return customers;
        }

        /// <summary>
        /// Insert 1 bản ghi dữ liệu (khách hàng) vào database
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public int Post(Customer customer)
        {
            //Kết nối với database 

            IDbConnection dbConnection = new MySqlConnection(_configuration.GetConnectionString("connectionString"));

            //Trả về số lượng dòng bị ảnh hưởng

            var rowsAffect = dbConnection.Execute("Proc_InsertCustomer", param: customer, commandType: CommandType.StoredProcedure);
            return rowsAffect;
        }

        /// <summary>
        /// Update 1 bản ghi dữ liệu (khách hàng) vào database dựa trên id được truyền vào
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public int Put(Guid id, Customer customer)
        {
            //Kết nối database 
            IDbConnection dbConnection = new MySqlConnection(_configuration.GetConnectionString("connectionString"));

            customer.CustomerId = id;
            //put dữ liệu thành công, trả về bản ghi ảnh hưởng
            //DynamicParameters dynamicParameters = new DynamicParameters();
            //dynamicParameters.Add("@CustomerId", id);
            //dynamicParameters.Add("@CustomerCode", customer.CustomerCode);
            //dynamicParameters.Add("@FullName", customer.FullName);
            //dynamicParameters.Add("@MemberCardCode", customer.MemberCardCode);

            //dynamicParameters.Add("@CustomerGroupId", customer.CustomerGroupId);
            //dynamicParameters.Add("@DateOfBirth", customer.DateOfBirth);
            //dynamicParameters.Add("@Gender", customer.Gender);
            //dynamicParameters.Add("@Email", customer.Email);
            //dynamicParameters.Add("@PhoneNumber", customer.PhoneNumber);
            //dynamicParameters.Add("@CompanyName", customer.CompanyName);
            //dynamicParameters.Add("@CompanyTaxCode", customer.CompanyTaxCode);
            //dynamicParameters.Add("@Address", customer.Address);

            var rowsAffect = dbConnection.Execute("Proc_UpdateCustomer", param: customer, commandType: CommandType.StoredProcedure);

            return rowsAffect;
        }
    }
}
