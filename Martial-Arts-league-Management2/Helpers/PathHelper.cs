using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace MartialArts
{
    partial class Helpers
    {
        

        public static string RootPath = "C:\\Users\\" + Environment.UserName.ToString() + "\\IBJJL_Application_Files\\";
        public static string SavedProjects = RootPath + "Projects\\";
        public static string SysData = RootPath + "SysData\\";

        public static bool IsDirExist;
        public static void checkPath()
        {
            Init(SavedProjects);
            // root path must be the last because IsDirExist is representing only that path
            Init(RootPath);  
            Init(SysData);
        
        }


     
            
       
        protected static void Init(string path)
        {
            if (Directory.Exists(path) == false)
            {
                // create directory
                try
                {
                    CreateDir(path);
                    // in the first time the app loads create desktop shortcut
                    if (path == SysData)
                    {
                        // do something in the future
                    }
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

        private static void CreateDir(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
                // check again if exist
                if (Directory.Exists(path) == true)
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

    class BinaryFiles : Helpers
    {
        public static string SavedAllContsBinaryFilePath
        {
            get
            {
                string filename = "AllVcSaveddata.bin";
                return Path.Combine(SavedProjects, filename);
            }
        }

        public static string SavedVbBinaryFilePath
        {
            get
            {
                string filename = "VbSaveddata.bin";
                return Path.Combine(SavedProjects, filename);
            }
        }

        public static string SavedUnplacedVcBinaryFilePath
        {
            get
            {
                string filename = "UnplacedVcSaveddata.bin";
                return Path.Combine(SavedProjects, filename);
            }
        }

        public static bool SavedDataExist(string filePath,out string LastSaved)
        {
            if (System.IO.File.Exists(filePath))
            {
                LastSaved = System.IO.File.GetLastAccessTime(filePath).ToString();
                return true;
            }
            else
            {
                LastSaved = "";
                return false;
            }
        }
    }

    class ProjectsSavedAsBinaryFiles : BinaryFiles
    {
        public string ProjectDirName = "";
        public string ProjectFullDir { get; set; } = "";
        public ProjectsSavedAsBinaryFiles(string ProjectDirName)
        {
            this.ProjectDirName = ProjectDirName;
        }

        public void SetFullDirWithoutCreating()
        {
            ProjectFullDir = SavedProjects + "\\" + ProjectDirName + "\\";
        }

        public bool CreateSubPath()
        {
            // first chack main dir
            if (IsProjectsPathExist(SavedProjects) == false)
            {
                return false;
            }

            // check if allready there is such project name
            if (IsProjectsPathExist(SavedProjects + "\\" + ProjectDirName) == true)
            {
                ShowGenericPromtForm("כבר קיים פרויקט בשם " + ProjectDirName + " " + "אנא בחר שם אחר");
                return false;
            }
                // create sub dir and discover if exist
                Init(SavedProjects + "\\" + ProjectDirName);
            if (IsProjectsPathExist(SavedProjects + "\\" + ProjectDirName) == true)
            {
                ProjectFullDir = SavedProjects + "\\" + ProjectDirName + "\\";
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool IsProjectsPathExist(string path)
        {
            // check again if exist
            if (Directory.Exists(path) == true)
            {
                return true;
            }
            else
            {
                // probabley a problem with the pc \ restrictions
                return false;
            }
        }


        public new string SavedAllContsBinaryFilePath
        {
            get
            {
                if (ProjectFullDir == "")
                    throw new Exception("Parent Directory is not Exist you must use bool CreateSubPath() to check if parent and child derectories created succesfuly");

                string filename = "AllVcSaveddata.bin";
                return Path.Combine(ProjectFullDir, filename);
            }
        }

        public new string SavedVbBinaryFilePath
        {
            get
            {
                if (ProjectFullDir == "")
                    throw new Exception("Parent Directory is not Exist you must use bool CreateSubPath() to check if parent and child derectories created succesfuly");

                string filename = "VbSaveddata.bin";
                return Path.Combine(ProjectFullDir, filename);
            }
        }

        public new string SavedUnplacedVcBinaryFilePath
        {
            get
            {
                if (ProjectFullDir == "")
                    throw new Exception("Parent Directory is not Exist you must use bool CreateSubPath() to check if parent and child derectories created succesfuly");

                string filename = "UnplacedVcSaveddata.bin";
                return Path.Combine(ProjectFullDir, filename);
            }
        }

        public static bool IsProjectDirExist()
        {

            // check again if exist
            if (Directory.Exists(SavedProjects) == true)
            {
                return true;
            }
            else
            {
                // probabley a problem with the pc \ restrictions
                return false;
            }
        }

        public static List<string> GetProjectNames()
        {
            List<string> result = new List<string>();

            if (IsProjectDirExist() == false)
            {
                return result;
            }

            foreach (string s in Directory.GetDirectories(SavedProjects))
            {
               result.Add(s.Remove(0, SavedProjects.Length));
            }

            return result;
        }

        public void DeleteProject()
        {
            if (ProjectFullDir == "")
                throw new Exception("you must use the method SetFullDirWithoutCreating() before deleting");

            if (MartialArts.Helpers.YesNoMessegeBox("האם אתה בטוח שברצונך למחוק את הפרויקט " + Environment.NewLine + ProjectDirName, "מחיקת פרויקט", System.Windows.Forms.MessageBoxIcon.Question) == true)
            {
                Directory.Delete(ProjectFullDir,true);
            }
        }

    }
}
