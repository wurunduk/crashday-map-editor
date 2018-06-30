﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour 
{
	public List<string>	tileNames;
	public List<P3DModel> tileModels;
	public List<Material[]> tileMaterials;

	private bool loaded = false;

	void Start ()
	{
		
	}


	public void LoadTiles()
	{
		if(!loaded)
		{
			loaded = true;

			tileNames = new List<string>();
			tileModels = new List<P3DModel>();
			tileMaterials = new List<Material[]>();

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

					//this one does not load for some reason
					if (pathToModel.Contains ("chkpoint"))
						continue;

					//load model, create mesh
					P3DModel model = parser.LoadFromFile (pathToModel);

					tileNames.Add (files [i].Substring (files [i].LastIndexOf ('/')+1));
					tileModels.Add (model);
					//tileTextures.Add(model.CreateTextures());
					tileMaterials.Add(model.CreateMaterials());
				}
			}
		}
	}
}
