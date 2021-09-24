using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common
{
    public class DataTransfer
    {

        public enum ResponseType
        {
            Success = 1,
            Failed = 0,
            Exception = -1
        }
    }

    public enum ResponseType
    {
        Success = 1,
        Failed = 0,
        NotFound = 2,
        Exception = -1
    }

    public class UserDetails
    {

        public AppUser User { get; set; }
        public string Message { get; set; }

        public ResponseType Response { get; set; }

        public int IntValue { get; set; }

        public UserDetails()
        {

        }

        public UserDetails(AppUser user, string message, ResponseType response, int intValue = 0)
        {
            User = user;
            Message = message;
            Response = response;
            IntValue = intValue;
        }
    }

    public class StringMessageCL
    {

        public string Message { get; set; }

        public string RetValue { get; set; }

        public ResponseType Response { get; set; }

        public int IntValue { get; set; }

        public StringMessageCL()
        {

        }

        public StringMessageCL(string message, ResponseType response, string retValue = "", int intValue = 0, string roleName = "")
        {
            Message = message;
            Response = response;
            RetValue = retValue;
            IntValue = intValue;

        }
    }

}

