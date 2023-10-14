using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

using VRC.SDK3.Avatars.ScriptableObjects;
using nadena.dev.modular_avatar.core;
using azarashino.info.expression_parameter_import.Runtime;
using azarashino.info.expression_parameter_import.Editor;
using static azarashino.info.expression_parameter_import.Tests.Editor.ExpressionParameterImportTestDefs;

namespace azarashino.info.expression_parameter_import.Tests.Editor
{
    public class ExpressionParameterImportCoreTest
    {
        [Test]
        public void MakeBackupTest()
        {
            var srcGameObject = new GameObject();
            srcGameObject.AddComponent<ExpressionParameterImport>();
            srcGameObject.GetComponent<ExpressionParameterImport>().SrcExpressionParameters = new VRCExpressionParameters();
            srcGameObject.AddComponent<ModularAvatarParameters>();

            var dstGameObject = srcGameObject.MakeBackup(namePrefix: "MakeBackUpTest_");
            Assert.That(dstGameObject, Is.Not.EqualTo(srcGameObject));
            Assert.That(dstGameObject.name, Is.EqualTo($"MakeBackUpTest_{srcGameObject.name}"));
            Assert.That(dstGameObject.activeSelf, Is.False);
        }

        [Test]
        [TestCase(ImportStrategy.ApplyAll, false, ExpectedResult = true)]
        [TestCase(ImportStrategy.ApplyAll, true, ExpectedResult = true)]
        [TestCase(ImportStrategy.NoOverwrite, false, ExpectedResult = true)]
        [TestCase(ImportStrategy.NoOverwrite, true, ExpectedResult = false)]
        [TestCase(ImportStrategy.OnlyOverwrite, false, ExpectedResult = false)]
        [TestCase(ImportStrategy.OnlyOverwrite, true, ExpectedResult = true)]
        public bool IsNeedImportTest(ImportStrategy importStrategy, bool isDstParamExists)
        {
            return importStrategy.IsNeedImport(isDstParamExists);
        }


        public static IEnumerable<ImportFromTestData> ImportFromTestDatas => ExpressionParameterImportTestDefs.ImportFromTestDatas;

        [Test]
        [TestCaseSource(nameof(ImportFromTestDatas))]
        public void ImportFromTest(ImportFromTestData testData)
        {
            testData.DoImport();
            Assert.That(testData.IsOk, Is.True);
        }

    }
}
