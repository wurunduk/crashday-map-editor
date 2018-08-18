using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AmbienceParser
{
	/// <summary>
	/// Ambience reading is UNFINISHED!
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public Ambience ReadAmbience(string path)
	{
		Ambience amb = new Ambience();
		string[] s = File.ReadAllLines(path);
		TextFileParser p = new TextFileParser(s);

		p.Skip();
		amb.Name = p.ReadString();
		amb.TexPath = p.ReadString();

		amb.SunColor = p.ReadColor();
		amb.MaxSunStrength = p.ReadFloat();
		amb.SunLightExponent = p.ReadFloat();

		p.Skip(6);

		amb.SunLightVector = p.ReadVector3();
		amb.SunRotationVector = p.ReadFloat();

		p.Skip(6);

		//skip color ramps
		p.Skip(p.ReadInt());
		
		p.Skip(4);

		amb.EnvironmentSound = p.ReadString();

		return amb;
	}
}
