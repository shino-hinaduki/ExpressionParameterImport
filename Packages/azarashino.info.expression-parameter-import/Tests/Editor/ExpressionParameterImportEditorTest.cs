using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using VRC.SDK3.Avatars.ScriptableObjects;
using nadena.dev.modular_avatar.core;
using azarashino.info.expression_parameter_import.Runtime;
using azarashino.info.expression_parameter_import.Editor;
using static azarashino.info.expression_parameter_import.Tests.Editor.ExpressionParameterImportTestDefs;

namespace azarashino.info.expression_parameter_import.Tests.Editor
{
    public class ExpressionParameterImportEditorTest
    {
        [Test]
        public void BakeValidationNullTest()
        {
            Assert.That(ExpressionParameterImportEditor.BakeValidation(new UnityEditor.MenuCommand(null)), Is.False);
        }

        [Test]
        public void BakeValidationIsInsufficientTest()
        {
            Assert.That(ExpressionParameterImportEditor.BakeValidation(new UnityEditor.MenuCommand(new ExpressionParameterImport())), Is.False);
        }

        [Test]
        public void BakeValidationOkTest()
        {
            Assert.That(ExpressionParameterImportEditor.BakeValidation(new UnityEditor.MenuCommand(new ExpressionParameterImport()
            {
                SrcExpressionParameters = ScriptableObject.CreateInstance<VRCExpressionParameters>(),
            })), Is.True);
        }
    }
}
