using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{

    // The resolution that the UI was initially designed for.
    Vector2 targetResolution = new Vector2(1920, 1080);

    // The Transform that should be resized, if not given then itself.
    Transform UIObject;

    void Start()
    {
        if (!UIObject)
            UIObject = transform;
        SizeResolution(new Vector2(Screen.width, Screen.height));
    }

    public void SizeResolution(Vector2 newResolution)
    {
        Vector3 scale = UIObject.localScale;
        scale.x = newResolution.x / targetResolution.x;
        scale.y = newResolution.y / targetResolution.y;
        UIObject.localScale = scale;
    }
}
