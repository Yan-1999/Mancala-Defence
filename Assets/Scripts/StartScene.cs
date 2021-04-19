using UnityEngine.UI;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public Button startButton;
    public Button exitButton;
    public Button backButton;
    public Button selectStage1;
    public Button selectStage2;
    public Canvas menuCanvas;
    public Canvas selectCanvas;



    void Start()
    {
        selectCanvas.gameObject.SetActive(false);
        startButton.onClick.AddListener(delegate ()
        {
                ShowSelectCanvas(true);
        });
        exitButton.onClick.AddListener(delegate ()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        });
        backButton.onClick.AddListener(delegate ()
        {
            ShowSelectCanvas(false);
        });
        selectStage1.onClick.AddListener(delegate ()
        {
            SelectStage("Stage1");
        });
        selectStage2.onClick.AddListener(delegate ()
        {
            SelectStage("Stage2");
        });
    }

    private void ShowSelectCanvas(bool show)
    {
        selectCanvas.gameObject.SetActive(show);
        menuCanvas.gameObject.SetActive(!show);
    }

    private void SelectStage(string name)
    {
        SceneManager.LoadScene(name);
    }
}
