using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class TileListEntry
{
	public string Name;
	public string ModelPath;
	public P3DModel Model;
	public List<Material> Materials;
	public Texture Icon;
	public IntVector2 Size;

	public LoadState CurrentLoadState;

	public enum LoadState
	{
		Cflloaded,
		ModelLoaded
	}

	public TileListEntry(string name, string modelPath, Texture icon, IntVector2 size)
	{
		CurrentLoadState = LoadState.Cflloaded;

		Name = name;
		ModelPath = modelPath;
		Icon = icon;
		Size = size;
	}

	public TileListEntry(string name, string modelPath, Texture icon, P3DModel model, List<Material> materials, IntVector2 size)
	{
		CurrentLoadState = LoadState.ModelLoaded;

		Name = name;
		ModelPath = modelPath;
		Icon = icon;
		Model = model;
		Materials = materials;
		Size = size;
	}
}

public class MaterialListEntry
{
	public string Name;
	public Material Material;
}


public class TileManager : MonoBehaviour
{
	public List<TileListEntry> TileList;

	public List<MaterialListEntry> Materials;

	public bool Loaded;

	public void LoadModelForTileId(int id)
	{
	    if (id >= TileList.Count || id < 0)
	    {
            Debug.LogError("Some tile was out of bounds!");
	        return;
	    }

		if (TileList[id].CurrentLoadState == TileListEntry.LoadState.ModelLoaded) return;

		P3DModel model = P3DParser.LoadFromFile(TileList[id].ModelPath);
		TileList[id].Model = model;

		TileList[id].Materials = new List<Material>(model.P3DNumTextures);
		for (int i = 0; i < model.P3DNumTextures; i++)
		{
			MaterialListEntry m = Materials.FirstOrDefault(x => x.Name == model.P3DRenderInfo[i].TextureFile);
			if (m == default(MaterialListEntry))
			{
				m = new MaterialListEntry();
				m.Material = model.CreateMaterial(i);
				m.Name = model.P3DRenderInfo[i].TextureFile;
				Materials.Add(m);
			}

			TileList[id].Materials.Add(m.Material);
		}

		TileList[id].CurrentLoadState = TileListEntry.LoadState.ModelLoaded;
	}

	public void LoadTiles()
	{
		if (Loaded) return;


		TileList = new List<TileListEntry>(270);
		Materials = new List<MaterialListEntry>(30);

		string[] files = System.IO.Directory.GetFiles (IO.GetCrashdayPath () + "/data/content/tiles/");

		for(int i = 0; i < files.Length; i++)
		{
			//read only cfl files in tiles folder
			if (files [i].Contains (".cfl"))
			{
				string name = files[i].Substring(files[i].LastIndexOf('/') + 1);

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
				Texture ic = TGAParser.LoadTGA(IO.GetCrashdayPath() + "/data/content/textures/pictures/tiles/" + name.Split('.')[0] + ".tga");

				TileList.Add(new TileListEntry(name, pathToModel, ic, size));
			}
		}

		Loaded = true;
	}
}
