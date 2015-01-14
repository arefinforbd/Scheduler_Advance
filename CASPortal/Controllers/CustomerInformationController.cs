using CASPortal.CASWCFService;
using CASPortal.Helper;
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

            CASWCFServiceClient cas = new CASWCFServiceClient();
            //string foldernames = cas.GetPrivateFolders(companyID, companyPassword, customerPassword, level4ID, customerID);
            string foldernames = Session["PrivateFolders"] as string;
            string[] folderList = foldernames.Split(',');

            StringBuilder sb = new StringBuilder("");
            sb.Append("<li style='cursor:pointer'><a>Select Folder</a></li>");

            foreach (var folder in folderList)
            {
                sb.Append("<li style='cursor:pointer'><a>" + folder + "</a></li>");
            }

            ViewBag.PrivateFolderList = sb;

            //foldernames = cas.GetPublicFolders(companyID, companyPassword, customerPassword, 1);
            foldernames = Session["PublicFolders"] as string;
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

        [HttpPost]
        public ActionResult GetMobileMessageStatus()
        {
            bool status = Session["MobileMessageStatus"] == null ? true : false;
            
            if (Session["MobileMessageStatus"] == null)
                Session["MobileMessageStatus"] = true;

            return Json(status, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFolderFiles(string folderName, string folderType, string clientTzOffset)
        {
            Folder folder;
            Folder[] folderArr = null;
            DataSet ds = new DataSet();
            List<Folder> folders = new List<Folder>();

            string timeZoneOffset = "";
            string companyID = Session["CompanyID"] as string;
            string companyPassword = Session["CompanyPassword"] as string;
            string customerID = Session["CustomerID"] as string;
            string customerPassword = Session["CustomerPassword"] as string;
            int level4ID = Convert.ToInt32(Session["Level4ID"]);

            CASWCFServiceClient cas = new CASWCFServiceClient();

            if(folderType.Equals("pvt"))
                folderArr = cas.GetPrivateFolderFiles(companyID, companyPassword, customerPassword, level4ID, customerID, folderName, timeZoneOffset);
            else if (folderType.Equals("pub"))
                folderArr = cas.GetPublicFolderFiles(companyID, companyPassword, customerPassword, level4ID, folderName, timeZoneOffset);

            if (folderArr != null && folderArr.Count() > 0)
            {
                foreach (Folder folderItem in folderArr)
                {
                    folder = new Folder();
                    folder.fileName = folderItem.fileName;
                    folder.fileDescription = folderItem.fileDescription;
                    folder.fileUploadedDate = folderItem.fileUploadedDate;
                    folders.Add(folder);
                }
            }

            return Json(folders, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadFile(string hiddenfoldertype, string hiddenfoldername, string hiddenfilename)
        {
            byte[] fileInfo = null;
            DataSet ds = new DataSet();
            string companyID = Session["CompanyID"] as string;
            string companyPassword = Session["CompanyPassword"] as string;
            string customerID = Session["CustomerID"] as string;
            string customerPassword = Session["CustomerPassword"] as string;
            int level4ID = Convert.ToInt32(Session["Level4ID"]);

            try
            {
                CASWCFServiceClient cas = new CASWCFServiceClient();

                if (hiddenfoldertype.ToLower().Contains("private"))
                    fileInfo = cas.GetPrivateFileInfo(companyID, companyPassword, customerPassword, level4ID, customerID, hiddenfoldername, hiddenfilename);
                else if (hiddenfoldertype.ToLower().Contains("public"))
                    fileInfo = cas.GetPublicFileInfo(companyID, companyPassword, customerPassword, level4ID, hiddenfoldername, hiddenfilename);

                return File(fileInfo, "application/octet", hiddenfilename);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error";
                return RedirectToAction("Index", "CustomerInformation");
                //return File(new byte[2], "application/octet", hiddenfilename);
            }
        }

        public ActionResult WelcomeMessageOld()
        {
            BaseHelper helper = new BaseHelper();
            if (!helper.IsValidUser())
                return RedirectToAction("Index", "Login");

            string companyName = "";
            string messageOfTheDay = "";
            byte[] image = new byte[200000];
            Welcome[] welcomeArr = null;

            string companyID = Session["CompanyID"] as string;
            string companyPassword = Session["CompanyPassword"] as string;
            string customerID = Session["CustomerID"] as string;
            string customerPassword = Session["CustomerPassword"] as string;
            int level4ID = Convert.ToInt32(Session["Level4ID"]);

            try
            {
                CASWCFServiceClient cas = new CASWCFServiceClient();

                welcomeArr = cas.LoginMessageOfDay(companyID, companyPassword, customerPassword, level4ID, customerID);

                if (welcomeArr != null && welcomeArr.Count() > 0)
                {
                    foreach (Welcome welcome in welcomeArr)
                    {
                        messageOfTheDay = welcome.MessageOfTheDay;
                        messageOfTheDay = messageOfTheDay.Replace("\na:link, span.MsoHyperlink\n\t{color:blue;\n\ttext-decoration:underline;}\na:visited, span.MsoHyperlinkFollowed\n\t{color:purple;\n\ttext-decoration:underline;}", "");
                        messageOfTheDay = messageOfTheDay.Replace("<span style='font-size:12.0pt;background:\nyellow'>Welcome to Temisoft Customer Access System</span>", "");
                        messageOfTheDay = messageOfTheDay.Replace("\n\n<p class=MsoNormal align=center style='margin-bottom:0cm;margin-bottom:.0001pt;\ntext-align:center;line-height:normal'><span\nstyle='font-size:12.0pt'>.</span></p>\n\n<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:\nnormal'><span style='font-size:12.0pt'>&nbsp;</span></p>\n\n<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:\nnormal'><span style='font-size:12.0pt'>&nbsp;</span></p>\n\n", "");
                        messageOfTheDay = messageOfTheDay.Replace("<a\nhref=\"http://www.temisoft.com.au/\">http://www.temisoft.com.au</a>", "<a\ntarget=\"_blank\"\nhref=\"http://www.temisoft.com.au/\">http://www.temisoft.com.au</a>");
                        companyName = welcome.CompanyName;

                        try
                        {
                            image = (byte[])welcome.CompanyLogo;
                        }
                        catch (Exception ex)
                        {
                            image = new byte[2];
                        }

                        Session["CompanyLogo"] = image;

                        ViewBag.MessageOfTheDay = messageOfTheDay;
                        ViewBag.CompanyName = companyName;
                        ViewBag.ByteImage = image;
                    }
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

        public ActionResult WelcomeMessage()
        {
            BaseHelper helper = new BaseHelper();
            if (!helper.IsValidUser())
                return RedirectToAction("Index", "Login");

            string companyName = "";
            string messageOfTheDay = "";
            byte[] image = new byte[200000];
            Welcome welcome;

            string companyID = Session["CompanyID"] as string;
            string companyPassword = Session["CompanyPassword"] as string;
            string customerID = Session["CustomerID"] as string;
            string customerPassword = Session["CustomerPassword"] as string;
            int level4ID = Convert.ToInt32(Session["Level4ID"]);

            try
            {
                CASWCFServiceClient cas = new CASWCFServiceClient();

                welcome = cas.GetCASDefaultData(companyID, companyPassword, customerPassword, level4ID, customerID);

                if (welcome != null)
                {
                    messageOfTheDay = welcome.MessageOfTheDay;
                    messageOfTheDay = messageOfTheDay.Replace("\na:link, span.MsoHyperlink\n\t{color:blue;\n\ttext-decoration:underline;}\na:visited, span.MsoHyperlinkFollowed\n\t{color:purple;\n\ttext-decoration:underline;}", "");
                    messageOfTheDay = messageOfTheDay.Replace("<span style='font-size:12.0pt;background:\nyellow'>Welcome to Temisoft Customer Access System</span>", "");
                    messageOfTheDay = messageOfTheDay.Replace("\n\n<p class=MsoNormal align=center style='margin-bottom:0cm;margin-bottom:.0001pt;\ntext-align:center;line-height:normal'><span\nstyle='font-size:12.0pt'>.</span></p>\n\n<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:\nnormal'><span style='font-size:12.0pt'>&nbsp;</span></p>\n\n<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:\nnormal'><span style='font-size:12.0pt'>&nbsp;</span></p>\n\n", "");
                    messageOfTheDay = messageOfTheDay.Replace("<a\nhref=\"http://www.temisoft.com.au/\">http://www.temisoft.com.au</a>", "<a\ntarget=\"_blank\"\nhref=\"http://www.temisoft.com.au/\">http://www.temisoft.com.au</a>");
                    companyName = welcome.CompanyName;

                    try
                    {
                        image = (byte[])welcome.CompanyLogo;
                    }
                    catch (Exception ex)
                    {
                        image = new byte[2];
                    }

                    Session["CompanyLogo"] = image;
                    Session["PrivateFolders"] = welcome.PrivateFolders;
                    Session["PublicFolders"] = welcome.PublicFolders;

                    ViewBag.MessageOfTheDay = messageOfTheDay;
                    ViewBag.CompanyName = companyName;
                    Session["CompanyName"] = companyName;
                    if (image == null)
                        image = new byte[2];

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