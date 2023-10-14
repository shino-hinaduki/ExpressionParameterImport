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
using System.Text;

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

        public class ImportFromTestData
        {
            public GameObject SrcGameObject { get; set; }
            public ModularAvatarParameters ExpectMaParam { get; set; }

            public ModularAvatarParameters SrcMaParam => SrcGameObject?.GetComponent<ModularAvatarParameters>();
            public IEnumerable<ExpressionParameterImport> SrcExParams => SrcGameObject?.GetComponents<ExpressionParameterImport>();


            public void DoImport()
            {
                foreach (var srcExParam in SrcExParams)
                {
                    SrcMaParam.ImportFrom(srcExParam);
                }
            }

            public bool IsOk => Enumerable.SequenceEqual(SrcMaParam.parameters, ExpectMaParam.parameters);
        }
        public static IEnumerable<ImportFromTestData> ImportFromTestDatas
        {
            get
            {
                yield break; // TODO
            }
        }

        [Test]
        [TestCaseSource(nameof(ImportFromTestDatas))]
        public void ImportFromTest(ImportFromTestData testData)
        {
            testData.DoImport();
            Assert.That(testData.IsOk, Is.True);
        }

    }
}
