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

	public bool Loaded = false;

	void Start ()
	{
		
	}


	public void LoadTiles()
	{
		if(!Loaded)
		{
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
					pathToModel = pathToModel.Remove(pathToModel.IndexOf("#")).Trim();
					pathToModel = IO.GetCrashdayPath() + "/data/content/models/" + pathToModel;

					//get the size of the model in tiles
					string sizeStr = cflFIle[3];
					sizeStr = sizeStr.Remove(sizeStr.IndexOf("#")).Trim();
					sizeStr = sizeStr.Replace(" ", string.Empty);
					IntVector2 size = new IntVector2(sizeStr[0]-'0', sizeStr[1]-'0');

					//load model, create mesh
					P3DModel model = parser.LoadFromFile (pathToModel);

					TileList.Add(new TileListEntry(files [i].Substring (files [i].LastIndexOf ('/')+1), model, model.CreateMaterials(), size));
				}
			}

			Loaded = true;
		}
	}
}
