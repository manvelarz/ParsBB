using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public static class Functions
    {
        public static string[] LineToObj(string line, string parent)
        {
            if (line.IndexOf("),") == -1)
            {
                string[] elements = line.Split('\'');
                switch (CountOfChar(line, '\''))
                {
                    case 4:
                        string[] ret4 = { parent + elements[1], elements[3] };
                        return ret4;
                    case 2:
                        if (line.IndexOf('\'') < line.IndexOf('='))
                        {
                            string[] ret2 = {elements[1], "" };
                            return ret2;
                        }
                        else// (line.IndexOf('\'') > line.IndexOf('='))
                        {
                            string[] subelements = line.TrimStart('\t').Split('\t');
                            string[] retsub2 = { parent + Convert.ToString(subelements[0]), elements[1] };
                            return retsub2;
                        }


                    default:
                        // You can use the default case.
                        string[] rett = { "------------------INVALID----------------------", "------------------INVALID----------------------" };
                        return rett;
                }
            }

            string[] ret = { "", "" };
            return ret;
        }


        public static int CountOfChar(string LineForChar, char CharToCount)
        {
            // string ggg = "fjkvh;sd;s;s;o";
            int countt = LineForChar.Where(c => c == CharToCount).Count();

            return countt;
        }


        public static string AddParent (string parent, string NewParent)
        {
            string tempPatent = parent + NewParent + ".";
            return tempPatent;
        }

        public static List<string> CustomSplit (string line, char CharToSplit)
        {
            List<string> LineList = line.Split(CharToSplit).ToList<string>();
            List<string> newLineList;
            for (int i = 0; i < LineList.Count; i++)
            {
                //char last =  LineArr[LineArr.Length - 1];
                if (LineList[i].Substring(LineList[i].Length - 1, 1) == @"\")
                {
                    LineList[i] = LineList[i].Substring(0, LineList[i].Length - 1);
                }
                 newLineList.Add(LineList[i]);
            }
            return newLineList;
        }
        
        public static string RemoveLastParent (string parent)
        {
            string[] ParentSplited = parent.Split('.');
            string NewParent = "";
            //if ()
            for (int i = 0; i < ParentSplited.Length - 2; i++)
            {
                NewParent = NewParent + ParentSplited[i] + ".";
            }
           // parent = NewParent;
            return NewParent;
        }

    }

