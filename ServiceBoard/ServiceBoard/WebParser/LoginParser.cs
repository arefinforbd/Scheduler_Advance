using ServiceBoard.SPBoardWCFService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceBoard.WebParser
{
    public class LoginParser
    {
        public bool Login(string companyID, string companyPassword, out int level4ID, out string message)
        {
            level4ID = 0;
            message = "";
            SPBoardWCFServiceClient sp = new SPBoardWCFServiceClient();

            bool status = sp.Login(companyID, companyPassword, out level4ID, out message);

            return status;
        }
    }
}