using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class IO
{
    private int _readOffset = 0;
    private int _writeOffset = 0;

    private readonly byte[] _data;

    public static string GetCrashdayPath()
    {
        string crashdayPath;

        if (PlayerPrefs.HasKey("crashpath"))
        {
	        crashdayPath = PlayerPrefs.GetString("crashpath");
            if (!Directory.Exists(crashdayPath))
            {
	            crashdayPath = EditorUtility.OpenFolderPanel("Select crashday folder", "", "");
                PlayerPrefs.SetString("crashpath", crashdayPath);
            }
        }
        else
        {
	        crashdayPath = EditorUtility.OpenFolderPanel("Select crashday folder", "", "");
            PlayerPrefs.SetString("crashpath", crashdayPath);
        }

        if (!File.Exists(crashdayPath + "/crashday.exe"))
        {
            PlayerPrefs.DeleteKey("crashpath");
	        crashdayPath = GetCrashdayPath();
        }

        return crashdayPath;
    }

	public static string RemoveComment(string input)
	{
		return input.IndexOf('#') > 0 ? input.Remove (input.IndexOf ('#')).Trim () : input.Trim ();
	}

    public IO(byte[] data)
    {
        _data = data;
    }

    public string ReadString()
    {
        for (var i = _readOffset; i < _data.Length; i++)
        {
            if (_data[i] == 0)
            {
                var oldOffset = _readOffset;
	            _readOffset = i + 1;
                return System.Text.Encoding.UTF8.GetString(_data, oldOffset, i - oldOffset);
            }
        }
        return "";
    }

	public void SetReadingOffest(int newOffset)
	{
		_readOffset = newOffset;
	}

	public void AddReadingOffset(int newOffest)
	{
		_readOffset += newOffest;
	}

    public int ReadInt()
    {
	    _readOffset += 4;
        return BitConverter.ToInt32(_data, _readOffset - 4);
    }

    public uint ReadUInt()
    {
	    _readOffset += 4;
        return BitConverter.ToUInt32(_data, _readOffset - 4);
    }

    public ushort ReadUShort()
    {
	    _readOffset += 2;
        return BitConverter.ToUInt16(_data, _readOffset - 2);
    }

    public short ReadShort()
    {
	    _readOffset += 2;
        return BitConverter.ToInt16(_data, _readOffset - 2);
    }

    public float ReadFloat()
    {
	    _readOffset += 4;
        return BitConverter.ToSingle(_data, _readOffset - 4);
    }

    public Vector3 ReadVector3()
    {
        return new Vector3(ReadFloat(), ReadFloat(), ReadFloat());
    }

    public byte ReadByte()
    {
	    _readOffset += 1;
        return _data[_readOffset-1];
    }

    public char ReadChar()
    {
	    _readOffset += 1;
        return BitConverter.ToChar(_data, _readOffset - 1);
    }
}
