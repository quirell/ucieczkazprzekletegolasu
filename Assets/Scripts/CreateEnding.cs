using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CreateEnding : MonoBehaviour {
	public GUIStyle titleStyle = new GUIStyle();
	public GUIStyle menuStyle = new GUIStyle();
	
	void OnGUI () {
		titleStyle.font = (Font)Resources.Load("MagicSchoolTwo");
		GUI.Label (new Rect (20, 20, 120, 50),
                "Gratulacje! Wygrałeś! Uciekłeś z przerażającego, przeklętego lasu!", 
                titleStyle);

		menuStyle.font = (Font)Resources.Load("MagicSchoolTwo");
	
		GUI.color = Color.yellow;
		Rect layerSelectPos = new Rect(30, 120, 120, 40);

		Event curEvent = Event.current;
		if (layerSelectPos.Contains(curEvent.mousePosition))
			menuStyle.normal.textColor = Color.white;
		else
			menuStyle.normal.textColor = Color.red;


		if (GUI.Button (new Rect (30, 120, 120, 40), "Menu", menuStyle)) {
			SceneManager.LoadScene("menu");
		}

		GUI.color = Color.yellow;
		layerSelectPos = new Rect(30, 180, 120, 40);
	
	}

}
