using System.Collections;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;

public class PackageManager : MonoBehaviour
{
    public static string[] DefaultArchives = {"dat000.cpk", "dat002.cpk", "dat005.cpk", "dat006.cpk"};
	
    public void LoadDefaultCPKs()
    {
		//disable for testing purposes, to fasten the load time
		//return;

        foreach (string s in DefaultArchives)
            LoadCPK(IO.GetCrashdayPath() + "\\data\\" + s, false);
    }


    private void LoadCPK(string FilePath, bool isMod = false)
    {
        if (System.IO.File.Exists(FilePath) == false)
            return;

        // Read file
        FileStream fs = null;
        try
        {
            fs = new FileStream(FilePath, FileMode.Open);
        }
        catch
        {
            Debug.Log("GameData file open exception: " + FilePath);
        }

        string path = IO.GetCrashdayPath();

        if (isMod) path += "\\moddata\\";
        else path += "\\data\\";

        if (!Directory.Exists(path)) Directory.CreateDirectory(path);

        if (fs != null)
        {
            try
            {
                // Read zip file
                ZipFile zf = new ZipFile(fs);
                int numFiles = 0;

                if (false)
                {
                    Debug.Log("Zip file failed integrity check!");
                    zf.IsStreamOwner = false;
                    zf.Close();
                    fs.Close();
                }
                else
                {
                    foreach (ZipEntry zipEntry in zf)
                    {
                        // Ignore directories
                        if (!zipEntry.IsFile)
                            continue;

                        string entryFileName = zipEntry.Name;

                        //dont load already unpacked files
                        if (File.Exists(path + entryFileName))
                        {
                            //Debug.Log(entryFileName + " was already unpacked");
                            continue;
                        }

                        // Skip .DS_Store files (these appear on OSX)
                        if (entryFileName.Contains("DS_Store"))
                            continue;

                        //Debug.Log("Unpacking zip file entry: " + entryFileName);

                        byte[] buffer = new byte[4096];     // 4K is optimum
                        Stream zipStream = zf.GetInputStream(zipEntry);

                        // Manipulate the output filename here as desired.
                        string fullZipToPath = path + entryFileName;

                        if (!Directory.Exists(path + Path.GetDirectoryName(entryFileName)))
                            Directory.CreateDirectory(path + Path.GetDirectoryName(entryFileName));

                        // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                        // of the file, but does not waste memory.
                        // The "using" will close the stream even if an exception occurs.
                        using (FileStream streamWriter = File.Create(fullZipToPath))
                        {
                            StreamUtils.Copy(zipStream, streamWriter, buffer);
                        }
                        numFiles++;
                    }

                    zf.IsStreamOwner = false;
                    zf.Close();
                    fs.Close();
                }
            }
            catch
            {
                Debug.Log("Zip file error!");
            }
        }
    }
}
