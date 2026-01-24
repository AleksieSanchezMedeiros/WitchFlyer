using UnityEngine;

public class LevelSegment : MonoBehaviour
{
    //Aleksie Sanchez
    // all level segments have a width and a difficulty
    // width in theory segments of multiple width can be used but havent been tested
    // difficulty is currently unused, but level builder could be modified to not have too many difficult segments in a row
    public float width = 20f; // how wide the segment is
    public int difficulty = 1; // optional for later
}