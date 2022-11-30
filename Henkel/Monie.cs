/*  This script is a part of the Henkel project
 *  Author: Branislav Juhás
 *  Date: 2022-11-30
 *  Last update: 2022-11-30
 *  
 *  --  File ( Monie.cs ) Description  --
 *
 * This file contains all the algorythmic data and code to process the data
 * and compute missing information.
 */

namespace Henkel
{
    internal class Monie
    {
        // Compute the missing information
        public static void Compute(Fault fault)
        {
            // Break the fault's cache into an array of the words
            string[] cache = fault.Cache.Split(' ');

            foreach (string rawword in cache)
            {
                string word = rawword.ToLower();

                if (Classification.Shorts.Contains(word))
                {
                    string longword = Classification.Longs[Array.IndexOf(Classification.Shorts, word)];

                    if (fault.Cause == "" && Classification.Causes.Contains(longword))
                    {
                        fault.Cause = longword;
                        fault.CauseIndex = Array.IndexOf(Classification.Causes, longword);
                    }
                    else if (fault.Cause != "" && fault.Classification == "" && Classification.Classifications[fault.CauseIndex].Contains(longword))
                    {
                        fault.Classification = longword;
                        fault.ClassificationIndex = Array.IndexOf(Classification.Classifications[fault.CauseIndex], longword);
                    }
                    else if (fault.Cause != "" && fault.Classification != "" && Classification.Types[Classification.ClassificationsPointers[fault.CauseIndex][fault.ClassificationIndex]].Contains(longword))
                    {
                        fault.Type = longword;
                        fault.TypeIndex = Array.IndexOf(Classification.Types[Classification.ClassificationsPointers[fault.CauseIndex][fault.ClassificationIndex]], longword);
                    }

                }
                else if (fault.BMK == "")
                {
                    fault.BMK = word;
                }
            }

            Processing.Pending.Add(Processing.Faults.IndexOf(fault));
            Interface.ProcessedFaults.Text = $"Processing Faults: {(Processing.Faults.Count - Processing.Pending.Count).ToString()}  |  Pending: {Processing.Pending.Count.ToString()}";

            // Repend if necessary
            if (Processing.Pending.Count == 1) { Processing.Rependate(); }
        }
    }
}