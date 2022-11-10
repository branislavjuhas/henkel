/*  This script is a part of the Henkel project
 *  Author: Branislav Juhás
 *  Date: 2022-11-9
 *  Last update: 2022-11-10
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
        #region Variables

        private static bool NetstalCabinetVisible = false;
        
        // Initializes the message status item to display the status of the program
        public static StatusItem MessageStatus = new StatusItem(Key.Null, "Ordern: Unknown", null);

        // Initialize the main status bar of the interface
        public static StatusBar Status = new StatusBar(new StatusItem[] {
            new StatusItem(Key.F4 | Key.CtrlMask, "~Ctrl F4~ Exit", () => { Application.Top.Running = false; }),
            new StatusItem(Key.F2, "~F2~ Export", () => { }),
            new StatusItem(Key.F5, "~F5~ Order", () => { Properties.ShowDialog(); }),
            new StatusItem(Key.F7, "~F7~ Settings", () => { }),
            MessageStatus
        })
        { ColorScheme = Colors.TopLevel };

        // Initialize the main input field for faults of the interface with its subinput fields
        public static TextField FaultInput = new TextField("")
        {
            X = 2,
            Y = 1,
            Width = Dim.Fill(2),
            Height = 1,
            ColorScheme = Colors.Menu
        };

        // Declare the subinput field for the netstal cabinet
        public static TextField NetstalCabinetInput = new TextField("")
        {
            X = 2,
            Y = 1,
            Width = 4,
            Height = 1,
            ColorScheme = Colors.Menu,
            Enabled = false
        };

        // Declare border for the netstal cabinet subinput field
        public static Label NetstalBorder = new Label("|")
        {
            X = Pos.Right(NetstalCabinetInput),
            Y = 1,
            Width = 1,
            ColorScheme = Colors.TopLevel,
            Visible = false
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

        #endregion

        // Initialize all the basic elements of the interface required from the start
        public static void Initialize()
        {
            Application.Init(); // Initialize the application
            Application.Top.Add(Status, FaultInput, FaultInputBorder, FaultInputEndBorder, NetstalCabinetInput, NetstalBorder); // Add necessary components to the application
            
            InitializeComponents();
            
            Application.Run(); // Run the application
        }

        // Initialize components of the interface that cannot be initialized in the header
        public static void InitializeComponents()
        {
            NetstalCabinetInput.Visible = false;
            //Event handler for the fault input field
            FaultInput.KeyPress += (e) =>
            {
                // Switch case for the key pressed
                switch (e.KeyEvent.Key)
                {
                    case Key.Space:

                        // If the netstal input field is not visible and the first character of the fault input field is a letter and the second is number
                        // then make the netstal input field visible, add first two characters of the fault input field to the netstal input field and remove them from the fault input field
                        // and move the cursor to the beggining of the fault input field and resize fault input to fit the netstal input field
                        if (!NetstalCabinetVisible && FaultInput.Text.Length > 1 && char.IsLetter(FaultInput.Text.ToString()[0]) && char.IsNumber(FaultInput.Text.ToString()[1]))
                        {
                            NetstalCabinetVisible = true;
                            NetstalCabinetInput.Visible = true;
                            NetstalCabinetInput.Text = $" {FaultInput.Text.ToString().Substring(0, 2).ToUpper()}";
                            FaultInput.Text = FaultInput.Text.ToString().Substring(2);
                            FaultInput.CursorPosition = 0;
                            FaultInput.X = Pos.Right(NetstalBorder) + 1;
                            FaultInput.Width = Dim.Fill(2);

                            NetstalBorder.Visible = true;
                            
                            e.Handled = true;
                        }
                        break;
                    case Key.Backspace:

                        // If the netstal field is visible and the cursor is at the beggining of the fault input field
                        // then make the netstal input field and its border invisible, resize the fault input field to fit
                        // the whole interface and move the cursor to the beggining of the fault input field
                        if (NetstalCabinetVisible && FaultInput.CursorPosition == 0)
                        {
                            NetstalCabinetVisible = false;
                            NetstalCabinetInput.Visible = false;
                            NetstalCabinetInput.Text = "";
                            FaultInput.X = 2;
                            FaultInput.Width = Dim.Fill(2);
                            NetstalBorder.Visible = false;
                            
                            e.Handled = true;
                        }
                        break;
                }

            };
        }

        // This void is used for updating the fault input field during runtime
        // Other subinputs can be added here
        public static void UpdateFaultInput()
        {
            
        }
    }
}
