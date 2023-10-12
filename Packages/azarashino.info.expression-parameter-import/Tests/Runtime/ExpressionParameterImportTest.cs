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
        [Test]
        public void IsInsufficientTest()
        {
            var dut = new ExpressionParameterImport();
            Assert.That(dut.IsInsufficient, Is.True); // expression parameters not set

            dut.ExpressionParameters = new ScriptableObject();
            Assert.That(dut.IsInsufficient, Is.False);
        }
    }
}
