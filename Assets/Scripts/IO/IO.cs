using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SFB;
using UnityEngine;

public class IO
{
    private int _readOffset;

    public List<byte> Data;
	public byte[] DataBytes;

    public static string GetCrashdayPath()
    {
        string crashdayPath;

        if (PlayerPrefs.HasKey("crashpath"))
        {
	        crashdayPath = PlayerPrefs.GetString("crashpath");
            if (!Directory.Exists(crashdayPath))
            {
	            crashdayPath = StandaloneFileBrowser.OpenFolderPanel("Select crashday folder", "", false)[0];
                PlayerPrefs.SetString("crashpath", crashdayPath);
            }
        }
        else
        {
	        crashdayPath = StandaloneFileBrowser.OpenFolderPanel("Select crashday folder", "", false)[0];
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

    public IO(List<byte> data)
    {
	    Data = data;
	    DataBytes = data.ToArray();
    }

	public void SetReadingOffest(int newOffset)
	{
		_readOffset = newOffset;
	}

	public void AddReadingOffset(int newOffest)
	{
		_readOffset += newOffest;
	}

	public void ResetData()
	{
		Data = new List<byte>();
		DataBytes = Data.ToArray();
	}
	
	//=======================
	//	READ/WRITE STUFF HERE
	//=======================

	public void WriteEmpty(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			Data.Add(0);
		}
	}

	//writes a string of given length
	//does not apped null termination at the end
	public void WriteFlag(string str)
	{
		Data.AddRange(Encoding.UTF8.GetBytes(str));
	}

	public void WriteString(string str)
	{
		WriteFlag(str);
		//Data.AddRange(Encoding.UTF8.GetBytes("\0"));
		Data.Add(0);
	}

    public string ReadString()
    {
        for (var i = _readOffset; i < Data.Count; i++)
        {
            if (Data[i] == 0)
            {
                var oldOffset = _readOffset;
	            _readOffset = i + 1;
                return Encoding.UTF8.GetString(Data.ToArray(), oldOffset, i - oldOffset);
            }
        }
        return "";
    }


	public void WriteInt(int i)
	{
		Data.AddRange(BitConverter.GetBytes(i));
	}

    public int ReadInt()
    {
	    _readOffset += 4;
        return BitConverter.ToInt32(DataBytes, _readOffset - 4);
    }


	public void WriteUInt(uint i)
	{
		Data.AddRange(BitConverter.GetBytes(i));
	}

    public uint ReadUInt()
    {
	    _readOffset += 4;
        return BitConverter.ToUInt32(DataBytes, _readOffset - 4);
    }


	public void WriteUShort(ushort i)
	{
		Data.AddRange(BitConverter.GetBytes(i));
	}

    public ushort ReadUShort()
    {
	    _readOffset += 2;
        return BitConverter.ToUInt16(DataBytes, _readOffset - 2);
    }


	public void WriteShort(short i)
	{
		Data.AddRange(BitConverter.GetBytes(i));
	}

    public short ReadShort()
    {
	    _readOffset += 2;
        return BitConverter.ToInt16(DataBytes, _readOffset - 2);
    }


	public void WriteFloat(float i)
	{
		Data.AddRange(BitConverter.GetBytes(i));
	}

    public float ReadFloat()
    {
	    _readOffset += 4;
        return BitConverter.ToSingle(DataBytes, _readOffset - 4);
    }


	public void WriteVector3(Vector3 i)
	{
		WriteFloat(i.x);
		WriteFloat(i.y);
		WriteFloat(i.z);
	}

    public Vector3 ReadVector3()
    {
        return new Vector3(ReadFloat(), ReadFloat(), ReadFloat());
    }


	public void WriteByte(byte i)
	{
		Data.Add(i);
	}

    public byte ReadByte()
    {
	    _readOffset += 1;
        return Data[_readOffset-1];
    }


	public void WriteChar(char i)
	{
		Data.AddRange(BitConverter.GetBytes(i));
	}

    public char ReadChar()
    {
	    _readOffset += 1;
        return BitConverter.ToChar(DataBytes, _readOffset - 1);
    }
}
