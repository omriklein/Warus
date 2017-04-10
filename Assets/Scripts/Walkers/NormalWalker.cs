using UnityEngine;
using System.Collections;

public class NormalWalker : CubeMovement
{
    private _MakeTorus mt = new _MakeTorus();

    protected override void Start()
    {
        base.Start();
        tile = base.Tile;
    }

    protected override void Update()
    {
        if(this.isPressed && this.myTurn)
        {
            if(mt.TilePressed != null)
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
        switch (diraction(tile))//tile - the tile that was pressed, diraction(tile) => string
        {
            case "Up": this.walkUp(); break;
            case "Down": this.walkDown(); break;
            case "Right": this.walkRight();  break;
            case "Left": this.walkLeft(); break;

            default:
                Debug.Log(string.Format("no such diractoin for this player. you are in {0} , {1} and you're trying to go to {2} , {3}"
                    , this.tile.ValueX, this.tile.ValueY, tile.ValueX, tile.ValueY));
                Unpress(); break;
        }

        base.Start();
    }

    private string diraction(Tile tile)
    {
        Tile myTile = this.Tile;

        if (myTile.Up == tile)
            return "Up";
        if (myTile.Down == tile)
            return "Down";
        if (myTile.Right == tile)
            return "Right";
        if (myTile.Left == tile)
            return "Left";

        return "";
    }
}

