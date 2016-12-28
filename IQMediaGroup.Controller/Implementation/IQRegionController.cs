using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Controller.Factory;
using System.Data;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Controller.Interface;

namespace IQMediaGroup.Controller.Implementation
{
    internal class IQRegionController : IIQRegionController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        private readonly IIQRegionModel _IIQRegionModel;

        public IQRegionController()
        {
            _IIQRegionModel = _ModelFactory.CreateObject<IIQRegionModel>();
        }

        public DataSet GetAllRegion()
        {
            try
            {               
                DataSet _DataSet;
                _DataSet = _IIQRegionModel.GetAllRegion();

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

        }

    }
}
