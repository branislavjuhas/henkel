/*  This script is a part of the Henkel project
 *  Author: Branislav Juhás
 *  Date: 2022-11-9
 *  Last update: 2022-11-10
 *  
 *  --  File ( Properties.cs ) Description  --
 *
 *  This file contains the code to initialize and maintain the properties dialog of the current
 *  order that is being registered. You can also find all the variables storing the data of the
 *  current ordProperties
 */

using Terminal.Gui; // Terminal.Gui
using NStack; // NStack

namespace Henkel
{
    class Properties
    {
        // Variables storing the data of the current order
        #region Variables

        // Shared order properties
        public static string OrderNumber = "";
        public static string CabinetType = "";
        public static string CabinetNumber = "";

        // Cahce orded properties
        private static string CacheOrderNumber = "";
        private static string CacheCabinetType = "";

        #endregion

        // Declares the properties dialog
        #region Dialog
        
        // Declares the close and save buttons for the dialog
        private static Button CloseButton = new Button("Close")
        {
            ColorScheme = Colors.TopLevel
        };
        private static Button SaveButton = new Button("Save", true)
        {
            ColorScheme = Colors.TopLevel
        };
        
        // Declares the dialog
        private static Dialog PropertiesDialog = new Dialog("Order Properties", 50, 12, CloseButton, SaveButton)
        {
            X = Pos.Center(),
            Y = Pos.Center(),
            ColorScheme = Colors.TopLevel
        };

        // Declares the order number input field with its label
        private static Label OrderNumberLabel = new Label("Order number:")
        {
            X = 2,
            Y = 1,
            ColorScheme = Colors.TopLevel
        };
        private static TextField OrderNumberInput = new TextField(OrderNumber)
        {
            X = Pos.Right(OrderNumberLabel) + 1,
            Y = 1,
            Width = 10,
            Height = 1,
            ColorScheme = Colors.Menu
        };

        // Declares the cabinet type input field with its label
        private static Label CabinetTypeLabel = new Label("Cabinet type:")
        {
            X = 2,
            Y = 3,
            ColorScheme = Colors.TopLevel
        };
        private static TextField CabinetTypeInput = new TextField("Unknown")
        {
            X = Pos.Right(CabinetTypeLabel) + 1,
            Y = 3,
            Width = 14,
            Height = 1,
            ColorScheme = Colors.Menu,
            Enabled = false
        };

        // Declares identification input field with its label
        private static Label IdentificationLabel = new Label("Identification:")
        {
            X = 2,
            Y = 5,
            ColorScheme = Colors.TopLevel
        };
        private static TextField IdentificationInput = new TextField("Type Dependand")
        {
            X = Pos.Right(IdentificationLabel) + 1,
            Y = 5,
            Width = 16,
            Height = 1,
            ColorScheme = Colors.Menu,
            Enabled = false
        };

        #endregion

        // Initializes the elements of the properties dialog and dialog itself
        public static void ShowDialog()
        {            
            // Set cache variables
            CacheOrderNumber = OrderNumber;
            CacheCabinetType = CabinetType;
            
            // More properties dialog elements initialization
            PropertiesDialog.Border.Effect3D = false;

            // When the order number input field is changed, the cabinet type input field is updated
            OrderNumberInput.TextChanged += (x) => 
            { 
                ExamineOrderNumber(); 
            };

            // Add the elements to the dialog
            PropertiesDialog.Add(OrderNumberLabel, OrderNumberInput, CabinetTypeLabel, CabinetTypeInput, IdentificationLabel, IdentificationInput);

            // Specifies dialog's buttons onclick actions
            CloseButton.Clicked += () =>
            {
                CloseDialog();
            };
            SaveButton.Clicked += () =>
            {
                if (CacheOrderNumber != "")
                {
                    OrderNumber = CacheOrderNumber;
                    CabinetType = CacheCabinetType;
                    CabinetNumber = IdentificationInput.Text.ToString();

                    // Show on the status bar that the order properties were saved
                    Interface.MessageStatus.Title = GetStatusMessage();
                }

                CloseDialog();
            };

            // Add the dialog into the interface and focus on the order number input field
            Application.Top.Add(PropertiesDialog);
            PropertiesDialog.SetFocus();
            OrderNumberInput.SetFocus();

            // Disable some parts of the interface
            Interface.FaultInput.Enabled = false;
            Interface.Status.Enabled = false;
        }

        // Assesses the data from order number input field and stores it into the variable 
        // and generate text for the cabinet type input field
        private static void ExamineOrderNumber()
        {
            // Chceck if firsts = 38 and if yes set the cabinet type to X Series
            // Or if firsts = 20 set the cabinet type to Netstal series
            if (OrderNumberInput.Text.Length == 8)
            {
                string firsts = OrderNumberInput.Text.ToString().Substring(0, 2);

                CacheOrderNumber = OrderNumberInput.Text.ToString();
                if (firsts == "38")
                {
                    CacheCabinetType = "X Series";
                    IdentificationInput.Text = "";
                    IdentificationInput.Enabled = true;
                }
                else if (firsts == "20")
                {
                    CacheCabinetType = "Netstal Series";
                    IdentificationInput.Text = "Variable";
                }
                else
                {
                    CacheCabinetType = "Unknown";
                    IdentificationInput.Text = "Type Dependand";
                    IdentificationInput.Enabled = false;
                }
            }
            else
            {
                CacheCabinetType = "Unknown";
                IdentificationInput.Text = "Type Dependand";
                IdentificationInput.Enabled = false;
            }

            CabinetTypeInput.Text = CacheCabinetType;
        }

        // Close the properties dialog
        private static void CloseDialog()
        {
            // Set cache variables to 0
            CacheOrderNumber = "";
            CacheCabinetType = "";

            // Close the dialog
            Application.Top.Remove(PropertiesDialog);
            Interface.FaultInput.Enabled = true;
            Interface.Status.Enabled = true;
            Interface.FaultInput.SetFocus();
        }

        // Function that retuns the string for the status bar message
        public static string GetStatusMessage()
        {
            // If the order number is not empty, return the order number
            // Else return the default message
            if (OrderNumber != "")
            { 
                // If the cabinet type is not empty, return the cabinet type and if it is 'X Series' return the cabinet number
                // Else return just the order number
                if (CabinetType != "")
                {
                    if (CabinetType == "X Series")
                    {
                        return $"Order: {OrderNumber} | {CabinetType} | {CabinetNumber}";
                    }
                    else
                    {
                        return $"Order: {OrderNumber} | {CabinetType}";
                    }
                }
                else
                {
                    return $"Order: {OrderNumber}";
                }
            }
            else
            {
                return "Order: Unknown";
            }
        }
    }
}
