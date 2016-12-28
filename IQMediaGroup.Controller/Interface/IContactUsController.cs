using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Controller.Interface
{
    /// <summary>
    /// Interface for ContactUs Controller
    /// </summary>
    public interface IContactUsController
    {
        /// <summary>
        ///  This method inserts contact details.
        /// </summary>
        /// <param name="p_ContactUs">Object of Core class of ContactUs</param>
        /// <returns>ContactMemberID if added successfully.</returns>
        string InsertContactDetails(IQMediaContactUs p_ContactUs);

        /// <summary>
        /// This method updates ContactUs Content
        /// </summary>
        /// <param name="p_ContactUsText">ContactUs Content Information</param>
        /// <returns></returns>
        string UpdateContactUsText(string p_ContactUsText);

        /// <summary>
        /// This meyhod gets Contact Us details.
        /// </summary>
        /// <returns>List of object of Contact Us Details</returns>
        List<IQMediaContactUs> GetContactUsDetails();
    }
}
