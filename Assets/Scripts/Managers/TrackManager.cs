using UnityEngine;
using System.Collections;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

public class TrackManager : MonoBehaviour
{
    public GameObject Dummy;
    public Transform Map;
    public Transform[,] Tiles;
	public TrackSavable CurrentTrack;

	public bool LoadedTrack = false;

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
	    LoadedTrack = false;
	    CurrentTrack = track;
        for (int i = 0; i < Map.childCount; i++)
        {
            Destroy(Map.GetChild(i).gameObject);
        }

		GetComponent<TerrainManager>().GenerateTerrain();
			
        for (int y = 0; y < track.Height; y++)
        {
            for (int x = 0; x < track.Width; x++)
            {
                if (track.TrackTiles[x, y].FieldId < track.FieldFilesNumber)
                {
					TileManager tileManager = GetComponent<TileManager> ();

					int index = tileManager.TileList.FindIndex(entry=>entry.Name == track.FieldFiles[track.TrackTiles [x, y].FieldId]);
					
					//The tile will be moved by the SetTile function later. The best moment to calcualte height is now.
					GameObject newTile = (GameObject) Instantiate(Dummy, new Vector3(0, tileManager.TileList[index].Model.P3DMeshes[0].Height/2, 0), Quaternion.identity);

                    newTile.name = x + ":" + y + " " + tileManager.TileList[index].Name;

					newTile.GetComponent<MeshFilter>().mesh = tileManager.TileList[index].Model.CreateMeshes()[0];
	                newTile.GetComponent<Renderer>().materials = tileManager.TileList[index].Materials;
					
	                newTile.transform.SetParent(Map);

	                Tile tile = newTile.AddComponent<Tile>();
					tile.SetupTile(track.TrackTiles [x, y], tileManager.TileList[index].Size, new IntVector2(x, y), this);
					tile.ApplyTerrain();
                }
            }
        }

		FindObjectOfType<Camera>().gameObject.transform.localPosition = new Vector3(0, 100, 0);
		FindObjectOfType<Camera>().transform.LookAt(new Vector3(CurrentTrack.Width*20, 0, CurrentTrack.Height*-20));
	    LoadedTrack = true;
    }
}
