using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class IO
{
    public int ReadOffset = 0;
    public int writeOffset = 0;

    private readonly byte[] _data;

    public static string GetCrashdayPath()
    {
        string CrashdayPath = "";
        if (PlayerPrefs.HasKey("crashpath"))
        {
            CrashdayPath = PlayerPrefs.GetString("crashpath");
            if (!Directory.Exists(CrashdayPath))
            {
                CrashdayPath = EditorUtility.OpenFolderPanel("Select crashday folder", "", "");
                PlayerPrefs.SetString("crashpath", CrashdayPath);
            }
        }
        else
        {
            CrashdayPath = EditorUtility.OpenFolderPanel("Select crashday folder", "", "");
            PlayerPrefs.SetString("crashpath", CrashdayPath);
        }
        if (!File.Exists(CrashdayPath + "/crashday.exe"))
        {
            PlayerPrefs.DeleteKey("crashpath");
            CrashdayPath = GetCrashdayPath();
        }
        return CrashdayPath;
    }

	public static string RemoveComment(string input)
	{
		if(input.IndexOf('#') > 0)
		{
			return input.Remove (input.IndexOf ('#')).Trim ();
		}
		else
		{
			return input.Trim ();			
		}
	}

    public IO(byte[] data)
    {
        this._data = data;
    }

    public string ReadString()
    {
        for (int i = ReadOffset; i < _data.Length; i++)
        {
            if (_data[i] == 0)
            {
                int oldOffset = ReadOffset;
                ReadOffset = i + 1;
                return System.Text.Encoding.UTF8.GetString(_data, oldOffset, i - oldOffset);
            }
        }
        return "";
    }

    public int ReadInt()
    {
        ReadOffset += 4;
        return BitConverter.ToInt32(_data, ReadOffset - 4);
    }

    public uint ReadUInt()
    {
        ReadOffset += 4;
        return BitConverter.ToUInt32(_data, ReadOffset - 4);
    }

    public ushort ReadUShort()
    {
        ReadOffset += 2;
        return BitConverter.ToUInt16(_data, ReadOffset - 2);
    }

    public short ReadShort()
    {
        ReadOffset += 2;
        return BitConverter.ToInt16(_data, ReadOffset - 2);
    }

    public float ReadFloat()
    {
        ReadOffset += 4;
        return BitConverter.ToSingle(_data, ReadOffset - 4);
    }

    public Vector3 ReadVector3()
    {
        return new Vector3(ReadFloat(), ReadFloat(), ReadFloat());
    }

    public byte ReadByte()
    {
        ReadOffset += 1;
        return _data[ReadOffset-1];
    }

    public char ReadChar()
    {
        ReadOffset += 1;
        return BitConverter.ToChar(_data, ReadOffset - 1);
    }
}
