using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CASPortal.Models
{
    public class Folder
    {
        public int lvl4sequence { get; set; }
        public string fileName { get; set; }
        public string fileDescription { get; set; }
        public string fileUploadedDate { get; set; }
        public string fileUploadedBy { get; set; }

        public List<string> PrivateFolders { get; set; }
        public List<string> PublicFolders { get; set; }
    }
}