using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Xml.Linq;
using System.Collections;
namespace PRUEBAS
{
    class Program
    {
        static void Main()
        {
            //Func.Maintemp();
            string path = @"C:\Users\zvssd\Documents\0000 php\CSV\Book2.xlsx";
            DataTable tbExel = Func.FromExcel(path);
            //IEnumerable<DataTable> tbExel = (IEnumerable<DataTable>) Func.FromExcel(path);
            // DataRow langsArr = new DataRow (tbExel.Rows.Find("USER_LANG"));
            //DataRow[] langsArr = new DataRow();
            //DataRow[] langsArr = tbExel.Select("Variable");
            DataRow User_Lang_ROW = tbExel.Rows[30];// (string 'USER_LANG'); // ("Variable = 'USER_LANG'")[0];  //  AND Sex = 'm'

            //string[] rowAsString = string.Join(", ", result[0].ItemArray);
            //Console.WriteLine("{0,49} ", result[2]);





            List<string> culinfAll = new List<string>();

            for (int i = 1; i < tbExel.Columns.Count; i++)//tbExel.Rows[j][0]
            {
                try
                {
                    List<string> culinf = Func.SamplesCultureInfo(User_Lang_ROW[i].ToString());
                    culinfAll.Add(culinf[2]);

                    XElement root = new XElement(culinf[0]);
                    XElement lang = new XElement("strings");// ("strings", tbExel.Rows[j][i]);
                    lang.SetAttributeValue("lcid", culinf[2]);
                    root.Add(lang);
                    Console.WriteLine("{0,49} ", User_Lang_ROW[i]);

                    for (int j = 0; j < tbExel.Rows.Count; j++)
                
                     {
                         XElement cElement = new XElement("item", tbExel.Rows[j][i]);
                         cElement.SetAttributeValue("key", tbExel.Rows[j][0]);
                         lang.Add(cElement);
                     }
                    //string XmlPath = "C:\\Users\\zvssd\\Documents\\0000 php\\XML\\" + culinf[2] + ".xml";
                    root.Save(@"C:\Users\zvssd\Documents\0000 php\XML\" + culinf[2] + ".xml");
                }
                catch
                {
                    Console.WriteLine(" LANG -- {0} Bad", User_Lang_ROW[i].ToString());

                }
            
            }





            //Func.SamplesCultureInfo("zh");
        }



    }

}
