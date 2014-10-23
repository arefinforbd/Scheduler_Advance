using CASPortal.CASService;
using CASPortal.Helper;
using CASPortal.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CASPortal.Controllers
{
    public class CustomerInformationController : Controller
    {
        //
        // GET: /CustomerInformation/
        public ActionResult Index()
        {
            /*TempData["CompanyID"] = "kevorkt";
            TempData["CompanyPassword"] = "";
            TempData["CustomerID"] = "1.000";
            TempData["CustomerPassword"] = "1.000";*/

            BaseHelper helper = new BaseHelper();
            if(!helper.IsValidUser())
                return RedirectToAction("Index", "Login");

            DataSet ds = new DataSet();
            byte[] image = new byte[200000];
            string companyID = Session["CompanyID"] as string;
            string companyPassword = Session["CompanyPassword"] as string;
            string customerID = Session["CustomerID"] as string;
            string customerPassword = Session["CustomerPassword"] as string;
            int level4ID = Convert.ToInt32(Session["Level4ID"]);

            CASWebService cas = new CASWebService();
            string foldernames = cas.GetPrivateFolders(companyID, companyPassword, customerPassword, level4ID, customerID);
            string[] folderList = foldernames.Split(',');

            StringBuilder sb = new StringBuilder("");
            sb.Append("<li style='cursor:pointer'><a>Select Folder</a></li>");

            foreach (var folder in folderList)
            {
                sb.Append("<li style='cursor:pointer'><a>" + folder + "</a></li>");
            }

            ViewBag.PrivateFolderList = sb;

            foldernames = cas.GetPublicFolders(companyID, companyPassword, customerPassword, 1);
            folderList = foldernames.Split(',');

            sb = new StringBuilder("");
            sb.Append("<li style='cursor:pointer'><a>Select Folder</a></li>");

            foreach (var folder in folderList)
            {
                sb.Append("<li style='cursor:pointer'><a>" + folder + "</a></li>");
            }

            ViewBag.PublicFolderList = sb;

            image = (byte[])Session["CompanyLogo"];
            ViewBag.ByteImage = image;

            return View();
        }

        public ActionResult GetFolderFiles(string folderName, string folderType, string clientTzOffset)
        {
            Folder folder;
            DataSet ds = new DataSet();
            List<Folder> folders = new List<Folder>();

            string timeZoneOffset = "";
            string companyID = Session["CompanyID"] as string;
            string companyPassword = Session["CompanyPassword"] as string;
            string customerID = Session["CustomerID"] as string;
            string customerPassword = Session["CustomerPassword"] as string;
            int level4ID = Convert.ToInt32(Session["Level4ID"]);

            CASWebService cas = new CASWebService();

            if(folderType.Equals("pvt"))
                ds = cas.GetPrivateFolderFiles(companyID, companyPassword, customerPassword, level4ID, customerID, folderName, timeZoneOffset);
            else if (folderType.Equals("pub"))
                ds = cas.GetPublicFolderFiles(companyID, companyPassword, customerPassword, level4ID, folderName, timeZoneOffset);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    folder = new Folder();
                    folder.fileName = row[1].ToString();
                    folder.fileDescription = row[2].ToString();
                    folder.fileUploadedDate = row[3].ToString();
                    folders.Add(folder);
                }
            }

            return Json(folders, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadFile(string hiddenfoldertype, string hiddenfoldername, string hiddenfilename)
        {
            DataSet ds = new DataSet();
            string companyID = Session["CompanyID"] as string;
            string companyPassword = Session["CompanyPassword"] as string;
            string customerID = Session["CustomerID"] as string;
            string customerPassword = Session["CustomerPassword"] as string;
            int level4ID = Convert.ToInt32(Session["Level4ID"]);

            try
            {
                CASWebService cas = new CASWebService();

                if (hiddenfoldertype.ToLower().Contains("private"))
                    ds = cas.GetPrivateFileInfo(companyID, companyPassword, customerPassword, level4ID, customerID, hiddenfoldername, hiddenfilename);
                else if (hiddenfoldertype.ToLower().Contains("public"))
                    ds = cas.GetPublicFileInfo(companyID, companyPassword, customerPassword, level4ID, hiddenfoldername, hiddenfilename);

                return File((byte[])(ds.Tables[0].Rows[0]["ttf_fileinfo"]), "application/octet", hiddenfilename);
            }
            catch (Exception ex)
            {
                return File(new byte[2], "application/octet", hiddenfilename);
            }
        }

        public ActionResult WelcomeMessage()
        {
            BaseHelper helper = new BaseHelper();
            if (!helper.IsValidUser())
                return RedirectToAction("Index", "Login");

            string companyName = "";
            string messageOfTheDay = "";
            DataSet ds = new DataSet();
            byte[] image = new byte[200000];

            string companyID = Session["CompanyID"] as string;
            string companyPassword = Session["CompanyPassword"] as string;
            string customerID = Session["CustomerID"] as string;
            string customerPassword = Session["CustomerPassword"] as string;
            int level4ID = Convert.ToInt32(Session["Level4ID"]);

            try
            {
                CASWebService cas = new CASWebService();

                ds = cas.LoginMessageOfDay(companyID, companyPassword, customerPassword, level4ID, customerID);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    messageOfTheDay = ds.Tables[0].Rows[0]["tt_motd"].ToString();
                    messageOfTheDay = messageOfTheDay.Replace("\na:link, span.MsoHyperlink\n\t{color:blue;\n\ttext-decoration:underline;}\na:visited, span.MsoHyperlinkFollowed\n\t{color:purple;\n\ttext-decoration:underline;}", "");
                    messageOfTheDay = messageOfTheDay.Replace("<span style='font-size:12.0pt;background:\nyellow'>Welcome to Temisoft Customer Access System</span>", "");
                    messageOfTheDay = messageOfTheDay.Replace("\n\n<p class=MsoNormal align=center style='margin-bottom:0cm;margin-bottom:.0001pt;\ntext-align:center;line-height:normal'><span\nstyle='font-size:12.0pt'>.</span></p>\n\n<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:\nnormal'><span style='font-size:12.0pt'>&nbsp;</span></p>\n\n<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:\nnormal'><span style='font-size:12.0pt'>&nbsp;</span></p>\n\n", "");
                    messageOfTheDay = messageOfTheDay.Replace("<a\nhref=\"http://www.temisoft.com.au/\">http://www.temisoft.com.au</a>", "<a\ntarget=\"_blank\"\nhref=\"http://www.temisoft.com.au/\">http://www.temisoft.com.au</a>");
                    companyName = ds.Tables[0].Rows[0]["tt_company_name"].ToString();

                    try
                    {
                        image = (byte[])ds.Tables[0].Rows[0]["tt_company_logo"];
                    }
                    catch(Exception ex){
                        image = new byte[2];
                    }

                    Session["CompanyLogo"] = image;
                    
                    ViewBag.MessageOfTheDay = messageOfTheDay;
                    ViewBag.CompanyName = companyName;
                    ViewBag.ByteImage = image;
                }
                else
                {
                    messageOfTheDay = "";
                    companyName = "";
                }

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult GetAdvertiseStatus()
        {
            bool adStatus = BaseHelper.AdvertisementStatus;

            return Json(adStatus, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TextEditor()
        {
            return View();
        }
	}
}