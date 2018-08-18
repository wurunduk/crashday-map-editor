using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceListEntry
{
	public string Name;
	public Ambience Ambience;
}

public class AmbienceManager : MonoBehaviour
{
	public List<AmbienceListEntry> Ambiences;

	void Start()
	{

	}

	public void LoadAmbiences()
	{
		Ambiences = new List<AmbienceListEntry>(3);

		AmbienceParser parser = new AmbienceParser();

		string[] files = System.IO.Directory.GetFiles (IO.GetCrashdayPath () + "/data/content/ambience/");
		for (int i = 0; i < files.Length; i++)
		{
			if (files[i].Contains(".amb") && !files[i].Contains(".special"))
			{
				AmbienceListEntry amb = new AmbienceListEntry();
				amb.Name = files[i].Substring(files[i].LastIndexOf('/'));
				amb.Ambience = parser.ReadAmbience(files[i]);

				Ambiences.Add(amb);
			}
		}
	}
}
