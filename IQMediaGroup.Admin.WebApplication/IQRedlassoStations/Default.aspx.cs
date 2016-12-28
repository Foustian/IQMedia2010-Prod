using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using IQMediaGroup.Core.HelperClasses;
using System.Configuration;

namespace IQMediaGroup.Admin.WebApplication.IQRedlassoStations
{
    [System.Web.Script.Services.ScriptService]  
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

         [System.Web.Services.WebMethod] 
        public static string[] GetPortfolioList(string prefixText, int count)
        {
            string[] TestString = new string[10];
            List<String> _ListOfStrings = GetStaions(prefixText, count);
            for (int _Count = 0; _Count < _ListOfStrings.Count; _Count++)
            {
                TestString[_Count] = _ListOfStrings[_Count];
            }
           

            return TestString;
        }

       
     

        private static List<String> GetStaions(string key, int count)
        {
            try
            {
                List<String> _ListOfStations = new List<string>();
                SqlConnection _SqlConnection = new SqlConnection(@"server=192.168.1.103;user id=IQMediaGroup;password=IMG@123;database=IQMediaGroup");
                SqlCommand _SqlCommand = new SqlCommand("usp_RedlassoStation_SelectStation", _SqlConnection);
                _SqlCommand.CommandType = CommandType.StoredProcedure;
                //MySqlConnection _MySqlConnection = new MySqlConnection(@"server=192.168.1.103;user id=IQMediaGroup;password=IMG@123;database=IQMediaGroup");
                //MySqlCommand _MySqlCommand = new MySqlCommand("usp_RedlassoStation_SelectStation", _MySqlConnection);
                //_MySqlCommand.CommandType = CommandType.StoredProcedure;
                string sOutPramName = string.Empty;
                string _ReturnValue = string.Empty;
                _SqlCommand.Parameters.Add("@SearchText", key);
                _SqlCommand.Parameters.AddWithValue("@Count", count);
                //_MySqlCommand.Parameters.AddWithValue("@SearchText", key);
                //_MySqlCommand.Parameters.AddWithValue("@Count", count);

                SqlDataAdapter _SqlDataAdapter = new SqlDataAdapter(_SqlCommand);

               // MySqlDataAdapter _MySqlDataAdapter = new MySqlDataAdapter(_MySqlCommand);
                DataSet _DataSet = new DataSet();

                _SqlDataAdapter.Fill(_DataSet);

                foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                {
                    string stations = _DataRow[0].ToString();

                    _ListOfStations.Add(stations);
                }
                return _ListOfStations;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
