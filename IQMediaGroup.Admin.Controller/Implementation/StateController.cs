using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Admin.Core.HelperClasses;
using System.Data;
using IQMediaGroup.Admin.Model.Factory;
using IQMediaGroup.Admin.Model.Interface;
namespace IQMediaGroup.Admin.Controller.Implementation
{
    internal class StateController : IStateController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IStateModel _IStateModel;

        public StateController()
        {
            _IStateModel = _ModelFactory.CreateObject<IStateModel>();
        }

        /// <summary>
        /// This method gets State Information
        /// Added By:Bhavik Barot   
        /// </summary>
        /// <param name="p_IsActive">Status of the State</param>
        /// <returns>List of object of State Class</returns>
        public List<State> GetStateInformation()
        {
            try
            {
                List<State> _ListOfState = null;

                DataSet _DataSet = _IStateModel.GetStateInfo();

                _ListOfState = FillListOfState(_DataSet);

                return _ListOfState;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method fills List of object State from DataSet
        /// Added By:Bhavik Barot 
        /// </summary>
        /// <param name="p_DataSet">DataSet contains StateInformation</param>
        /// <returns>List of Object of class States</returns>
        private List<State> FillListOfState(DataSet p_DataSet)
        {
            try
            {
                List<State> _ListOfState = new List<State>();

                if (p_DataSet != null && p_DataSet.Tables.Count > 0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        State _State = new State();

                        _State.StateKey = Convert.ToInt32(_DataRow["StateKey"]);
                        _State.StateName = _DataRow["StateName"].ToString();

                        _ListOfState.Add(_State);
                    }
                }

                return _ListOfState;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        
    }
}
