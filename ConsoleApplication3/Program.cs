using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Data;


class ReadFromFile
{
    


    static void Main()
    {
        string cvsfile = @"C:\Users\zvssd\Documents\0000 php\CSV\";
        string path = @"C:\Users\zvssd\Documents\0000 php\Lang Packs\";
        string[] dirs = Directory.GetDirectories(path);
        List<DataTable> AllLangTableList = new List<DataTable>();
        List<DataTable> AllERRTableList = new List<DataTable>();
        DataTable DataTable_All = new DataTable();
        foreach (string Papka in dirs )
        {
            //Console.WriteLine(Path.GetFileName(Papka));
            String langName = Path.GetFileName(Papka);
            List<DataTable> CurrtbList = Functions.GetTablesFromPath(Papka, langName);
            if (langName == "en")
            {
                AllLangTableList.Insert(0, CurrtbList[0]);
                AllERRTableList.Insert(0, CurrtbList[1]);
            }
            else
            {
                AllLangTableList.Add(CurrtbList[0]);
                AllERRTableList.Add(CurrtbList[1]);
            }
            
            //Functions.Table2Cvs(cvsfile + Path.GetFileName(Papka) + ".csv", CurrtbList[0]);
            //Functions.Table2Cvs(cvsfile + Path.GetFileName(Papka) + "Err.csv", CurrtbList[1]);
            
        }



        int t0Count = AllLangTableList[0].Rows.Count;
        int t1Count = AllLangTableList[1].Rows.Count;
        string ColName = AllLangTableList[0].Columns[1].ColumnName;
        string ColName1 = AllLangTableList[1].Columns[1].ColumnName;

        DataTable TblUnion = AllLangTableList.MergeAll("Variable");

        Functions.Table2Cvs(cvsfile + "Result" + ".csv", TblUnion);

         Console.ReadKey();
 


    }









}