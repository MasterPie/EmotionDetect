// -----------------------------------------------------------------------
// <copyright file="Item.cs" company="">
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
    public class Item : IItem
    {
        private Color color = Colors.Red;
        public Color AssignedColor
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        private string imageLocation = "";
        public string ImageLocation
        {
            get
            {
                return imageLocation;
            }
            set
            {
                imageLocation = value;
            }
        }

        public object Clone()
        {
            return new Item() { AssignedColor = color, ImageLocation = imageLocation };
        }

        private double dropSpeed = 5;
        public double DropSpeed
        {
            get
            {
                return dropSpeed;
            }
            set
            {
                dropSpeed = value;
            }
        }
    }
}
