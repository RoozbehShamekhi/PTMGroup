using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace PTMGroup.Common.Plugins
{
    public class FileManagement
    {
        public FileManagement()
        {

        }

        public void DeleteFileWithPath(string path)
        {

            if (File.Exists(path))
            {
                File.Delete(path);
            }

        }

        public void Dir_Empty(string root)
        {
            if (Directory.Exists(root))
            {

                int x = Directory.GetFiles(root).Count();

                if (x == 0)
                {
                    Directory.Delete(root);
                }
            }
        }
    }
}