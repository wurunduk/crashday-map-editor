using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileListEntry
{
	public string Name;
	public P3DModel Model;
	public Material[] Materials;
	public IntVector2 Size;

	public TileListEntry(string name, P3DModel model, Material[] materials, IntVector2 size)
	{
		Name = name;
		Model = model;
		Materials = materials;
		Size = size;
	}
}

public class TileManager : MonoBehaviour
{
	public List<TileListEntry> TileList;

	public bool Loaded;

	public void LoadTiles()
	{
		if (Loaded) return;


		TileList = new List<TileListEntry>();

		P3DParser parser = new P3DParser();

		string[] files = System.IO.Directory.GetFiles (IO.GetCrashdayPath () + "/data/content/tiles/");

		for(int i = 0; i < files.Length; i++)
		{
			//read only cfl files in tiles folder
			if (files [i].Contains (".cfl"))
			{
				//files[i] already contains crashday path, so we dont add it
				string[] cflFIle = System.IO.File.ReadAllLines(files[i]);

				//get model path from cfl file
				string pathToModel = cflFIle[2];
				pathToModel = IO.GetCrashdayPath() + "/data/content/models/" + IO.RemoveComment(pathToModel);

				//get the size of the model in tiles
				//the size is stored in form of (w h #some comment maybe)
				//so we just remove the coment and take the first and third char
				string sizeStr = cflFIle[3];
				sizeStr = IO.RemoveComment(sizeStr);
				IntVector2 size = new IntVector2(sizeStr[0]-'0', sizeStr[2]-'0');

				//load model
				P3DModel model = parser.LoadFromFile (pathToModel);

				TileList.Add(new TileListEntry(files [i].Substring (files [i].LastIndexOf ('/')+1), model, model.CreateMaterials(), size));
			}
		}

		Loaded = true;
	}
}
