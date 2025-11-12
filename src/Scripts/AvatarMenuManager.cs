using UnityEngine;

public class AvatarMenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject buildPanel;
    public GameObject facePanel;
    public GameObject skinPanel;
    public GameObject hairPanel;
    public GameObject wardrobePanel;
    void Start()
    {
        ShowMainMenu();
    }
    public void ShowMainMenu()
    {

        mainMenu.SetActive(true);
        buildPanel.SetActive(false);
        facePanel.SetActive(false);
        skinPanel.SetActive(false);
        hairPanel.SetActive(false);
        wardrobePanel.SetActive(false);
    }
    public void ShowBuildPanel()
    {
        mainMenu.SetActive(false);
        buildPanel.SetActive(true);
    }
    public void ShowFacePanel()
    {
        mainMenu.SetActive(false);
        facePanel.SetActive(true);
    }
    public void ShowSkinPanel()
    {
        mainMenu.SetActive(false);
        skinPanel.SetActive(true);
    }

    public void ShowHairPanel()
    {
        mainMenu.SetActive(false);
        hairPanel.SetActive(true);
    }    
    public void ShowWardrobePanel()
    {
        mainMenu.SetActive(false);
        wardrobePanel.SetActive(true);
    }
}


