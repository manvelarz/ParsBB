using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Data;
using System.Data.OleDb;

namespace PRUEBAS
{
    public class Func
    {

        public static List<string> SamplesCultureInfo(string lang)
        {
            CultureInfo[] cinfo = CultureInfo.GetCultures(CultureTypes.AllCultures);
             List<string> Ret = new List<string>();
             for (int i = 0; i < cinfo.Length; i++ )
             {
                 if (lang.Length == 2)
                 {
                     if (cinfo[i].TwoLetterISOLanguageName == lang)
                     {
                         Ret.Add(cinfo[i].Name);
                         Ret.Add(cinfo[i].EnglishName);
                         Ret.Add(cinfo[i].LCID.ToString("X4"));
                         //return Ret;
                     }
                 }
                 if (cinfo[i].Name == lang)
                 {
                     Ret.Add(cinfo[i].Name);
                     Ret.Add(cinfo[i].EnglishName);
                     Ret.Add(cinfo[i].LCID.ToString("X4"));
                     //return Ret;
                 }
                // Console.WriteLine("{0} -- {1} -- {2}", Ret[0], Ret[1], Ret[2]);
             }
             return Ret;

            //    //if (ci.TwoLetterISOLanguageName == lang)
            //    //{
            //    //    Console.WriteLine(ci.LCID.ToString("X4") + "\t--\t" + ci.Name + "\t--\t" + ci.EnglishName);
            //    //    //Console.Write("0x{0} {1} {2,-37}", ci.LCID.ToString("X4"), ci.Name, ci.EnglishName);
            //    //    //Console.WriteLine("0x{0} {1} {2}", ci.Parent.LCID.ToString("X4"), ci.Parent.Name, ci.Parent.EnglishName);


            //    //}
            //    ////else {

            //    ////    List<string> Res2 = new List<string>();
            //    ////    Res2.Add(ci.Name);
            //    ////    Res2.Add(ci.EnglishName);
            //    ////    Res2.Add(ci.LCID.ToString("X4"));
            //    ////    return Res2;

            //    ////}
            //    ////List<string> rees = new List<string>();
            //    ////return rees;
          


        }

        public static void Maintemp()
        {

            // Lists the cultures that use the Chinese language and determines if each is a neutral culture. 
            foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                if (ci.TwoLetterISOLanguageName == "ku")
                {
                    Console.Write("{0,-7} {1,-40}", ci.Name, ci.EnglishName);
                    if (ci.IsNeutralCulture)
                    {
                        Console.WriteLine(": neutral");
                    }
                    else
                    {
                        Console.WriteLine(": specific");
                    }
                }
            }

        }


        public static void printCultureInfo()
        {
            CultureInfo[] cinfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);


           
            for (int i = 0; i < cinfo.Length; i++) 
            {
                if (cinfo[i].TwoLetterISOLanguageName == "zh")
                {
                    Console.WriteLine("{0} --- {1} --- {2}", cinfo[i].Name, cinfo[i].EnglishName, cinfo[i].LCID.ToString("X4"));
                }
            }
        }

        public static DataTable FromExcel(string filePath)
        {
            DataTable dtexcel = new DataTable("FromExcel_Sheet1");
            bool hasHeaders = false;
            string HDR = hasHeaders ? "Yes" : "No";
            string strConn;
            if (filePath.Substring(filePath.LastIndexOf('.')).ToLower() == ".xlsx")
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\"";
            else
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            //Looping Total Sheet of Xl File
            /*foreach (DataRow schemaRow in schemaTable.Rows)
            {
            }*/
            //Looping a first Sheet of Xl File
            DataRow schemaRow = schemaTable.Rows[0];
            string sheet = schemaRow["TABLE_NAME"].ToString();
            if (!sheet.EndsWith("_"))
            {
                string query = "SELECT  * FROM [Sheet1$]";  //[" + sheet3 + "]"
                OleDbDataAdapter daexcel = new OleDbDataAdapter(query, conn);
                dtexcel.Locale = CultureInfo.CurrentCulture;
                daexcel.Fill(dtexcel);
            }

            conn.Close();
            return dtexcel;


        }




    }
}