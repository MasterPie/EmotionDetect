// -----------------------------------------------------------------------
// <copyright file="ILogger.cs" company="">
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
    public interface ILogger
    {
        void Start(ILoggable observable);
        void Stop();
    }
}
