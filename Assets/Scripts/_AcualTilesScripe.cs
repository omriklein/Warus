using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _AcualTilesScripe : MonoBehaviour
{
    private float radius = 9.8f;//big rings radius
    private float radius2 = 5.55f;//small rings radius

    public float angle_xz = 6.6f;//big ring    -- public for now, for debug
    public float angle_2 = 0.45f;//small ring  -- public for now, for debug

    public Material[] mats = new Material[2];//the materials that the tile will have ***Make private

    private bool isPressed = false;//does the tile is pressed

    private bool justPressed = false;//checks if the tile was just pressed (for the "Unpressed" function)

    private Tile tile;//the tile the object represent int the tiles map

    public Tile Tile
    {
        get { return tile; }
        set { tile = value; }
    }

    public float Angle_xz
    {
        get { return angle_xz; }
        set { angle_xz = value; }
    }

    public float Angle_2
    {
        get { return angle_2; }
        set { angle_2 = value; }
    }

    public bool IsPressed
    {
        get { return isPressed; }
        set { isPressed = value; }
    }

    public bool JustPressed
    {
        get { return justPressed; }
        set { justPressed = value; }
    }

    /// <summary>
    /// place the tile in the game
    /// </summary>
    void Start()
    {
        transform.localPosition = new Vector3(radius + radius2 * Mathf.Sin(angle_2), radius2 * Mathf.Cos(angle_2), 0);
        transform.localPosition = Quaternion.AngleAxis(angle_xz * 10.0f, Vector3.up) * transform.localPosition;
    }

    // Update is called once per frame
    void Update()///*******************TODO
    {

        //    //if (myTurn == true && isPressed)
        //    //OnMouseDown();
        //    if (!myTurn)
        //        Unpress();

        //    if (myTurn && isPressed)//if the object is pressed
        //    {
        //        HandleEnabled();//enable movement
        //    }
    }

    /// <summary>
    /// when the object is pressed
    /// </summary>
    public void OnMouseDown()
    {
        Renderer rnd = GetComponent<MeshRenderer>();
        rnd.material = mats[IsPressed ? 1 : 0];//change color
        IsPressed = !IsPressed;//opposit (not pressed => pressed)
        JustPressed = true;//the object was just pressed
    }

    /// <summary>
    /// unpress this object
    /// </summary>
    public void Unpress()
    {
        if (JustPressed)//if just pressed
        {
            JustPressed = false;
            this.GetComponentInParent<_MakeTorus>().TilePressed = (this.gameObject);//this is the chosen tile
            return;
        }
        IsPressed = false;//is not pressed any more (disables the movenemt)
        Renderer rnd = GetComponent<MeshRenderer>();
        rnd.material = mats[1];//change color
    }

}

