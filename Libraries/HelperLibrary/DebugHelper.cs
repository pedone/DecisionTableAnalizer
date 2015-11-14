#region Using directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
#endregion

namespace HelperLibrary
{
    public static class DebugHelper
    {

        #region Execute If Debug
        /// <summary>
        /// Execute the action only in debug mode.
        /// Exceptions are caught and ignored.
        /// </summary>
        [Conditional("DEBUG")]
        public static void ExecuteIfDebug(Action exe)
        {
            try
            {
                if (exe != null)
                    exe();
            }
            catch
            { }
        }
        #endregion

    }
}
