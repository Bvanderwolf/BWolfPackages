using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Test_LayoutValue 
{
    [Test]
    public void Test_GetValues_Spacing()
    {
        LayoutValue value = new LayoutValue(50f, 2, 50f);

        float[] values = value.GetValues(150f);

        Debug.Log(string.Join(",", values));
    }
}