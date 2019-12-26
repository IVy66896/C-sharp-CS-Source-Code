
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.Windows;
public class ZipHelper
{
   
    public static void ZipFile(string fileToZip, string zipedFile, int compressionLevel, int blockSize)
    {
        
        if (!System.IO.File.Exists(fileToZip))
        {
            throw new System.IO.FileNotFoundException("
        }

        using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile))
        {
            using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
            {
                using (System.IO.FileStream StreamToZip = new System.IO.FileStream(fileToZip, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);

                    ZipEntry ZipEntry = new ZipEntry(fileName);

                    ZipStream.PutNextEntry(ZipEntry);

                    ZipStream.SetLevel(compressionLevel);

                    byte[] buffer = new byte[blockSize];

                    int sizeRead = 0;

                    try
                    {
                        do
                        {
                            sizeRead = StreamToZip.Read(buffer, 0, buffer.Length);
                            ZipStream.Write(buffer, 0, sizeRead);
                        }
                        while (sizeRead > 0);
                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }

                    StreamToZip.Close();
                }

                ZipStream.Finish();
                ZipStream.Close();
            }

            ZipFile.Close();
        }
    }

    
    public static void ZipFile(string fileToZip, string zipedFile)
    {
      
        if (!File.Exists(fileToZip))
        {
            throw new System.IO.FileNotFoundException("

        using (FileStream fs = File.OpenRead(fileToZip))
        {
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();

            using (FileStream ZipFile = File.Create(zipedFile))
            {
                using (ZipOutputStream ZipStream = new ZipOutputStream(ZipFile))
                {
                    string fileName = fileToZip.Substring(fileToZip.LastIndexOf("\\") + 1);
                    ZipEntry ZipEntry = new ZipEntry(fileName);
                    ZipStream.PutNextEntry(ZipEntry);
                    ZipStream.SetLevel(5);

                    ZipStream.Write(buffer, 0, buffer.Length);
                    ZipStream.Finish();
                    ZipStream.Close();
                }
            }
        }
    }

    
    public static void ZipFileDirectory(string strDirectory, string zipedFile)
    {
        using (System.IO.FileStream ZipFile = System.IO.File.Create(zipedFile))
        {
            using (ZipOutputStream s = new ZipOutputStream(ZipFile))
            {
                ZipSetp(strDirectory, s, "");
            }
        }
    }

    
    private static void ZipSetp(string strDirectory, ZipOutputStream s, string parentPath)
    {
        if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar)
        {
            strDirectory += Path.DirectorySeparatorChar;
        }
        Crc32 crc = new Crc32();

        string[] filenames = Directory.GetFileSystemEntries(strDirectory);

        foreach (string file in filenames)
        {

            if (Directory.Exists(file))
            {
                string pPath = parentPath;
                pPath += file.Substring(file.LastIndexOf("\\") + 1);
                pPath += "\\";
                ZipSetp(file, s, pPath);
            }

            else
            {
                
                using (FileStream fs = File.OpenRead(file))
                {

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);

                    string fileName = parentPath + file.Substring(file.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(fileName);

                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;

                    fs.Close();

                    crc.Reset();
                    crc.Update(buffer);

                    entry.Crc = crc.Value;
                    s.PutNextEntry(entry);

                    s.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }

  
    public void UnZip(string zipedFile, string strDirectory, string password, bool overWrite)
    {
        ZipInputStream s = null;
        try
        {

            if (strDirectory == "")
                strDirectory = Directory.GetCurrentDirectory();
            if (!strDirectory.EndsWith("\\"))
                strDirectory = strDirectory + "\\";

            using (s = new ZipInputStream(File.OpenRead(zipedFile)))
            {
                s.Password = password;
                ZipEntry theEntry;

                while ((theEntry = s.GetNextEntry()) != null)
                {
                    theEntry.IsUnicodeText = true;
                    //LogTool.genMyErrorLog(theEntry.Name,"");
                    string directoryName = "";
                    string pathToZip = "";
                    pathToZip = theEntry.Name;

                    if (pathToZip.Contains("?"))
                    {
                        pathToZip = pathToZip.Replace('?', '_');
                        //MessageBox.Show("222");
                    }

                    if (pathToZip != "")
                        directoryName = Path.GetDirectoryName(pathToZip) + "\\";

                    string fileName = Path.GetFileName(pathToZip);

                    Directory.CreateDirectory(strDirectory + directoryName);

                    //if (fileName.Contains("Thumbnails"))
                    //{
                    //    MessageBox.Show("dd");
                    //}
                    if (fileName != "")
                    {
                        try
                        {
                            if ((File.Exists(strDirectory + directoryName + fileName) && overWrite) || (!File.Exists(strDirectory + directoryName + fileName)))
                            {
                                using (FileStream streamWriter = File.Create(strDirectory + directoryName + fileName))
                                {
                                    int size = 2048;
                                    byte[] data = new byte[2048];
                                    try
                                    {
                                        while (true)
                                        {
                                            size = s.Read(data, 0, data.Length);

                                            if (size > 0)
                                                streamWriter.Write(data, 0, size);
                                            else
                                                break;
                                        }
                                        streamWriter.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        streamWriter.Close();
                                    }
                                   
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }

                s.Close();
            }
        }
        catch (Exception ex)
        {
            try
            {
                s.Close();
            }
            catch
            {
            }
        }

    }


    public int GetZipFileCount(string zipedFile)
    {
        int count = 0;
        ZipInputStream s = null;
        try
        {
            using (s = new ZipInputStream(File.OpenRead(zipedFile)))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    count++;
                }

                s.Close();
            }
        }
        catch (Exception ex)
        {
            try
            {
                s.Close();
            }
            catch
            {
            }
            count = 0;
        }


        return count - 1;

    }

}
