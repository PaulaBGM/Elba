using UnityEngine;

public class TreeResourceNode : ResourceNode
{
    [Header("Fruit Drops")]
    [SerializeField] private FruitDrop[] fruits;

    [Header("Visual Fruits")]
    [SerializeField] private GameObject[] fruitVisuals;

    public override void ReceiveHit(GameObject attacker)
    {
        DropFruit();

        base.ReceiveHit(attacker);
    }

    private void DropFruit()
    {
        bool fruitDropped = false;

        foreach (FruitDrop fruit in fruits)
        {
            float roll = Random.Range(0f, 100f);

            if (roll > fruit.dropChance)
                continue;

            int amount = Random.Range(fruit.minAmount, fruit.maxAmount + 1);

            for (int i = 0; i < amount; i++)
            {
                Instantiate(
                    fruit.fruit.WorldPrefab,
                    transform.position + (Vector3)Random.insideUnitCircle * 1.5f,
                    Quaternion.identity);

                fruitDropped = true;
            }
        }

        if (fruitDropped)
            HideOneFruitVisual();
    }

    private void HideOneFruitVisual()
    {
        for (int i = 0; i < fruitVisuals.Length; i++)
        {
            if (fruitVisuals[i] != null && fruitVisuals[i].activeSelf)
            {
                fruitVisuals[i].SetActive(false);
                return;
            }
        }
    }

    public override void ResetNode()
    {
        base.ResetNode();

        foreach (GameObject visual in fruitVisuals)
        {
            if (visual != null)
                visual.SetActive(true);
        }
    }
}