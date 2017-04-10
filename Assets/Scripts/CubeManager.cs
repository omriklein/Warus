using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CubeManager : MonoBehaviour
{
    public GameObject winnerImage;
    public Text TeamPlayingText;

    private int numberOfCubes = 25; // the number of cubes (in each group)

    private _MakeTorus mt = new _MakeTorus();

    private float speed_xz = (Mathf.Deg2Rad * 360) / 30; //speed on the big rings
    private float speed_2 = (Mathf.Deg2Rad * 360) / 14; //speed on the big rings

    public Material firstGroupColor; // the normal color for the first group
    public Material secondGroupColor; // the normal color for the second group
    public Material activeGroupColor; // the color for the active cube

    private List<GameObject> firstGroup = new List<GameObject>(); // list of the cubes in the first group
    private List<GameObject> secondGroup = new List<GameObject>(); //  list of the cubes in the second group
    private bool isFirstActiveGroup; // if it's the first group's (player's) turn 

    private float scale = 15;

    //30 tiles x
    //14 tiles y
    private int TilesX = 30;
    private int TilesY = 14;

    Tile first; // the first tile (0,0)

    public GameObject cubee; // a prefub of the cube // not in the game
    public GameObject LineWalker; // a prefub of a lineWalker
    public GameObject SlantWalker; // a prefub of a slantWalker
    public GameObject HorseWalker; // a prefub of a HorseWalker
    public GameObject NormalWalker; // a prefub of a NormalWalker
    public GameObject KingWalker; // a prefub of a KingWalker

    private GameObject temporaryObject; // a temporary helper object

    private List<GameObject> cubesOrder;

    // Use this for initialization
    void Start()
    {
        cubesOrder = new List<GameObject>() {HorseWalker, NormalWalker,SlantWalker,NormalWalker,HorseWalker,
                                                                    NormalWalker,LineWalker,HorseWalker,LineWalker,NormalWalker,
                                                                    SlantWalker,KingWalker,KingWalker,KingWalker,SlantWalker,
                                                                    NormalWalker,LineWalker,HorseWalker,LineWalker,LineWalker,
                                                                    HorseWalker,NormalWalker,SlantWalker,NormalWalker,HorseWalker};

        winnerImage.SetActive(false);

        temporaryObject = (GameObject)(cubee); // make the temporary object

        first = Tile.CreateMap(TilesX, TilesY);//this map is connected later to the _Maketorus map 

        temporaryObject = putInFirstGroup(Instantiate<GameObject>(cubee), temporaryObject); // inisiate the temporary object

        temporaryObject.GetComponent<CubeMovement>().Tile = mt.First; // the temporary is in the first tile

        //match this map to the _MakeTorus map
        for (int i = 0; i < 9; i++)
        {
            temporaryObject.GetComponent<CubeMovement>().Tile = temporaryObject.GetComponent<CubeMovement>().Tile.Down;
        }

        // put the objects in there initial places
        for (int i = 0; i < numberOfCubes; i++) // the number of cubes in the game times
        {
            if (i % 5 == 0)
            {
                walkRight(temporaryObject);
                walkUp(temporaryObject);
                walkUp(temporaryObject);
                walkUp(temporaryObject);
                walkUp(temporaryObject);
                walkUp(temporaryObject);
            }

            firstGroup.Add(putInFirstGroup(Instantiate<GameObject>(cubesOrder[i]), temporaryObject)); // put a cube in the first group

            //go to the opposite place on the torus
            for (int j = 0; j < TilesX / 2; j++)
            {
                walkDown(temporaryObject); // make space between the cubes
            }
            for (int k = 0; k < TilesY / 2; k++)
            {
                walkRight(temporaryObject); // make space between the cubes
            }

            secondGroup.Add(putInSecondGroup(Instantiate<GameObject>(cubesOrder[i]), temporaryObject)); // put a cube in the second group

            //go to the opposite place on the torus
            for (int j = 0; j < TilesX / 2; j++)
            {
                walkDown(temporaryObject); // make space between the cubes
            }
            for (int k = 0; k < TilesY / 2; k++)
            {
                walkRight(temporaryObject); // make space between the cubes
            }

            walkDown(temporaryObject);

        }

        // active the first group
        isFirstActiveGroup = true;
        foreach (GameObject cube in firstGroup)
        { cube.GetComponent<CubeMovement>().myTurn = true; }

        //makes sure all the objects are not active
        foreach (GameObject cube in firstGroup)
        { cube.GetComponent<CubeMovement>().Unpress(); }
        foreach (GameObject cube in secondGroup)
        { cube.GetComponent<CubeMovement>().Unpress(); }

        Destroy(temporaryObject); // delete the temporary from the game (there is no need for him anymore)

    }

    // Update is called once per frame
    void Update()
    {
        mousePressedOnScreen();
        changeGroups();
        IsThereALoser();

    }

    private void TurnText()
    {
        if(isFirstActiveGroup)
        {
            TeamPlayingText.GetComponent<Text>().text = "It's the Red team's Turn";
            TeamPlayingText.GetComponent<Text>().color = Color.red; // color - red
        }
        else
        {
            TeamPlayingText.GetComponent<Text>().text = "It's the Blue team's Turn";
            TeamPlayingText.GetComponent<Text>().color = Color.blue; // color - blue
        }
    }

    private void IsThereALoser()
    {
        if (Loser() == "First Team")
        {
            //blue team is winner
            winnerImage.transform.GetChild(0).GetComponent<Text>().text = "The Blue Team Has Won";
            winnerImage.transform.GetChild(0).GetComponent<Text>().color = Color.blue;
            winnerImage.SetActive(true);
        }
        if (Loser() == "Second Team")
        {
            //red team is winner
            winnerImage.transform.GetChild(0).GetComponent<Text>().text = "The Red Team Has Won";
            winnerImage.transform.GetChild(0).GetComponent<Text>().color = Color.red;
            winnerImage.SetActive(true);
        }
    }
    private string Loser()
    {
        int count = 0;
        foreach (GameObject cube in firstGroup)
        {
            if (cube.GetComponent<KingWalker>() != null)
                count++;
        }
        if (count == 0)
            return "First Team";

        count = 0;
        foreach (GameObject cube in secondGroup)
        {
            if (cube.GetComponent<KingWalker>() != null)
                count++;
        }
        if (count == 0)
            return "Second Team";

        return null;
    }

    /// <summary>
    /// changes the active group if neccery
    /// </summary>
    private void changeGroups()
    {
        if (isFirstActiveGroup)
        {
            foreach (GameObject cube in firstGroup)//to all the group
            {
                if (!cube.GetComponent<CubeMovement>().myTurn)//if played
                {
                    changeActiveGroup(firstGroup, secondGroup);//change which group is active
                    break;
                }
            }
        }
        else
        {
            foreach (GameObject cube in secondGroup)//to all the group
            {
                if (!cube.GetComponent<CubeMovement>().myTurn)//if played
                {
                    changeActiveGroup(secondGroup, firstGroup);//change which group is active
                    break;
                }
            }
        }
        TurnText();
    }

    /// <summary>
    /// this function is called when the mouse pressed on the screen
    /// deactivates all the objects
    /// </summary>
    private void mousePressedOnScreen()
    {
        if (Input.GetMouseButtonUp(0)) //if the left mouse click is pressed
        {
            foreach (GameObject cubes in firstGroup) //check all the objects in the first group
            {
                cubes.GetComponent<CubeMovement>().Unpress(); //do the function "unpressed"
            }

            foreach (GameObject cubes in secondGroup) //check all the objects in the second group
            {
                cubes.GetComponent<CubeMovement>().Unpress(); //do the function "unpressed"
            }
        }
    }

    /// <summary>
    /// changes the active group
    /// </summary>
    /// <param name="group1">active -> disable</param>
    /// <param name="group2">disable -> active</param>
    private void changeActiveGroup(List<GameObject> group1, List<GameObject> group2)
    {
        foreach (GameObject cube in group1) // for all the objects in the gruop
        {
            cube.GetComponent<CubeMovement>().myTurn = false; // this is no longer it's turn
        }

        foreach (GameObject cube in group2) // for all the objects in the other group
        {
            cube.GetComponent<CubeMovement>().myTurn = true; // this is it's turn
        }

        isFirstActiveGroup = !isFirstActiveGroup; // the active group is the other group
    }

    /// <summary>
    /// delete an object from his group
    /// </summary>
    /// <param name="obj">the object you want to delete</param>
    public void deleteObject(GameObject obj)
    {
        if (firstGroup.Contains(obj)) //if the object is in the first group
            firstGroup.Remove(obj); // remove object from this group
        else if (secondGroup.Contains(obj)) //if the object is in the second group
            secondGroup.Remove(obj); // remove object from this group
        else // if this object is in nither of the groups
        {
            Debug.Log("could not delete this object"); // there is a problem
            return;
        }

        Destroy(obj); // destroy the object

    }

    /// <summary>
    /// walk the object down
    /// </summary>
    public void walkDown(GameObject obj)
    {
        obj.GetComponent<CubeMovement>().Tile.Cube = null;
        obj.GetComponent<CubeMovement>().Angle_xz -= speed_xz;
        obj.GetComponent<CubeMovement>().Tile = obj.GetComponent<CubeMovement>().Tile.Down;
        obj.GetComponent<CubeMovement>().Tile.Cube = obj;

    }

    /// <summary>
    /// walk the object up
    /// </summary>
    public void walkUp(GameObject obj)
    {
        obj.GetComponent<CubeMovement>().Tile.Cube = null;
        obj.GetComponent<CubeMovement>().Angle_xz += speed_xz;
        obj.GetComponent<CubeMovement>().Tile = obj.GetComponent<CubeMovement>().Tile.Up;
        obj.GetComponent<CubeMovement>().Tile.Cube = obj;

    }

    /// <summary>
    /// walk the object down
    /// </summary>
    public void walkRight(GameObject obj)
    {
        obj.GetComponent<CubeMovement>().Tile.Cube = null;
        obj.GetComponent<CubeMovement>().Angle_2 += speed_2;
        obj.GetComponent<CubeMovement>().Tile = obj.GetComponent<CubeMovement>().Tile.Right;
        obj.GetComponent<CubeMovement>().Tile.Cube = obj;
    }

    /// <summary>
    /// return the object for the first group
    /// </summary>
    /// <returns>the object</returns>
    private GameObject putInFirstGroup(GameObject obj, GameObject temporaryObject)
    {
        if (obj.GetComponent<LineWalker>() != null)
        {
            obj.GetComponent<LineWalker>().mats[1] = firstGroupColor;
            return groupLineWalker(obj, temporaryObject);
        }
        if (obj.GetComponent<SlantWalker>() != null)
        {
            obj.GetComponent<SlantWalker>().mats[1] = firstGroupColor;
            return groupSlantWalker(obj, temporaryObject);
        }
        if (obj.GetComponent<HorseWalker>() != null)
        {
            obj.GetComponent<HorseWalker>().mats[1] = firstGroupColor;
            return groupHorseWalker(obj, temporaryObject);
        }
        if (obj.GetComponent<KingWalker>() != null)
        {
            obj.GetComponent<KingWalker>().mats[1] = firstGroupColor;
            return groupKingWalker(obj, temporaryObject);
        }
        if (obj.GetComponent<NormalWalker>() != null)
        {
            obj.GetComponent<NormalWalker>().mats[1] = firstGroupColor;
            return groupNormalWalker(obj, temporaryObject);
        }

        return null;
    }
    /// <summary>
    /// return the object for the second group
    /// </summary>
    /// <returns>the object</returns>
    private GameObject putInSecondGroup(GameObject obj, GameObject temporaryObject)
    {
        if (obj.GetComponent<LineWalker>() != null)
        {
            obj.GetComponent<LineWalker>().mats[1] = secondGroupColor;
            return groupLineWalker(obj, temporaryObject);
        }
        if (obj.GetComponent<SlantWalker>() != null)
        {
            obj.GetComponent<SlantWalker>().mats[1] = secondGroupColor;
            return groupSlantWalker(obj, temporaryObject);
        }
        if (obj.GetComponent<HorseWalker>() != null)
        {
            obj.GetComponent<HorseWalker>().mats[1] = secondGroupColor;
            return groupHorseWalker(obj, temporaryObject);
        }
        if (obj.GetComponent<KingWalker>() != null)
        {
            obj.GetComponent<KingWalker>().mats[1] = secondGroupColor;
            return groupKingWalker(obj, temporaryObject);
        }
        if (obj.GetComponent<NormalWalker>() != null)
        {
            obj.GetComponent<NormalWalker>().mats[1] = secondGroupColor;
            return groupNormalWalker(obj, temporaryObject);
        }

        return null;
    }

    // continue the functoins above for all the types
    private GameObject groupNormalWalker(GameObject obj, GameObject temporaryObject)
    {
        obj.transform.SetParent(this.transform);
        //
        obj.GetComponent<NormalWalker>().mats[0] = activeGroupColor;
        obj.GetComponent<Renderer>().materials[0] = obj.GetComponent<NormalWalker>().mats[0];
        //
        obj.transform.localScale = new Vector3(obj.transform.localScale.x * scale, obj.transform.localScale.y * scale, obj.transform.localScale.z * scale);
        obj.GetComponent<NormalWalker>().Tile = temporaryObject.GetComponent<CubeMovement>().Tile;
        obj.GetComponent<NormalWalker>().Angle_xz = temporaryObject.GetComponent<CubeMovement>().Angle_xz;
        obj.GetComponent<NormalWalker>().Angle_2 = temporaryObject.GetComponent<CubeMovement>().Angle_2;
        return obj;
    }
    private GameObject groupKingWalker(GameObject obj, GameObject temporaryObject)
    {
        obj.transform.SetParent(this.transform);
        //
        obj.GetComponent<KingWalker>().mats[0] = activeGroupColor;
        obj.GetComponent<Renderer>().materials[0] = obj.GetComponent<KingWalker>().mats[0];
        //
        obj.transform.localScale = new Vector3(obj.transform.localScale.x * scale, obj.transform.localScale.y * scale, obj.transform.localScale.z * scale);
        obj.GetComponent<KingWalker>().Tile = temporaryObject.GetComponent<CubeMovement>().Tile;
        obj.GetComponent<KingWalker>().Angle_xz += temporaryObject.GetComponent<CubeMovement>().Angle_xz;
        obj.GetComponent<KingWalker>().Angle_2 += temporaryObject.GetComponent<CubeMovement>().Angle_2;
        return obj;
    }
    private GameObject groupLineWalker(GameObject obj, GameObject temporaryObject)
    {
        obj.transform.SetParent(this.transform);
        //
        obj.GetComponent<LineWalker>().mats[0] = activeGroupColor;
        obj.GetComponent<Renderer>().materials[0] = obj.GetComponent<CubeMovement>().mats[0];
        //
        obj.transform.localScale = new Vector3(obj.transform.localScale.x * scale, obj.transform.localScale.y * scale, obj.transform.localScale.z * scale);
        obj.GetComponent<LineWalker>().Tile = temporaryObject.GetComponent<CubeMovement>().Tile;
        obj.GetComponent<LineWalker>().Angle_xz = temporaryObject.GetComponent<CubeMovement>().Angle_xz;
        obj.GetComponent<LineWalker>().Angle_2 += temporaryObject.GetComponent<CubeMovement>().Angle_2;
        return obj;
    }
    private GameObject groupSlantWalker(GameObject obj, GameObject temporaryObject)
    {
        obj.transform.SetParent(this.transform);
        //
        obj.GetComponent<SlantWalker>().mats[0] = activeGroupColor;
        obj.GetComponent<Renderer>().materials[0] = obj.GetComponent<SlantWalker>().mats[0];
        //
        obj.transform.localScale = new Vector3(obj.transform.localScale.x * scale, obj.transform.localScale.y * scale, obj.transform.localScale.z * scale);
        obj.GetComponent<SlantWalker>().Tile = temporaryObject.GetComponent<CubeMovement>().Tile;
        obj.GetComponent<SlantWalker>().Angle_xz = temporaryObject.GetComponent<CubeMovement>().Angle_xz;
        obj.GetComponent<SlantWalker>().Angle_2 += temporaryObject.GetComponent<CubeMovement>().Angle_2;
        return obj;
    }
    private GameObject groupHorseWalker(GameObject obj, GameObject temporaryObject)
    {
        obj.transform.SetParent(this.transform);
        //
        obj.GetComponent<HorseWalker>().mats[0] = activeGroupColor;
        obj.GetComponent<Renderer>().materials[0] = obj.GetComponent<HorseWalker>().mats[0];
        //
        obj.transform.localScale = new Vector3(obj.transform.localScale.x * scale, obj.transform.localScale.y * scale, obj.transform.localScale.z * scale);
        obj.GetComponent<HorseWalker>().Tile = temporaryObject.GetComponent<CubeMovement>().Tile;
        obj.GetComponent<HorseWalker>().Angle_xz = temporaryObject.GetComponent<CubeMovement>().Angle_xz;
        obj.GetComponent<HorseWalker>().Angle_2 += temporaryObject.GetComponent<CubeMovement>().Angle_2;
        return obj;
    }
}
