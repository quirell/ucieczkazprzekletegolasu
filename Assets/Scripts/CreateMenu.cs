using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CreateMenu : MonoBehaviour {
	
	public GUIStyle titleStyle = new GUIStyle();
	public GUIStyle buttonStyle = new GUIStyle();

	void OnGUI () {
		titleStyle.font = (Font)Resources.Load("MagicSchoolTwo");
		//titleStyle.fontSize = 50;

		buttonStyle.font = (Font)Resources.Load("MagicSchoolTwo");
		//buttonStyle.fontSize = 40;

		GUI.color = Color.red;
		GUI.Label (new Rect (20, 20, 120, 50), "Ucieczka z zaczarowanego lasu", titleStyle);

		GUI.color = Color.yellow;
		if (GUI.Button (new Rect (30, 120, 120, 40), "Play", buttonStyle)) {
			Debug.Log("Start button clicked");
			//Application.LoadLevel ("infterr");
			SceneManager.LoadScene("infterr");
		}
			

		if (GUI.Button (new Rect (30, 180, 120, 40), "Exit", buttonStyle)) {
			Debug.Log("End button clicked");
		}

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
