using merge_EIP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace merge_EIP.Controllers
{
    public class FormImgAPIController : ApiController
    {
        FormModelEntities db = new FormModelEntities();
        public string Post(int FID)
        {
            dayOff image = new dayOff();
            image = db.dayOff.Where(m => m.dayoffNumber == FID).FirstOrDefault();
            string p3 = image.filePath;
            p3 = p3.Substring(1, p3.Length -1);
            return p3;
        }
    }
}
