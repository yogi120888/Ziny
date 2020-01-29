using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChemiFriend.API.Models.ImageUploadModel
{
    public class UploadDocumentModel
    {
        public int ImageSize { get; set; }
        public int ImageQuality { get; set; }
        public List<DocumentModel> documents { get; set; }
    }
}