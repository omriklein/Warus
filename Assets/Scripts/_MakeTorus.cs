using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class _MakeTorus : MonoBehaviour
{
    public List<GameObject> tilesGroup; // List of all the tiles in the game

    public List<Material> TilesMaterials;

    public float scale = 100; // the scale relative to the parent

    private float speed_xz = (Mathf.Deg2Rad * 360) / 30; //2f;//speed on the big rings
    private float speed_2 = (Mathf.Deg2Rad * 360) / 14; //2.5f;//speed on the small rings

    public float speedRotationY = 0.5f; // to rotate the tiles around the y axis in a constant speed
    private float RotationY = 360; // to rotate the tiles around the y axis
    private float RotationXZ = 0; // to rotate the tiles around the x and z axis

    //30 tiles x**
    //14 tiles y**
    private int TilesX = 30;// the number of tiles in the big rings
    private int TilesY = 14;// the number of tiles in the small rings

    private static Tile first; // the first tile (0,0)

    public GameObject tilePre; // prefub of the object

    public GameObject[] torusParts; // the parts that make the torus - 1,2,3,4,5,6,7
    private GameObject temporaryObject; // a temporary helper object

    private static GameObject tilePressed; // saves the tile in the game that is pressed

    /// <summary>
    /// the tile in the game that's pressed
    /// </summary>
    public GameObject TilePressed
    {
        get { return tilePressed; }
        set { tilePressed = value; }
    }

    public Tile First
    {
        get { return first; }
    }

    // Use this for initialization
    void Start()
    {

        Array.Reverse(torusParts); // revarse the torus partes (to correct the order)

        RotationY = 360 - 2 * speedRotationY;

        temporaryObject = (GameObject)(tilePre); // make a temporary helper object

        first = Tile.CreateMap(TilesX, TilesY);//make a tiles map

        temporaryObject = tileInGame(Instantiate<GameObject>(torusParts[0]), temporaryObject, 0);//initiate tile in the game
        temporaryObject.GetComponent<_AcualTilesScripe>().Tile = First; // the first tile in the map
        temporaryObject.GetComponent<_AcualTilesScripe>().Tile.AcualTile = temporaryObject.GetComponent<_AcualTilesScripe>();

        for (int times = 0; times < 2; times++) // for 2 times - the upper half and the bottom half
        {
            for (int i = 0; i < TilesY / 2; i++) // for every row in the half
            {
                for (int j = 0; j < TilesX; j++) // for every tile in the row
                {
                    tilesGroup.Add(tileInGame(Instantiate<GameObject>(torusParts[i]), temporaryObject,(j + i) % 2)); // put a part of the torus

                    walkUp(temporaryObject);
                    RotationY += speedRotationY;
                    if (i == TilesY / 2 - 1 && j == TilesX - 1)
                        walkUp(temporaryObject);
                }
                walkRight(temporaryObject);
                RotationY = 360 - 2 * speedRotationY;
            }

            RotationXZ = 180; // to rotate the bottom half upside down
            Array.Reverse(torusParts);
        }
        Destroy(temporaryObject);

        foreach (GameObject t in tilesGroup)
        {
            t.GetComponent<_AcualTilesScripe>().Unpress();
        }
    }

    // Update is called once per frame
    void Update()
    {
        mousePressedOnScreen();
    }

    private void mousePressedOnScreen()
    {
        if (Input.GetMouseButtonDown(0))//if the left click is pressed
        {
            TilePressed = null;
            foreach (GameObject tile in tilesGroup)//check all the objects
            {
                tile.GetComponent<_AcualTilesScripe>().Unpress();//do the function "Unpressed"
            }
        }
    }

    public void walkUp(GameObject obj)
    {
        obj.GetComponent<_AcualTilesScripe>().Angle_xz -= speed_xz;
        obj.GetComponent<_AcualTilesScripe>().Tile = obj.GetComponent<_AcualTilesScripe>().Tile.Up;
        //obj.GetComponent<_AcualTilesScripe>().Tile.Cube = obj;
    }

    public void walkRight(GameObject obj)
    {
        obj.GetComponent<_AcualTilesScripe>().Angle_2 -= speed_2;
        obj.GetComponent<_AcualTilesScripe>().Tile = obj.GetComponent<_AcualTilesScripe>().Tile.Right;
        //obj.GetComponent<_AcualTilesScripe>().Tile.Cube = obj;
    }

    private GameObject tileInGame(GameObject obj, GameObject temporaryObject, float num)
    {
        obj.GetComponent<_AcualTilesScripe>().mats[1] = TilesMaterials[(int)(1-num)];
        obj.GetComponent<MeshRenderer>().materials[0] = obj.GetComponent<_AcualTilesScripe>().mats[1];

        obj.transform.SetParent(this.transform);
        obj.transform.localScale = new Vector3(obj.transform.localScale.x * scale, obj.transform.localScale.y * scale, obj.transform.localScale.z * scale);
        obj.GetComponent<_AcualTilesScripe>().Tile = temporaryObject.GetComponent<_AcualTilesScripe>().Tile;
        
        //---the tiles rotation to make a torus
        obj.transform.localRotation = Quaternion.Euler(RotationXZ, RotationY + 45, 0);

        return obj;
    }
}
