/*  This script is a part of the Henkel project
 *  Author: Branislav Juhás
 *  Date: 2022-11-9
 *  Last update: 2022-12-02
 *  
 *  --  File ( Interface.cs ) Description  --
 *
 *  This file contains the code to initialize and maintain the interface of the program.
 *  It also contains the basic functioanlity of some elements in the interface.
 */

using Terminal.Gui; // Terminal.Gui

namespace Henkel
{
    internal class Interface
    {
        #region Variables

        //Variable that globally indicates if the key is handled
        public static bool KeyHandled = false;

        public static bool NetstalCabinetVisible = false;
        public static bool ClassificationBasicsVisible = false;

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

        // Declare the subinput field for the basic classification
        public static TextField ClassificationInput = new TextField("")
        {
            X = 2,
            Y = 1,
            Height = 1,
            ColorScheme = Colors.Menu,
            Visible = false
        };

        // Declare border for the basic classification
        public static Label ClassifyBorder = new Label("|")
        {
            X = Pos.Right(FaultInput),
            Y = 1,
            Width = 1,
            ColorScheme = Colors.TopLevel,
            Visible = false
        };

        // Declare the subinput field for the netstal cabinet
        public static TextField NetstalCabinetInput = new TextField("")
        {
            X = 2,
            Y = 1,
            Width = 4,
            Height = 1,
            ColorScheme = Colors.Menu,
            Enabled = false,
            Visible = false
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
        public static Label FaultInputBorder = new Label("[")
        {
            X = 1,
            Y = 1,
            ColorScheme = Colors.TopLevel
        };
        public static Label FaultInputEndBorder = new Label("]")
        {
            X = Pos.Right(FaultInput),
            Y = 1,
            Width = 1,
            ColorScheme = Colors.TopLevel
        };

        // Initialize the scrollview for the faults that are curently being processed
        public static FrameView FaultsFrame = new FrameView()
        {
            X = 1,
            Y = 3,
            Width = Dim.Fill(1),
            Height = Dim.Fill(2),
            ColorScheme = Colors.TopLevel
        };

        // Initialize the label that displays number of processed and pending faults
        public static Label ProcessedFaults = new Label("Processing Faults: 0  |  Pending: 0")
        {
            X = 1,
            Y = 3,
            Width = Dim.Fill(1),
            Height = 1,
            ColorScheme = Colors.TopLevel
        };

        #region Pending

        // Initialize the input field for the pending fault input
        public static TextField PendingFaultInput = new TextField("")
        {
            X = 3,
            Y = 5,
            Width = Dim.Fill(2),
            Height = 1,
            ColorScheme = Colors.Menu,
            Visible = false
        };

        // Initialize the input field for the pending BMK input
        public static TextField PendingBMKInput = new TextField("")
        {
            X = 3,
            Y = 7,
            Width = 25,
            Height = 1,
            ColorScheme = Colors.Menu,
            Visible = false
        };

        // Initialize the input field for the pending placement input
        public static TextField PendingPlacementInput = new TextField("")
        {
            X = Pos.Right(PendingBMKInput) + 2,
            Y = 7,
            Width = 25,
            Height = 1,
            ColorScheme = Colors.Menu,
            Visible = false
        };

        // Initialize the input field for the pending order number input
        public static TextField PendingOrderNumberInput = new TextField("")
        {
            X = Pos.Right(PendingPlacementInput) + 2,
            Y = 7,
            Width = 25,
            Height = 1,
            ColorScheme = Colors.Menu,
            Visible = false
        };

        // Initialize the input dropdown for the pending cause input
        public static ComboBox PendingCauseInput = new ComboBox(Classification.Causes)
        {
            X = 3,
            Y = 9,
            Width = 25,
            Height = 20,
            ColorScheme = Colors.Menu,
            Visible = false
        };

        // Initialize the input dropdown for the pending classification input
        public static ComboBox PendingClassificationInput = new ComboBox()
        {
            X = Pos.Right(PendingCauseInput) + 2,
            Y = 9,
            Width = 25,
            Height = 20,
            ColorScheme = Colors.Menu,
            Visible = false
        };

        // Initialize the input dropdown for the pending type input
        public static ComboBox PendingTypeInput = new ComboBox()
        {
            X = Pos.Right(PendingClassificationInput) + 2,
            Y = 9,
            Width = 25,
            Height = 20,
            ColorScheme = Colors.Menu,
            Visible = false
        };

        // Initialize the input button for the aproving pending input
        public static Button PendingApproveButton = new Button("Approve")
        {
            X = Pos.Right(PendingTypeInput) - 11,
            Y = 11,
            Height = 1,
            ColorScheme = Colors.TopLevel,
            Visible = false
        };

        #endregion

        #endregion

        // Initialize all the basic elements of the interface required from the start
        public static void Initialize()
        {
            Application.Init(); // Initialize the application

            // Add necessary components to the application
            Application.Top.Add(Status, FaultInput, FaultInputBorder, FaultInputEndBorder, NetstalCabinetInput,
            NetstalBorder, ClassificationInput, ClassifyBorder, ProcessedFaults, PendingFaultInput, PendingBMKInput,
            PendingPlacementInput, PendingOrderNumberInput, PendingCauseInput, PendingClassificationInput, PendingTypeInput,
            PendingApproveButton);

            InitializeComponents();

            Application.Run(); // Run the application
        }

        // Initialize components of the interface that cannot be initialized in the header
        public static void InitializeComponents()
        {

            FaultsFrame.Border.BorderStyle = BorderStyle.None; // Remove the border of the faults frame

            PendingApproveButton.Clicked += () => { Processing.Approve(); }; // Add the event handler for the approve button

            // Event hanndler for PendingCauseInput dropdown on down arrow key press
            PendingCauseInput.KeyPress += (e) =>
            {
                if (e.KeyEvent.Key == Key.CursorDown && !PendingCauseInput.IsShow) { PendingCauseInput.Expand(); e.Handled = true; }
            };

            // Event handler for PendingClassificationInput dropdown on down arrow key press
            PendingClassificationInput.KeyPress += (e) =>
            {
                if (e.KeyEvent.Key == Key.CursorDown && !PendingClassificationInput.IsShow) { PendingClassificationInput.Expand(); e.Handled = true; }
            };

            // Event handler for PendingTypeInput dropdown on down arrow key press
            PendingTypeInput.KeyPress += (e) =>
            {
                if (e.KeyEvent.Key == Key.CursorDown && !PendingTypeInput.IsShow) { PendingTypeInput.Expand(); e.Handled = true; }
            };

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
                        if (Properties.CabinetType == "Netstal Series" && !NetstalCabinetVisible && FaultInput.Text.Length > 1 && char.IsLetter(FaultInput.Text.ToString()[0]) && char.IsNumber(FaultInput.Text.ToString()[1]))
                        {
                            NetstalCabinetVisible = true;
                            NetstalCabinetInput.Visible = true;
                            NetstalCabinetInput.Text = $" {FaultInput.Text.ToString().Substring(0, 2).ToUpper()}";
                            FaultInput.Text = FaultInput.Text.ToString().Substring(2);
                            FaultInput.CursorPosition = 0;
                            FaultInput.X = Pos.Right(NetstalBorder) + 1;

                            if (!ClassificationBasicsVisible)
                            {
                                FaultInput.Width = Dim.Fill(2);
                            }

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
                            NetstalBorder.Visible = false;

                            if (!ClassificationBasicsVisible)
                            {
                                FaultInput.Width = Dim.Fill(2);
                            }

                            e.Handled = true;
                        }
                        else if (FaultInput.CursorPosition > 0 && ClassificationBasicsVisible)
                        {
                            FaultInput.Width = FaultInput.Text.Length - FaultInput.SelectedLength + 1;
                            ClassificationInput.Width = Dim.Fill(2);
                        }
                        break;

                    case (Key)59: //Key.Semicolon

                        // If the classification input field is not visible then make it visible, move cursor to the beggining of the classification input field,
                        // resize the fault input to fit its text and resize the classification input to fit the whole remaining space and focus it
                        if (!ClassificationBasicsVisible)
                        {
                            ClassificationBasicsVisible = true;
                            ClassificationInput.Visible = true;
                            ClassifyBorder.Visible = true;
                            ClassificationInput.Text = FaultInput.Text.ToString().Substring(FaultInput.CursorPosition);
                            ClassificationInput.CursorPosition = 0;
                            FaultInput.Width = FaultInput.Text.Length;
                            ClassificationInput.X = Pos.Right(FaultInput) + 2;
                            FaultInputEndBorder.X = Pos.Right(ClassificationInput);
                            ClassificationInput.Width = Dim.Fill(2);
                            ClassificationInput.SetFocus();

                            e.Handled = true;
                        }

                        break;

                    case Key.CursorRight:
                        // If the cursor is at the end of the fault input field and the classification input field is visible
                        // then move the cursor to the beggining of the classification input field
                        if (FaultInput.CursorPosition == FaultInput.Text.Length && ClassificationBasicsVisible)
                        {
                            ClassificationInput.SetFocus();
                            ClassificationInput.CursorPosition = 0;
                            e.Handled = true;
                        }
                        break;
                }

                // If the e key is a letter, number, space or symbol and the classification input field is visible
                // then resize the fault input field to fit its text and resize the classification input field to fit the whole remaining space

                if (e.KeyEvent.KeyValue < 1000 && !KeyHandled)
                {
                    char c = char.ConvertFromUtf32(e.KeyEvent.KeyValue)[0];

                    if (ClassificationBasicsVisible)
                    {
                        FaultInput.Width = FaultInput.Text.Length + 2;
                        ClassificationInput.Width = Dim.Fill(2);
                    }
                }
                KeyHandled = false;
            };

