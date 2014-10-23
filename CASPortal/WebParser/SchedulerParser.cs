using CASPortal.CASService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CASPortal.WebParser
{
    public class SchedulerParser
    {
        public void GetBusinessTime()
        {
            DataSet ds = new DataSet();

            try
            {
                CASWebService cas = new CASWebService();

                ds = cas.GetBusinessTime("kevorkt", "", "1.000", 1);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}