using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class LevelBuilder : MonoBehaviour
{
    //Aleksie Sanchez
    //LevelBuilder needs prefabs of segments, any width should work but hasnt been tested
    //As many segments as are designed
    //All segments need to have a LevelSegment script at the root, set the width accordingly
    //need the camera to know what to display and what to hide
    //Settings is the amount of settings it starts with and units/how many are allowed on screen before despawn

    [Header("Segments")]
    [SerializeField] private List<LevelSegment> allSegmentPrefabs = new List<LevelSegment>();
    [SerializeField] private List<LevelSegment> bossSegementPrefabs = new List<LevelSegment>();
    [SerializeField] private LevelID startingLevel;

    private enum SpawnMode { Normal, Boss }
    private SpawnMode spawnMode = SpawnMode.Normal;
    public bool bossFightActive = false;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Settings")]
    [SerializeField] private int startingSegments = 3;
    [SerializeField] private float despawnDistance = 40f;
    [SerializeField] private int maxSegments = 10;
    [SerializeField] private List<int> segementsPerLevel = new List<int>();
    [SerializeField] private int spawnedSegementCount = 0;
    private bool stopSpawning = false;

    private List<LevelSegment> spawnedSegments = new List<LevelSegment>();
    private List<LevelSegment> currentLevelSegments = new List<LevelSegment>();
    private int bagIndex = 0;
    private float nextSpawnX = 0f;

    [SerializeField] private LevelID currentLevel;
    private readonly Dictionary<LevelID, List<LevelSegment>> levelPools = new();

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    //start with preset amount
    void Start()
    {
        stopSpawning = false;
        BuildLevelPools();
        SetLevel(startingLevel, true);

        for (int i = 0; i < startingSegments; i++) SpawnNextSegment();
    }

    void Update()
    {
        if (cameraTransform.position.x > nextSpawnX - despawnDistance * 0.5f) SpawnNextSegment();
        DespawnOldSegments();
    }

    private void BuildLevelPools()
    {
        levelPools.Clear();
        foreach (LevelID id in Enum.GetValues(typeof(LevelID))) levelPools[id] = new List<LevelSegment>();
        foreach (LevelSegment segment in allSegmentPrefabs) levelPools[segment.levelID].Add(segment);
    }

    public void SetLevel(LevelID newLevel, bool immediate = false)
    {
        if (currentLevel == newLevel && !immediate) return;
        currentLevel = newLevel;

        // force the bag to rebuild from the new level segment pool on the next spawn
        currentLevelSegments.Clear();
        bagIndex = 0;
        spawnedSegementCount = 0;

        bossFightActive = false;
        spawnMode = SpawnMode.Normal;

        LevelTransitionDestruction();
    }

    void SpawnNextSegment()
    {
        if (stopSpawning) return;

        // Only spawn boss fight segements if you're in boss fight mode
        if (spawnMode == SpawnMode.Boss) {
            SpawnBossSegment();
            return;
        }

        // Begin boss fight at the end of the level instead of going to the next level
        int limit = GetSegmentsPerLevel(currentLevel);
        if (!bossFightActive && limit > 0 && spawnedSegementCount >= limit) {
            BeginBossFight();
            return;
        }

        if (currentLevelSegments.Count == 0 || bagIndex >= currentLevelSegments.Count)
            FillAndShuffleBag();

        LevelSegment prefab = currentLevelSegments[bagIndex];
        bagIndex++;

        Vector3 spawnPos;

        if (spawnedSegments.Count == 0) {
            spawnPos = Vector3.zero;
            nextSpawnX = prefab.width * 0.5f;
        } else {
            spawnPos = new Vector3(nextSpawnX + prefab.width * 0.5f, 0f, 0f);
            nextSpawnX += prefab.width;
        }

        LevelSegment newSeg = Instantiate(prefab, spawnPos, Quaternion.identity);
        spawnedSegments.Add(newSeg);

        spawnedSegementCount++;
    }

    private bool AdvanceToNextLevel()
    {
        int next = (int)currentLevel + 1;

        if (next >= Enum.GetValues(typeof(LevelID)).Length) {
            OnAllLevelsComplete();
            return false;
        }

        SetLevel((LevelID)next);
        return true;
    }

    private void OnAllLevelsComplete()
    {
        stopSpawning = true;
        GameManager.Instance.GameComplete();
    }

    private int GetSegmentsPerLevel(LevelID level)
    {
        int i = (int)level;
        if (segementsPerLevel == null || i < 0 || i >= segementsPerLevel.Count)
            return 0;
        return segementsPerLevel[i];
    }

    private void BeginBossFight()
    {
        if (bossFightActive) return;

        bossFightActive = true;
        spawnMode = SpawnMode.Boss;

        LevelTransitionDestruction();

        currentLevelSegments.Clear();
        bagIndex = 0;

        SpawnBossSegment();
    }

    public void OnBossDefeated()
    {
        if (!bossFightActive) return;

        bossFightActive = false;
        spawnMode = SpawnMode.Normal;

        if (!AdvanceToNextLevel())
            return;
    }

    private void SpawnBossSegment()
    {
        LevelSegment bossPrefab = GetBossSegmentForLevel(currentLevel);
        if (bossPrefab == null) {
            Debug.LogWarning($"No boss segment prefab assigned for {currentLevel}. Ending run.");
            OnAllLevelsComplete();
            return;
        }

        Vector3 spawnPos;

        if (spawnedSegments.Count == 0) {
            spawnPos = Vector3.zero;
            nextSpawnX = bossPrefab.width * 0.5f;
        } else {
            spawnPos = new Vector3(nextSpawnX + bossPrefab.width * 0.5f, 0f, 0f);
            nextSpawnX += bossPrefab.width;
        }

        LevelSegment newSeg = Instantiate(bossPrefab, spawnPos, Quaternion.identity);
        spawnedSegments.Add(newSeg);
    }

    private LevelSegment GetBossSegmentForLevel(LevelID level)
    {
        int i = (int)level;
        if (bossSegementPrefabs == null || i < 0 || i >= bossSegementPrefabs.Count)
            return null;
        return bossSegementPrefabs[i];
    }

    //used to get variation in every shuffle
    //we go through everything in the list once before we reshuffle it all in
    private void FillAndShuffleBag()
    {
        currentLevelSegments.Clear();

        List<LevelSegment> poolForCurrentLevel = levelPools[currentLevel];
        if (poolForCurrentLevel.Count <= 0) {
            Debug.LogWarning("NO SEGMENTS ADDED FOR THIS LEVEL POOL");
            currentLevelSegments.AddRange(allSegmentPrefabs);
        } else {
            currentLevelSegments.AddRange(poolForCurrentLevel);
        }


        for (int i = 0; i < currentLevelSegments.Count; i++) {
            int randomIndex = UnityEngine.Random.Range(i, currentLevelSegments.Count);
            (currentLevelSegments[i], currentLevelSegments[randomIndex]) = (currentLevelSegments[randomIndex], currentLevelSegments[i]);
        }

        bagIndex = 0;
    }

    void DespawnOldSegments()
    {
        if (spawnedSegments.Count == 0) return;

        LevelSegment oldest = spawnedSegments[0];

        if (cameraTransform.position.x - oldest.transform.position.x > despawnDistance ||
            spawnedSegments.Count > maxSegments)
        {
            Destroy(oldest.gameObject);
            spawnedSegments.RemoveAt(0);
        }
    }

    private void LevelTransitionDestruction()
    {
        while (spawnedSegments.Count > 2) {
            Destroy(spawnedSegments[0].gameObject);
            spawnedSegments.RemoveAt(0);
        }
    }
}

public enum LevelID
{
    Level1,
    Level2,
    Level3,
    Level4
}