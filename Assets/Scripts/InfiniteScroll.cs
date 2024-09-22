using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class InfiniteScroll : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerExitHandler
{
    public enum ScrollDirection
    {
        Horizontal,
        Vertical
    }

    public ScrollDirection scrollDirection = ScrollDirection.Horizontal; // Выбор направления скролла
    public float sensitivity = 1f;
    public float decelerationRate = 1f;
    public float maxVelocity = 5000f;
    public float autoScrollSpeed = 50f;
    public float minAutoScrollSpeed = 2f;
    public float autoScrollAcceleration = 2f;

    private RectTransform contentRect;
    private Vector2 lastPointerPosition;
    private Vector2 pointerVelocity;

    private bool isDragging = false;
    private bool isAutoScrolling = false;
    private int autoScrollDirection = 1;

    private Coroutine autoScrollCoroutine;

    void Start()
    {
        contentRect = GetComponent<RectTransform>();
        StartAutoScroll();
    }

    void Update()
    {
        if (Time.deltaTime == 0) return;

        if (!isDragging && isAutoScrolling)
        {
            autoScrollSpeed += autoScrollAcceleration * Time.deltaTime;
            autoScrollSpeed = Mathf.Max(autoScrollSpeed, minAutoScrollSpeed);

            foreach (Transform child in transform)
            {
                Vector3 pos = child.localPosition;
                
                // Проверка направления скролла
                if (scrollDirection == ScrollDirection.Horizontal)
                {
                    pos.x += autoScrollDirection * autoScrollSpeed * Time.deltaTime;
                }
                else
                {
                    pos.y += autoScrollDirection * autoScrollSpeed * Time.deltaTime;
                }

                if (float.IsNaN(pos.x) || float.IsNaN(pos.y) || float.IsNaN(pos.z))
                {
                    Debug.LogError("pos contains NaN values");
                    pos = Vector3.zero;
                }

                child.localPosition = pos;
            }

            ClampItemsToBounds();
        }
        else if (!isDragging && pointerVelocity.magnitude > 0.01f)
        {
            pointerVelocity -= pointerVelocity * decelerationRate * Time.deltaTime;
            pointerVelocity = Vector2.ClampMagnitude(pointerVelocity, maxVelocity);

            foreach (Transform child in transform)
            {
                Vector3 pos = child.localPosition;

                // Проверка направления скролла
                if (scrollDirection == ScrollDirection.Horizontal)
                {
                    pos.x += pointerVelocity.x * Time.deltaTime;
                }
                else
                {
                    pos.y += pointerVelocity.y * Time.deltaTime;
                }

                if (float.IsNaN(pos.x) || float.IsNaN(pos.y) || float.IsNaN(pos.z))
                {
                    Debug.LogError("pos contains NaN values");
                    pos = Vector3.zero;
                }

                child.localPosition = pos;
            }

            ClampItemsToBounds();
        }
        else if (!isDragging && pointerVelocity.magnitude <= 0.01f)
        {
            StopAutoScroll();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag == gameObject)
        {
            Vector2 currentPointerPosition = eventData.position;
            Vector2 delta = currentPointerPosition - lastPointerPosition;

            if (float.IsNaN(delta.x) || float.IsNaN(delta.y))
            {
                Debug.LogError("delta contains NaN values");
                delta = Vector2.zero;
            }

            pointerVelocity = delta / Time.deltaTime * sensitivity;

            if (float.IsNaN(pointerVelocity.x) || float.IsNaN(pointerVelocity.y))
            {
                Debug.LogError("pointerVelocity contains NaN values");
                pointerVelocity = Vector2.zero;
            }

            foreach (Transform child in transform)
            {
                Vector3 pos = child.localPosition;

                // Проверка направления скролла
                if (scrollDirection == ScrollDirection.Horizontal)
                {
                    pos.x += delta.x * sensitivity;
                }
                else
                {
                    pos.y += delta.y * sensitivity;
                }

                if (float.IsNaN(pos.x) || float.IsNaN(pos.y) || float.IsNaN(pos.z))
                {
                    Debug.LogError("pos contains NaN values");
                    pos = Vector3.zero;
                }

                child.localPosition = pos;
            }

            ClampItemsToBounds();
            lastPointerPosition = currentPointerPosition;

            if (autoScrollCoroutine != null)
            {
                StopCoroutine(autoScrollCoroutine);
                autoScrollCoroutine = null;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag == gameObject)
        {
            isDragging = true;
            lastPointerPosition = eventData.position;
            pointerVelocity = Vector2.zero;

            StopAutoScroll();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag == gameObject)
        {
            isDragging = false;
            autoScrollDirection = (int)Mathf.Sign(scrollDirection == ScrollDirection.Horizontal ? pointerVelocity.x : pointerVelocity.y);

            if (pointerVelocity.magnitude > 0.01f)
            {
                StartCoroutine(DecelerateVelocity());
            }

            ClampItemsToBounds();
        }
    }

    private IEnumerator DecelerateVelocity()
    {
        while (pointerVelocity.magnitude > 0.01f)
        {
            pointerVelocity -= pointerVelocity * decelerationRate * Time.deltaTime;
            pointerVelocity = Vector2.ClampMagnitude(pointerVelocity, maxVelocity);

            foreach (Transform child in transform)
            {
                Vector3 pos = child.localPosition;

                // Проверка направления скролла
                if (scrollDirection == ScrollDirection.Horizontal)
                {
                    pos.x += pointerVelocity.x * Time.deltaTime;
                }
                else
                {
                    pos.y += pointerVelocity.y * Time.deltaTime;
                }

                if (float.IsNaN(pos.x) || float.IsNaN(pos.y) || float.IsNaN(pos.z))
                {
                    Debug.LogError("pos contains NaN values");
                    pos = Vector3.zero;
                }

                child.localPosition = pos;
            }

            ClampItemsToBounds();
            yield return null;
        }
    }

    private void ClampItemsToBounds()
    {
        Rect bounds = contentRect.rect;

        foreach (Transform child in transform)
        {
            Vector3 pos = child.localPosition;

            // Проверка направления скролла
            if (scrollDirection == ScrollDirection.Horizontal)
            {
                if (pos.x > bounds.max.x)
                {
                    pos.x -= bounds.width;
                }
                else if (pos.x < bounds.min.x)
                {
                    pos.x += bounds.width;
                }
            }
            else
            {
                if (pos.y > bounds.max.y)
                {
                    pos.y -= bounds.height;
                }
                else if (pos.y < bounds.min.y)
                {
                    pos.y += bounds.height;
                }
            }

            if (float.IsNaN(pos.x) || float.IsNaN(pos.y) || float.IsNaN(pos.z))
            {
                Debug.LogError("pos contains NaN values");
                pos = Vector3.zero;
            }

            child.localPosition = pos;
        }
    }

    private void StartAutoScroll()
    {
        if (!isAutoScrolling)
        {
            isAutoScrolling = true;
            autoScrollSpeed = minAutoScrollSpeed;
            autoScrollCoroutine = StartCoroutine(AutoScrollCoroutine());
        }
    }

    private void StopAutoScroll()
    {
        if (isAutoScrolling)
        {
            isAutoScrolling = false;
            if (autoScrollCoroutine != null)
            {
                StopCoroutine(autoScrollCoroutine);
                autoScrollCoroutine = null;
            }
        }
    }

    private IEnumerator AutoScrollCoroutine()
    {
        while (isAutoScrolling)
        {
            foreach (Transform child in transform)
            {
                Vector3 pos = child.localPosition;

                // Проверка направления скролла
                if (scrollDirection == ScrollDirection.Horizontal)
                {
                    pos.x += autoScrollDirection * autoScrollSpeed * Time.deltaTime;
                }
                else
                {
                    pos.y += autoScrollDirection * autoScrollSpeed * Time.deltaTime;
                }

                if (float.IsNaN(pos.x) || float.IsNaN(pos.y) || float.IsNaN(pos.z))
                {
                    Debug.LogError("pos contains NaN values");
                    pos = Vector3.zero;
                }

                child.localPosition = pos;
            }

            ClampItemsToBounds();
            yield return null;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDragging)
        {
            isDragging = false;
            pointerVelocity = Vector2.zero;
        }
    }
}