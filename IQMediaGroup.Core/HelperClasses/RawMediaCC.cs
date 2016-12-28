using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    // this class is used to Fill TermOfOccurance object.
    //So this class needs to be same as TermOfOccurance class in PMG Search Search Module.

    [Serializable]
    public class RawMediaCC
    {

        int _timeOffset = 0;
        string _surroundingText = null;
        string _term;

        public int TimeOffset
        {
            set { _timeOffset = value; }
            get { return _timeOffset; }
        } 

        public string SurroundingText
        {
            set { _surroundingText = value; }
            get { return _surroundingText; }
        } 

        public string SearchTerm
        {
            set { _term = value; }
            get { return _term; }
        }
    }
}
