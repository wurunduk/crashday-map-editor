using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
	public class MeshData
	{
		public List<Vector3> Verticies;
		public List<Vector2> Uvs;
		public List<int> Tris;

		public MeshData()
		{
			Verticies = new List<Vector3>();
			Uvs = new List<Vector2>();
			Tris = new List<int>();
		}
	}

	public static MeshData GeneratePlane(MeshData md, int sizeX, int sizeY, Vector3 d, Vector3 rotation)
	{
		Quaternion rot = Quaternion.Euler(rotation);

		int offset = md.Verticies.Count;

		for (int y = 0; y < sizeY; y++)
		{
			for (int x = 0; x < sizeX; x++)
			{
				Vector3 pos = new Vector3(-d.x / 2 + x * (d.x / (sizeX-1)), d.y/2, d.z / 2 + -1 * y * (d.z / (sizeY-1)));
				pos = rot*pos;
				md.Verticies.Add(pos);
				md.Uvs.Add(new Vector2(x*(d.x/2)%sizeX, y%sizeY));
			}
		}

		for (int i = 0; i < sizeX*sizeY-sizeX; i++) 
		{			
			if ((i + 1)%sizeX == 0)
			{
				continue;
			}

			md.Tris.Add(offset + i+1);
			md.Tris.Add(offset + i+sizeX+1);
			md.Tris.Add(offset + i+sizeX);

			md.Tris.Add(offset + i);
			md.Tris.Add(offset + i+1);
			md.Tris.Add(offset + i+sizeX);
		}

		return md;
	}

	public static MeshData GeneratePlane(int sizeX, int sizeY, Vector3 dimensions, Vector3 rotation)
	{
		MeshData md = new MeshData();

		return GeneratePlane(md, sizeX, sizeY, dimensions, rotation);
	}

	public static Mesh GenerateCubeMesh(int subdivisions, Vector3 d)
	{
		int length = (subdivisions + 2);

		MeshData md = GeneratePlane(length, length, d, Vector3.zero);
		md = GeneratePlane(md, length, 2, new Vector3(d.z, d.x, d.y),  new Vector3(90,0,-90));
		md = GeneratePlane(md, length, 2, new Vector3(d.x, d.z, d.y),  new Vector3(90,90,-90));
		md = GeneratePlane(md, length, 2, new Vector3(d.z, d.x, d.y),  new Vector3(90,180,-90));
		md = GeneratePlane(md, length, 2, new Vector3(d.x, d.z, d.y),  new Vector3(90,270,-90));

		Mesh mesh = new Mesh();
		mesh.vertices = md.Verticies.ToArray();
		mesh.triangles = md.Tris.ToArray();
		mesh.uv = md.Uvs.ToArray();
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		return mesh;
	}
}
