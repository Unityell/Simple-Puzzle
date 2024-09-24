using UnityEngine;
using UnityEngine.UI;

public class InMouse : MonoBehaviour
{
    [HideInInspector] public Image Icon;
    [HideInInspector] public Socket Socket;

    void Start()
    {
        Icon = GetComponent<Image>();
        Clear();
    }

    public void SetItemToMouse(Sprite icon, Socket socket)
    {
        Move();
        this.Icon.enabled = true;
        this.Icon.sprite = icon;
        this.Socket = socket;
    }

    public void Clear()
    {
        this.Icon.enabled = false;
        Icon.sprite = null;
        Socket = null;
    }
    
    public void ReturnItemToSocket()
    {
        if (Socket != null)
        {
            Socket.SetItemToSocket(Icon.sprite);
            Clear();
        }
    }

    void LateUpdate()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (Icon.sprite != null)
            {
                ReturnItemToSocket();
            }
        }             
    }

    void Move()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z; 
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = worldPosition;        
    }

    void Update()
    {
        if (Icon.enabled) Move();
    }
}
