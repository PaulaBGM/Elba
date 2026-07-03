using System;
using UnityEngine;

public class TreeResourceNode : ResourceNode
{
    [Header("Fruit")]
    [SerializeField] private ResourceReward[] fruitRewards;
    [SerializeField] private Transform fruitContainer;
    [SerializeField] private int maxFruit = 5;

    [Header("Branches")]
    [SerializeField] private ResourceReward[] branchRewards;
    [SerializeField] private Transform branchContainer;
    [SerializeField] private int maxBranches = 5;
    [SerializeField] private float branchGrowthTime = 120f;

    private GameObject[] fruitVisuals;
    private GameObject[] branchVisuals;

    private int currentFruit;
    private int currentBranches;

    private float branchTimer;

    protected override void Awake()
    {
        base.Awake();

        CacheVisuals();

        currentFruit = Mathf.Min(maxFruit, fruitVisuals.Length);
        currentBranches = 0;

        SetAllFruitVisible(true);
        SetAllBranchesVisible(false);
    }

    private void Update()
    {
        GrowBranches();
    }

    private void CacheVisuals()
    {
        fruitVisuals = GetChildren(fruitContainer);
        branchVisuals = GetChildren(branchContainer);
    }

    private GameObject[] GetChildren(Transform parent)
    {
        if (parent == null)
            return Array.Empty<GameObject>();

        GameObject[] children = new GameObject[parent.childCount];

        for (int i = 0; i < parent.childCount; i++)
            children[i] = parent.GetChild(i).gameObject;

        return children;
    }

    private void GrowBranches()
    {
        if (currentBranches >= Mathf.Min(maxBranches, branchVisuals.Length))
            return;

        branchTimer += Time.deltaTime;

        if (branchTimer < branchGrowthTime)
            return;

        branchTimer = 0f;

        currentBranches++;

        branchVisuals[currentBranches - 1].SetActive(true);
    }

    public override void ReceiveHit(GameObject attacker, float damage)
    {
        AgentWeapon weapon = attacker.GetComponent<AgentWeapon>();

        if (weapon == null)
        {
            DropOneFruit();
            return;
        }

        DropOneFruit();

        base.ReceiveHit(attacker, damage);
    }

    protected override void OnDestroyed()
    {
        DropStoredBranches();
        DropRemainingFruit();

        SetAllBranchesVisible(false);
    }

    public override void ResetNode()
    {
        base.ResetNode();

        currentFruit = Mathf.Min(maxFruit, fruitVisuals.Length);
        currentBranches = 0;
        branchTimer = 0f;

        SetAllFruitVisible(true);
        SetAllBranchesVisible(false);
    }

    private void DropOneFruit()
    {
        if (currentFruit <= 0)
            return;

        currentFruit--;

        SpawnRewards(fruitRewards);

        fruitVisuals[currentFruit].SetActive(false);
    }

    private void DropRemainingFruit()
    {
        while (currentFruit > 0)
        {
            currentFruit--;

            SpawnRewards(fruitRewards);

            fruitVisuals[currentFruit].SetActive(false);
        }
    }

    private void DropStoredBranches()
    {
        for (int i = 0; i < currentBranches; i++)
            SpawnRewards(branchRewards);
    }

    private void SetAllFruitVisible(bool value)
    {
        foreach (GameObject fruit in fruitVisuals)
        {
            if (fruit != null)
                fruit.SetActive(value);
        }
    }

    private void SetAllBranchesVisible(bool value)
    {
        foreach (GameObject branch in branchVisuals)
        {
            if (branch != null)
                branch.SetActive(value);
        }
    }
}