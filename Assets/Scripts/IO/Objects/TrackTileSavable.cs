using System.Xml.Serialization;

[System.Serializable]
public class TrackTileSavable
{
	[XmlAttribute("id")]
    public ushort FieldId;
	[XmlAttribute("rotation")]
    public byte Rotation;
	[XmlAttribute("mirrored")]
    public byte IsMirrored;
	[XmlAttribute("height")]
    public byte Height;

	public TrackTileSavable()
	{
	}

	public TrackTileSavable(ushort id, byte rotation, byte isMirrored, byte height)
	{
		FieldId = id;
		Rotation = rotation;
		IsMirrored = isMirrored;
		Height = height;
	}

}
