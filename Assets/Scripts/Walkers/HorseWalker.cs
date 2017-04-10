using UnityEngine;
using System.Collections;

public class HorseWalker : CubeMovement
{
    private _MakeTorus mt = new _MakeTorus();

    // Use this for initialization
    override protected void Start()
    {
        base.Start();
        tile = base.tile;
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
        string[] diractions = diraction(tile).Split('+');//3 diraction to get to the tile (private string diraction)
        foreach (string dir in diractions)
        {
            switch (dir)
            {
                case "Up": this.walkUp(); break;
                case "Down": this.walkDown(); break;
                case "Right": this.walkRight(); break;
                case "Left": this.walkLeft(); break;
                default:
                    Debug.Log(string.Format("no such diractoin for this player. you are in {0} , {1} and you're trying to go to {2} , {3}"
                        , this.tile.ValueX, this.tile.ValueY, tile.ValueX, tile.ValueY));
                    Unpress(); break;
            }
        }
        base.Start();
    }

    private string diraction(Tile tile)
    {

        Tile myTile = this.tile;

        Tile upTile = myTile.Up.Up;
        Tile downTile = myTile.Down.Down;
        Tile rightTile = myTile.Right.Right;
        Tile leftTile = myTile.Left.Left;

        if (upTile.Left == tile)
            return "Up+Up+Left";
        if (upTile.Right == tile)
            return "Up+Up+Right";
        if (downTile.Right == tile)
            return "Down+Down+Right";
        if (downTile.Left == tile)
            return "Down+Down+Left";
        if (rightTile.Up == tile)
            return "Right+Right+Up";
        if (rightTile.Down == tile)
            return "Right+Right+Down";
        if (leftTile.Up == tile)
            return "Left+Left+Up";
        if (leftTile.Down == tile)
            return "Left+Left+Down";

        return "";//not found

    }
}
