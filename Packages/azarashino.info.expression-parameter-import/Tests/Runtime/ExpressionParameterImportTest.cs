using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using azarashino.info.expression_parameter_import.Runtime;

namespace azarashino.info.expression_parameter_import.Tests.Runtime
{
    public class ExpressionParameterImportTest
    {
        internal class ExampleScriptableObject : ScriptableObject { }

        [Test]
        public void IsInsufficientTest()
        {
            // create
            var go = new GameObject();
            go.AddComponent<ExpressionParameterImport>();
            var dut = go.GetComponent<ExpressionParameterImport>();

            // expression parameters not set
            Assert.That(dut.IsInsufficient, Is.True);
            // expression parameters set
            dut.ExpressionParameters = ScriptableObject.CreateInstance<ExampleScriptableObject>();
            Assert.That(dut.IsInsufficient, Is.False);
        }
    }
}
