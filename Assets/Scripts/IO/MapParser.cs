using System.Diagnostics;
using UnityEngine;

public class MapParser
{
    //reads map from file to Track object
    public TrackSavable ReadMap(string path)
    {
        TrackSavable Track = new TrackSavable();
        byte[] data = System.IO.File.ReadAllBytes(path);
        IO io = new IO(data);

        //ignore "CDTRK" string in the start of the file
        io.SetReadingOffest(5);

        //unused current time thing
        Track.CurrentTime = io.ReadInt();

        //author
        Track.Author = io.ReadString();

        //comment
        Track.Comment = io.ReadString();

        //skip unneeded block
        for (int n = 0; n < 20; n++)
        {
	        io.ReadInt();
	        io.ReadString();
        }

        //track style (race, derby, htf)
	    Track.Style = io.ReadByte();

        //ambience
        Track.Ambience = io.ReadString();

        //amount of fileds used on map
        Track.FieldFilesNumber = io.ReadUShort();

        //name of fields
        Track.FieldFiles = new string[Track.FieldFilesNumber];
        for (int n = 0; n < Track.FieldFilesNumber; n++)
        {
            Track.FieldFiles[n] = io.ReadString();
        }

        //width and height in tiles
        Track.Width = io.ReadUShort();
        Track.Height = io.ReadUShort();

        Track.TrackTiles = new TrackTileSavable[Track.Width, Track.Height];

        for (int y = 0; y < Track.Height; y++)
        {
            for (int x = 0; x < Track.Width; x++)
            {
                TrackTileSavable newTile = new TrackTileSavable();
                newTile.FieldId = io.ReadUShort();
	            newTile.Rotation = io.ReadByte();
                newTile.IsMirrored = io.ReadByte();
                newTile.Height = io.ReadByte();

                Track.TrackTiles[x, y] = newTile;
            }
        }


        Track.DynamicObjectFilesNumber = io.ReadUShort();
        Track.DynamicObjectFiles = new string[Track.DynamicObjectFilesNumber];
        for (int i = 0; i < Track.DynamicObjectFilesNumber; i++)
        {
            Track.DynamicObjectFiles[i] = io.ReadString();
        }

        Track.DynamicObjectsNumber = io.ReadUShort();
        Track.DynamicObjects = new DynamicObjectSavable[Track.DynamicObjectsNumber];
        for (int i = 0; i < Track.DynamicObjectsNumber; i++)
        {
            DynamicObjectSavable newDynamicObject = new DynamicObjectSavable();
            newDynamicObject.ObjectId = io.ReadUShort();
            newDynamicObject.Position = io.ReadVector3();
            newDynamicObject.Rotation = io.ReadFloat();
            Track.DynamicObjects[i] = newDynamicObject;
        }


        Track.CheckpointsNumber = io.ReadUShort();
        Track.Checkpoints = new ushort[Track.CheckpointsNumber];
        for (int i = 0; i < Track.CheckpointsNumber; i++)
        {
            Track.Checkpoints[i] = io.ReadUShort();
        }

        Track.Permission = io.ReadByte();

        Track.GroundBumpyness = io.ReadFloat();

        Track.Scenery = io.ReadByte();

        Track.Heightmap = new float[Track.Width * 4 + 1, Track.Height * 4 + 1];
        
        for (int y = 0; y < Track.Height * 4 + 1; y++)
        {
            for (int x = 0; x < Track.Width * 4 + 1; x++)
            {
                Track.Heightmap[x, y] = io.ReadFloat();
            }
        }

        return Track;
    }
}
