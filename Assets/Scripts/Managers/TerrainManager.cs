using UnityEngine;
using System.Collections;

public class TerrainManager : MonoBehaviour
{
	private TrackManager _tm;
	public Transform Terrain;

	void Start()
	{
		_tm = GetComponent<TrackManager>();
	}

	public void GenerateTerrain()
	{
		if (!_tm) return;

		int sizeX = _tm.CurrentTrack.Width * 4 + 1;
		int sizeY = _tm.CurrentTrack.Height * 4 + 1;

		Vector3[] verticies = new Vector3[sizeX*sizeY];
		Vector3[] normals = new Vector3[sizeX*sizeY];
		Vector2[] uvs = new Vector2[sizeX*sizeY];
		int[] tris = new int[(sizeX*sizeY-sizeX)*6];

		for (int y = 0; y < sizeY; y++)
		{
			for (int x = 0; x < sizeX; x++)
			{
				//-10 is half of the Tile size to the center the terrain. According to thethe tile is divided every 5 meters
				verticies[x+sizeX*y] = new Vector3(-10 + x*5, _tm.CurrentTrack.Heightmap[x,y]*_tm.CurrentTrack.GroundBumpyness*5, 10 + y*-5);
				normals[x + sizeX * y] = Vector3.down;
				uvs[x+sizeX*y] = new Vector2(x*10%sizeX, y%sizeY);
			}
		}

		for (int i = 0; i < tris.Length / 6; i++) 
		{			
			if ((i + 1)%sizeX == 0)
			{
				continue;
			}

			tris[i * 6 + 2] = i + sizeX;
			tris[i * 6 + 1] = i + sizeX + 1;
			tris[i * 6 + 0] = i + 1;
			
			tris[i * 6 + 5] = i + sizeX;
			tris[i * 6 + 4] = i + 1;
			tris[i * 6 + 3] = i;
		}

		Terrain.GetComponent<MeshFilter>().mesh.vertices = verticies;
		Terrain.GetComponent<MeshFilter>().mesh.triangles = tris;
		Terrain.GetComponent<MeshFilter>().mesh.normals = normals;
		Terrain.GetComponent<MeshFilter>().mesh.RecalculateNormals();
		Terrain.GetComponent<MeshFilter>().mesh.uv = uvs;

		Terrain.GetComponent<MeshCollider>().sharedMesh = Terrain.GetComponent<MeshFilter>().sharedMesh;

	}
}
