using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChemiFriend.API.Models.ImageUploadModel
{
    public class DocumentModel
    {
        public string FileName { get; set; }
        public string PicBase64 { get; set; }
        public string FileExtenstion { get; set; }
        public byte[] Filebytes { get; set; }
        public Guid FileGuid { get; set; }
        public string FileUniqueName { get; set; }
    }
}