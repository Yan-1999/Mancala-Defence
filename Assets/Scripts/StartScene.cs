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

    public Vector3 startPosition;
    public Vector3 endPosition;
    public CameraMove mainCameraMove;
    private bool isStart = true;

    void Start()
    {
        Camera.main.gameObject.transform.position = startPosition;
        selectCanvas.gameObject.SetActive(false);
        startButton.onClick.AddListener(delegate ()
        {
                MoveCamera(true);
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
            MoveCamera(false);
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

    private void Update()
    {
        if (!mainCameraMove.canMove)
        {
            if (isStart)
            {
                menuCanvas.gameObject.SetActive(true);
            }
            else
            {
                selectCanvas.gameObject.SetActive(true);
            }
        }
    }

    private void MoveCamera(bool isSelect)
    {
        if(isSelect)
        {
            menuCanvas.gameObject.SetActive(false);
            Camera.main.GetComponentInChildren<CameraMove>().MoveCamera(startPosition, endPosition);
            isStart = false;
        }
        else
        {
            selectCanvas.gameObject.SetActive(false);
            Camera.main.GetComponentInChildren<CameraMove>().MoveCamera(endPosition, startPosition);
            isStart = true;
        }
    }


    private void SelectStage(string name)
    {
        SceneManager.LoadScene(name);
    }
}
