using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IQMediaGroup.Model.Interface
{
    public interface INB_GenreModel 
    {
        /// <summary>
        /// Select All Genre from the Database
        /// </summary>
        /// <returns>DataSet</returns>
        DataSet GetAllGenre();


    }
}
