using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MISA.CukCuk.Core.Exceptions
{
    public class CustomerException:Exception
    {

        public CustomerException(string message):base(message)
        {

        }

        /// <summary>
        /// Kiểm tra xem mã khách hàng có null không, nếu có thì ném ra exception
        /// </summary>
        /// <param name="customerCode"></param>
        public static void CheckNullCustomerCode(string customerCode)
        {
            if (string.IsNullOrEmpty(customerCode))
            {
                var response = new
                {
                    devMsg = "Mã khách hàng không được phép để trống",
                    MISACode = "001"
                };

                throw new CustomerException(response.devMsg);
            }
        }
        /// <summary>
        /// Kiểm tra xem email có hợp lệ hay không, nếu không hợp lệ thì ném ra exception
        /// </summary>
        /// <param name="email"></param>
        public static void CheckValidEmail(string email)
        {
            //Trả về true nếu email đúng format
            if(!Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                var response = new
                {
                    devMsg = "Email không đúng định dạng",
                    MISACode = "001"
                };
                throw new CustomerException(response.devMsg);
            }
        }
        /// <summary>
        /// Kiểm tra xem số điện thoại có đúng định dạng không, nếu không đúng thì ném ra exception
        /// </summary>
        /// <param name="phoneNumber"></param>
        public static void CheckValidPhoneNumber(string phoneNumber)
        {
            //Trả về true nếu số điện thoại đúng format
            if(!Regex.IsMatch(phoneNumber, @"^((\+0?1\s)?)\(?\d{3}\)?[\s.\s]\d{3}[\s.-]\d{4}$"))
            {
                var response = new
                {
                    devMsg = "Số điện thoại không đúng định dạng",
                    MISACode = "001"
                };
                throw new CustomerException(response.devMsg);
            }
        }


    }
}
