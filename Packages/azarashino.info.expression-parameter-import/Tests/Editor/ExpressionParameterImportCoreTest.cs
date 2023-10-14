using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

using ExpressionParameters = VRC.SDK3.Avatars.ScriptableObjects.VRCExpressionParameters;
using nadena.dev.modular_avatar.core;
using azarashino.info.expression_parameter_import.Runtime;
using azarashino.info.expression_parameter_import.Editor;

namespace azarashino.info.expression_parameter_import.Tests.Editor
{
    public class ExpressionParameterImportCoreTest
    {
        internal class ExampleScriptableObject : ScriptableObject
        {
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

        public class ImportFromTestData
        {
            public ModularAvatarParameters SrcMaParam { get; set; }
            public ExpressionParameterImport SrcExParam { get; set; }

            public ModularAvatarParameters ExpectMaParam { get; set; }

            public bool IsOk(ModularAvatarParameters actMaParam) => Enumerable.SequenceEqual(actMaParam.parameters, ExpectMaParam.parameters);
        }
        public static IEnumerable<ImportFromTestData> ImportFromTestDatas
        {
            get
            {
                yield return new ImportFromTestData()
                {
                    SrcMaParam = new ModularAvatarParameters()
                    {
                        parameters = new List<ParameterConfig>() {
                            new ParameterConfig() {
                            },
                        }
                    },
                    SrcExParam = new ExpressionParameterImport()
                    {
                        Storategy = ImportStrategy.ApplyAll,
                        ExpressionParameters = new ExpressionParameters()
                        {
                            parameters = new ExpressionParameters.Parameter[] {
                            }
                        }
                    },
                    ExpectMaParam = new ModularAvatarParameters()
                    {
                        parameters = new List<ParameterConfig>() {
                            new ParameterConfig() {
                            },
                        }
                    },

                };
            }
        }

        [TestCaseSource(nameof(ImportFromTestData))]
        public void ImportFromTest(ImportFromTestData testData)
        {
            Assert.That(testData.IsOk(testData.SrcMaParam.ImportFrom(testData.SrcExParam)), Is.True);
        }

    }
}
