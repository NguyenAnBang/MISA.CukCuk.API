using MISA.CukCuk.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.CukCuk.Core.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// Repository làm việc với database 
        /// </summary>
        /// <returns></returns>
        /// 

        //Lấy tất cả bản ghi từ database
        public IEnumerable<Customer> GetAll();
        //Lấy 1 bản ghi theo id
        public Customer GetCustomerById(Guid id);
        //Insert 1 bản ghi vào database
        public int Post(Customer customer);
        //Update 1 bản ghi theo id
        public int Put(Guid id, Customer customer);
        //Xóa 1 bản ghi theo id
        public int Delete(Guid id);
        //Check mã khách hàng có trùng không
        public bool CheckDuplicateCustomerCode(string customerCode);
        //Phân trang
        public IEnumerable<Customer> GetCustomerPaging(int pageIndex, int pageSize);


    }
}
