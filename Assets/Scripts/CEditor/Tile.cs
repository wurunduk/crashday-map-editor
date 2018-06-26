using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private TrackTileSavable _trackTileSavable;
    private string _size;
	private TrackManager _tm;

    public Vector2 GridPosition;

    public void SetupTile(TrackTileSavable trackTileSavable, string size, Vector2 gridPosition, TrackManager tm)
    {
		_trackTileSavable = trackTileSavable;
	    _size = size;
	    GridPosition = gridPosition;
	    _tm = tm;

		SetRotation(trackTileSavable.Rotation);
    }

    public void SetRotation(byte rotation)
    {
        _trackTileSavable.Rotation = rotation;
        transform.localRotation = Quaternion.Euler(0, (rotation - 1) * 90, 0);

        if (_trackTileSavable.IsMirrored != 0)
        {
			//ghetto way of flipping
            if (rotation == 1 || rotation == 3)
            {
                transform.localScale = new Vector3(1, 1, -1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }

        if (_size == "11")
        {
            transform.localPosition = new Vector3(GridPosition.y * TrackManager.TileSize, transform.localPosition.y, GridPosition.x * TrackManager.TileSize);
        }
        else if (_size == "22")
        {
            transform.localPosition = new Vector3(GridPosition.y * TrackManager.TileSize + TrackManager.TileSize / 2.0f, transform.localPosition.y, GridPosition.x * TrackManager.TileSize + TrackManager.TileSize / 2.0f);
        }
        else if (_size == "21")
        {
            if (rotation == 1 || rotation == 3)
            {
                transform.localPosition = new Vector3(GridPosition.y * TrackManager.TileSize + TrackManager.TileSize / 2.0f, transform.localPosition.y, GridPosition.x * TrackManager.TileSize);
            }
            else
                transform.localPosition = new Vector3(GridPosition.y * TrackManager.TileSize, transform.localPosition.y, GridPosition.x * TrackManager.TileSize + TrackManager.TileSize - TrackManager.TileSize / 2.0f);
        }
        else
        {
            if (rotation == 1 || rotation == 3)
            {
                transform.localPosition = new Vector3(GridPosition.y * TrackManager.TileSize, transform.localPosition.y, GridPosition.x * TrackManager.TileSize + TrackManager.TileSize / 2.0f);
            }
            else
                transform.localPosition = new Vector3(GridPosition.y * TrackManager.TileSize + TrackManager.TileSize, transform.localPosition.y, GridPosition.x * TrackManager.TileSize);
        }

    }

    public void Flip()
    {
        
    }

	public void ApplyTerrain()
	{
		/*Vector3[] verticies = GetComponent<MeshFilter>().mesh.vertices;
		for (int i = 0; i < verticies.Length; i++)
		{
			
			int posX = Mathf.FloorToInt(GridPosition.x * 4 + verticies[i].x / 5*(_size[0]-'0') + 2);
			int posY = Mathf.FloorToInt(GridPosition.y * 4 + verticies[i].z / 5*(_size[1]-'0') + 2);

			if(posX < 0 || posY < 0 || posX> _tm.CurrentTrack.Width*4-1 || posY> _tm.CurrentTrack.Height*4-1) continue;

			verticies[i].y +=  (_tm.CurrentTrack.Heightmap[posX+1, posY+1] - _tm.CurrentTrack.Heightmap[posX,posY])
			                   *((verticies[i]+ new Vector3(10,0,0)*(_size[0]-'0') + new Vector3(0,0,10)*(_size[1]-'0')).magnitude*_tm.CurrentTrack.Height)/(20*Mathf.Sqrt(2));
		}
			
		GetComponent<MeshFilter>().mesh.vertices = verticies;*/
	}
}
