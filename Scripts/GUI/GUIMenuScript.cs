using UnityEngine;

public class GUIMenuScript : MonoBehaviour
{
    protected Camera m_Camera = null;
    protected Input m_Input = null;
    private bool locked = false;
    private bool showMenu = false;
    //public string mainMenuSceneName;
    private bool showGraphicsDropDown = false;

    void Start()
    {
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    showMenu = !showMenu;
        //    locked = !locked;
        //    if (locked)
        //    {
        //        Screen.lockCursor = false;
        //    }
        //    else
        //    {
        //    }
        //}
    }

    void OnClick()
    {
        locked = !locked;
        showMenu = !showMenu;
    }

    void OnGUI()
    {
        if (!showMenu)
            return;

        //Make a background box
        GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 250, 200), "Setting Menu");

        //Make Main Menu button
        //if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 250, 50), "Main Menu"))
        //    Application.LoadLevel(mainMenuSceneName);

        //Make Change Graphics Quality button
        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2, 250, 50), "Change Graphics Quality"))
        {

            if (showGraphicsDropDown == false)
                showGraphicsDropDown = true;
            else
                showGraphicsDropDown = false;
        }

        //Create the Graphics settings buttons, these won't show automatically, they will be called when
        //the user clicks on the "Change Graphics Quality" Button, and then dissapear when they click on it again...
        if (showGraphicsDropDown == true)
        {
            if (GUI.Button(new Rect(Screen.width / 2 + 150, Screen.height / 2, 250, 50), "Fastest"))
                QualitySettings.SetQualityLevel(0);
            if (GUI.Button(new Rect(Screen.width / 2 + 150, Screen.height / 2 + 50, 250, 50), "Fast"))
                QualitySettings.SetQualityLevel(1);
            if (GUI.Button(new Rect(Screen.width / 2 + 150, Screen.height / 2 + 100, 250, 50), "Simple"))
                QualitySettings.SetQualityLevel(2);
            if (GUI.Button(new Rect(Screen.width / 2 + 150, Screen.height / 2 + 150, 250, 50), "Good"))
                QualitySettings.SetQualityLevel(3);
            if (GUI.Button(new Rect(Screen.width / 2 + 150, Screen.height / 2 + 200, 250, 50), "Beautiful"))
                QualitySettings.SetQualityLevel(4);
            if (GUI.Button(new Rect(Screen.width / 2 + 150, Screen.height / 2 + 250, 250, 50), "Fantastic"))
                QualitySettings.SetQualityLevel(5);

            if (Input.GetKeyDown(KeyCode.Escape))
                showGraphicsDropDown = false;
        }

        //Make quit game button
        //if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 50, 250, 50), "Quit Game"))
        //{
        //    Application.Quit();
        //}

    }
}