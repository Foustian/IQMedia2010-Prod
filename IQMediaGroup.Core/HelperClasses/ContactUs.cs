using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class IQMediaContactUs
    {
        /// <summary> 
        /// Represents Primary Key
        /// </summary>
        public Guid? ContactMemberID { get; set; }

        /// <summary>
        /// Represents First Name of Customer
        /// </summary>
        public string FirstName { get; set; }

        /// <summary> 
        /// Represents LastName of Customer
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Represents Email of Customer
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Represents Company Name of Customer
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Represents Title of Customer
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Represents ContactNo of Customer
        /// </summary>
        public string TelephoneNo { get; set; }

        /// <summary>
        /// Represents Comment given by Customer
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Represents CreatedBy
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Represents ModifiedBy
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Represents CreatedDate
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Represents ModifiedDate
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Represents Flag for particular record is active or not.
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Represents ContactUs Information
        /// </summary>
        public string ContactUsText { get; set; }

        /// <summary>
        /// Represents TimeToContact
        /// </summary>
        public string TimeToContact { get; set; }

        /// <summary>
        /// Represents HearAboutUs
        /// </summary>
        public string HearAboutUs { get; set; }

        /// <summary>
        /// Represents ContactUsInfo
        /// </summary>
        public string ContactUsInfo { get; set; }
    }
}
