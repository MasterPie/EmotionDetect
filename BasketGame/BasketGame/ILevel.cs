// -----------------------------------------------------------------------
// <copyright file="ILevel.cs" company="">
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
    public interface ILevel
    {
        int ID { get; set; }
        int Speed {get;set;}
        int LocationRandomness { get; set; }
        int VarietyRandomness { get; set; }
    }
}
