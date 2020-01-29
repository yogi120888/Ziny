using ChemiFriend.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ChemiFriend.ENTITY
{
    public class Registration : BaseEntity
    {
        [Key]
        public Int64 RegistrationId { get; set; }
        public Int64 UserId { get; set; }
        [ForeignKey("UserId")]
        public Usermaster Usermasters { get; set; }
        public string FirmName { get; set; }
        public string LicenceNo { get; set; }
        public string LicenceImage { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public int Country { get; set; }
        public int State { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PANNo { get; set; }
        public string GSTNo { get; set; }
        public string GSTNoImage { get; set; }
    }
}
