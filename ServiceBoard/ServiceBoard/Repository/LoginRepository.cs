using ServiceBoard.WebParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceBoard.Repository
{
    public class LoginRepository
    {
        public bool Login(string companyID, string companyPassword, out int level4ID, out string message)
        {
            LoginParser parser = new LoginParser();
            bool status = parser.Login(companyID, companyPassword, out level4ID, out message);

            return status;
        }
    }
}