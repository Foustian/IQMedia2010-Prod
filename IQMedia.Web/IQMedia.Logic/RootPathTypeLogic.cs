﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Domain;

namespace IQMedia.Logic
{
    public class RootPathTypeLogic : BaseLogic, ILogic
    {
        /// <summary>
        /// Gets the RootPathType by its name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The RootPathType.</returns>
        public RootPathType GetRootPathTypeByName(string name)
        {
            return Context.RootPathTypes.FirstOrDefault(rpt => rpt.Name == name);
        }
    }
}
