using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileLogger
{
    class ResearchFile
    {
        // Determine if it is a text file
        public static bool IsFileTxt(string pathToFile)
        {
                try
                {
                    string line;
                    StreamReader file = new StreamReader(pathToFile);
                    while ((line = file.ReadLine()) != null)
                    {
                        if (line != "")
                        {
                            if (HasBinaryContent(line))
                            {
                                return false;
                            }
                        }
                    }
                    file.Close();
                }
                catch (FileNotFoundException e)
                {
                    MessageBox.Show(e.StackTrace, "File was not found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.StackTrace, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                return true;
        }
        // if any control character exist (aside from the standard \r\n), then it is probably not a text file.
        private static bool HasBinaryContent(string content)
        {
            return content.Any(ch => char.IsControl(ch) && ch != '\r' && ch != '\n');
        }
    }
}
