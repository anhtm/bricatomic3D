using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GridTemplateTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void GetNearestPointOnGridTest()
        {
            GridTemplate gt = GameObject.FindObjectOfType<GridTemplate>();

            Vector3 testPosition = new Vector3(1.28f, 2.0f, 6.67f);
            Vector3 result = gt.GetNearestPointOnGrid(testPosition);
            Vector3 expectedPosition = new Vector3(1.0f, 2f, 7.0f);

            Assert.AreEqual(result.x, expectedPosition.x);
            Assert.AreEqual(result.z, expectedPosition.z);
        }

    }
}
