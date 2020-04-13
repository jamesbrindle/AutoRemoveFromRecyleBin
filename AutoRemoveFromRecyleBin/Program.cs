using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Shell32;

namespace AutoRemoveFromRecyleBin
{
    class Program
    {
        [DllImport("shell32.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SHUpdateRecycleBinIcon();


        [STAThread]
        static void Main(string[] args)
        {
            while (true)
            {
                string[] recycledFilenames = GetRecycleBinFilenames().ToArray();
                string[] recycledPaths = GetRecycleBinPaths().ToArray();

                for (int i = 0; i < recycledFilenames.Length; i++)
                {
                    if (recycledFilenames[i].EndsWith(".gsl"))
                        File.Delete(recycledPaths[i]);
                }

                SHUpdateRecycleBinIcon();


                Thread.Sleep(30000);
            }
        }

        [STAThread]
        public static IEnumerable<string> GetRecycleBinFilenames()
        {
            Shell shell = new Shell();
            Folder recycleBin = shell.NameSpace(10);

            foreach (FolderItem2 recfile in recycleBin.Items())
            {
                // Filename
                yield return recfile.Name;

                // full recyclepath
                // yield return recfile.Path;
            }

            Marshal.FinalReleaseComObject(shell);
        }

        [STAThread]
        public static IEnumerable<string> GetRecycleBinPaths()
        {
            Shell shell = new Shell();
            Folder recycleBin = shell.NameSpace(10);

            foreach (FolderItem2 recfile in recycleBin.Items())
            {
                // Filename
                yield return recfile.Path;

                // full recyclepath
                // yield return recfile.Path;
            }

            Marshal.FinalReleaseComObject(shell);
        }
    }
}
