using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;
using System.Configuration;
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Admin.Model.Factory;
using IQMediaGroup.Admin.Model.Interface;

namespace IQMediaGroup.Admin.Controller.Implementation
{
    internal class PromotionalVideoController : IPromotionalVideoController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IPromotionalVideoModel _IPromotionalVideoModel;

        public PromotionalVideoController()
        {
            _IPromotionalVideoModel = _ModelFactory.CreateObject<IPromotionalVideoModel>();
        }

        /// <summary>
        /// Description: This method Gets the Promotional Video Information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_VideoID">VideoID</param>
        /// <returns>Video Information for the VideoID</returns>
        public PromotionalVideo GetPromotionalVideoByVideoID()
        {
            PromotionalVideo _PromotionalVideo = new PromotionalVideo();
            DataSet _DataSet = new DataSet();
            try
            {
                _DataSet = _IPromotionalVideoModel.GetPromotionalVideoByVideoID();
                _PromotionalVideo = FillPromotionalVideoInformation(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _PromotionalVideo;
        }

        /// <summary>
        /// Description: This method Gets the Promotional Video Information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_VideoID">VideoID</param>
        /// <returns>Video Information for the VideoID</returns>
        public List<PromotionalVideo> GetPromotionalVideoByPageName(string p_PageName)
        {
            List<PromotionalVideo> _ListOfPromotionalVideoInformation = null;
            DataSet _DataSet = new DataSet();
            try
            {
                _DataSet = _IPromotionalVideoModel.GetPromotionalVideoByPageName(p_PageName);
                _ListOfPromotionalVideoInformation = FillPromotionalVideoInformationByPageName(_DataSet);
                return _ListOfPromotionalVideoInformation;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description: This method fills the Promotional Video Information.
        /// Added By: Bhavik Barot
        /// </summary>
        private PromotionalVideo FillPromotionalVideoInformation(DataSet _DataSet)
        {
            PromotionalVideo _PromotionalVideo = new PromotionalVideo();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        
                        _PromotionalVideo.FilePath = Convert.ToString(_DataRow["FilePath"]);
                        _PromotionalVideo.DisplayPageName = Convert.ToString(_DataRow["DisplayPageName"]);
                        _PromotionalVideo.IsDisplay = Convert.ToBoolean(_DataRow["IsDisplay"]);
                        _PromotionalVideo.IsActive = Convert.ToBoolean(_DataRow["IsActive"]);
                        _PromotionalVideo.SrcPath = Convert.ToString(_DataRow["SrcPath"]);
                        _PromotionalVideo.MoviePath = Convert.ToString(_DataRow["MoviePath"]);
                        
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _PromotionalVideo;
        }

        /// <summary>
        /// Description: This method fills the Promotional Video Information.
        /// Added By: Bhavik Barot
        /// </summary>
        private List<PromotionalVideo> FillPromotionalVideoInformationByPageName(DataSet _DataSet)
        {
           
            List<PromotionalVideo> _ListOfPromotionalVideoInformation = new List<PromotionalVideo>();
            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        PromotionalVideo _PromotionalVideo = new PromotionalVideo();
                        _PromotionalVideo.FilePath = Convert.ToString(_DataRow["FilePath"]);
                        _PromotionalVideo.DisplayPageName = Convert.ToString(_DataRow["DisplayPageName"]);
                        _PromotionalVideo.IsDisplay = Convert.ToBoolean(_DataRow["IsDisplay"]);
                        _PromotionalVideo.IsActive = Convert.ToBoolean(_DataRow["IsActive"]);
                        _PromotionalVideo.SrcPath = Convert.ToString(_DataRow["SrcPath"]);
                        _PromotionalVideo.MoviePath = Convert.ToString(_DataRow["MoviePath"]);
                        _PromotionalVideo.Position = Convert.ToString(_DataRow["Position"]);

                        _ListOfPromotionalVideoInformation.Add(_PromotionalVideo);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfPromotionalVideoInformation;
        }
    }
}
