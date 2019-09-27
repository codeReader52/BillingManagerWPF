using Microsoft.Win32;
using System.IO;
using System.Windows;
using System;

namespace BillingManagement.Utils
{
    public class FileToByteStreamUiService : IPopUpWinService<string, byte[]>
    {
        public string Input { get; set; }
        public byte[] Output { get; set; }

        public void DoModal()
        {
            OpenFileDialog filePicker = new OpenFileDialog();
            filePicker.InitialDirectory = Environment.CurrentDirectory;
            Output = new byte[] { };

            if(filePicker.ShowDialog() == true)
            {
                string fileNamePath = filePicker.FileName;

                Output = new byte[0];
                try
                {
                    Output = File.ReadAllBytes(fileNamePath);
                    return;
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Unable to parse file due to error: {e.ToString()}.\nNothing is saved.");
                }
            }
            else
            {
                Output = new byte[0];
            }
        }
    }
}
