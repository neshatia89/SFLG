using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Simplification
{
    class Program
    {

        static int MinValue = 60;
        static int MaxValue = 100;
        static int NumberOfPoints = 300;
        static int MaxXWidthPoints = 300;
        static int WindowSize = 10;
        static int NumberOfSimplifiedPoints = (int)NumberOfPoints / WindowSize;



        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            Boolean RandomDatageneratorFlag = true;
             string textFile = @"D:\SFLG inputs\InputFile2.txt";
            string[] lines = File.ReadAllLines(textFile);

            string[] RequredSpaces = { "R", "R", "S" };



            string text = "-" + Environment.NewLine;

            // Set a variable to the Documents path.
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            List<Point> AllPointsArray = new List<Point>();
            List<Point> SFLGSimplifiedAllPoints = new List<Point>();
            List<Point> PIPSimplifiedAllPoints = new List<Point>();

            if (RandomDatageneratorFlag)
            {
                Point p;
                int RandNum = MinValue;
                for (int i = 0; i < NumberOfPoints; i++)
                {
                    int temprand = RandomNumber(MinValue, MaxValue);
                    while ((RandNum - 5) > temprand || temprand > (RandNum + 5))
                    {
                        temprand = RandomNumber(60, 100);
                    }
                    RandNum = temprand;



                    p = new Point(i, RandNum, NumberOfPoints);
                    if ((i == 0) || (i == (NumberOfPoints - 1)))
                    {
                        p.PIPSelected = 100;
                    }
                    text += ("\n" + p._Y);

                    AllPointsArray.Add(p);
                }
            }
            else
            {
                Point p;
                int x = 0;
                foreach (string line in lines)
                {
                    
                    Console.WriteLine(line);
                    double value = double.Parse(line);

                    p = new Point(x, value, NumberOfPoints);
                    if ((x == 0) || (x == (NumberOfPoints - 1)))
                    {
                        p.PIPSelected = 100;
                    }
                    text += ("\n" + p._Y);

                    AllPointsArray.Add(p);
                    x++;
                }
            }




            for (int i = 0; i < NumberOfPoints; i++)
            {
                //Console.WriteLine("P_X: " + AllPointsArray[i]._X + " P_Y: " + AllPointsArray[i]._Y);
            }

            for (int i = 0; i < NumberOfPoints; i++)
            {
                for (int j = 0; j < NumberOfPoints; j++)
                {
                    if (i == j)
                    {
                        //Console.WriteLine("###");
                        AllPointsArray[i].DistanceToOtherPoitns[j] = -1;
                        continue;
                    }

                    AllPointsArray[i].DistanceToOtherPoitns[j] = EucledeanDistance(AllPointsArray[i], AllPointsArray[j]);
                }
            }

            for (int i = 0; i < NumberOfPoints; i++)
            {
                AllPointsArray[i].CalculateAverageTheDistance();
            }



            int counter2 = 0;
            double max = -100;
            int maxAIndex = 0;
            text += ("\n \n \n");
            text += ("XXXXXXXX");
            text += ("\n \n \n");

            for (int i = 0; i < NumberOfPoints; i++)
            {
                if (AllPointsArray[i].AVGDistance > max)
                {
                    max = AllPointsArray[i].AVGDistance;
                    maxAIndex = i;
                }

                if (((i % WindowSize == 0) && (i != 0)) || (i == (NumberOfPoints - 1)))
                {
                    counter2++;
                    max = -100;
                    //Console.WriteLine("maxAIndex = " + maxAIndex);
                    AllPointsArray[maxAIndex].SFLGSelected = 100;
                    text += ("\n" + maxAIndex);
                }
            }

            AllPointsArray[0].SFLGSelected = 100;

            text += ("\n \n \n");
            text += ("YYYYYYYYY");
            text += ("\n \n \n");
            for (int i = 0; i < NumberOfPoints; i++)
            {
                if (AllPointsArray[i].SFLGSelected == 100)
                {
                    SFLGSimplifiedAllPoints.Add(AllPointsArray[i]);
                    text += ("\n" + AllPointsArray[i]._Y);
                }
            }
            //Console.WriteLine("CC = " + counter2);


            //Point TA = new Point(0, 2, 0);
            //Point TB = new Point(2, 8, 0);
            //double tempm = CalculateSlope(TA, TB);
            //double tempC = CalculateC(TA, TB, tempm);
            //double disT = CalculatePointDistanceToLine(new Point(5, 1, 0), tempm, tempC);


            //PIP
            //PIP
            //PIP
            Boolean FirstPointFound = false;
            Boolean SecondPointFound = false;
            int FirstPointIndex = 0;
            int SecondPointIndex = 0;
            int NumberOfSelectedPointersCounter = 2;


            for (int i = 0; i < NumberOfPoints; i++)
            {
                if (AllPointsArray[i].PIPSelected == 100)
                {
                    if (FirstPointFound == false)
                    {
                        if (i == (NumberOfPoints - 1))
                        {
                            i = -1;
                            FirstPointFound = false;
                            continue;
                        }

                        FirstPointFound = true;
                        FirstPointIndex = i;
                        continue;
                    }

                    if (SecondPointFound == false)
                    {
                        if (i == (FirstPointIndex + 1))
                        {
                            FirstPointIndex = (FirstPointIndex + 1);
                            SecondPointFound = false;
                            if (i == (NumberOfPoints - 1))
                            {
                                FirstPointFound = false;
                                i = -1;
                            }
                            continue;
                        }

                        SecondPointFound = true;
                        SecondPointIndex = i;
                        double m = CalculateSlope(AllPointsArray[FirstPointIndex], AllPointsArray[SecondPointIndex]);
                        double c = CalculateC(AllPointsArray[FirstPointIndex], AllPointsArray[SecondPointIndex], m);
                        //Console.WriteLine("{{ M:" + m + " C: " + c + " A_X:" + AllPointsArray[FirstPointIndex]._X + " A_Y:" + AllPointsArray[FirstPointIndex]._Y + " B_X:" + AllPointsArray[SecondPointIndex]._X + " B_Y:" + AllPointsArray[SecondPointIndex]._Y);

                        for (int j = (FirstPointIndex + 1); j <= (SecondPointIndex - 1); j++)
                        {
                            AllPointsArray[j].DistanceToTheLine = CalculatePointDistanceToLine(AllPointsArray[j], m, c);
                        }

                        if (SecondPointIndex != (NumberOfPoints - 1))
                        {
                            FirstPointIndex = SecondPointIndex;
                            SecondPointFound = false;
                            continue;
                        }

                        double maxDis = -100;
                        int MaxDistanceIndex = 0;
                        for (int j = 0; j < NumberOfPoints; j++)
                        {
                            if (AllPointsArray[j].DistanceToTheLine > maxDis)
                            {
                                MaxDistanceIndex = j;
                                maxDis = AllPointsArray[j].DistanceToTheLine;
                            }
                        }


                        AllPointsArray[MaxDistanceIndex].PIPSelected = 100;
                        NumberOfSelectedPointersCounter++;


                        if (NumberOfSelectedPointersCounter < NumberOfSimplifiedPoints)
                        {
                            i = -1;
                            for (int j = 0; j < NumberOfPoints; j++)
                            {
                                AllPointsArray[j].DistanceToTheLine = 0;
                            }
                            FirstPointFound = false;
                            SecondPointFound = false;
                        }


                    }
                }
            }



            text += ("\n \n \n");
            text += ("PIP - XXXXX");
            text += ("\n \n \n");

            for (int i = 0; i < NumberOfPoints; i++)
            {
                if (AllPointsArray[i].PIPSelected == 100)
                {
                    PIPSimplifiedAllPoints.Add(AllPointsArray[i]);
                    text += ("\n" + i);
                }
            }

            text += ("\n \n \n");
            text += ("PIP - YYYY");
            text += ("\n \n \n");



            for (int i = 0; i < NumberOfPoints; i++)
            {
                if (AllPointsArray[i].PIPSelected == 100)
                {
                    text += ("\n" + AllPointsArray[i]._Y);
                    //Console.WriteLine("##################     " + " A_X:" + AllPointsArray[i]._X + " A_Y:" + AllPointsArray[i]._Y + " B_X:" + AllPointsArray[i]._X + " B_Y:" + AllPointsArray[i]._Y);
                }
            }


            // Write the text to a new file named "WriteFile.txt".
            File.WriteAllText(Path.Combine(docPath, "WriteFile.txt"), text);





            //###############################################################
            //###############################################################
            //Area Calculation
            //Area Calcualtion 
            //###############################################################
            //###############################################################

            List<EmptySpacesClass> AllSpacesNoSimplification2 = new List<EmptySpacesClass>();
            List<EmptySpacesClass> AllSpacesPIP = new List<EmptySpacesClass>();
            List<EmptySpacesClass> AllSpacesSFLG = new List<EmptySpacesClass>();

            int PointsNumber = 0;
            PointsNumber = NumberOfPoints;
            Console.WriteLine("All Area \n");
            calcualteAreas(AllPointsArray, AllSpacesNoSimplification2, PointsNumber, MaxXWidthPoints);

            PointsNumber = NumberOfSimplifiedPoints;
            Console.WriteLine("PIP Area \n");
            calcualteAreas(PIPSimplifiedAllPoints, AllSpacesPIP, PointsNumber, MaxXWidthPoints);

            Console.WriteLine("SFLG Area \n");
            calcualteAreas(SFLGSimplifiedAllPoints, AllSpacesSFLG, PointsNumber, MaxXWidthPoints);

            

            List<EmptySpacesClass> maxXIndexNon = new List<EmptySpacesClass>();
            List<EmptySpacesClass> maxXIndexPIP = new List<EmptySpacesClass>();
            List<EmptySpacesClass> maxXIndexSFLG = new List<EmptySpacesClass>();

            Console.WriteLine("\n\n");
            Console.WriteLine("*** NON *** ");
            SPaceAllocationMethod(AllSpacesNoSimplification2, RequredSpaces);

            Console.WriteLine("\n*** PIP *** ");
            SPaceAllocationMethod(AllSpacesPIP, RequredSpaces);

            Console.WriteLine("\n*** SFLG *** ");
            SPaceAllocationMethod(AllSpacesSFLG, RequredSpaces);

            Console.WriteLine("\n\n");
            //Log(AllSpacesNoSimplification2, SqIndex, "SSS");

            Console.WriteLine("\n\n");







            //for (int i = 0; i < RequredSpaces.Length; i++)
            //{
            //    if (RequredSpaces[i] == "R")
            //    {
            //        double maxX = -100;
            //        EmptySpacesClass xx = new EmptySpacesClass(MinValue, MaxValue, false, 0, NumberOfPoints);
            //        for (int j = 0; j < AllSpacesNoSimplification2.Count; j++)
            //        {
            //            if (AllSpacesNoSimplification2[j].RectangleSpace == true)
            //            {
            //                if (AllSpacesNoSimplification2[j].CalculatedArea > maxX)
            //                {
            //                    Boolean ContinueFlag = false;
            //                    for (int t = 0; t < maxXIndexNon.Count; t++)
            //                    {
            //                        if(AllSpacesNoSimplification2[j].LeftPointX < maxXIndexNon[t].LeftPointX && AllSpacesNoSimplification2[j].RightPointX > maxXIndexNon[t].RightPointX)
            //                        {
            //                            ContinueFlag = true;
            //                            break;
            //                        }
            //                        if(maxXIndexNon[t].LeftPointX < AllSpacesNoSimplification2[j].LeftPointX && maxXIndexNon[t].RightPointX > AllSpacesNoSimplification2[j].LeftPointX)
            //                        {
            //                            ContinueFlag = true;
            //                            break;
            //                        }
            //                        if (maxXIndexNon[t].LeftPointX < AllSpacesNoSimplification2[j].RightPointX && maxXIndexNon[t].RightPointX > AllSpacesNoSimplification2[j].RightPointX)
            //                        {
            //                            ContinueFlag = true;
            //                            break;
            //                        }
            //                    }

            //                    if (ContinueFlag == true)
            //                    {
            //                        continue;
            //                    }
            //                    maxX = AllSpacesNoSimplification2[j].CalculatedArea;
            //                    xx = AllSpacesNoSimplification2[j];
            //                }
            //            }

            //        }
            //        maxXIndexNon.Add(xx);

            //        xx = new EmptySpacesClass(MinValue, MaxValue, false, 0, NumberOfPoints);
            //        maxX = -100;
            //        for (int j = 0; j < AllSpacesPIP.Count; j++)
            //        {
            //            if (AllSpacesPIP[j].RectangleSpace == true)
            //            {
            //                if (AllSpacesPIP[j].CalculatedArea > maxX)
            //                {
            //                    Boolean ContinueFlag = false;
            //                    for (int t = 0; t < maxXIndexPIP.Count; t++)
            //                    {
            //                        if (AllSpacesPIP[j].LeftPointX < maxXIndexPIP[t].LeftPointX && AllSpacesPIP[j].RightPointX > maxXIndexPIP[t].RightPointX)
            //                        {
            //                            ContinueFlag = true;
            //                            break;
            //                        }
            //                        if (maxXIndexPIP[t].LeftPointX < AllSpacesPIP[j].LeftPointX && maxXIndexPIP[t].RightPointX > AllSpacesPIP[j].LeftPointX)
            //                        {
            //                            ContinueFlag = true;
            //                            break;
            //                        }
            //                        if (maxXIndexPIP[t].LeftPointX < AllSpacesPIP[j].RightPointX && maxXIndexPIP[t].RightPointX > AllSpacesPIP[j].RightPointX)
            //                        {
            //                            ContinueFlag = true;
            //                            break;
            //                        }
            //                    }
            //                    if (ContinueFlag == true)
            //                    {
            //                        continue;
            //                    }
            //                    maxX = AllSpacesPIP[j].CalculatedArea;
            //                    xx = AllSpacesPIP[j];
            //                }

            //            }

            //        }

            //        maxXIndexPIP.Add(xx);

            //        xx = new EmptySpacesClass(MinValue, MaxValue, false, 0, NumberOfPoints);
            //        maxX = -100;
            //        for (int j = 0; j < AllSpacesSFLG.Count; j++)
            //        {
            //            if (AllSpacesSFLG[j].RectangleSpace == true)
            //            {
            //                if (AllSpacesSFLG[j].CalculatedArea > maxX)
            //                {
            //                    Boolean ContinueFlag = false;
            //                    for (int t = 0; t < maxXIndexSFLG.Count; t++)
            //                    {
            //                        if (AllSpacesSFLG[j].LeftPointX < maxXIndexSFLG[t].LeftPointX && AllSpacesSFLG[j].RightPointX > maxXIndexSFLG[t].RightPointX)
            //                        {
            //                            ContinueFlag = true;
            //                            break;
            //                        }
            //                        if (maxXIndexSFLG[t].LeftPointX < AllSpacesSFLG[j].LeftPointX && maxXIndexSFLG[t].RightPointX > AllSpacesSFLG[j].LeftPointX)
            //                        {
            //                            ContinueFlag = true;
            //                            break;
            //                        }
            //                        if (maxXIndexSFLG[t].LeftPointX < AllSpacesSFLG[j].RightPointX && maxXIndexSFLG[t].RightPointX > AllSpacesSFLG[j].RightPointX)
            //                        {
            //                            ContinueFlag = true;
            //                            break;
            //                        }
            //                    }
            //                    if (ContinueFlag == true)
            //                    {
            //                        continue;
            //                    }
            //                    maxX = AllSpacesSFLG[j].CalculatedArea;
            //                    xx = AllSpacesSFLG[j];
            //                }

            //            }

            //        }

            //        maxXIndexSFLG.Add(xx);
            //    }
            //    else if (RequredSpaces[i] == "S")
            //    {
            //        double maxX = -100;
            //        EmptySpacesClass xx = new EmptySpacesClass(MinValue, MaxValue, false, 0, NumberOfPoints);
            //        for (int j = 0; j < AllSpacesNoSimplification2.Count; j++)
            //        {
            //            if (AllSpacesNoSimplification2[j].SquareSpace == true)
            //            {
            //                if (AllSpacesNoSimplification2[j].CalculatedArea > maxX)
            //                {
            //                    Boolean ContinueFlag = false;
            //                    for (int t = 0; t < maxXIndexNon.Count; t++)
            //                    {
            //                        if(maxXIndexNon[t].LeftPointX == AllSpacesNoSimplification2[j].LeftPointX)
            //                        {
            //                            ContinueFlag = true;
            //                            break;
            //                        }
            //                        if (AllSpacesNoSimplification2[j].LeftPointX <= maxXIndexNon[t].LeftPointX && AllSpacesNoSimplification2[j].RightPointX >= maxXIndexNon[t].RightPointX)
            //                        {
            //                            ContinueFlag = true;
            //                            break;
            //                        }
            //                        if (maxXIndexNon[t].LeftPointX <= AllSpacesNoSimplification2[j].LeftPointX && maxXIndexNon[t].RightPointX >= AllSpacesNoSimplification2[j].LeftPointX)
            //                        {
            //                            ContinueFlag = true;
            //                            break;
            //                        }
            //                        if (maxXIndexNon[t].LeftPointX <= AllSpacesNoSimplification2[j].RightPointX && maxXIndexNon[t].RightPointX >= AllSpacesNoSimplification2[j].RightPointX)
            //                        {
            //                            ContinueFlag = true;
            //                            break;
            //                        }
            //                    }
            //                    if (ContinueFlag == true)
            //                    {
            //                        continue;
            //                    }
            //                    maxX = AllSpacesNoSimplification2[j].CalculatedArea;
            //                    xx = AllSpacesNoSimplification2[j];
            //                }
            //            }

            //        }
            //        maxXIndexNon.Add(xx);

            //        xx = new EmptySpacesClass(MinValue, MaxValue, false, 0, NumberOfPoints);
            //        maxX = -100;
            //        for (int j = 0; j < AllSpacesPIP.Count; j++)
            //        {
            //            if (AllSpacesPIP[j].SquareSpace == true)
            //            {
            //                if (AllSpacesPIP[j].CalculatedArea > maxX)
            //                {
            //                    Boolean ContinueFlag = false;
            //                    for (int t = 0; t < maxXIndexPIP.Count; t++)
            //                    {
            //                        if (maxXIndexPIP[t].LeftPointX == AllSpacesPIP[j].LeftPointX)
            //                        {
            //                            ContinueFlag = true;
            //                            break;
            //                        }
            //                        if (AllSpacesPIP[j].LeftPointX <= maxXIndexPIP[t].LeftPointX && AllSpacesPIP[j].RightPointX >= maxXIndexPIP[t].RightPointX)
            //                        {
            //                            ContinueFlag = true;
            //                            break;
            //                        }
            //                        if (maxXIndexPIP[t].LeftPointX <= AllSpacesPIP[j].LeftPointX && maxXIndexPIP[t].RightPointX >= AllSpacesPIP[j].LeftPointX)
            //                        {
            //                            ContinueFlag = true;
            //                            break;
            //                        }
            //                        if (maxXIndexPIP[t].LeftPointX <= AllSpacesPIP[j].RightPointX && maxXIndexPIP[t].RightPointX >= AllSpacesPIP[j].RightPointX)
            //                        {
            //                            ContinueFlag = true;
            //                            break;
            //                        }
            //                    }

            //                    if (ContinueFlag == true)
            //                    {
            //                        continue;
            //                    }
            //                    maxX = AllSpacesPIP[j].CalculatedArea;
            //                    xx = AllSpacesPIP[j];
            //                }

            //            }

            //        }

            //        maxXIndexPIP.Add(xx);

            //        xx = new EmptySpacesClass(MinValue, MaxValue, false, 0, NumberOfPoints);
            //        maxX = -100;
            //        for (int j = 0; j < AllSpacesSFLG.Count; j++)
            //        {
            //            if (AllSpacesSFLG[j].SquareSpace == true)
            //            {
            //                if (AllSpacesSFLG[j].CalculatedArea > maxX)
            //                {
            //                    Boolean ContinueFlag = false;
            //                    for (int t = 0; t < maxXIndexSFLG.Count; t++)
            //                    {
            //                        ContinueFlag = false;
            //                        if (maxXIndexSFLG[t].LeftPointX == AllSpacesSFLG[j].LeftPointX)
            //                        {
            //                            ContinueFlag = true;
            //                            break;
            //                        }

            //                        if (AllSpacesSFLG[j].LeftPointX <= maxXIndexSFLG[t].LeftPointX && AllSpacesSFLG[j].RightPointX >= maxXIndexSFLG[t].RightPointX)
            //                        {
            //                            ContinueFlag = true;
            //                            break;
            //                        }
            //                        if (maxXIndexSFLG[t].LeftPointX <= AllSpacesSFLG[j].LeftPointX && maxXIndexSFLG[t].RightPointX >= AllSpacesSFLG[j].LeftPointX)
            //                        {
            //                            ContinueFlag = true;
            //                            break;
            //                        }
            //                        if (maxXIndexSFLG[t].LeftPointX <= AllSpacesSFLG[j].RightPointX && maxXIndexSFLG[t].RightPointX >= AllSpacesSFLG[j].RightPointX)
            //                        {
            //                            ContinueFlag = true;
            //                            break;
            //                        }
            //                    }

            //                    if(ContinueFlag == true)
            //                    {
            //                        continue;
            //                    }
            //                    maxX = AllSpacesSFLG[j].CalculatedArea;
            //                    xx = AllSpacesSFLG[j];
            //                }

            //            }

            //        }

            //        maxXIndexSFLG.Add(xx);
            //    }



            //    // the space should be recalculate stuff 


            //}


            //Console.WriteLine("\n\n NON");
            //for (int i = 0; i < maxXIndexNon.Count; i++)
            //{
            //    Console.WriteLine("[CALCULATED] LX: " + maxXIndexNon[i].LeftPointX + " LY: " + maxXIndexNon[i].LeftPointY + " RX: " + maxXIndexNon[i].RightPointX + " RY: " + maxXIndexNon[i].RightPointY + " Area: " + maxXIndexNon[i].CalculatedArea + " Rectangle Flag: " + maxXIndexNon[i].RectangleSpace + " Square Flag: " + maxXIndexNon[i].SquareSpace + " String: " + maxXIndexNon[i].test);
            //    Console.WriteLine(maxXIndexNon[i].LeftPointX);
            //}

            //Console.WriteLine("\n\n PIP");

            //for (int i = 0; i < maxXIndexPIP.Count; i++)
            //{
            //    Console.WriteLine("[CALCULATED] LX: " + maxXIndexPIP[i].LeftPointX + " LY: " + maxXIndexPIP[i].LeftPointY + " RX: " + maxXIndexPIP[i].RightPointX + " RY: " + maxXIndexPIP[i].RightPointY + " Area: " + maxXIndexPIP[i].CalculatedArea + " Rectangle Flag: " + maxXIndexPIP[i].RectangleSpace + " Square Flag: " + maxXIndexPIP[i].SquareSpace + " String: " + maxXIndexPIP[i].test);
            //    Console.WriteLine(maxXIndexPIP[i].LeftPointX);
            //}

            //Console.WriteLine("\n\n SFLG");
            //for (int i = 0; i < maxXIndexSFLG.Count; i++)
            //{
            //    Console.WriteLine("[CALCULATED] LX: " + maxXIndexSFLG[i].LeftPointX + " LY: " + maxXIndexSFLG[i].LeftPointY + " RX: " + maxXIndexSFLG[i].RightPointX + " RY: " + maxXIndexSFLG[i].RightPointY + " Area: " + maxXIndexSFLG[i].CalculatedArea + " Rectangle Flag: " + maxXIndexSFLG[i].RectangleSpace + " Square Flag: " + maxXIndexSFLG[i].SquareSpace + " String: " + maxXIndexSFLG[i].test);
            //    Console.WriteLine(maxXIndexSFLG[i].LeftPointX);
            //}





            Console.WriteLine("FINISHED");
            Console.ReadLine();

        }

        private static void SPaceAllocationMethod(List<EmptySpacesClass> AllSpacesNoSimplification2, string[] RequredSpaces)
        {
            for (int j = 0; j < RequredSpaces.Length; j++)
            {
                double MaxXIndexSquare = 0;
                double MaxXIndexRectangle = 0;
                int RecIndex = 0;
                int SqIndex = 0;

                double MaxAreaRectangle = -100;
                double MaxAreaSquare = -100;

                for (int i = 0; i < AllSpacesNoSimplification2.Count; i++)
                {
                    if (AllSpacesNoSimplification2[i].SquareSpace == true && AllSpacesNoSimplification2[i].PickedOrnot == false)
                    {
                        if (AllSpacesNoSimplification2[i].CalculatedArea > MaxAreaSquare)
                        {
                            SqIndex = i;
                            MaxAreaSquare = AllSpacesNoSimplification2[i].CalculatedArea;
                            MaxXIndexSquare = AllSpacesNoSimplification2[i].LeftPointX;
                        }
                    }

                    if (AllSpacesNoSimplification2[i].RectangleSpace == true && AllSpacesNoSimplification2[i].PickedOrnot == false)
                    {
                        if (AllSpacesNoSimplification2[i].CalculatedArea > MaxAreaRectangle)
                        {
                            RecIndex = i;
                            MaxAreaRectangle = AllSpacesNoSimplification2[i].CalculatedArea;
                            MaxXIndexRectangle = AllSpacesNoSimplification2[i].LeftPointX;
                        }
                    }
                }//end of for (going through all spaces and select the biggest square and rectanlge areas

                if (RequredSpaces[j] == "R")
                {
                    AllSpacesNoSimplification2[RecIndex].PickedOrnot = true;
                    Log(AllSpacesNoSimplification2, RecIndex, "RRR");
                    UpdateSpaces(AllSpacesNoSimplification2);
                }

                if (RequredSpaces[j] == "S")
                {
                    if (AllSpacesNoSimplification2[RecIndex].HeightSp > AllSpacesNoSimplification2[SqIndex].HeightSp)
                    {

                        //Log(AllSpacesNoSimplification2, RecIndex, "BreakThis");
                        double p0 = MinValue;
                        double p1 = (MinValue + AllSpacesNoSimplification2[RecIndex].HeightSp);

                        double Np0 = ChangeTheNumberInNewRange(p0, MinValue, (MaxValue - MinValue), NumberOfPoints, 0);
                        double Np1 = ChangeTheNumberInNewRange(p1, MinValue, (MaxValue - MinValue), NumberOfPoints, 0);
                        double newHight = Math.Abs(Np0 - Np1);

                        EmptySpacesClass newSpace = new EmptySpacesClass(MinValue, MaxValue, AllSpacesNoSimplification2[RecIndex].ButtomTrue, 0, NumberOfPoints);
                        newSpace.LeftPointX = (AllSpacesNoSimplification2[RecIndex].LeftPointX + newHight + 1); ;
                        newSpace.RightPointX = AllSpacesNoSimplification2[RecIndex].RightPointX;
                        newSpace.RightPointY = newSpace.LeftPointY = AllSpacesNoSimplification2[RecIndex].LeftPointY;
                        newSpace.CalculateArea();
                        newSpace.test += "Hoohoo";
                        AllSpacesNoSimplification2.Add(newSpace);

                        AllSpacesNoSimplification2[RecIndex].RightPointX = (AllSpacesNoSimplification2[RecIndex].LeftPointX + newHight);
                        AllSpacesNoSimplification2[RecIndex].test += " - MAIN HOOHOO -";
                        AllSpacesNoSimplification2[RecIndex].CalculateArea();

                        AllSpacesNoSimplification2[RecIndex].PickedOrnot = true;
                        Log(AllSpacesNoSimplification2, RecIndex, "SSSH");
                        UpdateSpaces(AllSpacesNoSimplification2);

                    }
                    else
                    {
                        AllSpacesNoSimplification2[SqIndex].PickedOrnot = true;
                        Log(AllSpacesNoSimplification2, SqIndex, "SSS");
                        UpdateSpaces(AllSpacesNoSimplification2);
                    }

                }

            }
        }

        //####################
        //####################
        // End of Main


        static public void Log(List<EmptySpacesClass> AllSpacesNoSimplification2, int SqIndex, string FirstPart)
        {
            Console.WriteLine("[" + FirstPart + "]  [LX: " + AllSpacesNoSimplification2[SqIndex].LeftPointX + " [LY: " + AllSpacesNoSimplification2[SqIndex].LeftPointY + " [RX: " + AllSpacesNoSimplification2[SqIndex].RightPointX + " [RY: " + AllSpacesNoSimplification2[SqIndex].RightPointY + " [Area: " + AllSpacesNoSimplification2[SqIndex].CalculatedArea + " [BUT: " + AllSpacesNoSimplification2[SqIndex].ButtomTrue + " [String: " + AllSpacesNoSimplification2[SqIndex].test);
        }


        static public void calcualteAreas(List<Point> AllPointsArray, List<EmptySpacesClass> AllSpacesNoSimplification, int PointsNumber, int MaxXWidth)
        {
            double steps = 0.5;
            double YIndexesOnYaxis = MinValue;
            Point PLeft = new Point(0, 0, 0);
            Point PRight = new Point(0, 0, 0);


            //Buttom to the first Point
            while (YIndexesOnYaxis <= AllPointsArray[0]._Y)
            {

                //Console.WriteLine(YIndexesOnYaxis);
                CalculateRightLeftPointsLowerPart(AllPointsArray, out PLeft, out PRight, new Point(0, YIndexesOnYaxis, 0), MaxXWidth);
                //Console.WriteLine("[B] Y on X-Axis: " + YIndexesOnYaxis + " Plef_X: " + PLeft._X + " Plef_Y: " + PLeft._Y + " PRight_X: " + PRight._X + " PRight_Y: " + PRight._Y);

                EmptySpacesClass NewSpace = new EmptySpacesClass(MinValue, MaxValue, true, 0, NumberOfPoints);
                if (PLeft._X == PRight._X)
                {
                    NewSpace.LeftPointX = 0;
                    NewSpace.LeftPointY = NewSpace.RightPointY = YIndexesOnYaxis;
                    NewSpace.RightPointX = PLeft._X;
                }
                else
                {
                    NewSpace.LeftPointX = 0;
                    NewSpace.LeftPointY = NewSpace.RightPointY = YIndexesOnYaxis;

                    //should be calculated on the line 
                    NewSpace.RightPointX = CalculateXInterSectionOfTwoLines(new Point(0, YIndexesOnYaxis, 0), new Point(0, YIndexesOnYaxis, 0), PLeft, PRight);
                }
                NewSpace.test += "OnXAxisLower";
                NewSpace.CalculateArea();

                //Console.WriteLine("LX: " + NewSpace.LeftPointX + " LY: " + NewSpace.LeftPointY + " RX: " + NewSpace.RightPointX + " RY: " + NewSpace.RightPointY + " Area: " + NewSpace.CalculatedArea);
                AllSpacesNoSimplification.Add(NewSpace);
                YIndexesOnYaxis += steps;
            }

            //top to the first point 
            YIndexesOnYaxis = MaxValue;
            while (YIndexesOnYaxis >= AllPointsArray[0]._Y)
            {

                //Console.WriteLine(YIndexesOnYaxis);
                CalculateRightLeftPointsUpperPart(AllPointsArray, out PLeft, out PRight, new Point(0, YIndexesOnYaxis, 0), MaxXWidth);
                //Console.WriteLine("[T] Y on X-Axis: " + YIndexesOnYaxis + " Plef_X: " + PLeft._X + " Plef_Y: " + PLeft._Y + " PRight_X: " + PRight._X + " PRight_Y: " + PRight._Y);

                EmptySpacesClass NewSpace = new EmptySpacesClass(MinValue, MaxValue, false, 0, NumberOfPoints);

                if (PLeft._X == PRight._X)
                {
                    NewSpace.LeftPointX = 0;
                    NewSpace.LeftPointY = NewSpace.RightPointY = YIndexesOnYaxis;
                    NewSpace.RightPointX = PLeft._X;
                }
                else
                {
                    NewSpace.LeftPointX = 0;
                    NewSpace.LeftPointY = NewSpace.RightPointY = YIndexesOnYaxis;

                    //should be calculated on the line 
                    NewSpace.RightPointX = CalculateXInterSectionOfTwoLines(new Point(0, YIndexesOnYaxis, 0), new Point(0, YIndexesOnYaxis, 0), PLeft, PRight);
                }
                NewSpace.test += "OnXAxisUpper";
                NewSpace.CalculateArea();
                //Console.WriteLine("LX: " + NewSpace.LeftPointX + " LY: " + NewSpace.LeftPointY + " RX: " + NewSpace.RightPointX + " RY: " + NewSpace.RightPointY + " Area: " + NewSpace.CalculatedArea);

                AllSpacesNoSimplification.Add(NewSpace);
                YIndexesOnYaxis -= steps;
            }




            Point PLeft2 = new Point(0, 0, 0);
            Point PRight2 = new Point(0, 0, 0);

            for (int i = 0; i < (PointsNumber - 1); i++)
            {
                Point HLeft = AllPointsArray[i];
                Point HRight = AllPointsArray[i + 1];
                double m = CalculateSlope(HLeft, HRight);
                double c = CalculateC(HLeft, HRight, m);

                //Lower Area
                if (HLeft._Y <= HRight._Y)
                {
                    for (double xj = (HLeft._X + 0.1); xj <= (HRight._X - 0.1); xj += steps)
                    {
                        double yj = ((m) * (xj) + c);
                        //Console.WriteLine("LX: " + HLeft._X + " RX: " + HRight._X + " LY: " + HLeft._Y + " RY: " + HRight._Y + " XJ: " + xj + " yj: " + yj);
                        CalculateRightLeftPointsLowerPart(AllPointsArray, out PLeft2, out PRight2, new Point(xj, yj, 0), MaxXWidth);
                        EmptySpacesClass NewSpace = new EmptySpacesClass(MinValue, MaxValue, true, 0, NumberOfPoints);

                        if (PLeft2._X == PRight2._X)
                        {
                            NewSpace.LeftPointX = xj;
                            NewSpace.LeftPointY = NewSpace.RightPointY = yj;
                            NewSpace.RightPointX = PLeft2._X;
                        }
                        else
                        {
                            NewSpace.LeftPointX = xj;
                            NewSpace.LeftPointY = NewSpace.RightPointY = yj;
                            NewSpace.RightPointX = CalculateXInterSectionOfTwoLines(new Point(xj, yj, 0), new Point(xj + 1, yj, 0), PLeft2, PRight2);
                            int temp = 0;
                        }
                        NewSpace.test += "TestAreaLower";
                        NewSpace.CalculateArea();
                        //Console.WriteLine("LX: " + NewSpace.LeftPointX + " LY: " + NewSpace.LeftPointY + " RX: " + NewSpace.RightPointX + " RY: " + NewSpace.RightPointY + " Area: " + NewSpace.CalculatedArea);
                        AllSpacesNoSimplification.Add(NewSpace);
                    }
                }
                //Upper area
                else
                {
                    for (double xj = (HLeft._X + 0.1); xj <= (HRight._X - 0.1); xj += steps)
                    {
                        double yj = ((m) * (xj) + c);
                        //Console.WriteLine("LX: " + HLeft._X + " RX: " + HRight._X + " LY: " + HLeft._Y + " RY: " + HRight._Y + " XJ: " + xj + " yj: " + yj);
                        CalculateRightLeftPointsUpperPart(AllPointsArray, out PLeft2, out PRight2, new Point(xj, yj, 0), MaxXWidth);
                        EmptySpacesClass NewSpace = new EmptySpacesClass(MinValue, MaxValue, false, 0, NumberOfPoints);

                        if (PLeft2._X == PRight2._X)
                        {
                            NewSpace.LeftPointX = xj;
                            NewSpace.LeftPointY = NewSpace.RightPointY = yj;
                            NewSpace.RightPointX = PLeft2._X;
                        }
                        else
                        {
                            NewSpace.LeftPointX = xj;
                            NewSpace.LeftPointY = NewSpace.RightPointY = yj;
                            NewSpace.RightPointX = CalculateXInterSectionOfTwoLines(new Point(xj, yj, 0), new Point(xj + 1, yj, 0), PLeft2, PRight2);
                            int temp = 0;
                        }
                        NewSpace.test += "TestArea";
                        NewSpace.CalculateArea();
                        AllSpacesNoSimplification.Add(NewSpace);
                    }
                }

            }




            for (int i = 0; i < AllSpacesNoSimplification.Count; i++)
            {
                //Console.WriteLine("LX: " + AllSpacesNoSimplification[i].LeftPointX + " LY: " + AllSpacesNoSimplification[i].LeftPointY + " RX: " + AllSpacesNoSimplification[i].RightPointX + " RY: " + AllSpacesNoSimplification[i].RightPointY + " Area: " + AllSpacesNoSimplification[i].CalculatedArea + " [Sq: " + AllSpacesNoSimplification[i].SquareSpace + " [Re: " + AllSpacesNoSimplification[i].RectangleSpace);
                if (AllSpacesNoSimplification[i].test.Contains("OnXAxisUpper"))
                {
                    //Console.WriteLine("LX: " + AllSpacesNoSimplification[i].LeftPointX + " LY: " + AllSpacesNoSimplification[i].LeftPointY + " RX: " + AllSpacesNoSimplification[i].RightPointX + " RY: " + AllSpacesNoSimplification[i].RightPointY + " Area: " + AllSpacesNoSimplification[i].CalculatedArea + " [[String:  " + AllSpacesNoSimplification[i].test);
                }

            }

            double maxAreaLower = -10;
            double lowerMaxX = 0;
            double maxAreaUpper = -10;
            double UpperMaxX = 0;
            int MaxUpperIndex = 0;
            int MaxLowerIndex = 0;

            for (int i = 0; i < AllSpacesNoSimplification.Count; i++)
            {
                if (AllSpacesNoSimplification[i].ButtomTrue == true)
                {
                    //
                    if (AllSpacesNoSimplification[i].RectangleSpace == true || AllSpacesNoSimplification[i].SquareSpace == true)
                    {
                        if (AllSpacesNoSimplification[i].CalculatedArea > maxAreaLower)
                        {
                            maxAreaLower = AllSpacesNoSimplification[i].CalculatedArea;
                            lowerMaxX = AllSpacesNoSimplification[i].LeftPointX;
                            MaxLowerIndex = i;
                        }
                    }
                }
                else
                {
                    //
                    if (AllSpacesNoSimplification[i].RectangleSpace == true || AllSpacesNoSimplification[i].SquareSpace == true)
                    {
                        if (AllSpacesNoSimplification[i].CalculatedArea > maxAreaUpper)
                        {
                            //Console.WriteLine(AllSpacesNoSimplification[i].test);
                            maxAreaUpper = AllSpacesNoSimplification[i].CalculatedArea;
                            UpperMaxX = AllSpacesNoSimplification[i].LeftPointX; ;
                            MaxUpperIndex = i;
                        }
                    }
                }
            }


            //Console.WriteLine("\n\n");
            Console.WriteLine("[L] LX: " + AllSpacesNoSimplification[MaxLowerIndex].LeftPointX + " LY: " + AllSpacesNoSimplification[MaxLowerIndex].LeftPointY + " RX: " + AllSpacesNoSimplification[MaxLowerIndex].RightPointX + " RY: " + AllSpacesNoSimplification[MaxLowerIndex].RightPointY + " Area: " + AllSpacesNoSimplification[MaxLowerIndex].CalculatedArea + " Rectangle Flag: " + AllSpacesNoSimplification[MaxLowerIndex].RectangleSpace + " Square Flag: " + AllSpacesNoSimplification[MaxLowerIndex].SquareSpace + " String: " + AllSpacesNoSimplification[MaxLowerIndex].test);
            Console.WriteLine("[U] LX: " + AllSpacesNoSimplification[MaxUpperIndex].LeftPointX + " LY: " + AllSpacesNoSimplification[MaxUpperIndex].LeftPointY + " RX: " + AllSpacesNoSimplification[MaxUpperIndex].RightPointX + " RY: " + AllSpacesNoSimplification[MaxUpperIndex].RightPointY + " Area: " + AllSpacesNoSimplification[MaxUpperIndex].CalculatedArea + " Rectangle Flag: " + AllSpacesNoSimplification[MaxUpperIndex].RectangleSpace + " Square Flag: " + AllSpacesNoSimplification[MaxUpperIndex].SquareSpace + " String: " + AllSpacesNoSimplification[MaxUpperIndex].test);
            Console.WriteLine("lowerMaxX: " + lowerMaxX);
            Console.WriteLine("UpperMaxX: " + UpperMaxX);
            Console.WriteLine("\n\n");
        }


        static public double ChangeTheNumberInNewRange(double oldValue, double M1, double R1, double R2, double M2)
        {
            //vOut = (V - M1) * R2 / R1 + M2
            double result = 0;
            result = (((oldValue - M1) * R2) / R1) + M2;
            return result;
        }



        static public void UpdateSpaces(List<EmptySpacesClass> mySpaces)
        {
            List<EmptySpacesClass> PickedSpaces = new List<EmptySpacesClass>();
            for (int i = 0; i < mySpaces.Count; i++)
            {
                if (mySpaces[i].PickedOrnot == true)
                {
                    PickedSpaces.Add(mySpaces[i]);
                }
            }

            for (int i = 0; i < mySpaces.Count; i++)
            {
                if (mySpaces[i].PickedOrnot == false)
                {
                    for (int j = 0; j < PickedSpaces.Count; j++)
                    {
                        if (mySpaces[i].ButtomTrue == PickedSpaces[j].ButtomTrue)
                        {
                            if (mySpaces[i].RightPointX >= PickedSpaces[j].LeftPointX && mySpaces[i].RightPointX <= PickedSpaces[j].RightPointX && mySpaces[i].LeftPointX <= PickedSpaces[j].LeftPointX)
                            {
                                mySpaces[i].RightPointX = PickedSpaces[j].LeftPointX;
                                mySpaces[i].test += " # [1]Area Recalculation # ";
                                //not sure
                                mySpaces[i].TopY = mySpaces[i].ButtomY = PickedSpaces[j].RightPointY;
                                mySpaces[i].CalculateArea();
                                continue;
                            }

                            if (mySpaces[i].LeftPointX >= PickedSpaces[j].LeftPointX && mySpaces[i].LeftPointX <= PickedSpaces[j].RightPointX && mySpaces[i].RightPointX >= PickedSpaces[j].RightPointX)
                            {
                                mySpaces[i].LeftPointX = PickedSpaces[j].RightPointX;
                                mySpaces[i].test += " # [2]Area Recalculation # ";
                                //not sure
                                mySpaces[i].TopY = mySpaces[i].ButtomY = PickedSpaces[j].RightPointY;
                                mySpaces[i].CalculateArea();
                                continue;
                            }

                            if ((mySpaces[i].RightPointX >= PickedSpaces[j].LeftPointX && mySpaces[i].RightPointX <= PickedSpaces[j].RightPointX) && (mySpaces[i].LeftPointX >= PickedSpaces[j].LeftPointX && mySpaces[i].LeftPointX <= PickedSpaces[j].RightPointX))
                            {
                                //not sure
                                mySpaces[i].TopY = mySpaces[i].ButtomY = PickedSpaces[j].RightPointY;
                                mySpaces[i].CalculateArea();
                                continue;
                            }

                            if ((mySpaces[i].LeftPointX <= PickedSpaces[j].LeftPointX && mySpaces[i].RightPointX >= PickedSpaces[j].RightPointX))
                            {

                                EmptySpacesClass newSpace = new EmptySpacesClass(MinValue, MaxValue, mySpaces[i].ButtomTrue, 0, NumberOfPoints);
                                newSpace.LeftPointX = PickedSpaces[j].RightPointX;
                                newSpace.RightPointX = mySpaces[i].RightPointX;
                                newSpace.RightPointY = newSpace.LeftPointY = mySpaces[i].LeftPointY;
                                newSpace.test += " # New Created Area # ";
                                //not sure
                                //newSpace.TopY = newSpace.ButtomY = PickedSpaces[j].RightPointY;
                                newSpace.CalculateArea();
                                mySpaces.Add(newSpace);
                                
                                mySpaces[i].RightPointX = PickedSpaces[j].LeftPointX;
                                mySpaces[i].test += " # [4]Area Recalculation # ";
                                //not sure
                                //mySpaces[i].TopY = mySpaces[i].ButtomY = PickedSpaces[j].RightPointY;
                                mySpaces[i].CalculateArea();
                                continue;

                            }
                        }
                    }
                }
            }
        }











        // Generate a random number between two numbers  
        static public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        static double EucledeanDistance(Point A, Point B)
        {
            double result = 0;
            //Console.WriteLine("P_X: " + A._X + " P_Y: " + B._Y);
            double Temp = (Math.Pow(((double)A._X - B._X), 2) + Math.Pow((A._Y - B._Y), 2));
            result = Math.Sqrt(Temp);
            //Console.WriteLine("MMM: " + Temp);
            //Console.WriteLine($"EUC = " + result);
            return result;
        }


        static double CalculateSlope(Point A, Point B)
        {
            double result = 0;
            result = ((double)(B._Y - A._Y) / (B._X - A._X)); ;
            return result;
        }

        static double CalculateC(Point A, Point B, double m)
        {
            double result = 0;
            result = ((double)A._Y - (m * A._X));
            //Console.WriteLine("CCC: " + ((double)m * A._X));
            return result;
        }

        static double CalculatePointDistanceToLine(Point A, double m, double c)
        {
            double result = 0;
            result = (Math.Abs(((-m * A._X) + ((double)A._Y) - (c)) / (Math.Sqrt(((Math.Pow((-m), 2)) + (Math.Pow((1), 2)))))));
            //Console.WriteLine($"TTT = " + result);
            return result;
        }


        static public void CalculateRightLeftPointsLowerPart(List<Point> points, out Point Left, out Point Right, Point MyPoint, int MaxX)
        {
            Boolean found = false;
            Left = new Point(MaxX, MyPoint._Y, 9);
            Right = new Point(MaxX, MyPoint._Y, 9);
            int index = 0;

            for (int i = 0; i < points.Count; i++)
            {
                if (points[i]._X > MyPoint._X)
                {
                    index = i;
                    break;
                }
            }

            for (int i = index; i < (points.Count - 1); i++)
            {
                if ((points[i]._Y >= MyPoint._Y) && (points[i + 1]._Y <= MyPoint._Y))
                {
                    Left = points[i];
                    Right = points[i + 1];
                    found = true;
                    break;
                }
            }
        }

        static public void CalculateRightLeftPointsUpperPart(List<Point> points, out Point Left, out Point Right, Point MyPoint, int MaxX)
        {
            Boolean found = false;
            Left = new Point(MaxX, MyPoint._Y, 9);
            Right = new Point(MaxX, MyPoint._Y, 9);

            int index = 0;

            for (int i = 0; i < points.Count; i++)
            {
                if (points[i]._X > MyPoint._X)
                {
                    index = i;
                    break;
                }
            }

            //Ali
            for (int i = index; i < (points.Count - 1); i++)
            {
                if ((points[i + 1]._Y >= MyPoint._Y) && (points[i]._Y <= MyPoint._Y))
                {
                    Left = points[i];
                    Right = points[i + 1];
                    found = true;
                    break;
                }
            }
        }
        static public double CalculateXInterSectionOfTwoLines(Point Hleft, Point Hright, Point Left, Point Right)
        {
            double result = 0;
            double Hm = 0;
            double Hc = Hleft._Y;
            double m = CalculateSlope(Left, Right);
            double c = CalculateC(Left, Right, m);
            result = (c - Hc) / (-m);
            //Console.WriteLine("X: " + result);
            return result;

        }


    }


    public class Point
    {
        public int SFLGSelected = 0;
        public int PIPSelected = 0;
        static public int NumberOfPoints = 300;
        public double AVGDistance = 0;
        public double DistanceToTheLine = 0;

        public double _X { get; set; }
        public double _Y { get; set; }

        public Point(double x, double y, int numOfPoints)
        {
            _X = x;
            _Y = y;
            NumberOfPoints = numOfPoints;
        }


        public double[] DistanceToOtherPoitns = new double[NumberOfPoints];

        public void CalculateAverageTheDistance()
        {
            double result = 0;
            for (int i = 0; i < DistanceToOtherPoitns.Length; i++)
            {
                if (DistanceToOtherPoitns[i] > 0)
                {
                    result += (double)DistanceToOtherPoitns[i];
                }
            }
            //Console.WriteLine($"TTT = " + result);
            AVGDistance = ((double)result / (DistanceToOtherPoitns.Length - 1));
        }
    }

    public class EmptySpacesClass
    {


        public Boolean ButtomTrue;

        public double CalculatedArea { get; set; }

        public double LeftPointX { get; set; }
        public double LeftPointY { get; set; }
        public double RightPointX { get; set; }
        public double RightPointY { get; set; }
        public double ButtomY { get; set; }
        public double TopY { get; set; }
        public string test { get; set; }
        public double ButtomY2 { get; set; }
        public double TopY2 { get; set; }

        public Boolean SquareSpace { get; set; }
        public Boolean RectangleSpace { get; set; }

        public Boolean PickedOrnot = false;

        private double OldRangeSquareOffset = 10;
        private double NewSquareOffSet = 10;
        private double minRectangleHight = 30;
        private double minRectangleWidth = 10;
        public double MinX { get; set; }
        public double MaxX { get; set; }
        public double MaxY { get; set; }
        public double MinY { get; set; }

        public double WidthSp = 0;
        public double HeightSp = 0;



        public void CalculateSquareSpaces(double Width, double Height)
        {

            if ((Width <= (Height + NewSquareOffSet)) && (Width >= (Height - NewSquareOffSet)))
            {
                this.test += " - [SquareSpace = true] - ";
                this.SquareSpace = true;
                return;
            }
            else if ((Height <= (Width + NewSquareOffSet)) && (Height >= (Width - NewSquareOffSet)))
            {
                this.test += " - [SquareSpace = true] - ";
                this.SquareSpace = true;
                return;
            }
            this.test += " - [SquareSpace = false] - ";
        }

        public void CalculateRectangleSpace(double Width, double Height)
        {

            if (this.SquareSpace == true)
            {
                this.test += " - [RectangleSpace = false] - ";
                this.RectangleSpace = false;
                return;
            }

            double W_H = Width / Height;
            double H_W = Height / Width;
            if (Height < minRectangleWidth)
            {
                this.test += " - [RectangleSpace = false] - ";
                this.test += " - @@Height < minRectangleWidth - ";
                this.RectangleSpace = false;
                return;
            }
            if (Height > Width)
            {
                this.test += " - [RectangleSpace = false] - ";
                this.test += " - @@Height > Width - ";
                this.RectangleSpace = false;
                return;
            }
            if (W_H > 6 || W_H < 1.5)
            {
                this.test += " - [RectangleSpace = false] - ";
                this.test += " - @@W_H > 6 || W_H < 1.5 :: - " + W_H;
                this.RectangleSpace = false;
                return;
            }

            
            this.test += " - [RectangleSpace = true] - ";
            this.RectangleSpace = true;
            //Console.WriteLine("W_H: " + W_H + "H_W: " + H_W);
        }


        public EmptySpacesClass(double ButtomValue, double TopValue, Boolean ButTrueFlag, double miX, double maX)
        {
            this.ButtomY2 = this.ButtomY = ButtomValue;
            this.TopY2 = this.TopY = TopValue;
            this.ButtomTrue = ButTrueFlag;

            this.MaxY = TopY;
            this.MinY = ButtomY;
            this.MaxX = maX;
            this.MinX = miX;
        }

        

        public double CalculateArea()
        {
            double result = 0;
            double newPointY = 0;
            double newButtomOrtop = 0;
            //NewSquareOffSet = ChangeTheNumberInNewRange(OldRangeSquareOffset, MinY, (MaxY - MinY), (MaxX - MinX), MinX);
            // Console.WriteLine("NewSquareOffSet: " + NewSquareOffSet);

            if (this.ButtomTrue == true)
            {
                newPointY = ChangeTheNumberInNewRange(RightPointY, MinY, (MaxY - MinY), (MaxX - MinX), MinX);
                newButtomOrtop = ChangeTheNumberInNewRange(ButtomY2, MinY, (MaxY - MinY), (MaxX - MinX), MinX);
                //Console.WriteLine("newPointY: " + newPointY);

                double Width = Math.Abs(RightPointX - LeftPointX);
                double Height = Math.Abs(newPointY - newButtomOrtop);
                CalculateSquareSpaces(Width, Height);
                CalculateRectangleSpace(Width, Height);

                this.WidthSp = Math.Abs(RightPointX - LeftPointX);
                this.HeightSp = Math.Abs(RightPointY - ButtomY);
                result = Math.Abs(RightPointX - LeftPointX) * Math.Abs(RightPointY - ButtomY);
                this.CalculatedArea = result;
            }
            else
            {
                newPointY = ChangeTheNumberInNewRange(RightPointY, MinY, (MaxY - MinY), (MaxX - MinX), MinX);
                newButtomOrtop = ChangeTheNumberInNewRange(TopY2, MinY, (MaxY - MinY), (MaxX - MinX), MinX);
                //Console.WriteLine("newButtomOrtop: " + newButtomOrtop);

                double Width = Math.Abs(RightPointX - LeftPointX);
                double Height = Math.Abs(newPointY - newButtomOrtop);

                CalculateSquareSpaces(Width, Height);
                CalculateRectangleSpace(Width, Height);
                this.WidthSp = Math.Abs(RightPointX - LeftPointX);
                this.HeightSp = Math.Abs(RightPointY - TopY2);
                result = Math.Abs(RightPointX - LeftPointX) * Math.Abs(RightPointY - TopY);
                this.CalculatedArea = result;
            }
            //Console.WriteLine("[Area]: " + result);    
            return result;
        }

        public double ChangeTheNumberInNewRange(double oldValue, double M1, double R1, double R2, double M2)
        {
            //vOut = (V - M1) * R2 / R1 + M2
            double result = 0;
            result = (((oldValue - M1) * R2) / R1) + M2;
            return result;
        }

        public void SquareOrRectangle()
        {

        }



    }



}
