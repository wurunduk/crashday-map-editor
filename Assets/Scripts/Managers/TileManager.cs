using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileListEntry
{
	public string Name;
	public string ModelPath;
	public P3DModel Model;
	public List<Material> Materials;
	public IntVector2 Size;

	public LoadState CurrentLoadState;

	public enum LoadState
	{
		Cflloaded,
		ModelLoaded
	}

	public TileListEntry(string name, string modelPath, IntVector2 size)
	{
		CurrentLoadState = LoadState.Cflloaded;

		Name = name;
		ModelPath = modelPath;
		Size = size;
	}

	public TileListEntry(string name, string modelPath, P3DModel model, List<Material> materials, IntVector2 size)
	{
		CurrentLoadState = LoadState.ModelLoaded;

		Name = name;
		ModelPath = modelPath;
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
		if (TileList[id].CurrentLoadState == TileListEntry.LoadState.ModelLoaded) return;

		P3DParser parser = new P3DParser();

		P3DModel model = parser.LoadFromFile(TileList[id].ModelPath);
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

				TileList.Add(new TileListEntry(files [i].Substring (files [i].LastIndexOf ('/')+1), pathToModel, size));
			}
		}

		Loaded = true;
	}
}
