using System.IO;
using UnityEngine;
using UnityEditor;

public class Controller : MonoBehaviour
{
    public TrackSavable Track;
    public P3DModel Model;
    public string CrashdayPath = "";

	void Start ()
	{
	    CrashdayPath = IO.GetCrashdayPath();
		GetComponent<TileManager> ().LoadTiles ();
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 5, 100, 30), "Load map"))
        {
            string path = EditorUtility.OpenFilePanel("Open trk file", CrashdayPath + "/user/", "trk");
            if (path.Length != 0)
            {
				PlayerPrefs.SetString("lastmappath", path);
				MapParser mapParser = new MapParser();
                Track = mapParser.ReadMap(path);
                GetComponent<TrackManager>().LoadTrack(Track);
            }
        }

	    if (PlayerPrefs.HasKey("lastmappath"))
	    {
		    string LastMapPath = PlayerPrefs.GetString("lastmappath");
		    if (File.Exists(LastMapPath))
		    {
			    if (GUI.Button(new Rect(115, 5, 100, 30), "Load last map"))
			    {
				    MapParser mapParser = new MapParser();
				    Track = mapParser.ReadMap(LastMapPath);
				    GetComponent<TrackManager>().LoadTrack(Track);
			    }
		    }
	    }

	    if (GUI.Button(new Rect(220, 5, 100, 30), "Save map"))
	    {
		    string path = EditorUtility.SaveFilePanel("Save trk file", CrashdayPath + "/user/", "my_awesome_track", "trk");
			MapParser mapParser = new MapParser();
			mapParser.SaveMap(GetComponent<TrackManager>().CurrentTrack, path);
	    }
    }

}
