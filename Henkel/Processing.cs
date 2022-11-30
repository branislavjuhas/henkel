/*  This script is a part of the Henkel project
 *  Author: Branislav Juhás
 *  Date: 2022-11-23
 *  Last update: 2022-11-30
 *  
 *  --  File ( Processing.cs ) Description  --
 *
 * This file contains the Processing class, which is used to process the data
 * after it has been entered and confirmed by the user & Mnca class, which is
 * used to analyze and classify the data after it has been processed.
 */

using Terminal.Gui; // Terminal.Gui library

namespace Henkel
{
    public class Processing
    {
        public static List<Fault> Faults = new List<Fault>();

        public static List<int> Pending = new List<int>();

        public static void Process()
        {
            // If it is an empty fault return from function

            if (Interface.FaultInput.Text.ToString().Trim() == "") { return; }

            string place = "";

            if (Properties.CabinetType == "Netstal Series")
            {
                place = Interface.NetstalCabinetInput.Text.ToString();
            }
            else
            {
                place = Properties.CabinetNumber;
            }

            Faults.Add(new Fault(Interface.FaultInput.Text.ToString(), Interface.ClassificationInput.Text.ToString(), Properties.OrderNumber, Properties.CabinetType, place));
            Interface.ProcessedFaults.Text = $"Processing Faults: {(Faults.Count - Pending.Count).ToString()}  |  Pending: {Pending.Count.ToString()}";

            Monie.Compute(Faults[Faults.Count - 1]);

            // If the NetstalCabinetInput is visible reset the input and hide it
            // do the same with the ClassificationInput

            if (Interface.NetstalCabinetInput.Visible)
            {
                Interface.NetstalCabinetVisible = false;
                Interface.NetstalCabinetInput.Visible = false;
                Interface.NetstalCabinetInput.Text = "";
                Interface.FaultInput.X = 2;
                Interface.NetstalBorder.Visible = false;
                Interface.NetstalCabinetInput.Visible = false;
            }

            if (Interface.ClassificationInput.Visible)
            {
                Interface.ClassificationBasicsVisible = false;
                Interface.ClassificationInput.Visible = false;
                Interface.ClassifyBorder.Visible = false;
                Interface.ClassificationInput.Text = "";
            }

            Interface.FaultInput.Width = Dim.Fill(2);
            Interface.FaultInput.Text = "";
            Interface.FaultInputBorder.X = 1;
            Interface.FaultInputEndBorder.X = Pos.Right(Interface.FaultInput);
            Interface.FaultInput.SetFocus();
        }

        // Function to fill all the pending fields with fault
        public static void Rependate()
        {
            Fault pender = Faults[Pending[0]];

            Interface.PendingFaultInput.Text = pender.FaultText;
            Interface.PendingBMKInput.Text = pender.BMK;
            Interface.PendingPlacementInput.Text = pender.Placement;
            Interface.PendingOrderNumberInput.Text = pender.OrderNumber;

            if (pender.Cause != "")
            {
                Interface.PendingCauseInput.SelectedItem = pender.CauseIndex;
                Interface.PendingClassificationInput.SetSource(Classification.Classifications[pender.CauseIndex]);

                if (pender.Classification != "")
                {
                    Interface.PendingClassificationInput.SelectedItem = pender.ClassificationIndex;
                    Interface.PendingTypeInput.SetSource(Classification.Types[Classification.ClassificationsPointers[pender.CauseIndex][pender.ClassificationIndex]]);

                    if (pender.Type != "")
                    {
                        Interface.PendingTypeInput.SelectedItem = pender.TypeIndex;
                    }
                }
            }

            // Make all the pending visual elements visible
            Interface.PendingFaultInput.Visible = true;
            Interface.PendingBMKInput.Visible = true;
            Interface.PendingPlacementInput.Visible = true;
            Interface.PendingOrderNumberInput.Visible = true;
            Interface.PendingCauseInput.Visible = true;
            Interface.PendingClassificationInput.Visible = true;
            Interface.PendingTypeInput.Visible = true;
        }
    }

    public class Fault
    {
        public string OrderNumber = "";
        public string Series = "";


        public string Placement = "";
        public string BMK = "";
        public string FaultText = "";

        public string Cause = "";
        public string Classification = "";
        public string Type = "";

        public string Cache = "";

        public int CauseIndex = 0;
        public int ClassificationIndex = 0;
        public int TypeIndex = 0;

        public Fault(string faultText, string cache, string order, string series, string placement = "", string bmk = "", string faulttext = "", string cause = "", string classification = "", string type = "")
        {
            FaultText = faultText;
            Cache = cache;
            OrderNumber = order;
            Series = series;
            Placement = placement;
            BMK = bmk;
            Cause = cause;
            Classification = classification;
            Type = type;
        }
    }
}
