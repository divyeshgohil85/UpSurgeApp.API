using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UpSurgeApp.API.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        //public enum ResponseType
        //{
        //    Success = 1,
        //    Failed = 0,
        //    Exception = -1
        //}

        public class DropdownCL
        {
            public int ID { get; set; }
            public string Value { get; set; }

        }
        public class DropdownListCL
        {

            public List<DropdownCL> Data { get; set; }
          

            public string Message { get; set; }
            public DropdownListCL(List<DropdownCL> _data,  string _message)
            {
                Data = _data;
                Message = _message;
            }
        }

        public ApiResponse(int statusCode, string message= null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
           

        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                404 => "Resource not found",
                500 => "Internal server error",
                _ => null
            };
        }

    }
}
