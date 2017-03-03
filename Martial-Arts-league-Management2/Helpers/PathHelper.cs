using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace MartialArts
{
    partial class Helpers
    {
        public static string RootPath = "C:\\Users\\" + Environment.UserName.ToString() + "\\IBJJL_Application_Files\\";
        public static bool IsDirExist;
        public static void checkPath()
        {
            if (Directory.Exists(RootPath) == false)
            {
                // create directory
                try
                {
                    CreateDir();
                }
                catch (Exception ex)
                {
                    IsDirExist = false;
                }
            }

            else
            {
                IsDirExist = true;
            }
        }

        private static void CreateDir()
        {
            try
            {
                Directory.CreateDirectory(RootPath);
                // check again if exist
                if (Directory.Exists(RootPath) == true)
                    IsDirExist = true;
                else // probabley a problem with the pc \ restrictions
                    IsDirExist = false;
            }
            catch
            {
                IsDirExist = false;
            }
        }

        public static void OpenArchiveFolder()
        {
            try
            {
                if (IsDirExist == true) 
                System.Diagnostics.Process.Start(RootPath);
            }
            catch
            {

            }
        }
    }
}
