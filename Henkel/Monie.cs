/*  This script is a part of the Henkel project
 *  Author: Branislav Juhás
 *  Date: 2022-11-30
 *  Last update: 2022-12-05
 *  
 *  --  File ( Monie.cs ) Description  --
 *
 * This file contains MONIE 1.1.2 ENGINE SOURCE CODE.
 */

namespace Henkel
{
    internal class Monie
    {
        public static string Version = "1.1.2";

        // Compute the missing information
        public static void Compute(Fault fault)
        {
            // Break the fault's cache into an array of the words
            string[] cache = fault.Cache.Split(' ');

            foreach (string rawword in cache)
            {
                string word = rawword.ToLower();

                string longword = "";

                // If the word is shortcut, replace it with the full word
                // else, replace all characters '_' in the word string by ' ' and store them in the longword string
                if (Classification.Shorts.Contains(word))
                {
                    longword = Classification.Longs[Array.IndexOf(Classification.Shorts, word)];
                }
                else
                {
                    longword = word.Replace('_', ' ');
                }

                // Chceck if the longword can be one of the classification elements and apply it
                if (fault.Cause == "" && Classification.Causes.Contains(longword))
                {
                    fault.Cause = longword;
                    fault.CauseIndex = Array.IndexOf(Classification.Causes, longword);
                }
                else if (fault.Cause != "" && fault.Classifications == "" && Classification.Classifications[fault.CauseIndex].Contains(longword))
                {
                    fault.Classifications = longword;
                    fault.ClassificationIndex = Array.IndexOf(Classification.Classifications[fault.CauseIndex], longword);
                }
                else if (fault.Cause != "" && fault.Classifications != "" && Classification.Types[Classification.ClassificationsPointers[fault.CauseIndex][fault.ClassificationIndex]].Contains(longword))
                {
                    fault.Type = longword;
                    fault.TypeIndex = Array.IndexOf(Classification.Types[Classification.ClassificationsPointers[fault.CauseIndex][fault.ClassificationIndex]], longword);
                }
                else if (word.Length >= 2 && word[0] == '.' && word.Substring(1).All(char.IsDigit))
                {
                    // If the first character of the word is '/' and all the others are numeric values
                    // Chceck if the numeric value after that first '/' is within length of array of 
                    // fault's words and if yes, set word of fault's text at the numeric value index as a BMK
                    int index = int.Parse(word.Substring(1));

                    string[] words = fault.FaultText.Split(' ');

                    if (index <= words.Length && index > 0)
                    {
                        fault.BMK = words[index - 1].ToUpper();
                    }
                }
                else if (word.Length >= 2 && word[word.Length - 1] == '.' && word.Substring(0, word.Length - 1).All(char.IsDigit))
                {
                    // If the first character of the word is '/' and all the others are numeric values
                    // Chceck if the numeric value after that first '/' is within length of array of 
                    // fault's words and if yes, set word from right side of fault's text at the numeric value index as a BMK
                    int index = int.Parse(word.Substring(0, word.Length - 1));

                    string[] words = fault.FaultText.Split(' ');

                    if (index <= words.Length && index > 0)
                    {
                        fault.BMK = words[words.Length - index].ToUpper();
                    }
                }
                else if (fault.BMK == "")
                {

                    fault.BMK = word.ToUpper();
                }
            }

            // If some of the information is missing except the Order Number, Cabinet Type and Cabinet Number and the user wants to use the distances, compute the distances
            if ((fault.Classifications == "" || fault.Type == "") && Settings.UseDistances == 1 && fault.Cause != "")
            {
                int minimalDistance = 1000;
                int minimalDistanceIndex = 0;
                int minimalDistanceOption = 0;

                // Split the fault's text into an array of words
                string[] words = fault.FaultText.Split(' ');

                int y = 0;
                if (fault.Classifications == "")
                {
                    foreach (string rawword in words)
                    {
                        string word = rawword.ToLower();

                        int x = 0;

                        string[] classificationsList;

                        // If the Settings.DistancesLanguage is set to 0, use the English list of classifications
                        // else, use the original list of classifications
                        if (Settings.DistancesLanguage == 0)
                        {
                            classificationsList = Classification.AutomaticClassifications[fault.CauseIndex];
                        }
                        else
                        {
                            classificationsList = Classification.OriginalAutomaticClassifications[fault.CauseIndex];
                        }



                        foreach (string rawclassification in classificationsList)
                        {
                            int distance = 1000;
                            string classification = rawclassification.ToLower();
                            if (rawclassification.Contains(' '))
                            {
                                distance = MultiCoCompute(fault.FaultText.ToLower(), classification) - 1;
                            }
                            else
                            {
                                distance = GetDamerauLevenshteinDistance(word, classification);
                            }


                            if (distance < minimalDistance)
                            {
                                minimalDistance = distance;
                                minimalDistanceIndex = y;
                                minimalDistanceOption = x;
                            }

                            x++;
                        }

                        y++;
                    }

                    if (minimalDistance <= Settings.MaximalDistance)
                    {
                        fault.Classifications = Classification.Classifications[fault.CauseIndex][minimalDistanceOption];
                        fault.ClassificationIndex = minimalDistanceOption;
                    }
                }

                // If the distance is less or equal than Settings.MaximalDistance, apply the classification
                if (fault.Classifications != "")
                {
                    minimalDistance = 1000;
                    minimalDistanceIndex = 0;
                    minimalDistanceOption = 0;

                    y = 0;

                    string[] typesList;

                    // If the Settings.DistancesLanguage is set to 0, use the English list of types
                    // else, use the original list of types
                    if (Settings.DistancesLanguage == 0)
                    {
                        typesList = Classification.Types[Classification.ClassificationsPointers[fault.CauseIndex][fault.ClassificationIndex]];
                    }
                    else
                    {
                        typesList = Classification.OriginalTypes[Classification.ClassificationsPointers[fault.CauseIndex][fault.ClassificationIndex]];
                    }

                    foreach (string rawword in words)
                    {
                        string word = rawword.ToLower();

                        int x = 0;

                        foreach (string rawtype in typesList)
                        {
                            string type = rawtype.ToLower();
                            int distance = GetDamerauLevenshteinDistance(word, type);

                            if (distance < minimalDistance)
                            {
                                minimalDistance = distance;
                                minimalDistanceIndex = y;
                                minimalDistanceOption = x;
                            }

                            x++;
                        }

                        y++;
                    }

                    if (minimalDistance <= Settings.MaximalDistance)
                    {
                        fault.Type = Classification.Types[Classification.ClassificationsPointers[fault.CauseIndex][fault.ClassificationIndex]][minimalDistanceOption];
                        fault.TypeIndex = minimalDistanceOption;
                    }
                }

            }
            // Do the same for the type



            Processing.Pending.Add(Processing.Faults.IndexOf(fault));
            Interface.ProcessedFaults.Text = $"Processing Faults: {(Processing.Faults.Count - Processing.Pending.Count).ToString()}  |  Pending: {Processing.Pending.Count.ToString()}  |  Finished: {(Export.NetsalFaults.Count + Export.Xfaults.Count + Export.UndefinedFaults.Count).ToString()}";

            // Repend if necessary
            if (Processing.Pending.Count == 1) { Processing.Rependate(); }
        }

