using System.Collections.Generic;
using System.Xml.Serialization;

[System.Serializable]
public class TrackSavable
{
	[XmlAttribute("time")]
    public int CurrentTime;         //unused
	[XmlAttribute("author")]
    public string Author;
	[XmlAttribute("comment")]
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
	[XmlAttribute("style")]
    public byte Style;
	[XmlAttribute("ambience")]
    public string Ambience;

	[XmlAttribute("fieldFiles")]
    public ushort FieldFilesNumber;
	[XmlAttribute("fields")]
    public List<string> FieldFiles;
	[XmlAttribute("width")]
    public ushort Width;
	[XmlAttribute("height")]
    public ushort Height;
	[XmlAttribute("tracktiles")]
    public List<List<TrackTileSavable>> TrackTiles;

	[XmlAttribute("dynobjsfilesnum")]
    public ushort DynamicObjectFilesNumber;
	[XmlAttribute("dynobjsfiles")]
    public List<string> DynamicObjectFiles;
	[XmlAttribute("dynobjsnum")]
    public ushort DynamicObjectsNumber;
	[XmlAttribute("dynobjs")]
    public List<DynamicObjectSavable> DynamicObjects;

	[XmlAttribute("checkpointnumber")]
    public ushort CheckpointsNumber;
	[XmlAttribute("checkpoints")]
    public List<ushort> Checkpoints;
	[XmlAttribute("permission")]
    public byte Permission;
	[XmlAttribute("bumpyness")]
    public float GroundBumpyness;
	[XmlAttribute("scenery")]
    public byte Scenery;
	[XmlAttribute("heightmap")]
    public List<List<float>> Heightmap;
}
