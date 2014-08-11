using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel;
using System.Linq;


class ReadFromFile
{
    static string parent;


    static void Main()
    {
        string cvsfile = @"C:\Users\zvssd\Documents\0000 php\CSV\";

        //string path = @"C:\Users\zvssd\Documents\0000 php\american_english_1_3_0\language\en_us";
        //string path = @"C:\Users\zvssd\Documents\0000 php\spanish_(casual_honorifics)_1_0_6";
        //string path = @"C:\Users\zvssd\Documents\0000 php\russian_1_0_11";
        //string path = @"C:\Users\zvssd\Documents\0000 php\arabic_1_6_0_3";
        string path = @"C:\Users\zvssd\Documents\0000 php";
        string[] dirs = Directory.GetDirectories(path);
        List<DataTable> AllLangTableList = new List<DataTable>();
        List<DataTable> AllERRTableList = new List<DataTable>();
        DataTable DataTable_All = new DataTable();
        foreach (string Papka in dirs )
        {
            //Console.WriteLine(Path.GetFileName(Papka));
            String langName = Path.GetFileName(Papka);
            List<DataTable> CurrtbList = GetTablesFromPath(Papka, langName);
            if (langName == "en_us")
            {
                AllLangTableList.Insert(0, CurrtbList[0]);
                AllERRTableList.Insert(0, CurrtbList[1]);
            }
            else
            {
                AllLangTableList.Add(CurrtbList[0]);
                AllERRTableList.Add(CurrtbList[1]);
            }

            Functions.Table2Cvs(cvsfile + Path.GetFileName(Papka) + ".csv", CurrtbList[0]);
            Functions.Table2Cvs(cvsfile + Path.GetFileName(Papka) + "Err.csv", CurrtbList[1]);
            
        }

        int t0Count = AllLangTableList[0].Rows.Count;
        int t1Count = AllLangTableList[1].Rows.Count;
        string ColName = AllLangTableList[0].Columns[1].ColumnName;
        string ColName1 = AllLangTableList[1].Columns[1].ColumnName;
        //string ColName2 = AllLangTableList[0].Columns[1].ColumnName;
        //var tablesJoinend = from t1 in AllLangTableList[0].Rows.Cast<DataRow>()
        //                    join t2 in AllLangTableList[1].Rows.Cast<DataRow>() on t1["Variable"] equals t2["Variable"]
        //                    //join t3 in AllLangTableList[2].Rows.Cast<DataRow>() on t1["Variable"] equals t3["Variable"]
        //                    select new  {t1 
        //                    };
        //DataTable_All = tablesJoinend.CopyToDataTable();
        var query =
            from t1 in AllLangTableList[0].AsEnumerable()
            join t2 in AllLangTableList[1].AsEnumerable() on t1[0] equals t2[0] 
            //into gj
            //from subgj in gj.DefaultIfEmpty()
            select new { ID = t1[0], Field1 = t1[1], Field2 = t2[1] };
 
        DataTable newTable = new DataTable();
        newTable.Columns.Add("Variable", typeof(string));
        newTable.Columns.Add("Field1", typeof(string));
        newTable.Columns.Add("Field2",typeof(string));



        foreach (var rowInfo in query) 
        {
            newTable.Rows.Add(rowInfo.ID, rowInfo.Field1, rowInfo.Field2);
        }

        int ResultRowCount = newTable.Rows.Count;
        DataTable RemNull = newTable.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare((field as string).Trim(), string.Empty) == 0)).CopyToDataTable();

        Functions.Table2Cvs(cvsfile + "Result" + ".csv", RemNull);

        Console.ReadKey();
        
        //var joinedList = (from t1 in AllLangTableList[0].Rows.Cast<DataRow>()
        //                  join t2 in AllLangTableList[1].Rows.Cast<DataRow>() on t1.Table.Columns[0] equals t2.Table.Columns[0] into temp
        //   from t2 in temp.DefaultIfEmpty()
        //   select new 
        //   {
        //       t1.Table.Columns[0].ColumnName,
        //       t1.Table.Columns[1].ColumnName,
        //       t2.Table.Columns[0].ColumnName,
        //       t2.Table.Columns[1].ColumnName


        //     //t1.Variable
        //     //,t2.Variable
        //     //, ProductName = detail == null ? String.Empty : detail.ProductName
        //     //, Amount = detail == null ? null : detail.Amount
        //   });

        //DataTable_All = joinedList.CopyToDataTable();

        //var joinedList = (from ord in orders
        //   join detail in orderDetails on ord.OrderID equals detail.OrderID into temp
        //   from detail in temp.DefaultIfEmpty()
        //   select new
        //   {
        //     ord.CustomerName
        //     , ord.OrderDate 
        //     , ProductName = detail == null ? String.Empty : detail.ProductName
        //     , Amount = detail == null ? null : detail.Amount
        //   });

        //foreach (DataColumn column in AllLangTableList[0].Columns)
        //{
        //    Console.WriteLine(row[column]);
        //}







        //for (int i = 1; i < AllLangTableList.Count; i++)
        //{
        //    var tablesJoinend = from t1 in AllLangTableList[i-1].Rows.Cast<DataRow>()
        //                        join t2 in AllLangTableList[i].Rows.Cast<DataRow>() on t1["Variable"] equals t2["Variable"]
        //                        select t1;
        //    DataTable_All = tablesJoinend.CopyToDataTable();

        //}
        

       //List<DataTable> tbList = GetTablesFromPath(path, langName);




    }


    public static List<DataTable> GetTablesFromPath(string path, string langName)
    {

        //string PathAndFile = @"C:\Users\zvssd\Documents\0000 php\american_english_1_3_0\language\en_us\captcha_qa.php";
        //string text1 = System.IO.File.ReadAllText(PathAndFile);
        int countline = 0;
        int countfiles = 0;
        Console.OutputEncoding = Encoding.UTF8;
        Console.SetWindowSize(160, 30);
        DataTable table = new DataTable(langName);
       // table.Columns.Add("ID", typeof(date));
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

            //Console.WriteLine("{0} --- {1}", file, Functions.GetEncoding(file));
            countfiles++;
            //string file = @"C:\Users\zvssd\Documents\0000 php\american_english_1_3_0\language\en_us\common.php";
            string result = System.IO.Path.GetFileName(file);
            if (!(result == "permissions_phpbb.php" || result == "help_bbcode.php" || result == "help_faq.php" || result == "install.php"))
            {
                string[] lines = System.IO.File.ReadAllLines(file);

                //Console.WriteLine(result);

                //Console.WriteLine(result + " \n \n");

                for (int i = 0; i <= lines.Length - 1; i++)
                {
                    if ((lines[i].Contains("=>") & (Functions.CountOfChar(lines[i], '\'')) >= 2) || lines[i].Contains("),"))
                    {
                        if ((lines[i].Contains("=>") & lines[i].Contains("array")))//|| lines[i].Contains("),"))
                        {
                            parent = Functions.AddParent(parent, Functions.LineToObj(lines[i], parent)[0]);

                        }//if array 1
                        else if (lines[i].IndexOf("),") != -1)       
                        {
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
  //                                      Console.WriteLine("\t " + countline + " -- " + ObjResult[0] + " ---> " + ObjResult[1]);
                                        table.Rows.Add(ObjResult); // Add 2 data rows.
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

         //string ll = @"		'M jS, \'y, H:i'		=> 'Jan 1st, \'07, 13:37',";
         //List<string> rrr = Functions.CustomSplit(ll, '\'');
         //foreach (string gggg in rrr)
         //{
         //    Console.WriteLine("-"+gggg+"-");
         //}
         


 

       
        // DirSearch (PathAndFile);
        // Keep the console window open in debug mode.
        System.Console.ReadKey();
    }






}