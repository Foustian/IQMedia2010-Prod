using System;
using System.Linq;
using IQMedia.Domain;

namespace IQMedia.Logic
{
    public class UserLogic : BaseLogic, ILogic
    {
        /// <summary>
        /// Gets the user by the user's GUID.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <returns>A user.</returns>
        public User GetUser(Guid guid)
        {
            var cust = Context.Customers.SingleOrDefault(c => guid.Equals(c.CustomerGUID.Value));
            if (cust != null)
                return User.Parse(cust);
            
            //Customer must be null so lets try LegacyCustomer
            var legCust = Context.LegacyCustomers.SingleOrDefault(c => guid.Equals(c.Guid));
            return User.Parse(legCust);
        }

        /// <summary>
        /// Gets the user by the user's username.
        /// </summary>
        /// <param name="username">The username of the desired user.</param>
        /// <returns>A user.</returns>
        public User GetUserByUsername(string username)
        {
            //NOTE: IQ's system doesn't support usernames so we check against email...
            var cust = Context.Customers.SingleOrDefault(c => c.Email == username);
            if (cust != null)
                return User.Parse(cust);

            //Customer must be null so lets try LegacyCustomer
            //NOTE: For Legacy reasons, we check username OR email
            //We're using FirstOrDefault here just because there is the rare chance there may be conflicting usernames/emails
            var legCust = Context.LegacyCustomers.FirstOrDefault(c => c.UserName == username || c.Email == username);
            return User.Parse(legCust);
        }

        /// <summary>
        /// Gets the user by the user's email address.
        /// </summary>
        /// <param name="email">The email address of the desired user.</param>
        /// <returns>A user.</returns>
        public User GetUserByEmail(string email)
        {
            var cust = Context.Customers.SingleOrDefault(c => c.Email == email);
            if (cust != null)
                return User.Parse(cust);

            //Customer must be null so lets try LegacyCustomer
            var legCust = Context.LegacyCustomers.SingleOrDefault(c => c.Email == email);
            return User.Parse(legCust);
        }

    }
}
