// -----------------------------------------------------------------------
// <copyright file="BasicLevel.cs" company="">
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
    public class BasicLevel : ILevel
    {
        private int id = -1;
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        //private Color[] itemVariety = null;
        //public Color[] ItemVariety
        //{
        //    get
        //    {
        //        return itemVariety;
        //    }
        //    set
        //    {
        //        itemVariety = value;
        //    }
        //}

        private int speed = 1;
        public int Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
            }
        }

        private int locRandomness = 10;
        public int LocationRandomness
        {
            get
            {
                return locRandomness;
            }
            set
            {
                locRandomness = value;
            }
        }

        private int varietyRandomness = 5;
        public int VarietyRandomness
        {
            get
            {
                return varietyRandomness;
            }
            set
            {
                varietyRandomness = value;
            }
        }
    }
}
