using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CreateMenu : MonoBehaviour {
	public GUIStyle titleStyle = new GUIStyle();
	public GUIStyle buttonStyle = new GUIStyle();
	public GUIStyle exitButtonStyle = new GUIStyle();

	void OnGUI () {
        
        GUI.Box (new Rect (0, 30, Screen.width, 70), " ");

        titleStyle.font = (Font)Resources.Load("MagicSchoolTwo");
        GUI.Box (new Rect (0, 40, Screen.width, 40), "Ucieczka z zaczarowanego lasu", titleStyle);

		buttonStyle.font = (Font)Resources.Load("MagicSchoolTwo");
		exitButtonStyle.font = (Font)Resources.Load("MagicSchoolTwo");

        GUI.color = Color.yellow;
        Rect layerSelectPos = new Rect(0, 140, Screen.width, 40);

		Event curEvent = Event.current;
		if (layerSelectPos.Contains(curEvent.mousePosition))
			buttonStyle.normal.textColor = Color.white;
		else
            buttonStyle.normal.textColor = Color.red;


        if (GUI.Button (new Rect (0, 140, Screen.width, 40), "Play", buttonStyle)) {
			SceneManager.LoadScene("infterr");
		}

        GUI.color = Color.yellow;
        layerSelectPos = new Rect(0, 200, Screen.width, 40);
		if (layerSelectPos.Contains(curEvent.mousePosition))
			exitButtonStyle.normal.textColor = Color.white;
		else
            exitButtonStyle.normal.textColor = Color.red;
		
        if (GUI.Button (new Rect (0, 200, Screen.width, 40), "Exit", exitButtonStyle)) {
			Application.Quit ();
		}

	}

}
