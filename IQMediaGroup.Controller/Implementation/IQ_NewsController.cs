using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using System.Data;
using PMGSearch;
using System.Configuration;
using IQMediaGroup.Core.Enumeration;
using System.Web;

namespace IQMediaGroup.Controller.Implementation
{
    public class IQ_NewsController : IIQ_NewsController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IIQ_NewsModel _IIQ_NewsModel;

        public IQ_NewsController()
        {
            _IIQ_NewsModel = _ModelFactory.CreateObject<IIQ_NewsModel>();
        }

        public List<IQ_News> GetIQNews()
        {
            try
            {
                DataSet _Result;
                _Result = _IIQ_NewsModel.GetIQNews();
                List<IQ_News> _ListOfIQ_News = FillIQ_NewsInformation(_Result);

                return _ListOfIQ_News;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<IQ_News> FillIQ_NewsInformation(DataSet _DataSet)
        {
            try
            {
                List<IQ_News> _ListOfIQ_News = new List<IQ_News>();

                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable _DataTable = _DataSet.Tables[0];

                    foreach (DataRow _DataRow in _DataTable.Rows)
                    {
                        IQ_News _IQ_News = new IQ_News();

                        if (_DataTable.Columns.Contains("Headline") && !_DataRow["Headline"].Equals(DBNull.Value))
                        {
                            _IQ_News.Headline = Convert.ToString(_DataRow["Headline"]);
                        }

                        if (_DataTable.Columns.Contains("ReleaseDate") && !_DataRow["ReleaseDate"].Equals(DBNull.Value))
                        {
                            _IQ_News.ReleaseDate = Convert.ToDateTime(_DataRow["ReleaseDate"]);
                        }

                        if (_DataTable.Columns.Contains("Detail") && !_DataRow["Detail"].Equals(DBNull.Value))
                        {
                            _IQ_News.Detail = Convert.ToString(_DataRow["Detail"]);
                        }

                        if (_DataTable.Columns.Contains("Url") && !_DataRow["Url"].Equals(DBNull.Value))
                        {
                            _IQ_News.Url = Convert.ToString(_DataRow["Url"]);
                        }
                        if (_DataTable.Columns.Contains("SubHead") && !_DataRow["SubHead"].Equals(DBNull.Value))
                        {
                            _IQ_News.SubHead = Convert.ToString(_DataRow["SubHead"]);
                        } 


                        _ListOfIQ_News.Add(_IQ_News);

                    }
                }

                return _ListOfIQ_News;

            }
            catch (Exception)
            {
                throw;
            }
        }

    }

}