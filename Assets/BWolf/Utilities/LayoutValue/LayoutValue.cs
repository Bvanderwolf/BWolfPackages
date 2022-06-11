using UnityEngine;

public struct LayoutValue
{
    public float spacing;

    public int childCount;

    public float childWidth;

    public LayoutValue(float spacing, int childCount, float childWidth)
    {
        this.spacing = spacing;
        this.childCount = childCount;
        this.childWidth = childWidth;
    }

    public float[] GetValues(float layoutWidth)
    {
        float[] values = new float[childCount];
        float value = spacing;

        for (int i = 0; i < values.Length; i++)
        {
            values[i] = value;
            value += spacing + childWidth;
        }

        return values;
    }
    
    public float[] GetValues(float startValue, float layoutWidth)
    {
        float possibleSpacing = ((layoutWidth - startValue) - (childCount * childWidth)) / (childCount + 1);
        float usedSpacing = Mathf.Max(spacing, possibleSpacing);
        float[] values = new float[childCount];
        float value = spacing;

        for (int i = 0; i < values.Length; i++)
        {
            values[i] = value;
            value += spacing + childWidth;
        }

        return values;
    }
}
