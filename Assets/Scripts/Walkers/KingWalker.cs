using UnityEngine;
using System.Collections;

public class KingWalker : CubeMovement
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
        if (isPressed && myTurn)
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
        string[] diractions = diraction(tile).Split('+');//vertical + horizontal
        foreach (string dir in diractions)
        {
            switch (dir)
            {
                case "Up": walkUp(); break;
                case "Down": walkDown(); break;
                case "Right": walkRight(); break;
                case "Left": walkLeft(); break;
            }
        }
        base.Start();
    }

    private string diraction(Tile tile)
    {
        //lines
        if (this.Tile.Up == tile)
            return "Up";
        if (this.Tile.Down == tile)
            return "Down";
        if (this.Tile.Right == tile)
            return "Right";
        if (this.Tile.Left == tile)
            return "Left";
        //cross
        if (this.Tile.Up.Right == tile)
            return "Up+Right";
        if (this.Tile.Up.Left == tile)
            return "Up+Left";
        if (this.Tile.Down.Right == tile)
            return "Down+Right";
        if (this.Tile.Down.Left == tile)
            return "Down+Left";

        return "";
    }
}
