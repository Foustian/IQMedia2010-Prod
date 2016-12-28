using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using System.Data;

namespace IQMediaGroup.Controller.Implementation
{
    internal class IQCoreClipMetaController : IIQCoreClipMetaController
    {
         private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        private readonly IIQCoreClipMetaModel _IIQCoreClipMetaModel;
        //List<Clip> _ListOfTempClip = new List<Clip>();

        public IQCoreClipMetaController()
        {
            _IIQCoreClipMetaModel = _ModelFactory.CreateObject<IIQCoreClipMetaModel>();
        }

        /// <summary>
        /// This method get the OutPutPAth of Clip By ClipGUID
        /// </summary>
        /// <param name="ClipGUID"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetClipPathByClipGUID(Guid ClipGUID)
        {
            try
            {
                Dictionary<string, string> p_OutputPath = new Dictionary<string,string>();
                //string _OutPutPath = string.Empty;
                //string _OutPutFtpPath = string.Empty;
                DataSet _DataSet;
                _DataSet = _IIQCoreClipMetaModel.GetClipPathByClipGUID(ClipGUID);
                if (_DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    p_OutputPath.Add("FilePath", Convert.ToString(_DataSet.Tables[0].Rows[0]["FilePath"]));
                    p_OutputPath.Add("FTPFileLocation", Convert.ToString(_DataSet.Tables[0].Rows[0]["FTPFileLocation"]));
                    //_OutPutPath = Convert.ToString(_DataSet.Tables[0].Rows[0]["FilePath"]);
                    //_OutPutFtpPath = Convert.ToString(_DataSet.Tables[0].Rows[0]["FTPFileLocation"]);
                }
                return p_OutputPath;                
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

        }

        /// <summary>
        /// This method get the OutPutPAth of Clip By ClipGUID
        /// </summary>
        /// <param name="ClipGUID"></param>
        /// <returns></returns>
        public string UpdateDownloadCountByClipGUID(Guid ClipGUID)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _IIQCoreClipMetaModel.UpdateDownloadCountByClipGUID(ClipGUID);

                return _Result;

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

        }
    }
}
