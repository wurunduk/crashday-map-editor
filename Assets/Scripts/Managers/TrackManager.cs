using UnityEngine;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

public class TrackManager : MonoBehaviour
{
    public GameObject Dummy;
    public Transform Map;
    public Transform[,] Tiles;

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
        for (int i = 0; i < Map.childCount; i++)
        {
            Destroy(Map.GetChild(i).gameObject);
        }
			
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

                    
                    newTile.name = x + ":" + y;

                    MeshFilter mf = (MeshFilter)newTile.gameObject.GetComponent(typeof(MeshFilter));
                    MeshRenderer mr = (MeshRenderer)newTile.gameObject.GetComponent(typeof(MeshRenderer));
					mf.mesh = tileManager.tileMeshes [index];
                    //mr.GetComponent<Renderer>().material.color = Color.grey;

                    newTile.transform.SetParent(Map);
					newTile.GetComponent<Tile>().SetTileSavable(track.TrackTiles [x, y]);
	                newTile.GetComponent<Tile>().size = size;
	                newTile.GetComponent<Tile>().PositionX = x;
	                newTile.GetComponent<Tile>().PositionY = y;
					newTile.GetComponent<Tile>().SetRotation (track.TrackTiles [x, y].Rotation);
                }
            }
        }
    }
}
