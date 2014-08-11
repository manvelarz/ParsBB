using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

    public static class Functions
    {
        public static string[] LineToObj(string line, string parent)
        {
            if (line.IndexOf("),") == -1)
            {
                List<string> elements = Functions.CustomSplit(line.Trim(), '\'');
                switch (elements.Count)
                {
                    case 5:
                        string[] ret5 = { parent + elements[1], elements[3] };
                        return ret5;
                    case 3:
                        if (line.IndexOf('\'') < line.IndexOf('='))
                        {
                            string[] ret3 = {elements[1], "" };
                            return ret3;
                        }
                        else// (line.IndexOf('\'') > line.IndexOf('='))
                        {
                            string[] subelements = line.TrimStart('\t').Split('\t');
                            string[] retsub2 = { parent + Convert.ToString(subelements[0]), elements[1] };
                            return retsub2;
                        }
                    case 4:
                            string[] ret4 = {"PErr","err_ Multi Line",elements[1] };
                            return ret4;
                    case 7:
                            string[] ret7 = { parent + elements[1], elements[3] };
                            return ret7;
                    default:
                            string[] rett = { "PErr", "UNNOWN", elements[1] };
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

        public static List<string> CustomSplit(string line, char CharToSplit)
        {
            List<string> LineList = line.Split(CharToSplit).ToList<string>();
            List<string> newLineList = new List<string>();
            for (int i = 0; i < LineList.Count; i++)
            {

                if (LineList[i].EndsWith(@"\") )
                {
                    if (LineList[i+1].EndsWith(@"\") )
                    {
                        newLineList.Add((LineList[i].Substring(0, LineList[i].Length - 1)) + "{" + (LineList[i + 1].Substring(0, LineList[i + 1].Length - 1)) + "}" + LineList[i + 2]);
                    i=i+2;
                    }
                    else
                    {
                        newLineList.Add((LineList[i].Substring(0, LineList[i].Length - 1)) + LineList[i + 1]);
                        i++;
                       // Console.WriteLine("VVVOOORRRIIIAAAAAAA");
                    }
                }
                else
                {
                    newLineList.Add(LineList[i]);
                }

            }
            return newLineList;
        }	


        public static void Table2Cvs(string FilePath, DataTable table)
        {
           // List<DataTable> tbList = GetTablesFromPath(path, langName);


            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = table.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join("^^^", columnNames));

            foreach (DataRow row in table.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join("^^^", fields));
            }

            File.WriteAllText(FilePath, sb.ToString());
        }

    }

