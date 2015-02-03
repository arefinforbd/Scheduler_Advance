using CASPortal.CASWCFService;
using CASPortal.WebParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASPortal.Repository
{
    public class BaseRepository
    {
        public List<Advertisement> GetAdvertisement()
        {
            BaseParser baseParser = new BaseParser();
            List<Advertisement> ads = new List<Advertisement>();

            ads = baseParser.GetAdvertisement();

            return ads;
        }
    }
}