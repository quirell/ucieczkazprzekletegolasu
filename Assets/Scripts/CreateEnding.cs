using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CreateEnding : MonoBehaviour {
	public GUIStyle titleStyle = new GUIStyle();
	public GUIStyle menuStyle = new GUIStyle();
	
	void OnGUI () {
		titleStyle.font = (Font)Resources.Load("MagicSchoolTwo");
        if (Chest.success) {
            GUI.Label (new Rect (0, 40, Screen.width, 40),
                "Gratulacje! Wygrałeś! Uciekłeś z przerażającego, przeklętego lasu!", 
                titleStyle);
        } else {
            GUI.Label (new Rect (0, 40, Screen.width, 40),
                "O nie, przegrales! Musisz zostać w przerażającym, przeklętym lesie na zawsze!", 
                titleStyle);
        }

        GUI.Label (new Rect (0, 300, Screen.width, 40),
            "Credits:...", 
            titleStyle);
        
		menuStyle.font = (Font)Resources.Load("MagicSchoolTwo");
        Rect layerSelectPos = new Rect(0, 200, Screen.width, 40);

		Event curEvent = Event.current;
		if (layerSelectPos.Contains(curEvent.mousePosition))
            menuStyle.normal.textColor = Color.gray;
		else
            menuStyle.normal.textColor = Color.white;


        if (GUI.Button (new Rect (0, 200, Screen.width, 40), "Menu", menuStyle)) {
			SceneManager.LoadScene("menu");
		}

            
	}

}
