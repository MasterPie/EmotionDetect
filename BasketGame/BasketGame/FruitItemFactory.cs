﻿// -----------------------------------------------------------------------
// <copyright file="FruitItemFactory.cs" company="">
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
    public class FruitItemFactory : IItemFactory
    {
        private Dictionary<Color, Item> itemMap;

        public FruitItemFactory()
        {
            itemMap = new Dictionary<Color, Item>();

            itemMap[Colors.Red] = new Item() { AssignedColor = Colors.Red };
            itemMap[Colors.Green] = new Item() { AssignedColor = Colors.Green };
            itemMap[Colors.Yellow] = new Item() { AssignedColor = Colors.Yellow };
            itemMap[Colors.Blue] = new Item() { AssignedColor = Colors.Blue };
            itemMap[Colors.Orange] = new Item() { AssignedColor = Colors.Orange };
        }

        public IItem Create(Color color)
        {
            if(itemMap.ContainsKey(color))
                return (IItem)itemMap[color].Clone();

            return (IItem)itemMap.Values.First().Clone();
        }
    }
}
