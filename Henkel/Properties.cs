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

        public static string OrderNumber = "";
        public static string CabinetType = "";
        public static string CabinetNumber = "";

        #endregion

        // Declares the properties dialog
        #region Dialog

        // Declares the dialog
        private static Dialog PropertiesDialog = new Dialog("Properties", 50, 15)
        {
            X = Pos.Center(),
            Y = Pos.Center(),
            ColorScheme = Colors.TopLevel
        };

        // Declares the order number input field with its label
        private static Label OrderNumberLabel = new Label("Order number:")
        {
            X = 1,
            Y = 1,
            ColorScheme = Colors.TopLevel
        };
        private static TextField OrderNumberInput = new TextField("")
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
            X = 1,
            Y = 3,
            ColorScheme = Colors.TopLevel
        };

        private static TextField CabinetTypeInput = new TextField("")
        {
            X = Pos.Right(CabinetTypeLabel) + 1,
            Y = 3,
            Width = 14,
            Height = 1,
            ColorScheme = Colors.Menu,
            Enabled = false
        };

        #endregion

        // Initializes the elements of the properties dialog and dialog itself
        public static void ShowDialog()
        {
            // More properties dialog elements initialization
            PropertiesDialog.Border.Effect3D = false;

            // Add the elements to the dialog
            PropertiesDialog.Add(OrderNumberLabel, OrderNumberInput, CabinetTypeLabel, CabinetTypeInput);

            // Add the dialog into the interface
            Application.Top.Add(PropertiesDialog);

            // Disable some parts of the interface
            Interface.FaultInput.Enabled = false;
            Interface.Status.Enabled = false;
        }

        // Assesses the data from order number input field and stores it into the variable 
        // and generate text for the cabinet type input field
        private void ExamineOrderNumber()
        {
            string firsts = OrderNumberInput.Text.ToString().Substring(0, 2);

            // Chceck if firsts = 38 and if yes set the cabinet type to X Series
            // Or if firsts = 20 set the cabinet type to Netstal series
            if (OrderNumberInput.Text.Length == 8)
            {
                OrderNumber = OrderNumberInput.Text.ToString();
                if (firsts == "38")
                {
                    CabinetType = "X Series";
                }
                else if (firsts == "20")
                {
                    CabinetType = "Netstal Series";
                }
                else
                {
                    CabinetType = "Unknown";
                }
            }
            else
            {
                CabinetType = "Unknown";
            }

            CabinetTypeInput.Text = CabinetType;
        }
    }
}