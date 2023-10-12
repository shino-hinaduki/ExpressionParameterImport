using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace azarashino.info.expression_parameter_import.Tests.Editor
{
    public class ExpressionParameterImportPluginTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void ExpressionParameterImportPluginTestSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator ExpressionParameterImportPluginTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
