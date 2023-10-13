﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

using nadena.dev.modular_avatar.core;
using azarashino.info.expression_parameter_import.Runtime;
using azarashino.info.expression_parameter_import.Editor;

namespace azarashino.info.expression_parameter_import.Tests.Editor
{
    public class ExpressionParameterImportCoreTest
    {
        internal class ExampleScriptableObject : ScriptableObject { }

        [Test]
        public void MakeBackupTest()
        {
            var srcGameObject = new GameObject();
            srcGameObject.AddComponent<ExpressionParameterImport>();
            srcGameObject.GetComponent<ExpressionParameterImport>().ExpressionParameters = ScriptableObject.CreateInstance<ExampleScriptableObject>();
            srcGameObject.AddComponent<ModularAvatarParameters>();

            var dstGameObject = srcGameObject.MakeBackup(namePrefix: "MakeBackUpTest_");
            Assert.That(dstGameObject, Is.Not.EqualTo(srcGameObject));
            Assert.That(dstGameObject.name, Is.EqualTo($"MakeBackUpTest_{srcGameObject.name}"));
            Assert.That(dstGameObject.activeSelf, Is.False);
        }

    }
}
