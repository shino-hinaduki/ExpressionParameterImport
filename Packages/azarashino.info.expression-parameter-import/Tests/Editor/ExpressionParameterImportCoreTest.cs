using System.Collections;
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
        public void GetParameterImportTargetsNull()
        {
            var menuCommand = new MenuCommand(null);
            Assert.That(menuCommand.GetParameterImportTargets(), Is.Null);
        }

        [Test]
        public void GetParameterImportTargetsOnlyExParam1()
        {
            var go = new GameObject();
            go.AddComponent<ExpressionParameterImport>();

            var menuCommand = new MenuCommand(go.GetComponent<ExpressionParameterImport>());
            Assert.That(menuCommand.GetParameterImportTargets(), Is.Null);
        }

        [Test]
        public void GetParameterImportTargetsOnlyExParam2()
        {
            var go = new GameObject();
            go.AddComponent<ExpressionParameterImport>();
            go.GetComponent<ExpressionParameterImport>().ExpressionParameters = ScriptableObject.CreateInstance<ExampleScriptableObject>();

            var menuCommand = new MenuCommand(go.GetComponent<ExpressionParameterImport>());
            Assert.That(menuCommand.GetParameterImportTargets(), Is.Null);
        }


        [Test]
        public void GetParameterImportTargetsReady()
        {
            var go = new GameObject();
            go.AddComponent<ExpressionParameterImport>();
            go.GetComponent<ExpressionParameterImport>().ExpressionParameters = ScriptableObject.CreateInstance<ExampleScriptableObject>();
            go.AddComponent<ModularAvatarParameters>();

            var menuCommand = new MenuCommand(go.GetComponent<ExpressionParameterImport>());
            var dst = menuCommand.GetParameterImportTargets();
            Assert.That(dst, Is.Not.Null);

            var (gameObject, srcParam, maParam) = dst.Value;
            Assert.That(gameObject, Is.EqualTo(go));
            Assert.That(srcParam, Is.EqualTo(go.GetComponent<ExpressionParameterImport>()));
            Assert.That(maParam, Is.EqualTo(go.GetComponent<ModularAvatarParameters>()));
        }

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
