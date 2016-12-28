using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Data;

namespace IQMediaGroup.Model.Interface
{
    /// <summary>
    /// Interface of ContactUs
    /// </summary>
    public interface IContactUsModel
    {
        /// <summary>
        ///  This method inserts contactus details.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_ContactUs">Object of Core class of ContactUs</param>
        /// <returns>ContactMemberID if added successfully.</returns>
        string InsertContactDetails(IQMediaContactUs p_ContactUs);

        /// <summary>
        /// This method updates ContactUs Content Information.
        /// </summary>
        /// <param name="p_ContactUsText">ContactUs Content</param>
        /// <returns></returns>
        string UpdateContactUsText(string p_ContactUsText);

        /// <summary>
        /// Description:This method will get Contact us details.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <returns>Dataset of Contact Us details.</returns>
        DataSet GetContactUsDetails();
    }
}
