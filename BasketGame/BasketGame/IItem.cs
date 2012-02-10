// -----------------------------------------------------------------------
// <copyright file="IItem.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace BasketGame
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Media;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IItem : ICloneable
    {
        Color AssignedColor { get; set; }
        string ImageLocation { get; set; }
    }
}
