using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class P3DModel
{
    public enum P3DMaterial
    {
        Flat = 0,
        FlatMetal = 1,
        Gouraud = 2,
        GouraudMetal = 3,
        GouraudMetalEnv = 4,
        Shining = 5
    }

    public class RenderInfo
    {
        public string TextureFile;
    };

    public class TextureInfo
    {
        public short TextureStart;
        public short NumFlat;
        public short NumFlatMetal;
        public short NumGouraud;
        public short NumGouraudMetal;
        public short NumGouraudMetalEnv;
        public short NumShining;
    }

    public class P3DLight
    {
        public string Name;
        public Vector3 Pos;
        public float Range;
        public int Color;

        public byte ShowCorona;
        public byte ShowLensFlares;

        public byte LightUpEnvironment;
    };

    public class P3DTexPolygon
    {
        public string Texture;
        public P3DMaterial Material;

        public short P1;
        public short P2;
        public short P3;

        public float U1, V1;
        public float U2, V2;
        public float U3, V3;
    };

    public class P3DMesh
    {
        public string Name;
        public uint Flags;
        public short NumVertices;
        public short NumPolys;

        public TextureInfo[] TextureInfos;

        public Vector3[] Vertex;
        public P3DTexPolygon[] Poly;

        public Vector3 LocalPos;
        public float Length;
        public float Height;
        public float Depth;

        public P3DMesh(byte numTextures)
        {
            TextureInfos = new TextureInfo[numTextures];
        }
    };

    public short P3DNumMeshes;
    public P3DMesh[] P3DMeshes;

    public short P3DNumLights;
    public P3DLight[] P3DLights;

    public float P3DLength;
    public float P3DDepth;
    public float P3DHeight;

    public int P3DUserDataSize;
    public string P3DUserDataPtr;

    public byte P3DNumTextures;
    public RenderInfo[] P3DRenderInfo;

    public static Texture2D LoadTextureDXT(byte[] ddsBytes, TextureFormat textureFormat)
    {
        if (textureFormat != TextureFormat.DXT1 && textureFormat != TextureFormat.DXT5)
            Debug.LogError("Invalid TextureFormat. Only DXT1 and DXT5 formats are supported by this method.");

        byte ddsSizeCheck = ddsBytes[4];
        if (ddsSizeCheck != 124)
            Debug.LogError("Invalid DDS DXTn texture. Unable to read");  //this header byte should be 124 for DDS image files

        int height = ddsBytes[13] * 256 + ddsBytes[12];
        int width = ddsBytes[17] * 256 + ddsBytes[16];

        int DDS_HEADER_SIZE = 128;
        byte[] dxtBytes = new byte[ddsBytes.Length - DDS_HEADER_SIZE];
        Buffer.BlockCopy(ddsBytes, DDS_HEADER_SIZE, dxtBytes, 0, ddsBytes.Length - DDS_HEADER_SIZE);

        Texture2D texture = new Texture2D(width, height, textureFormat, false);
        texture.LoadRawTextureData(dxtBytes);
        texture.Apply();

        return (texture);
    }

	public Material[] CreateMaterials()
	{
		List<Material> materials = new List<Material>();
		for (int i = 0; i < P3DNumTextures; i++)
		{
			byte[] data = System.IO.File.ReadAllBytes(IO.GetCrashdayPath() + "/data/content/textures/" + P3DRenderInfo[i].TextureFile.Remove(P3DRenderInfo[i].TextureFile.Length - 4) + ".dds");
			Texture2D tex = LoadTextureDXT(data, TextureFormat.DXT5);

			Material mat = new Material(Shader.Find("Standard"));
			mat.mainTexture = tex;
			materials.Add(mat);
		}

		return materials.ToArray();
	}

    public Mesh[] CreateMeshes()
    {
        Mesh[] m = new Mesh[P3DNumMeshes];
        for (int i = 0; i < P3DNumMeshes; i++)
        {
			//REMOVE THIS***************************************************************************************************************************///
			//MADE FOR FASTER TESTING ONLY
			//
	        if (i != 0) continue;

            Mesh newMesh = new Mesh();
            newMesh.name = P3DMeshes[i].Name;

			//unity requires models to have the same amount of vertices as polys.
            int size = 0;
            if (P3DMeshes[i].NumVertices > P3DMeshes[i].NumPolys)
            {
                size = P3DMeshes[i].NumVertices;
            }
            else
            {
                size = P3DMeshes[i].NumPolys;
            }


            Vector3[] verts = new Vector3[size];
            for (int v = 0; v < P3DMeshes[i].NumVertices; v++)
            {
                verts[v] = P3DMeshes[i].Vertex[v];
            }
            for (int v = P3DMeshes[i].NumVertices; v < size; v++)
            {
				verts[v] = new Vector3(0f,0f,0f);
            }

            newMesh.vertices = verts;

	        List<string> textures = new List<string>();
			List<Vector2> uv = new List<Vector2>();
	        List<List<int>> tri = new List<List<int>>();
	        for (int n = 0; n < P3DNumTextures; n++)
	        {
		        textures.Add(P3DRenderInfo[n].TextureFile);
				tri.Add(new List<int>());
	        }

            for (int v = 0; v < P3DMeshes[i].NumPolys; v++)
            {
	            int index = textures.FindIndex(x => x.Contains(P3DMeshes[i].Poly[v].Texture));
                tri[index].Add(P3DMeshes[i].Poly[v].P1);
                tri[index].Add(P3DMeshes[i].Poly[v].P2);
                tri[index].Add(P3DMeshes[i].Poly[v].P3);

		        uv.Add(new Vector2(P3DMeshes[i].Poly[v].U1, P3DMeshes[i].Poly[v].V1));
		        //uv.Add(new Vector2(P3DMeshes[i].Poly[v].U2, P3DMeshes[i].Poly[v].V2));
		        //uv.Add(new Vector2(P3DMeshes[i].Poly[v].U3, P3DMeshes[i].Poly[v].V3));
            }

	        newMesh.subMeshCount = P3DNumTextures;

	        newMesh.SetUVs(0, uv);
	        for (int n = 0; n < P3DNumTextures; n++)
	        {
				newMesh.SetTriangles(tri[n], n);
	        }

            newMesh.RecalculateNormals();
	        newMesh.RecalculateBounds();

            m[i] = newMesh;
        }
        return m;
    }
}
