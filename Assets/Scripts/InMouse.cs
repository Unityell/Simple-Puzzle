using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

public class InMouse : MonoBehaviour
{
    [Inject] EventBus EventBus;
    [HideInInspector] public Image Icon;
    [HideInInspector] public Socket Socket;
    void Start()
    {
        Icon = GetComponent<Image>();
    }
    public void SetItemToMouse(Sprite Icon, Socket Socket)
    {
        this.Icon.enabled = true;
        this.Icon.sprite = Icon;
        this.Socket = Socket;
    }
    public void Clear()
    {
        this.Icon.enabled = false;
        Icon.sprite = null;
        Socket = null;
    }
    public void ReturnItemToSocket()
    {
        Socket.SetItemToSocket(Icon.sprite);
        Clear();
    }
    void LateUpdate()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if(Icon.sprite != null)
            {
                ReturnItemToSocket();
            }
        }             
    }
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z; 
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = worldPosition;
    }
}
