// -----------------------------------------------------------------------
// <copyright file="ItemSpawnEventArgs1.cs" company="">
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
    public class ItemSpawnEventArgs : EventArgs
    {
        public double DropOffset;
        public int FallSpeed;
        public IItem Item;
    }
}
