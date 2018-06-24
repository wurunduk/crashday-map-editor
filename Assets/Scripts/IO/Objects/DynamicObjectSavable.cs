using UnityEngine;

public class DynamicObjectSavable
{
    //uint16_t objid, vector3 pos, float rotation
    //vector3 is a struct of float x, float y float z[total 12 bytes]
    public ushort ObjectId;
    public Vector3 Position;
    public float Rotation;
}
