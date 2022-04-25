
using UnityEngine;

public class CameraScreenResolution : MonoBehaviour
{

    public bool maintainWidth = true;
    [Range(-1, 1)] public float adaptPostion = 0;
    float defaultHeight, defaultWidth;
    Vector3 cameraPos;

    void Start()
    {
        cameraPos = Camera.main.transform.position;
        defaultHeight = Camera.main.orthographicSize;
        defaultWidth = Camera.main.orthographicSize * Camera.main.aspect;
    }

    void Update()
    {
        if (maintainWidth)
        {
            Camera.main.orthographicSize = defaultWidth / Camera.main.aspect;
            Camera.main.transform.position = new Vector3(cameraPos.x, adaptPostion * (defaultHeight - Camera.main.orthographicSize), cameraPos.z);
        } else
        {
            Camera.main.transform.position = new Vector3(adaptPostion * (defaultWidth - Camera.main.orthographicSize*Camera.main.aspect), cameraPos.y, cameraPos.z);
        }
    }
}
