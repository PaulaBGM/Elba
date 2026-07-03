using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private float maxValue;
    [SerializeField] private float currentValue;

    public float Current => currentValue;
    public float Max => maxValue;
    public float Percent => maxValue <= 0 ? 0 : currentValue / maxValue;

    public Stat(float maxValue)
    {
        this.maxValue = maxValue;
        currentValue = maxValue;
    }

    public void SetCurrent(float value)
    {
        currentValue = Mathf.Clamp(value, 0f, maxValue);
    }

    public void Modify(float amount)
    {
        currentValue = Mathf.Clamp(currentValue + amount, 0f, maxValue);
    }
}