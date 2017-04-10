using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    private float slow = 3;
    private float sp = 0.05f;
    private float speed = 5.0f;
    private float speedRotation = 0.175f;
    private float speedRotation2 = 1f;
    private float radius = 150f;
    private float angle_2 = 0f;
    private float angle_xz = 0f;
    private float ZoomSpeed = 50f;
    private float minRadius = 20f;
    private float maxRadius = 400f;

    private float posx;
    private float posy;
    private float previosY;
    private float previosX;
    private float previosScrollData;

    private bool isMousepressed = false;

    // Use this for initialization
    void Start()
    {
        speed = 100 * sp;
        speedRotation = 3.5f * sp;
        speedRotation2 = 20 * sp;
    }

    // Update is called once per frame
    void Update()
    {
        Zoom();
        MouseMovement();
    }

    private void MouseMovement()
    {
        if (Input.GetMouseButtonDown(1))
        {
            posx = Input.mousePosition.x;
            posy = Input.mousePosition.y;
            isMousepressed = true;
            previosY = angle_2;
            previosX = angle_xz;
        }
        if (isMousepressed)
        {
            angle_2 = previosY + (posy - Input.mousePosition.y) / slow * sp;
            angle_xz = previosX + (Input.mousePosition.x - posx) / slow* sp;
        }

        if (Input.GetMouseButtonUp(1))
            isMousepressed = false;

        transform.localPosition = new Vector3(radius * Mathf.Sin(angle_2), radius * Mathf.Cos(angle_2), 0);
        transform.localPosition = Quaternion.AngleAxis(angle_xz * speed * 2, Vector3.up) * transform.localPosition;
        transform.localRotation = Quaternion.Euler(90 + Mathf.Rad2Deg * angle_2 * speedRotation2, 90 + Mathf.Rad2Deg * angle_xz * speedRotation, 0);

    }

    private void Zoom()
    {
        float Data = Input.GetAxis("Mouse ScrollWheel");
        radius -= Data * ZoomSpeed;
        radius = Mathf.Clamp(radius, minRadius, maxRadius);
        previosScrollData = Data;
    }
}
