// -----------------------------------------------------------------------
// <copyright file="FruitThemeBuilder.cs" company="">
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
    public class FruitThemeBuilder : AbstractThemeBuilder
    {
        public override IItemFactory GetItemFactory()
        {
            return new FruitItemFactory();
        }
    }
}
