using ChemiFriend.Web.Models.ImageUploadModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChemiFriend.Web.Models.InputModel
{
    public class RegistrationModel
    {
        public Int64 RegistrationId { get; set; }
        [Required]
        public Int64 UserId { get; set; }
        [Required]
        public string FirmName { get; set; }
        [Required]
        public string LicenceNo { get; set; }
        //[Required] --- To do 
        public string LicenceImage { get; set; }
        public DocumentModel DocLicence { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        [Required]
        public string Email { get; set; }
        public int Country { get; set; }
        [Required]
        public int State { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required]
        public string PANNo { get; set; }
        public string GSTNo { get; set; }
        public string GSTNoImage { get; set; }
        public DocumentModel DocGSTNo { get; set; }
        public byte Status { get; set; }
        public bool IsDeleted { get; set; }
        public Int64 CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int64? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}