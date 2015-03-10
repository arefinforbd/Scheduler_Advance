using CASPortal.CASWCFService;
using CASPortal.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CASPortal.WebParser
{
    public class BaseParser
    {
        public List<Advertisement> GetAdvertisement()
        {
            Advertisement[] adArr = null;
            List<Advertisement> ads = new List<Advertisement>();
            DataSet ds = new DataSet();
            CASWCFServiceClient cas = new CASWCFServiceClient();

            adArr = BaseHelper.DynamicAd ? cas.GetAdvertisement() : GetAdvertisementTemp();

            if (adArr != null)
            {
                foreach (Advertisement ad in adArr)
                    ads.Add(new Advertisement() { Header = ad.Header, ImageURL = ad.ImageURL, TextContents = ad.TextContents });

                return ads;
            }

            return null;
        }

        private Advertisement[] GetAdvertisementTemp()
        {
            List<Advertisement> ads = new List<Advertisement>()
            {
                new Advertisement() { Header = BaseHelper.AdHeader1, ImageURL = "", TextContents = BaseHelper.AdText1},
                new Advertisement() { Header = BaseHelper.AdHeader2, ImageURL = "", TextContents = BaseHelper.AdText2 },
                new Advertisement() { Header = BaseHelper.AdHeader3, ImageURL = "", TextContents = BaseHelper.AdText3 }
            };

            return ads.ToArray();
        }
    }
}