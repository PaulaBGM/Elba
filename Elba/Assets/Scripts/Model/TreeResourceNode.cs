using UnityEngine;

public class TreeResourceNode : ResourceNode
{
    [Header("Fruit")]
    [SerializeField] private ResourceReward[] fruitRewards;
    [SerializeField] private GameObject[] fruitVisuals;
    [SerializeField] private int maxFruit = 5;

    [Header("Branches")]
    [SerializeField] private ResourceReward[] branchRewards;
    [SerializeField] private int maxBranches = 5;
    [SerializeField] private float branchGrowthTime = 120f;

    private int currentFruit;
    private int currentBranches;

    private float branchTimer;

    protected override void Awake()
    {
        base.Awake();

        currentFruit = maxFruit;
        currentBranches = 0;
    }

    private void Update()
    {
        GrowBranches();
    }

    private void GrowBranches()
    {
        if (currentBranches >= maxBranches)
            return;

        branchTimer += Time.deltaTime;

        if (branchTimer < branchGrowthTime)
            return;

        branchTimer = 0f;
        currentBranches++;
    }

    public override void ReceiveHit(GameObject attacker)
    {
        AgentWeapon weapon = attacker.GetComponent<AgentWeapon>();

        if (weapon == null)
        {
            DropOneFruit();
            return;
        }

        DropOneFruit();

        base.ReceiveHit(attacker);
    }

    protected override void OnDestroyed()
    {
        DropStoredBranches();
        DropRemainingFruit();
    }

    public override void ResetNode()
    {
        base.ResetNode();

        currentFruit = maxFruit;
        currentBranches = 0;
        branchTimer = 0f;

        foreach (GameObject visual in fruitVisuals)
        {
            if (visual != null)
                visual.SetActive(true);
        }
    }

    private void DropOneFruit()
    {
        if (currentFruit <= 0)
            return;

        currentFruit--;

        SpawnRewards(fruitRewards);

        HideOneFruitVisual();
    }

    private void DropRemainingFruit()
    {
        while (currentFruit > 0)
        {
            currentFruit--;

            SpawnRewards(fruitRewards);
        }
    }

    private void DropStoredBranches()
    {
        for (int i = 0; i < currentBranches; i++)
        {
            SpawnRewards(branchRewards);
        }
    }

    private void HideOneFruitVisual()
    {
        for (int i = fruitVisuals.Length - 1; i >= 0; i--)
        {
            if (fruitVisuals[i] == null)
                continue;

            if (!fruitVisuals[i].activeSelf)
                continue;

            fruitVisuals[i].SetActive(false);
            return;
        }
    }
}