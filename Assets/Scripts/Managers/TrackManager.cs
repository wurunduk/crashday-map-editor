using UnityEngine;
using System.Collections;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;

public class TrackManager : MonoBehaviour
{
    public GameObject TilePrefab;
    public Transform MapParentTransform;
    public Transform[,] Tiles;
	public TrackSavable CurrentTrack;

	public bool LoadedTrack;

    public static int TileSize = 20;

	private TileManager _tm;

	void Start()
	{
		_tm = GetComponent<TileManager> ();
	}

    public void LoadTrack(TrackSavable track)
    {
	    LoadedTrack = false;
	    CurrentTrack = track;

		//clear tiles left from the old loaded track
        for (int i = 0; i < MapParentTransform.childCount; i++)
        {
            Destroy(MapParentTransform.GetChild(i).gameObject);
        }

		GetComponent<TerrainManager>().GenerateTerrain();

		Tiles = new Transform[track.Width, track.Height];
			
        for (int y = 0; y < track.Height; y++)
        {
            for (int x = 0; x < track.Width; x++)
            {
                if (track.TrackTiles[x, y].FieldId < track.FieldFilesNumber)
                {
					int index = _tm.TileList.FindIndex(entry=>entry.Name == track.FieldFiles[track.TrackTiles [x, y].FieldId]);
					
					//The tile will be moved by the SetTile function later. The best moment to calcualte height is now.
					GameObject newTile = (GameObject) Instantiate(TilePrefab, new Vector3(0, _tm.TileList[index].Model.P3DMeshes[0].Height/2, 0), Quaternion.identity);

                    newTile.name = x + ":" + y + " " + _tm.TileList[index].Name;

					//set the model and textures for the tile
					newTile.GetComponent<MeshFilter>().mesh = _tm.TileList[index].Model.CreateMeshes()[0];
	                newTile.GetComponent<Renderer>().materials = _tm.TileList[index].Materials;
					
	                newTile.transform.SetParent(MapParentTransform);

	                Tile tile = newTile.AddComponent<Tile>();
					tile.SetupTile(track.TrackTiles [x, y], _tm.TileList[index].Size, new IntVector2(x, y), this);
					tile.ApplyTerrain();

	                Tiles[x, y] = newTile.transform;
                }
            }
        }

		FindObjectOfType<Camera>().gameObject.transform.localPosition = new Vector3(0, 100, 0);
		FindObjectOfType<Camera>().transform.LookAt(new Vector3(CurrentTrack.Width*20, 0, CurrentTrack.Height*-20));
	    LoadedTrack = true;
    }
}