        public static int MultiCoCompute(string faultText, string classify)
        {
            int changes = 0;

            // Chceck each word in the classify string and find the most 
            // similar word in the faultText string using the Damerau-Levenshtein distance
            // and add the difference to the changes variable

            string[] words = classify.Split(' ');

            int y = 0;

            foreach (string word in words)
            {
                int minimalDistance = 1000;
                int minimalDistanceIndex = 0;
                int minimalDistanceOption = 0;

                int x = 0;

                string[] words2 = faultText.Split(' ');

                foreach (string word2 in words2)
                {
                    int distance = GetDamerauLevenshteinDistance(word, word2);

                    if (distance < minimalDistance)
                    {
                        minimalDistance = distance;
                        minimalDistanceIndex = y;
                        minimalDistanceOption = x;
                    }

                    x++;
                }

                y++;

                changes += minimalDistance;

            }

            changes /= words.Length;

            // If the changes are less or equal than Settings.CoDistance, subtract 3 from the changes
            // variable, so it will be chosen as the most similar classification
            if (changes <= Settings.CoDistance)
            {
                changes -= 3;
            }

            return changes;
        }

        public static int GetDamerauLevenshteinDistance(string s, string t)
        {
            var bounds = new { Height = s.Length + 1, Width = t.Length + 1 };

            int[,] matrix = new int[bounds.Height, bounds.Width];

            for (int height = 0; height < bounds.Height; height++) { matrix[height, 0] = height; };
            for (int width = 0; width < bounds.Width; width++) { matrix[0, width] = width; };

            for (int height = 1; height < bounds.Height; height++)
            {
                for (int width = 1; width < bounds.Width; width++)
                {
                    int cost = (s[height - 1] == t[width - 1]) ? 0 : 1;
                    int insertion = matrix[height, width - 1] + 1;
                    int deletion = matrix[height - 1, width] + 1;
                    int substitution = matrix[height - 1, width - 1] + cost;

                    int distance = Math.Min(insertion, Math.Min(deletion, substitution));

                    if (height > 1 && width > 1 && s[height - 1] == t[width - 2] && s[height - 2] == t[width - 1])
                    {
                        distance = Math.Min(distance, matrix[height - 2, width - 2] + cost);
                    }

                    matrix[height, width] = distance;
                }
            }

            return matrix[bounds.Height - 1, bounds.Width - 1];
        }
    }
}