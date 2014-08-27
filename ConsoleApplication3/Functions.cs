using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Xml.Linq;

    public static class Functions
    {
        static string parent; 

        public static List<DataTable> GetTablesFromPath(string path, string langName)
        {

            int countline = 0;
            int countfiles = 0;
            Console.OutputEncoding = Encoding.UTF8;
            Console.SetWindowSize(160, 30);
            DataTable table = new DataTable(langName);
            table.Columns.Add("Variable", typeof(string));
            table.Columns.Add(langName, typeof(string));
            DataTable Errtable = new DataTable(langName);
            Errtable.Columns.Add("ErrVariable", typeof(string));
            Errtable.Columns.Add("Error Tipe", typeof(string));
            Errtable.Columns.Add("File", typeof(string));
            Errtable.Columns.Add("LINE", typeof(int));
            //Console.SetWindowPosition(-100, -200);

            foreach (string file in Directory.EnumerateFiles(path, "*.php", SearchOption.AllDirectories))
            {
                parent = string.Empty;
                countfiles++;
                string result = System.IO.Path.GetFileName(file);
                if (!(result == "permissions_phpbb.php" || result == "help_bbcode.php" || result == "help_faq.php" || result == "install.php" || result == "search_ignore_words.php" || result == "search_synonyms.php"))
                {
                    string[] lines = System.IO.File.ReadAllLines(file);

                    for (int i = 0; i <= lines.Length - 1; i++)
                    {
                        if ((lines[i].Contains("=>") & (Functions.CountOfChar(lines[i], '\'')) >= 2) || lines[i].Trim() == ")," || lines[i].Trim() == ")")
                        {
                            if ((lines[i].Contains("=>") & lines[i].Contains("array")))//
                            {
                                parent = Functions.AddParent(parent, Functions.LineToObj(lines[i], parent)[0]);

                            }//if array 1
                            else if ((lines[i].Trim() == "),") || lines[i].Trim() == ")")
                            {
                                string[] jj = lines[i].Split(')');
                                parent = Functions.RemoveLastParent(parent);
                                // break;
                            }
                            countline++;
                            if (lines[i].IndexOf("array") == -1)
                            {
                                string[] ObjResult = Functions.LineToObj(lines[i], parent);
                                if (ObjResult.Length == 3)
                                {
                                    Errtable.Rows.Add(ObjResult[2], ObjResult[1], result, i);
                                }
                                else//              esi chisht arjeqnern en 
                                {
                                    if (!(parent == "dateformats." || ObjResult[1].Contains("%")))
                                    {
                                        //Console.WriteLine("\t " + countline + " -- " + ObjResult[0] + " ---> " + ObjResult[1]);
                                        table.Rows.Add(ObjResult); // Add 2 data rows.
                                        if (ObjResult[0] == "tokens.IDENTIFIER")
                                        {
                                            string[] gggg = Functions.LineToObj(lines[i], parent);
                                        }
                                    }

                                }
                            }
                        }// if par 
                    }//for i
                    countfiles++;
                }// IF bb FILES
            }// foreach recursive DIR

            Console.WriteLine("LANG --- {0}\t\t" + "LINES  --- {1} \t\tFILES  --- {2}", langName, countline, countfiles);
            List<DataTable> listToRet = new List<DataTable>();


            listToRet.Add(table);
            listToRet.Add(Errtable);
            return listToRet;


            //System.Console.ReadKey();
        }

        public static string[] LineToObj(string line, string parent)
        {

            string[] ttt = {line.Trim(), Convert.ToString(line.Trim() != ")"), Convert.ToString(line.IndexOf("),") == -1)};
            if (!(line.IndexOf("),") != -1 || line.Trim() == ")"))
            {
                List<string> elements = Functions.CustomSplit(line.Trim(), '\'');
                switch (elements.Count)
                {
                    case 5:
                        string[] ret5 = { parent + elements[1].Trim(), elements[3] };
                        return ret5;
                    case 3:
                        if (line.IndexOf('\'') < line.IndexOf('='))
                        {
                            string[] ret3 = {elements[1], "" };
                            return ret3;
                        }
                        else// (line.IndexOf('\'') > line.IndexOf('='))
                        {
                            string[] subelements = elements[0].TrimStart().Split('=');
                            string[] retsub2 = { parent + Convert.ToString(subelements[0].Trim()), elements[1] };
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

            table.WriteXml( FilePath + ".xml");
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = table.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join("♣", columnNames));

            foreach (DataRow row in table.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join("♣", fields));
            }

            File.WriteAllText(FilePath, sb.ToString());
        }

        public static void MergeTablesWithoutDuplicateRows(ref DataTable parentTable, DataTable childTable)
        {
            if (parentTable.Rows.Count > 0)
            {
                foreach (DataRow row in childTable.Rows)
                {
                    string filter = string.Format("");
                    DataRow[] foundRows = parentTable.Select(filter);
                    if (foundRows.Length > 0)
                    {
                        foreach (DataRow r in foundRows)
                        {
                            foreach (DataColumn dc in r.Table.Columns)
                            {
                                r[dc.ColumnName] = row[dc.ColumnName];
                            }
                        }
                    }
                    else
                    {
                        parentTable.ImportRow(row);
                    }
                }
            }
            else
                parentTable.Merge(childTable);
        }

        public static DataTable MergeAll(this IList<DataTable> tables, String primaryKeyColumn)
        {
            if (!tables.Any())
                throw new ArgumentException("Tables must not be empty", "tables");
            if(primaryKeyColumn != null)
                foreach(DataTable t in tables)
                    if(!t.Columns.Contains(primaryKeyColumn))
                        throw new ArgumentException("All tables must have the specified primarykey column " + primaryKeyColumn, "primaryKeyColumn");

            if(tables.Count == 1)
                return tables[0];

            DataTable table = new DataTable("TblUnion");
            table.BeginLoadData(); // Turns off notifications, index maintenance, and constraints while loading data
            foreach (DataTable t in tables)
            {
                table.Merge(t); // same as table.Merge(t, false, MissingSchemaAction.Add);
            }
            table.EndLoadData();

            if (primaryKeyColumn != null)
            {
                // since we might have no real primary keys defined, the rows now might have repeating fields
                // so now we're going to "join" these rows ...
                var pkGroups = table.AsEnumerable()
                    .GroupBy(r => r[primaryKeyColumn]);
                var dupGroups = pkGroups.Where(g => g.Count() > 1);
                foreach (var grpDup in dupGroups)
                { 
                    // use first row and modify it
                    DataRow firstRow = grpDup.First();
                    foreach (DataColumn c in table.Columns)
                    {
                        if (firstRow.IsNull(c))
                        {
                            DataRow firstNotNullRow = grpDup.Skip(1).FirstOrDefault(r => !r.IsNull(c));
                            if (firstNotNullRow != null)
                                firstRow[c] = firstNotNullRow[c];
                        }
                    }
                    // remove all but first row
                    var rowsToRemove = grpDup.Skip(1);
                    foreach(DataRow rowToRemove in rowsToRemove)
                        table.Rows.Remove(rowToRemove);
                }
            }

            return table;
        }

        public static DataTable XElementToDataTable(XElement x)
        {
            DataTable dt = new DataTable();

            XElement setup = (from p in x.Descendants() select p).First();
            foreach (XElement xe in setup.Descendants()) // build your DataTable
                dt.Columns.Add(new DataColumn(xe.Name.ToString(), typeof(string))); // add columns to your dt

            var all = from p in x.Descendants(setup.Name.ToString()) select p;
            foreach (XElement xe in all)
            {
                DataRow dr = dt.NewRow();
                foreach (XElement xe2 in xe.Descendants())
                    dr[xe2.Name.ToString()] = xe2.Value; //add in the values
                dt.Rows.Add(dr);
            }
            return dt;
        }

            }

