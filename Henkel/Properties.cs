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

        // Declares the cabinet type dropdown with its label
        private static Label CabinetTypeLabel = new Label("Cabinet type:")
        {
            X = 1,
            Y = 3,
            ColorScheme = Colors.TopLevel
        };

        #endregion

        // Initializes the elements of the properties dialog and dialog itself
        public static void ShowDialog()
        {
            // More properties dialog elements initialization
            PropertiesDialog.Border.Effect3D = false;

            // Add the elements to the dialog
            PropertiesDialog.Add(OrderNumberLabel, OrderNumberInput, CabinetTypeLabel, CabinetTypeDropdown);

            // Add the dialog into the interface
            Application.Top.Add(PropertiesDialog);

            // Disable some parts of the interface
            Interface.FaultInput.Enabled = false;
            Interface.Status.Enabled = false;
        }
    }
}