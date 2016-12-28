using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Controller.Interface
{
    public interface INB_GenreController
    {
        /// <summary>
        /// Select All Genre from the Database
        /// </summary>
        /// <returns>returns list of Genre</returns>
        List<NB_Genre> GetAllGenre();
    }
}
