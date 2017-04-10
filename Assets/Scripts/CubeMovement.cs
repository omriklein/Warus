using UnityEngine;
using System.Collections;
using System;

public class CubeMovement : MonoBehaviour
{
    private float radius = 100;//big rings radius
    private float radius2 = 51;//small rings radius

    private float angle_xz = 0f; //big rings
    private float angle_2 = (Mathf.Deg2Rad * 360) / (14 * 4);//small rings

    private float speed_xz = (Mathf.Deg2Rad * 360) / 30; //speed on the big rings
    private float speed_2 = (Mathf.Deg2Rad * 360) / 14; //speed on the small rings

    public Material[] mats = new Material[2];//the materials that the object will have ***Make private

    protected bool isPressed = false;//does the cube is pressed
    protected bool justPressed = false;//checks if the object was just pressed (for the "Unpressed" function)
    public bool myTurn = false;//is it the object's team turn

    protected Tile tile;//the tile the object is on

    /// <summary>
    /// the tile the object is on
    /// </summary>
    public Tile Tile
    {
        get { return tile; }
        set { tile = value; }
    }

    /// <summary>
    /// big rings
    /// </summary>
    public float Angle_xz
    {
        get { return angle_xz; }
        set { angle_xz = value; }
    }

    /// <summary>
    /// small rings
    /// </summary>
    public float Angle_2
    {
        get { return angle_2; }
        set { angle_2 = value; }
    }

    /// <summary>
    /// put the object in position
    /// </summary>
    protected virtual void Start()
    {

        //postion - CHECK - the Equation
        transform.localPosition = new Vector3((radius + radius2 * Mathf.Cos(Angle_2)) * Mathf.Cos(Angle_xz), radius2 * Mathf.Sin(Angle_2), (radius + radius2 * Mathf.Cos(Angle_2)) * Mathf.Sin(Angle_xz));

        //rotation - MAKE

        transform.localRotation = Quaternion.Euler(90 - Mathf.Rad2Deg * angle_2 , 90 - Mathf.Rad2Deg * angle_xz, 0);
    }

    // Update is called once per frame
    virtual protected void Update()//********TODO
    {
        //if this is my turn and the object is pressed - the other player turn

        //if (myTurn == true && isPressed)
        //OnMouseDown(); 

        //if this is not my durn - unpress this object ==> this is not good
        //the script will run on all the cubes every time
        //TODO - put the unpress inside the change group function

        //if (!myTurn)
        //    Unpress();

        if (/*myTurn && */isPressed)//if the object is pressed (and this is my turn)
        {
            HandleEnabled();//enable movement
        }
    }

    private void HandleEnabled()
    {
        if (Input.GetKeyDown(KeyCode.W))//walk up
        { walkUp(); }

        if (Input.GetKeyDown(KeyCode.S))//walk down
        { walkDown(); }

        if (Input.GetKeyDown(KeyCode.D))//walk right
        { walkRight(); }

        if (Input.GetKeyDown(KeyCode.A))//walk left
        { walkLeft(); }

        Start();// place at posotion and rotation

    }

    //functions to walk one step in any diraction
    protected void walkUp()
    {
        if (tile.Up.Cube == null || tile.Up.Cube != null && !tile.Up.Cube.GetComponent<CubeMovement>().myTurn)
        {
            Angle_xz += speed_xz;
            myTurn = false;
            tile.Cube = null;
            tile = tile.Up;
            if (tile.Cube != null)
                transform.parent.gameObject.GetComponent<CubeManager>().deleteObject(tile.Cube);
            tile.Cube = this.gameObject;

            //Unpress()???
        }
    }
    protected void walkDown()
    {
        if (tile.Down.Cube == null || tile.Down.Cube != null && !tile.Down.Cube.GetComponent<CubeMovement>().myTurn)
        {
            Angle_xz -= speed_xz;
            myTurn = false;
            tile.Cube = null;
            tile = tile.Down;
            if (tile.Cube != null)
                transform.parent.gameObject.GetComponent<CubeManager>().deleteObject(tile.Cube);
            tile.Cube = this.gameObject;
        }
    }
    protected void walkRight()
    {
        if (tile.Right.Cube == null || tile.Right.Cube != null && !tile.Right.Cube.GetComponent<CubeMovement>().myTurn)
        {
            angle_2 += speed_2;
            myTurn = false;
            tile.Cube = null;
            tile = tile.Right;
            if (tile.Cube != null)
                transform.parent.gameObject.GetComponent<CubeManager>().deleteObject(tile.Cube);
            tile.Cube = this.gameObject;
        }
    }
    protected void walkLeft()
    {
        if (tile.Left.Cube == null || tile.Left.Cube != null && !tile.Left.Cube.GetComponent<CubeMovement>().myTurn)
        {
            angle_2 -= speed_2;
            myTurn = false;
            tile.Cube = null;
            tile = tile.Left;
            if (tile.Cube != null)
                transform.parent.gameObject.GetComponent<CubeManager>().deleteObject(tile.Cube);
            tile.Cube = this.gameObject;
        }
    }

    /// <summary>
    /// when the object is pressed
    /// </summary>
    protected void OnMouseUp()
    {
        if (myTurn)
        {
            Renderer rnd = GetComponent<MeshRenderer>();
            rnd.material = mats[isPressed ? 1 : 0];//change color (material)
            isPressed = !isPressed;//opposit (not pressed => pressed)
            justPressed = true;//the object was just pressed
        }
    }

    /// <summary>
    /// unpress this object
    /// </summary>
    public void Unpress()
    {
        if (justPressed)//if just pressed
        {
            justPressed = false;
            return;
        }
        isPressed = false;//is not pressed any more (disables the movenemt)
        Renderer rnd = GetComponent<MeshRenderer>();
        rnd.material = mats[1];//change color
    }

}
