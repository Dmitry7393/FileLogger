using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileLogger
{
    public partial class Form1 : Form
    {
        private string pathFileLogger = "FileLogger.txt";
        private bool isCompleted = true;
        Queue<string> queueFiles = new Queue<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //If user has deleted FileLogger.txt - create new one
            CreateFileLoggerIfNotExist();

            //Add new file to queue, where it will be handled
            queueFiles.Enqueue(txtBoxFilePath.Text);
            WriteToFileLogger();
        }
        private async void WriteToFileLogger()
        {
            if (isCompleted == true)
            {
                if(queueFiles.Count > 0)
                await HandleFile(queueFiles.Dequeue());
            }
        }
        // The following method runs asynchronously. 
        // The UI thread is not blocked during the delay
        private async Task HandleFile(string filePath)
        {   
            await Task.Run(() =>
            {
                isCompleted = false;
                try
                {
                    if (ResearchFile.IsFileTxt(txtBoxFilePath.Text))
                    {
                        //Append lines to FileLogger.txt
                        using (StreamWriter sw = File.AppendText(pathFileLogger))
                        {

                            string line;
                            StreamReader file = new StreamReader(filePath);
                            while ((line = file.ReadLine()) != null)
                            {
                                sw.WriteLine(line);
                            }
                            file.Close();
                        }
                    }   
                }
                catch (FileNotFoundException e)
                {
                    MessageBox.Show(e.StackTrace, "File was not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.StackTrace, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                isCompleted = true;
            });
            WriteToFileLogger();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CreateFileLoggerIfNotExist();
        }
        private void CreateFileLoggerIfNotExist()
        {
            if (!File.Exists(pathFileLogger))
            {
                File.Create(pathFileLogger).Dispose();
            }
        }
    }
}
