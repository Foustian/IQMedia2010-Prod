using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Collections;
using System.Data;
using System.Configuration;
using Microsoft.SharePoint.Utilities;
using System.Web.UI.WebControls;

namespace AmexCash.Helper
{
    public class Helper
    {
        public static bool IsAdmin(string strSite)
        {
            try
            {
                using (SPSite site = new SPSite(strSite))
                {
                    using (SPWeb oWeb = site.OpenWeb())
                    {
                        if (oWeb.Exists)
                        {
                            string currUserName = SPContext.Current.Web.CurrentUser.Name;
                            if (oWeb.UserIsSiteAdmin)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string getGroup(string url)
        {
            string ugroup = string.Empty;
            try
            {

                //SPWeb spWeb = Microsoft.SharePoint.WebControls.SPControl.GetContextWeb(System.Web.HttpContext.Current);
                //SPUser currentUser = spWeb.CurrentUser;
                //SPGroupCollection userGroups = currentUser.Groups;
                SPSite oSite = new SPSite(url);
                SPWeb oWeb = oSite.OpenWeb();
                SPGroupCollection userGroups = oWeb.SiteGroups;

                string strEntry = System.Configuration.ConfigurationSettings.AppSettings["Entry"].ToString();
                string[] strGroupEntry = strEntry.Split(',');
                ArrayList alGroupEntry = new ArrayList(strGroupEntry);


                string strPurchase = System.Configuration.ConfigurationSettings.AppSettings["Purchase"].ToString();
                string[] strGroupPurchase = strPurchase.Split(',');
                ArrayList alGroupPurchase = new ArrayList(strGroupPurchase);


                string strPayment = System.Configuration.ConfigurationSettings.AppSettings["Payment"].ToString();
                string[] strGroupPayment = strPayment.Split(',');
                ArrayList alGroupPayment = new ArrayList(strGroupPayment);


                string strPaymentProcessing = System.Configuration.ConfigurationSettings.AppSettings["PaymentProcessing"].ToString();
                string[] strGroupPaymentProcessing = strPaymentProcessing.Split(',');
                ArrayList alGroupPaymentProcessing = new ArrayList(strGroupPaymentProcessing);

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    foreach (SPGroup group in userGroups)
                    {
                        SPGroup gName1 = group;

                        if (alGroupEntry.Contains(gName1.Name))
                        {
                            bool Test2 = group.ContainsCurrentUser;
                            if (Test2)
                            {
                                ugroup = "Entry";
                                break;
                            }
                        }
                        if (alGroupPurchase.Contains(gName1.Name))
                        {
                            bool Test2 = group.ContainsCurrentUser;
                            if (Test2)
                            {
                                ugroup = "Purchase";
                                break;
                            }
                        }
                        if (alGroupPayment.Contains(gName1.Name))
                        {
                            bool Test2 = group.ContainsCurrentUser;
                            if (Test2)
                            {
                                ugroup = "Payment";
                                break;
                            }
                        }
                        if (alGroupPaymentProcessing.Contains(gName1.Name))
                        {
                            bool Test2 = group.ContainsCurrentUser;
                            if (Test2)
                            {
                                ugroup = "PaymentProcessing";
                                break;
                            }
                        }


                        //if (group.Name.Contains(strRole))
                        //if (alGroupEntry.Contains(group.Name))
                        //{
                        //    bool IsHe = isUserInGroup(group, currentUser.LoginName);
                        //    if (IsHe)
                        //    {
                        //        return "Entry";

                        //    }
                        //}
                        //if (alGroupPurchase.Contains(group.Name))
                        //{
                        //    bool IsHe = isUserInGroup(group, currentUser.LoginName);
                        //    if (IsHe)
                        //    {
                        //        return "Purchase";

                        //    }
                        //}
                        //if (alGroupPayment.Contains(group.Name))
                        //{
                        //    bool IsHe = isUserInGroup(group, currentUser.LoginName);
                        //    if (IsHe)
                        //    {
                        //        return "Payment";

                        //    }
                        //}
                        //if (alGroupPaymentProcessing.Contains(group.Name))
                        //{
                        //    bool IsHe = isUserInGroup(group, currentUser.LoginName);
                        //    if (IsHe)
                        //    {
                        //        return "PaymentProcessing";
                        //    }
                        //}

                    }
                });


                return ugroup;


            }
            catch (Exception ex)
            {

            }
            return string.Empty;
        }

        protected static Boolean isUserInGroup(SPGroup oGroupName, String sUserLoginName)
        {
            Boolean bUserIsInGroup = false;
            try
            {
                SPUser x = oGroupName.Users[sUserLoginName];
                bUserIsInGroup = true;
            }
            catch (SPException)
            {
                bUserIsInGroup = false;
            }
            return bUserIsInGroup;
        }

        public DataTable GetAmexCashData(string url, DropDownList drpMonth, DropDownList drpYear)
        {
            string GeneratedQuery = GetCamlQuery(drpMonth, drpYear);
            DataTable dtinvoice = null;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(url))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList list = web.Lists[ConfigurationSettings.AppSettings["AmexCashList"].ToString()];
                            SPQuery query = new SPQuery();
                            query.CalendarDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            //query.Query = "</View>";                            
                            query.Query = GeneratedQuery;


                            //  "<Eq>" +
                            //    "<FieldRef Name='IsActive' /><Value Type='Boolean'>1</Value>" +
                            //    "</Eq>" +
                            //"</And></Where>";


                            //query.Query = "<Where><And>" +
                            //                "<Eq>" +
                            //                "<FieldRef Name='Billdate' /><Value IncludeTimeValue='FALSE' Type='DateTime'>" + SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now.Date) + @"</Value>" +
                            //                "</Eq>" +
                            //                  "<Eq>" +
                            //                    "<FieldRef Name='IsActive' /><Value Type='Boolean'>1</Value>" +
                            //                    "</Eq>" +
                            //                "</And></Where>";


                            SPListItemCollection listitemcoll = list.GetItems(query);
                            dtinvoice = listitemcoll.GetDataTable();
                            

                        }

                    }
                });
                return dtinvoice;

            }
            catch (Exception ex) { throw ex; }
        }

        private string GetCamlQuery(DropDownList drpMonth, DropDownList drpYear)
        {
            string query = string.Empty;
            //var o = Enum.Parse(typeof(MonthEnum), s);
            //int monthCount = (int)Enum.Parse(typeof(MonthEnum), drpMonth.SelectedItem.Text);
            DateTime startDate;
            DateTime endDate;
            try
            {

                if (drpMonth.SelectedIndex <= 0 && drpYear.SelectedIndex <= 0) //Both not selected
                {
                    query = "<Where><Eq><FieldRef Name='IsActive' /><Value Type='Boolean'>1</Value></Eq></Where>";
                }
                else if (drpMonth.SelectedIndex > 0 && drpYear.SelectedIndex > 0) // Both Selected
                {
                    startDate = new DateTime(Convert.ToInt32(drpYear.SelectedItem.Text), Convert.ToInt32(drpMonth.SelectedValue), 1);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                    query = BuildQuery(startDate, endDate);

                }
                else if (drpMonth.SelectedIndex > 0 && drpYear.SelectedIndex <= 0) // Only month selected
                {
                    startDate = new DateTime(Convert.ToInt32(ConfigurationSettings.AppSettings["StartYear"]), Convert.ToInt32(drpMonth.SelectedValue), 1);
                    endDate = new DateTime(DateTime.Now.Year, Convert.ToInt32(drpMonth.SelectedValue), startDate.AddMonths(1).AddDays(-1).Day);
                    query = BuildQuery(startDate, endDate);
                }
                else if (drpMonth.SelectedIndex <= 0 && drpYear.SelectedIndex > 0) // Only year selected
                {
                    startDate = new DateTime(Convert.ToInt32(drpYear.SelectedItem.Text), 1, 1);
                    endDate = new DateTime(Convert.ToInt32(drpYear.SelectedItem.Text), 12, 31);
                    query = BuildQuery(startDate, endDate);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return query;
        }

        private string BuildQuery(DateTime startDate, DateTime endDate)
        {
            return "<Where><And>" +
                                    "<And>" +
                                   "<Geq>" +
                                   "<FieldRef Name='Billdate' /><Value IncludeTimeValue='FALSE' Type='DateTime'>" + SPUtility.CreateISO8601DateTimeFromSystemDateTime(startDate) + @"</Value>" +
                                   "</Geq>" +
                                    "<Leq>" +
                                   "<FieldRef Name='Billdate' /><Value IncludeTimeValue='FALSE' Type='DateTime'>" + SPUtility.CreateISO8601DateTimeFromSystemDateTime(endDate) + @"</Value>" +
                                   "</Leq>" +
                                   "</And>" +
                                   "<Eq>" +
                                   "<FieldRef Name='IsActive' /><Value Type='Boolean'>1</Value>" +
                                   "</Eq>" +
                                   "</And></Where>";
        }
    }
}
