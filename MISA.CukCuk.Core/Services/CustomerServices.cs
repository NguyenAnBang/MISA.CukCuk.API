using MISA.CukCuk.Core.Entities;
using MISA.CukCuk.Core.Exceptions;
using MISA.CukCuk.Core.Interfaces.Repositories;
using MISA.CukCuk.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace MISA.CukCuk.Core.Services
{
    public class CustomerServices : ICustomerServices
    {
        ICustomerRepository _customerRepository;
        public CustomerServices(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Xóa 1 bản ghi theo id, nhận giá trị từ customerRepository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Delete(Guid id)
        {
            var rowsAffect = _customerRepository.Delete(id);
            return rowsAffect;
        }

        /// <summary>
        /// Lấy tất cả bản ghi từ database, nhận giá trị từ customerRepository
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> GetAll()
        {
            var customers = _customerRepository.GetAll();
            return customers;
        }

        /// <summary>
        /// Lấy 1 bản ghi từ database dựa theo id, nhận giá trị từ customerRepository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Customer GetCustomerById(Guid id)
        {
            var customer = _customerRepository.GetCustomerById(id);
            return customer;
        }

        /// <summary>
        /// Phân trang
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<Customer> GetCustomerPaging(int pageIndex, int pageSize)
        {
            var customers = _customerRepository.GetCustomerPaging(pageIndex, pageSize);
            return customers;
        }

        /// <summary>
        /// Insert 1 bản ghi vào database, nhận dữ liệu từ customerRepository, validate dữ liệu, rồi đẩy dữ liệu về controller
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public int Post(Customer customer)
        {
            //Validate dữ liệu

            //Kiểm tra xem customerCode có null hay không (phía client)
            CustomerException.CheckNullCustomerCode(customer.CustomerCode);
                //static là gọi luôn, không cần khởi tạo phương thức gọi đến nó
            //Kiểm tra xem fullName có null hay không (phía client)

            //Kiểm tra xem customerCode đã tồn tại chưa (phía server) (duplicate)
            var isExists = _customerRepository.CheckDuplicateCustomerCode(customer.CustomerCode);
            if(isExists == true)
            {
                throw new CustomerException("MÃ khách hàng đã tồn tại trên hệ thống!.");
            }

            //Kiểm tra email hợp lệ không (phía client, không cần kết nối database)
            CustomerException.CheckValidEmail(customer.Email);

            //Kiểm tra số điện thoại hợp lệ không
            CustomerException.CheckValidPhoneNumber(customer.PhoneNumber);


            var rowsAffect = _customerRepository.Post(customer);

            return rowsAffect;
        }

        /// <summary>
        /// Update 1 bản ghi vào database dựa theo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public int Put(Guid id, Customer customer)
        {
            //Validate dữ liệu
            //Kiểm tra email hợp lệ
            CustomerException.CheckValidEmail(customer.Email);
            //Kiểm tra số điện thoại hợp lệ
            CustomerException.CheckValidPhoneNumber(customer.PhoneNumber);

            var rowsAffect = _customerRepository.Put(id, customer);
            return rowsAffect;
        }
    }
}
