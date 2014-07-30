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
        //string path = @"C:\Users\zvssd\Documents\0000 php\american_english_1_3_0\language\en_us";
        string path = @"C:\Users\zvssd\Documents\0000 php\test";
        string PathAndFile = @"C:\Users\zvssd\Documents\0000 php\american_english_1_3_0\language\en_us\captcha_qa.php";
        string cvsfile = @"C:\Users\zvssd\Documents\0000 php\test.csv";
        string text1 = System.IO.File.ReadAllText(PathAndFile);
        int countline = 0;
        int countfiles = 0;
        Console.SetWindowSize(160, 30);
        DataTable table = new DataTable();
       // table.Columns.Add("ID", typeof(date));
        table.Columns.Add("Variable", typeof(string));
        table.Columns.Add("Value English", typeof(string));

        //Console.SetWindowPosition(-100, -200);

         foreach (string file in Directory.EnumerateFiles(path, "*.php", SearchOption.AllDirectories))
        {
            countfiles++;
        //string file = @"C:\Users\zvssd\Documents\0000 php\american_english_1_3_0\language\en_us\common.php";
        string result = System.IO.Path.GetFileName(file);
        if (result != "permissions_phpbb.php")
        {
            string[] lines = System.IO.File.ReadAllLines(file);
          
            // Console.WriteLine(file);

            Console.WriteLine(result);

            for (int i = 0; i <= lines.Length - 1; i++)
            {
                if ((lines[i].Contains("=>") & (Functions.CountOfChar(lines[i], '\'')) >= 2) || lines[i].Contains("),"))
                {
                    if ((lines[i].Contains("=>") & lines[i].Contains("array")) )//|| lines[i].Contains("),"))
                    {
                        parent = Functions.AddParent(parent, Functions.LineToObj(lines[i], parent)[0]);
                        
                    }//if array 1
                    else if (lines[i].IndexOf("),") != -1)        //   lines[i].Contains("),")) ;
                    {
                       parent = Functions.RemoveLastParent(parent);
                      // break;
                    }
                    countline++;
                    if (lines[i].IndexOf("array")==-1)
                    {
                    Console.WriteLine("\t " + countline + " -- " + Functions.LineToObj(lines[i], parent)[0] + " ---> " + Functions.LineToObj(lines[i], parent)[1]);
                    //table.Rows.Add(Functions.LineToObj(lines[i], parent)); // Add five data rows.
                  

                    }
                }// if par 
                //else
                //{
                //    countline++;
                //    Console.WriteLine(countline);
                //}
            }//for i
            countfiles++;
            } // foreach recursive DIR
        }

         Console.WriteLine(" LINES  --- {0} \nLINES  --- {1}", countline, countfiles);
         string ll = "		'M jS, \'y, H:i'		=> 'Jan 1st, \'07, 13:37',";
         List<string> rrr = Functions.CustomSplit(ll, '\'');
         foreach (string gggg in rrr)
         {
             Console.WriteLine(gggg);
         }
         


         StringBuilder sb = new StringBuilder();

         IEnumerable<string> columnNames = table.Columns.Cast<DataColumn>().
                                           Select(column => column.ColumnName);
         sb.AppendLine(string.Join(",", columnNames));

         foreach (DataRow row in table.Rows)
         {
             IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
             sb.AppendLine(string.Join(",", fields));
         }

         File.WriteAllText(cvsfile, sb.ToString());

       
        // DirSearch (PathAndFile);
        // Keep the console window open in debug mode.
        System.Console.ReadKey();
    }






}