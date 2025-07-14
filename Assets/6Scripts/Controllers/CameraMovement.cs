using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    public Vector3 startPos;
    [SerializeField] float speed;
    [SerializeField] Vector2 camPos;
    public Vector2 xBorder;
    public Vector2 zBorder;

    [SerializeField] private GameObject cameraGO;
    [SerializeField] private float rotationAngle = -45f; // Кут обертання

    [Header("More settings")]
    [SerializeField] float increaseRangeZoneX;
    [SerializeField] float increaseRangeZoneY;
    public static CameraMovement instance;

    public bool canMove;
    public bool drag;
    private Vector2 lastTouchPosition;

    [SerializeField] bool linearMoving;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        camPos = new Vector2(startPos.x, startPos.z);
        speed = Screen.width / 10;
        SetBorders();
    }
    public void SetBorders()
    {
        List<IslandData> islands = IslandBuilding.Instance.GetIslands();
        xBorder = Vector2.zero;
        zBorder = Vector2.zero;
        for (int i = 0; i < islands.Count; i++)
        {
            if (xBorder.x < islands[i].gameObject.transform.position.x)
            {
                xBorder.x = islands[i].transform.position.x;
            }
            if (xBorder.y > islands[i].transform.position.x)
            {
                xBorder.y = islands[i].transform.position.x;
            }

            if (zBorder.x < islands[i].transform.position.z)
            {
                zBorder.x = islands[i].transform.position.z;
            }
            if (zBorder.y > islands[i].transform.position.z)
            {
                zBorder.y = islands[i].transform.position.z;
            }
        }
        xBorder += new Vector2(increaseRangeZoneY, -increaseRangeZoneX);
        zBorder += new Vector2(increaseRangeZoneX, -increaseRangeZoneY);
    }
    private void OnDrawGizmos()
    {
        Vector3 point1 = new Vector3(xBorder.x, cameraGO.transform.position.y, zBorder.y);
        Vector3 point2 = new Vector3(xBorder.x, cameraGO.transform.position.y, zBorder.x);
        Vector3 point3 = new Vector3(xBorder.y, cameraGO.transform.position.y, zBorder.x);
        Vector3 point4 = new Vector3(xBorder.y, cameraGO.transform.position.y, zBorder.y);
        Debug.DrawLine(point1, point2, Color.blue);
        Debug.DrawLine(point2, point3, Color.blue);
        Debug.DrawLine(point3, point4, Color.blue);
        Debug.DrawLine(point4, point1, Color.blue);
    }
    public void ChangeMovingType(bool linear)
    {

        if (linear == false)
        {
            linearMoving = linear;
            speed = Screen.width / 10;
            // Обчислення кута обертання в радіанах
            float angleRad = rotationAngle * Mathf.Deg2Rad;
            float cos = Mathf.Cos(angleRad);
            float sin = Mathf.Sin(angleRad);

            // Отримання поточної позиції трансформа
            Vector3 localPosition = transform.localPosition;

            // Перетворення координат трансформа назад в координати camPos
            float newX = localPosition.x * cos + localPosition.z * sin;
            float newY = -localPosition.x * sin + localPosition.z * cos;

            // Присвоєння значень в camPos
            camPos.x = newX;
            camPos.y = newY;
        }
        else
        {
            linearMoving = linear;
            speed = Screen.width / 20;
            camPos.x = transform.position.x;
            camPos.y = transform.position.z;
        }
    }
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Початок перетягування
                lastTouchPosition = touch.position;
                if (!EventSystem.current.IsPointerOverGameObject(0))
                {
                    canMove = true;
                }
            }
            else if (touch.phase == TouchPhase.Moved && canMove)
            {
                drag = true;
                // Рух перетягування
                Vector2 deltaPosition = touch.position - lastTouchPosition;

                if (linearMoving)
                {
                    camPos.x -= deltaPosition.x / speed;
                    camPos.y -= deltaPosition.y / speed;
                    // Обмеження camPos за межами бордерів
                    camPos.x = Mathf.Clamp(camPos.x, xBorder.y, xBorder.x);
                    camPos.y = Mathf.Clamp(camPos.y, zBorder.y, zBorder.x);

                    cameraGO.transform.localPosition = new Vector3(camPos.x, cameraGO.transform.position.y, camPos.y);

                    // Оновлення останньої позиції торкання
                    lastTouchPosition = touch.position;
                }
                else
                {
                    camPos.x += deltaPosition.y / speed;
                    camPos.y -= deltaPosition.x / speed;
                    // Обрахування нової позиції з врахуванням обертання
                    float angleRad = rotationAngle * Mathf.Deg2Rad;
                    float cos = Mathf.Cos(angleRad);
                    float sin = Mathf.Sin(angleRad);

                    float newX = camPos.x * cos - camPos.y * sin;
                    float newZ = camPos.x * sin + camPos.y * cos;

                    // Обмеження camPos за локальними осями
                    newX = Mathf.Clamp(newX, xBorder.y, xBorder.x);
                    newZ = Mathf.Clamp(newZ, zBorder.y, zBorder.x);

                    // Застосування нової позиції
                    Vector3 localPosition = new Vector3(newX, transform.position.y, newZ);
                    transform.localPosition = localPosition;

                    // Зворотне обчислення camPos з врахуванням нової позиції
                    camPos.x = localPosition.x * cos + localPosition.z * sin;
                    camPos.y = -localPosition.x * sin + localPosition.z * cos;

                    // Оновлення останньої позиції торкання
                    lastTouchPosition = touch.position;
                }

            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                // Завершення перетягування
                canMove = false;
                drag = false;
            }
        }
    }
}