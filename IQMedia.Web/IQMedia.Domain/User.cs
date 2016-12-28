using System;

namespace IQMedia.Domain
{
    /// <summary>
    /// A User is a happy medium between LegacyCustomer(Redlasso) and Customer(IQ)
    /// It contains all of the properties that each shares and everything needed
    /// to act on the data.
    /// </summary>
    public class User
    {
        #region Properties
        public Guid Guid { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }

        public string Comment { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        #endregion

        #region Parsing Methods


        /// <summary>
        /// Parses the specified customer into a user.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns>A user.</returns>
        public static User Parse(Customer customer)
        {
            if (customer == null) return null;

            var user = new User
                           {
                               Comment = customer.CustomerComment,
                               CreatedBy = customer.CreatedBy,
                               CreatedDate = customer.CreatedDate.GetValueOrDefault(DateTime.MinValue),
                               Email = customer.Email,
                               FirstName = customer.FirstName,
                               LastName = customer.LastName,
                               Guid = customer.CustomerGUID.GetValueOrDefault(Guid.Empty),
                               IsActive = customer.IsActive.GetValueOrDefault(false),
                               ModifiedBy = customer.ModifiedBy,
                               ModifiedDate = customer.ModifiedDate.GetValueOrDefault(DateTime.MinValue),
                               Password = customer.CustomerPassword,
                               //TODO: Determine how IQ wants to cast username (for now, use email)
                               UserName = customer.Email
                           };
            return user;
        }

        /// <summary>
        /// Parses the specified legacy customer into a user.
        /// </summary>
        /// <param name="customer">The legacy customer.</param>
        /// <returns>A user.</returns>
        public static User Parse(LegacyCustomer customer)
        {
            if (customer == null) return null;

            var user = new User
                           {
                               Comment = customer.Comment,
                               CreatedBy = customer.CreatedBy,
                               CreatedDate = customer.CreatedDate,
                               Email = customer.Email,
                               FirstName = customer.FirstName,
                               LastName = customer.LastName,
                               Guid = customer.Guid,
                               IsActive = !customer.IsLockedOut,
                               ModifiedBy = customer.ModifiedBy,
                               ModifiedDate = customer.ModifiedDate,
                               Password = customer.Password,
                               UserName = customer.UserName
                           };
            return user;
        }
        #endregion
    }
}