            FaultInput.KeyUp += (e) =>
            {
                if (e.KeyEvent.Key == Key.Enter)
                {
                    Processing.Process();
                    e.Handled = true;
                    KeyHandled = true;
                }
            };

            ClassificationInput.KeyPress += (e) =>
            {
                switch (e.KeyEvent.Key)
                {
                    case Key.Backspace:

                        // Else if the classification input field is visible and the cursor is at the beggining of the classification input field
                        // then make the classification input field and its border invisible, resize the fault input field to fit all the interface
                        // and move the cursor to the end of the fault input field
                        if (ClassificationBasicsVisible && ClassificationInput.CursorPosition == 0)
                        {
                            ClassificationBasicsVisible = false;
                            ClassificationInput.Visible = false;
                            ClassifyBorder.Visible = false;
                            FaultInput.Width = Dim.Fill(2);
                            FaultInput.Text += ClassificationInput.Text;
                            ClassificationInput.Text = "";
                            FaultInput.CursorPosition = FaultInput.Text.Length;
                            FaultInputEndBorder.X = Pos.Right(FaultInput);
                            FaultInput.SetFocus();

                            e.Handled = true;
                        }
                        break;

                    case Key.CursorLeft:
                        // If the cursor is at the beggining of the classification input field and the fault input field is visible
                        // then move the cursor to the end of the fault input field
                        if (ClassificationInput.CursorPosition == 0 && FaultInput.Visible)
                        {
                            FaultInput.SetFocus();
                            FaultInput.CursorPosition = FaultInput.Text.Length;
                            e.Handled = true;
                        }
                        break;
                }
            };

            ClassificationInput.KeyUp += (e) =>
            {
                if (e.KeyEvent.Key == Key.Enter)
                {
                    Processing.Process();
                    e.Handled = true;
                }
            };

            // Handler for when the user changes input in the pending cause item input field
            PendingCauseInput.Leave += (e) =>
            {
                if (PendingCauseInput.SelectedItem >= 0) { PendingClassificationInput.SetSource(Classification.Classifications[PendingCauseInput.SelectedItem]); }
            };

            // Handler for when the user changes input in the pending classification item input field
            PendingClassificationInput.Leave += (e) =>
            {
                if (PendingClassificationInput.SelectedItem >= 0 ) { PendingTypeInput.SetSource(Classification.Types[Classification.ClassificationsPointers[PendingCauseInput.SelectedItem][PendingClassificationInput.SelectedItem]]); }
            };
        }
    }
}
