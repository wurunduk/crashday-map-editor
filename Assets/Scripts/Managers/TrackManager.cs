using UnityEngine;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

public class TrackManager : MonoBehaviour
{
    public GameObject Dummy;
    public Transform Map;
    public Transform[,] Tiles;
	public TrackSavable CurrentTrack;

    public static int TileSize = 20;
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void LoadTrack(TrackSavable track)
    {
	    CurrentTrack = track;
        for (int i = 0; i < Map.childCount; i++)
        {
            Destroy(Map.GetChild(i).gameObject);
        }

		GetComponent<TerrainManager>().GenerateTerrain();
	    //return;
			
        for (int y = 0; y < track.Height; y++)
        {
            for (int x = 0; x < track.Width; x++)
            {
                if (track.TrackTiles[x, y].FieldId < track.FieldFilesNumber)
                {
					TileManager tileManager = GetComponent<TileManager> ();

					int index = tileManager.tileNames.IndexOf (track.FieldFiles[track.TrackTiles [x, y].FieldId]);

                    string pathToCfl = IO.GetCrashdayPath() + "/data/content/tiles/" + track.FieldFiles[track.TrackTiles[x, y].FieldId];
                    string[] cflFIle = System.IO.File.ReadAllLines(pathToCfl);
					
					GameObject newTile = (GameObject) Instantiate(Dummy, new Vector3(x*TileSize, tileManager.tileModels[index].P3DMeshes[0].Height/2, y*TileSize), Quaternion.identity);

                    //get the size of the model in tiles
                    string size = cflFIle[3];
                    size = size.Remove(size.IndexOf("#")).Trim();
                    size = size.Replace(" ", string.Empty);

					//get the name of the model
	                string name = cflFIle[2];
	                name = name.Remove(name.IndexOf(".p3d")).Trim();

                    
                    newTile.name = x + ":" + y + " " + name;

                    MeshFilter mf = (MeshFilter)newTile.gameObject.GetComponent(typeof(MeshFilter));
                    //MeshRenderer mr = (MeshRenderer)newTile.gameObject.GetComponent(typeof(MeshRenderer));
					mf.mesh = tileManager.tileModels[index].CreateMeshes()[0];

	                newTile.transform.SetParent(Map);

	                Tile tile = newTile.AddComponent<Tile>();
					tile.SetupTile(track.TrackTiles [x, y], size, new Vector2(x, y), this);
					//tile.ApplyTerrain();
                }
            }
        }
    }
}
