using prj202405.lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1

{

    [ComVisible(true)]
    public partial class Form1 : Form
    {
        public void m1()
        {
            MessageBox.Show("Add function called from HTML!");
        }
        public Form1()
        {
            testCls.test();
        corex.    SetFeatures(55000);
            InitializeComponent();
            //webBrowser1.Document.InvokeScript("MyCLickFunction");
            
           // new ScriptManager(this).list_click();


            string filePath = @"../../idx.htm";
            filePath = filex.GetAbsolutePath(filePath);
            //  filePath = @"idx.htm";
            Console.WriteLine(filePath);
            File.AppendAllText("log2024.log", filePath);
            Console.WriteLine("File.Exists(filePath=>" + File.Exists(filePath));
            File.AppendAllText("log2024.log", "\nFile.Exists(filePath=>" + File.Exists(filePath));
            // Enable JavaScript in the WebBrowser control
            webBrowser1.ObjectForScripting = new ScriptManager(this);
            this.webBrowser1.Navigate(new Uri(filePath));

          
        }

        //dep
        //private void Form1_Load(object sender, EventArgs e)
        //{
        //    string filePath = @"D:\0prj\mdsj\WindowsFormsApp1\idx.htm";
        //    //  filePath = @"idx.htm";
        //    Console.WriteLine(filePath);
        //    File.AppendAllText("log2024.log", filePath);
        //    Console.WriteLine("File.Exists(filePath=>"+File.Exists(filePath));
        //    File.AppendAllText("log2024.log", "\nFile.Exists(filePath=>" + File.Exists(filePath));
        //    // Enable JavaScript in the WebBrowser control
        //    webBrowser1.ObjectForScripting = new ScriptManager(this);
        //    this.webBrowser1.Navigate(new Uri(filePath));
        //}

            private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // Set the path to the local HTML file
           // string filePath = @"D:\0prj\mdsj\WindowsFormsApp1\idx.htm";
          //  this.webBrowser1.Navigate(new Uri(filePath));
        }
    }
}
