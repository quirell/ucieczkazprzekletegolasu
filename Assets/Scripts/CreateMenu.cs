using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CreateMenu : MonoBehaviour {
	public GUIStyle titleStyle = new GUIStyle();
	public GUIStyle buttonStyle = new GUIStyle();
	public GUIStyle exitButtonStyle = new GUIStyle();

	void OnGUI () {
		titleStyle.font = (Font)Resources.Load("MagicSchoolTwo");
		GUI.Label (new Rect (20, 20, 120, 50), "Ucieczka z zaczarowanego lasu", titleStyle);

		buttonStyle.font = (Font)Resources.Load("MagicSchoolTwo");
		exitButtonStyle.font = (Font)Resources.Load("MagicSchoolTwo");

		GUI.color = Color.yellow;
		Rect layerSelectPos = new Rect(30, 120, 120, 40);

		Event curEvent = Event.current;
		if (layerSelectPos.Contains(curEvent.mousePosition))
			buttonStyle.normal.textColor = Color.white;
		else
			buttonStyle.normal.textColor = Color.red;


		if (GUI.Button (new Rect (30, 120, 120, 40), "Play", buttonStyle)) {
			SceneManager.LoadScene("infterr");
		}

		GUI.color = Color.yellow;
		layerSelectPos = new Rect(30, 180, 120, 40);
		if (layerSelectPos.Contains(curEvent.mousePosition))
			exitButtonStyle.normal.textColor = Color.white;
		else
			exitButtonStyle.normal.textColor = Color.red;
		
		if (GUI.Button (new Rect (30, 180, 120, 40), "Exit", exitButtonStyle)) {
			Application.Quit ();
		}

	}

}
