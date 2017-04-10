using UnityEngine;
using System.Collections;

public class LineWalker : CubeMovement
{
    private _MakeTorus mt = new _MakeTorus();

    override protected void Start()
    {
        base.Start();//place the object in place
        tile = base.Tile;
        //function to rotate the object will be in the <CubeMovement>
    }

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

    //walks the path to the destination
    private void walkPath(Tile tile)
    {
        string[] dir = diraction(tile).Split(' ');
        if(dir.Length != 2)
        {
            Debug.Log(string.Format("no such diractoin for this player. you are in {0} , {1} and you're trying to go to {2} , {3}"
                    , this.tile.ValueX, this.tile.ValueY, tile.ValueX, tile.ValueY));
            Unpress();
        }
        else
        {
            switch(dir[0])
            {
                case "up":
                    for (int i = 0; i < int.Parse(dir[1]); i++)
                    {
                        this.walkUp();
                        base.Start();
                    }break;
                case "down":
                    for (int i = 0; i < int.Parse(dir[1]); i++)
                    {
                        this.walkDown();
                        base.Start();
                    }
                    break;
                case "right":
                    for (int i = 0; i < int.Parse(dir[1]); i++)
                    {
                        this.walkRight();
                        base.Start();
                    }
                    break;
                case "left":
                    for (int i = 0; i < int.Parse(dir[1]); i++)
                    {
                        this.walkLeft();
                        base.Start();
                    }
                    break;
            }
        }
        /*switch (diraction(tile))//tile - the tile that was pressed, diraction(tile) => string
        {
            case "up":
                do
                {
                    this.walkUp();
                    base.Start();
                } while (this.tile != (tile)); break;
            case "down":
                do
                {
                    this.walkDown();
                    base.Start();
                } while (this.Tile != (tile)); break;
            case "right":
                do
                {
                    this.walkRight();
                    base.Start();
                } while (this.Tile != (tile)); break;
            case "left":
                do
                {
                    this.walkLeft();
                    base.Start();
                } while (this.Tile != (tile)); break;

            default:
                Debug.Log(string.Format("no such diractoin for this player. you are in {0} , {1} and you're trying to go to {2} , {3}"
                    , this.tile.ValueX, this.tile.ValueY, tile.ValueX, tile.ValueY));
                Unpress();  break;
        }*/
    }

    /// <summary>
    /// the diraction the object should move to
    /// </summary>
    /// <param name="tile">destination</param>
    /// <returns>path (in string)</returns>
    private string diraction(Tile tile)
    {
        Tile myTile = this.Tile;

        int count = 0;//left / down
        //int count2 = 0;//right / up
        //bool rootUpDown = false;

        if (myTile == tile)
            return "";

        do
        {
            myTile = myTile.Up;
            count++;
            if (myTile == tile && (myTile.Cube == null || !myTile.Cube.GetComponent<CubeMovement>().myTurn)) //if there is any cube in my team
                return "up " + count;
            if (myTile.Cube != null)
                break;
        } while (myTile != this.tile);

        count = 0;
        do
        {
            myTile = myTile.Down;
            count++;
            if (myTile == tile && (myTile.Cube == null || !myTile.Cube.GetComponent<CubeMovement>().myTurn)) //if there is any cube in my team
                return "down " + count;
            if (myTile.Cube != null)
                break;
        } while (myTile != this.tile);

        count = 0;
        do
        {
            myTile = myTile.Right;
            count++;
            if (myTile == tile && (myTile.Cube == null || !myTile.Cube.GetComponent<CubeMovement>().myTurn)) //if there is any cube in my team
                return "right " + count;
            if (myTile.Cube != null)
                break;
        } while (myTile != this.tile);

        count = 0;
        do
        {
            myTile = myTile.Left;
            count++;
            if (myTile == tile && (myTile.Cube == null || !myTile.Cube.GetComponent<CubeMovement>().myTurn)) //if there is any cube in my team
                return "left " + count;
            if (myTile.Cube != null)
                break;
        } while (myTile != this.tile);

        return "";

        /*do
        {
            myTile = myTile.Up;
            count1++;
            if (myTile == tile)
            {
                rootUpDown = true;
                count2 = count1;
                count1 = 0;
            }
        } while (myTile != this.Tile);

        if (!rootUpDown)
        {
            count1 = 0;
            count2 = 0;
            do
            {
                count1++;
                myTile = myTile.Right;
                if (myTile == (tile))
                {
                    count2 = count1;
                    count1 = 0;
                    break;
                }
            } while (myTile != this.Tile);
        }

        if (count2 == 0)
            return "";

        if (rootUpDown)
        {
            if (count2 < count1)
                return "up";
            else
                return "down";
        }
        else
        {
            if (count2 > count1)
                return "right";
            else
                return "left";
        }*/
    }
}
