using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SlantWalker : CubeMovement
{
    private _MakeTorus mt = new _MakeTorus();

    // Use this for initialization
    override protected void Start()
    {
        base.Start();
        tile = base.Tile;
    }

    // Update is called once per frame
    override protected void Update()
    {
        if (this.isPressed && this.myTurn)
        {
            if (mt.TilePressed != null)
            {
                walkPath(mt.TilePressed.GetComponent<_AcualTilesScripe>().Tile);
                this.Unpress();
                mt.TilePressed.GetComponent<_AcualTilesScripe>().Unpress();
                mt.TilePressed = null;
            }
        }
    }

    private void walkPath(Tile tile)
    {
        string[] diractions = diraction(tile).Split('+');//3 diraction to get to the tile (private string diraction)
        if (diractions.Length != 3)
        {
            Debug.Log(string.Format("no such diractoin for this player. you are in {0} , {1} and you're trying to go to {2} , {3}"
                    , this.tile.ValueX, this.tile.ValueY, tile.ValueX, tile.ValueY));
            return;
        }

        int times = int.Parse(diractions[2]);
        for (int i = 0; i < times; i++)
        {
            foreach (string dir in new string[] { diractions[0], diractions[1] })
            {
                switch (dir)
                {
                    case "Up": this.walkUp(); break;
                    case "Down": this.walkDown(); break;
                    case "Right": this.walkRight(); break;
                    case "Left": this.walkLeft(); break;
                }
            }
        }
        base.Start();
    }

    private string diraction(Tile tile)
    {
        Tile myTileUR = this.Tile;
        Tile myTileUL = this.Tile;
        Tile myTileDR = this.Tile;
        Tile myTileDL = this.Tile;

        for (int i = 1; i <= 3; i++)
        {
            myTileUR = cross(myTileUR, "Up", "Right");
            myTileUL = cross(myTileUL, "Up", "Left");
            myTileDR = cross(myTileDR, "Down", "Right");
            myTileDL = cross(myTileDL, "Down", "Left");

            if (myTileUR == tile)
            {
                if (myTileUR.Cube == null || !myTileUR.Cube.GetComponent<CubeMovement>().myTurn)
                    return "Up+Right+" + i;
                else
                    return "";
            }
            if (myTileUL == tile)
            {
                if (myTileUL.Cube == null || !myTileUL.Cube.GetComponent<CubeMovement>().myTurn)
                    return "Up+Left+" + i;
                else
                    return "";
            }
            if (myTileDR == tile)
            {
                if (myTileDR.Cube == null || !myTileDR.Cube.GetComponent<CubeMovement>().myTurn)
                    return "Down+Right+" + i;
                else
                    return "";
            }
            if (myTileDL == tile)
            {
                if (myTileDL.Cube == null || !myTileDL.Cube.GetComponent<CubeMovement>().myTurn)
                    return "Down+Left+" + i;
                else
                    return "";
            }
            if (myTileUR.Cube != null)
                myTileUR = this.Tile;
            if (myTileUL.Cube != null)
                myTileUL = this.Tile;
            if (myTileDR.Cube != null)
                myTileDR = this.Tile;
            if (myTileDL.Cube != null)
                myTileDL = this.Tile;
        }

        return "";
    }

    private Tile cross(Tile tile, string dirVertical, string dirHorizontal)
    {
        if (dirVertical == "Up")
        {
            tile = tile.Up;
            if (dirHorizontal == "Right")
                return tile.Right;
            else if (dirHorizontal == "Left")
                return tile.Left;
        }
        else if (dirVertical == "Down")
        {
            tile = tile.Down;
            if (dirHorizontal == "Right")
                return tile.Right;
            else if (dirHorizontal == "Left")
                return tile.Left;
        }
        return null;
    }
}
