using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    //Aleksie Sanchez
    //LevelBuilder needs prefabs of segments, any width should work but hasnt been tested
    //As many segments as are designed
    //All segments need to have a LevelSegment script at the root, set the width accordingly
    //need the camera to know what to display and what to hide
    //Settings is the amount of settings it starts with and units/how many are allowed on screen before despawn

    [Header("Segment Prefabs")]
    public LevelSegment[] segmentPrefabs;

    [Header("References")]
    public Transform cameraTransform;

    [Header("Settings")]
    public int startingSegments = 3;
    public float despawnDistance = 40f;
    public int maxSegments = 10;

    private List<LevelSegment> spawnedSegments = new List<LevelSegment>();
    private List<LevelSegment> prefabBag = new List<LevelSegment>(); //for better randomization
    private int bagIndex = 0;
    private float nextSpawnX = 0f;

    //start with preset amount
    void Start()
    {
        for (int i = 0; i < startingSegments; i++)
            SpawnNextSegment();
    }

    //check if far enough to set new one
    void Update()
    {
        // ff camera is approaching the last spawn point, add another
        if (cameraTransform.position.x > nextSpawnX - despawnDistance * 0.5f)
            SpawnNextSegment();

        // clean up old segments
        DespawnOldSegments();
    }

    void SpawnNextSegment()
    {
        // refill and shuffle mixed up bag if empty
        if (prefabBag.Count == 0 || bagIndex >= prefabBag.Count)
            FillAndShuffleBag();

        // take the next prefab from the mixed bag
        LevelSegment prefab = prefabBag[bagIndex];
        bagIndex++;

        Vector3 spawnPos;

        if (spawnedSegments.Count == 0)
        {
            // first segment has to be center at origin
            spawnPos = Vector3.zero;
            nextSpawnX = prefab.width * 0.5f;
        }
        else
        {
            //regular spawning for every segment after
            spawnPos = new Vector3(nextSpawnX + prefab.width * 0.5f, 0f, 0f);
            nextSpawnX += prefab.width;
        }

        //create the new segment and save it for reference
        LevelSegment newSeg = Instantiate(prefab, spawnPos, Quaternion.identity);
        spawnedSegments.Add(newSeg);
    }


    void DespawnOldSegments()
    {
        //need at least one to despawn
        if (spawnedSegments.Count == 0)
            return;

        //despawn the oldest
        LevelSegment oldest = spawnedSegments[0];

        // if camera has passed it by far enough, despawn
        if (cameraTransform.position.x - oldest.transform.position.x > despawnDistance ||
            spawnedSegments.Count > maxSegments)
        {
            Destroy(oldest.gameObject);
            spawnedSegments.RemoveAt(0);
        }
    }

    //used to get variation in every shuffle
    //we go through everything in the list once before we reshuffle it all in
    private void FillAndShuffleBag()
    {
        prefabBag.Clear();
        prefabBag.AddRange(segmentPrefabs);

        //add them all in randomly
        for (int i = 0; i < prefabBag.Count; i++)
        {
            int rand = Random.Range(i, prefabBag.Count);
            LevelSegment temp = prefabBag[i];
            prefabBag[i] = prefabBag[rand];
            prefabBag[rand] = temp;
        }

        //reset starting
        bagIndex = 0;
    }
}