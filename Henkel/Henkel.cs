/*  Henkel Software
 *  Author: Branislav Juhás
 *  Date: 2022-11-9
 *  Description: Software for registration of faults for the KraussMaffei Corporation
 *  Version: 1.0
 *  Last update: 2022-11-10
 *
 *  This version of program requires .NET 6.0 or higher and utilizes Terminal.Gui 1.8.2, NStack.Core 1.0.5 & the MNCA engine 1.0.2 included in the project
 *  
 *  --  File ( Henkel.cs ) Description  --
 *
 * This file contains the code to initialize application.
 */

namespace Henkel
{
    class Program
    {
        #region variables

        public static string Version = "1.0";

        #endregion

        // Initialize the application
        static void Main(string[] args)
        {
            Interface.Initialize();
        }
    }
}