using UnityEngine;

public class Arrow: MonoBehaviour
{
    public Arrow prefab;
    private string[] images;
    private float time = 0;
    private int index = 0;
    private float fps = 25;
    private Renderer rend;
    private int cellCount;
    private bool canNext = false;

    void Start()
    {
        images = new string[29];
        rend = gameObject.GetComponent<Renderer>();
        for(;index<images.Length;index++)
        {
            images[index] = "Arrow/Arrow" + index.ToString();
        }
        index = 0;
        canNext = cellCount + 1 < GameManager.Instance.Map.Cells.Length;

    }


    void Update()
    {
        time += Time.deltaTime;

        if (time >= 1 / fps)
        {
            index++;
            time = 0;
        }
        if (index > images.Length - 1)
        {
            Destroy(gameObject);
        }
        else
        {
            if (index == 8)
            {
                if (canNext)
                {
                    StartNext();
                    canNext = false;
                }
            }
            rend.material.mainTexture = Resources.Load(images[index]) as Texture2D;
        }

    }

    public void StartNext()
    {
        Arrow arrow = Instantiate(prefab);
        arrow.cellCount = cellCount + 1;
        arrow.transform.position = GameManager.Instance.Map.Cells[(arrow.cellCount+1) 
            % GameManager.Instance.Map.Cells.Length].transform.position;
        arrow.transform.LookAt(GameManager.Instance.Map.Cells[arrow.cellCount].transform);
        arrow.transform.position = GameManager.Instance.Map.Cells[arrow.cellCount].transform.position + new Vector3(0, 0.1f, 0);
    }

    static public void StartFirst(Arrow origin)
    {
        Arrow arrow = Instantiate(origin);
        arrow.cellCount = 0;
        arrow.transform.position = GameManager.Instance.Map.Cells[(arrow.cellCount + 1)
            % GameManager.Instance.Map.Cells.Length].transform.position;
        arrow.transform.LookAt(GameManager.Instance.Map.Cells[arrow.cellCount].transform);
        arrow.transform.position = GameManager.Instance.Map.Cells[arrow.cellCount].transform.position + new Vector3(0, 0.1f, 0);
    }
}


