using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Tile
{

    private int valueX;
    private int valueY;
    private Tile right;
    private Tile left;
    private Tile up;
    private Tile down;

    private GameObject cube;
    private _AcualTilesScripe acualTile;


    public int ValueX
    {
        get { return this.valueX; }
        set { this.valueX = value; }
    }
    public int ValueY
    {
        get { return this.valueY; }
        set { this.valueY = value; }
    }

    /// <summary>
    /// the tile above
    /// </summary>
    public Tile Up
    {
        get { return up; }
        private set
        {
            up = value;
            if (value != null)
                value.down = this;
        }
    }
    /// <summary>
    /// the tile under
    /// </summary>
    public Tile Down
    {
        get { return down; }
        private set
        {
            down = value;
            if (value != null)
                value.up = this;
        }
    }
    /// <summary>
    /// the tile on the right
    /// </summary>
    public Tile Right
    {
        get { return right; }
        private set
        {
            right = value;
            value.left = this;
        }
    }
    /// <summary>
    /// the tile on the left
    /// </summary>
    public Tile Left
    {
        get { return left; }
        private set
        {
            left = value;
            value.up = this;
        }
    }

    /// <summary>
    /// the player (cube) on this tile
    /// </summary>
    public GameObject Cube
    {
        get { return cube; }
        set { cube = value; }
    }

    /// <summary>
    /// the tile in the game
    /// </summary>
    public _AcualTilesScripe AcualTile
    {
        get { return acualTile; }
        set { acualTile = value; }
    }

    /// <summary>
    /// tile in the back (not visible)
    /// </summary>
    public Tile(int valueX, int valueY) : this(valueX, valueY, null, null, null, null) { }

    /// <summary>
    /// tile in the back (not visible)
    /// </summary>
    public Tile(int valueX, int valueY, Tile right, Tile left, Tile up, Tile down)
    {
        this.valueX = valueX;
        this.valueY = valueY;
        this.right = right;
        this.left = left;
        this.up = up;
        this.down = down;
    }

    /// <summary>
    /// make a board / map of tiles
    /// </summary>
    /// <returns> returns the first tile</returns>
    public static Tile CreateMap(int height, int weight)
    {
        if (height <= 0 || weight <= 0)//check if the map has height and weight
            throw new Exception("the height and the weight have to be more then zero.");

        Tile first = new Tile(0, 0);// the first tile
        Tile last1 = first;//for the rows
        Tile last2 = first;//for the columns

        /// <summary>
        /// makes the tiles and link between them
        /// </summary>

        for (int i = 0; i < height; i++)
        {
            Console.Write(last2.ValueX + "/" + last2.valueY + ", ");
            for (int j = 1; j < weight; j++)
            {
                last2.Right = new Tile(i, j);//new tile in the same row
                last2 = last2.Right;
                Console.Write(last2.ValueX + "/" + last2.valueY + ", ");

                if (last2.Left != null && last2.Left.Up != null && last2.Left.Up.Right != null)
                {
                    last2.Up = last2.Left.Up.Right;//link
                }
                if (i == (height - 1))
                {
                    last1.Down = first;
                    last2.Down = last2.Left.Down.Right;
                }
            }
            last2.Right = last1;

            last1.Down = new Tile(i + 1, 0);
            last1 = last1.Down;
            last2 = last1;
        }
        return first;
    }

    public void Walk(Tile n)
    {
        Debug.Log(" you are at " + n.ValueX + "/" + n.valueY);

        if (Input.GetKeyDown(KeyCode.S))
        {
            Walk(n.Down);
            if (n.Down.Cube != null)
                Debug.Log("met");
        }
        if (Input.GetKeyDown(KeyCode.W))
            Walk(n.Up);
        if (Input.GetKeyDown(KeyCode.D))
            Walk(n.Right);
        if (Input.GetKeyDown(KeyCode.A))
            Walk(n.Left);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
