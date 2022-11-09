/*  This script is a part of the Henkel project
 *  Author: Branislav Juhás
 *  Date: 2022-11-9
 *  Last update: 2022-11-9
 *  
 *  --  File ( Interface.cs ) Description  --
 *
 *  This file contains the code to initialize and maintain the interface of the program.
 *  It also contains the basic functioanlity of some elements in the interface.
 */

using Terminal.Gui; // Terminal.Gui
using NStack; // NStack

namespace Henkel
{
    internal class Interface
    {

        // Initialize the main status bar of the interface
        private static StatusBar Status = new StatusBar(new StatusItem[] {
            new StatusItem(Key.F4 | Key.CtrlMask, "~Ctrl F4~ Exit", () => { Application.Top.Running = false; }),
            new StatusItem(Key.F2, "~F2~ Export", () => { }),
            new StatusItem(Key.F5, "~F5~ Order", () => { }),
            new StatusItem(Key.F7, "~F7~ Settings", () => { }),
        })
        { ColorScheme = Colors.TopLevel };

        // Initialize the main input field for faults of the interface
        private static TextField FaultInput = new TextField("")
        {
            X = 2,
            Y = 1,
            Width = Dim.Fill(2),
            Height = 1,
            ColorScheme = Colors.Menu
        };

        // Initialize borders for the fault input field
        private static Label FaultInputBorder = new Label("[")
        {
            X = 1,
            Y = 1,
            ColorScheme = Colors.TopLevel
        };
        private static Label FaultInputEndBorder = new Label("]")
        {
            X = Pos.Right(FaultInput),
            Y = 1,
            ColorScheme = Colors.TopLevel
        };

        // Initialize all the basic elements of the interface required from the start
        public static void Initialize()
        {
            Application.Init(); // Initialize the application
            Application.Top.Add(Status, FaultInput, FaultInputBorder, FaultInputEndBorder); // Add necessary components to the application
            Application.Run(); // Run the application
        }
    }
}