using System.Collections.Generic;

public class TrackSavable
{
    public int CurrentTime;         //unused
    public string Author;           
    public string Comment;
    /*
     * 5times
     * 
     * int32 Best2MinStuntScore 
     * char* Best2MinStuntScorePlayers (NULL terminated string) 
     * int32 Best3MinStuntScore 
     * char* Best3MinStuntScorePlayers (NULL terminated string) 
     * int32 Best5MinStuntScore 
     * char* Best5MinStuntScorePlayers (NULL terminated string) 
     * int32 BestRacingLapTime 
     * char* BestRacingLapTimePlayers (NULL terminated string)
     */
    public byte Style;
    public string Ambience;

    public ushort FieldFilesNumber;
    public List<string> FieldFiles;
    public ushort Width;
    public ushort Height;
    public List<List<TrackTileSavable>> TrackTiles;

    public ushort DynamicObjectFilesNumber;
    public List<string> DynamicObjectFiles;
    public ushort DynamicObjectsNumber;
    public List<DynamicObjectSavable> DynamicObjects;

    public ushort CheckpointsNumber;
    public List<ushort> Checkpoints;
    public byte Permission;
    public float GroundBumpyness;
    public byte Scenery;
    public List<List<float>> Heightmap;
}
