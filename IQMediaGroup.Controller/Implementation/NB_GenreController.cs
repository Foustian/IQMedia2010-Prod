using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Model.Factory;



namespace IQMediaGroup.Controller.Implementation
{
    internal class NB_GenreController : INB_GenreController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly INB_GenreModel _INB_GenreModel;

        public NB_GenreController()
        {
            _INB_GenreModel = _ModelFactory.CreateObject<INB_GenreModel>();
        }

        public List<NB_Genre> GetAllGenre()
        {
            try
            {
                DataSet _DataSet = null;

                _DataSet = _INB_GenreModel.GetAllGenre();

                List<NB_Genre> _ListOfNB_Genre = FillNB_GenreInfo(_DataSet);

                return _ListOfNB_Genre;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private List<NB_Genre> FillNB_GenreInfo(DataSet _DataSet)
        {
            List<NB_Genre> _ListOfNB_Genre = new List<NB_Genre>();
            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {

                        NB_Genre _NB_Genre = new NB_Genre();
                        if (_DataSet.Tables[0].Columns.Contains("ID") && _DataRow["ID"] != null)
                            _NB_Genre.ID = Convert.ToInt32(_DataRow["ID"]);

                        if (_DataSet.Tables[0].Columns.Contains("Name") && _DataRow["Name"] != null)
                            _NB_Genre.Name = Convert.ToString(_DataRow["Name"]);

                        if (_DataSet.Tables[0].Columns.Contains("Label") && _DataRow["Label"] != null)
                            _NB_Genre.Label = Convert.ToString(_DataRow["Label"]);

                        if (_DataSet.Tables[0].Columns.Contains("IsActive") && _DataRow["IsActive"] != null)
                            _NB_Genre.IsActive = Convert.ToBoolean(_DataRow["IsActive"]);

                        _ListOfNB_Genre.Add(_NB_Genre);
                    }
                }
                return _ListOfNB_Genre;
            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}
