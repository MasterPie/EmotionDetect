// -----------------------------------------------------------------------
// <copyright file="ILevelManager.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace BasketGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ILevelManager
    {
        void LoadLevels(List<ILevel> levels);
        void Reset();
        ILevel Prev();
        /// <summary>
        /// Returns the next level or the first level if there is no previous level
        /// </summary>
        /// <returns>next or first level</returns>
        ILevel Next();
    }
}
