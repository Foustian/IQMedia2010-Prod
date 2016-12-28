using System;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;

namespace IQMediaGroup.Controller.Implementation
{
    public class UGC_Upload_LogController:IUGC_Upload_LogController
    {
        ModelFactory _ModelFactory = new ModelFactory();
        IUGC_Upload_LogModel _IUGC_Upload_LogModel = null;

        public UGC_Upload_LogController()
        {
            _IUGC_Upload_LogModel = _ModelFactory.CreateObject<IUGC_Upload_LogModel>();
        }

        public string Insert(UGC_Upload_Log p_UGC_Upload_Log)
        {
            try
            {
                string _Restult = _IUGC_Upload_LogModel.Insert(p_UGC_Upload_Log);

                return _Restult;
            }
            catch (Exception _Exception) 
            {
                throw _Exception;
            }
        }
    }
}