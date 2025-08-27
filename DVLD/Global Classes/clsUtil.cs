using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace DVLD
{
    public class clsUtil
    {

        public static string GenerateGuid()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }
        
        
        public static string ReplaceFileNameWithGUID(string SourceFilePath)
        {
            
            FileInfo File = new FileInfo(SourceFilePath);
            string ext = File.Extension;

            return GenerateGuid() + ext;
        }


        public static bool CreateFolderIfDoesNotExist(string Folder)
        {
            if (!Directory.Exists(Folder))
            {
                try
                {
                    Directory.CreateDirectory(Folder);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error creating folder: " + ex.Message);
                    return false;
                }
            }

            return true;
        }

        public static bool CopyPersonImageToImagesFolder(ref string SourceFilePath)
        {
            string DestinationFolder = @"C:\DVLD-People-Images-Mine\";

            if (!CreateFolderIfDoesNotExist(DestinationFolder))
            {
                return false;
            }

            string destinationFile = DestinationFolder + ReplaceFileNameWithGUID(SourceFilePath);

            try
            {
                File.Copy(SourceFilePath, destinationFile, true);
            }

            catch (IOException iox)
            {
                MessageBox.Show(iox.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SourceFilePath = destinationFile;
            return true;
        }

    }
}
