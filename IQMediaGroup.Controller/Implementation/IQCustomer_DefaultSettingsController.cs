using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using System.Xml.Linq;
using System.Data;

namespace IQMediaGroup.Controller.Implementation
{
    internal class IQCustomer_DefaultSettingsController : IIQCustomer_DefaultSettingsController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IIQCustomer_DefaultSettingsModel _IIQCustomer_DefaultSettingsModel;

        public IQCustomer_DefaultSettingsController()
        {
            _IIQCustomer_DefaultSettingsModel = _ModelFactory.CreateObject<IIQCustomer_DefaultSettingsModel>();
        }
    }
}
