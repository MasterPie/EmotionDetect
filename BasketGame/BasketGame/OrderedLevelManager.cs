// -----------------------------------------------------------------------
// <copyright file="OrderedLevelManager.cs" company="">
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
    public class OrderedLevelManager : ILevelManager
    {
        private List<ILevel> levels;
        private int i = 0;

        public void Reset()
        {
            i = -1;
        }

        public ILevel Prev()
        {
            if (levels == null)
                throw new Exception("No levels have been loaded.");

            if (i <= 0)
                return levels[0];

            return levels[--i];
        }

        public ILevel Next()
        {
            if (levels == null)
                throw new Exception("No levels have been loaded.");

            if (i < (levels.Count - 1))
                return levels[++i];

            return levels[i];
        }

        public void LoadLevels(List<ILevel> inLevels)
        {
            this.levels = inLevels;
            this.levels = levels.OrderBy(x => x.ID).ToList<ILevel>();
        }
    }
}
