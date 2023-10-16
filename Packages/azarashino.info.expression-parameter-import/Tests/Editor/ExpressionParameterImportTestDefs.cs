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
using System.CodeDom;
using System;

namespace azarashino.info.expression_parameter_import.Tests.Editor
{
    public static class ExpressionParameterImportTestDefs
    {
        public class ImportFromTestData
        {
            #region Test Input
            public List<ParameterConfig> TestSrcMaParams { get; set; }
            public List<VRCExpressionParameters.Parameter> TestSrcExParamDatas { get; set; }
            public ImportStrategy TestImportStrategy { get; set; }
            public List<ParameterConfig> TestExpectMaParams { get; set; }
            #endregion

            #region Test Internal
            internal GameObject _srcGameObject { get; set; }

            internal ModularAvatarParameters _srcMaParam => _srcGameObject?.GetComponent<ModularAvatarParameters>();
            internal IEnumerable<ExpressionParameterImport> _srcExParams => _srcGameObject?.GetComponents<ExpressionParameterImport>();
            #endregion

            public override string ToString()
            {
                var sb = new StringBuilder($"Test({TestImportStrategy}, ma = [");
                TestSrcMaParams?.ForEach(x => sb.Append($"{{{x.nameOrPrefix} {x.syncType} {x.defaultValue}{(x.saved ? " saved" : "")}{(x.localOnly ? "" : " synced")}}}, "));
                sb.Append("], ex = [");
                TestSrcExParamDatas?.ForEach(x => sb.Append($"{{{x.name} {x.valueType} {x.defaultValue}{(x.saved ? " saved" : "")}{(x.networkSynced ? " synced" : "")}}}, "));
                sb.Append($"])");
                return sb.ToString();
            }

            /// <summary>
            /// テスト用のGameObjectやParameter中身を生成する
            /// remark: 事前(CaseSourceとして、もしくはSetUpで) GameObject作ると、TestCaseSourceで取り出したときにはDisposeされていたため、テスト関数に入ってから生成するようにした
            /// </summary>
            /// <param name="srcMaParams"></param>
            /// <param name="srcExParamDatas"></param>
            /// <returns></returns>
            public void Initialize()
            {
                this._srcGameObject = new GameObject();

                var maParam = this._srcGameObject.AddComponent<ModularAvatarParameters>();
                maParam.parameters = (TestSrcMaParams ?? Enumerable.Empty<ParameterConfig>()).ToList();

                var srcParam = this._srcGameObject.AddComponent<ExpressionParameterImport>();
                srcParam.Strategy = TestImportStrategy;
                srcParam.SrcExpressionParameters = ScriptableObject.CreateInstance<VRCExpressionParameters>();
                srcParam.SrcExpressionParameters.parameters = (TestSrcExParamDatas ?? Enumerable.Empty<VRCExpressionParameters.Parameter>()).ToArray();
            }

            public void DoImport()
            {
                if (_srcGameObject == null)
                {
                    this.Initialize();
                }

                foreach (var srcExParam in _srcExParams)
                {
                    _srcMaParam.ImportFrom(srcExParam);
                }
            }

            public bool IsOk => Enumerable.SequenceEqual(_srcMaParam.parameters, TestExpectMaParams ?? Enumerable.Empty<ParameterConfig>());
        }
        public static IEnumerable<ImportFromTestData> ImportFromTestDatas
        {
            get
            {
                #region Test Assets Config

                // 下地用
                var maConfigA1 = new ParameterConfig()
                {
                    nameOrPrefix = "A",
                    remapTo = "A",
                    internalParameter = false,
                    isPrefix = false,
                    syncType = ParameterSyncType.Float,
                    localOnly = false,
                    defaultValue = 0.5f,
                    saved = true,
                };
                var exParamA1 = new VRCExpressionParameters.Parameter()
                {
                    name = "A",
                    valueType = VRCExpressionParameters.ValueType.Float,
                    defaultValue = 0.5f,
                    networkSynced = true,
                    saved = true,
                };

                var maConfigB1 = new ParameterConfig()
                {
                    nameOrPrefix = "B",
                    remapTo = "B",
                    internalParameter = false,
                    isPrefix = false,
                    syncType = ParameterSyncType.Bool,
                    localOnly = true,
                    defaultValue = 1.0f,
                    saved = false,
                };
                var exParamB1 = new VRCExpressionParameters.Parameter()
                {
                    name = "B",
                    valueType = VRCExpressionParameters.ValueType.Bool,
                    defaultValue = 1.0f,
                    networkSynced = false,
                    saved = false,
                };

                var maConfigC1 = new ParameterConfig()
                {
                    nameOrPrefix = "C",
                    remapTo = "C",
                    internalParameter = false,
                    isPrefix = false,
                    syncType = ParameterSyncType.Int,
                    localOnly = false,
                    defaultValue = 0,
                    saved = false,
                };
                var exParamC1 = new VRCExpressionParameters.Parameter()
                {
                    name = "C",
                    valueType = VRCExpressionParameters.ValueType.Float,
                    defaultValue = 0,
                    networkSynced = true,
                    saved = false,
                };

                // 上書き確認用
                var maConfigA2 = new ParameterConfig()
                {
                    nameOrPrefix = "A",
                    remapTo = "A",
                    internalParameter = false,
                    isPrefix = false,
                    syncType = ParameterSyncType.Int,
                    localOnly = true,
                    defaultValue = 1.0f,
                    saved = false,
                };
                var exParamA2 = new VRCExpressionParameters.Parameter()
                {
                    name = "A",
                    valueType = VRCExpressionParameters.ValueType.Int,
                    defaultValue = 1.0f,
                    networkSynced = false,
                    saved = false,
                };
                var maConfigB2 = new ParameterConfig()
                {
                    nameOrPrefix = "B",
                    remapTo = "B",
                    internalParameter = false,
                    isPrefix = false,
                    syncType = ParameterSyncType.Bool,
                    localOnly = false,
                    defaultValue = 0.0f,
                    saved = true,
                };
                var exParamB2 = new VRCExpressionParameters.Parameter()
                {
                    name = "B",
                    valueType = VRCExpressionParameters.ValueType.Bool,
                    defaultValue = 0.0f,
                    networkSynced = true,
                    saved = true,
                };
                var maConfigC2 = new ParameterConfig()
                {
                    nameOrPrefix = "C",
                    remapTo = "C",
                    internalParameter = false,
                    isPrefix = false,
                    syncType = ParameterSyncType.Bool,
                    localOnly = true,
                    defaultValue = 0.0f,
                    saved = true,
                };
                var exParamC2 = new VRCExpressionParameters.Parameter()
                {
                    name = "C",
                    valueType = VRCExpressionParameters.ValueType.Bool,
                    defaultValue = 0.0f,
                    networkSynced = false,
                    saved = true,
                };

                #endregion

                foreach (var storategy in Enum.GetValues(typeof(ImportStrategy)).Cast<ImportStrategy>())
                {
                    // null data
                    yield return new ImportFromTestData()
                    {
                        TestSrcMaParams = null,
                        TestSrcExParamDatas = null,
                        TestImportStrategy = storategy,
                        TestExpectMaParams = null,
                    };
                    // no write
                    yield return new ImportFromTestData()
                    {
                        TestSrcMaParams = new List<ParameterConfig>() { maConfigA1, },
                        TestSrcExParamDatas = new List<VRCExpressionParameters.Parameter>() { },
                        TestImportStrategy = storategy,
                        TestExpectMaParams = new List<ParameterConfig>() { maConfigA1 }
                    };
                }
                // only ex
                yield return new ImportFromTestData()
                {
                    TestSrcMaParams = new List<ParameterConfig>() { },
                    TestSrcExParamDatas = new List<VRCExpressionParameters.Parameter>() { exParamA1 },
                    TestImportStrategy = ImportStrategy.ApplyAll,
                    TestExpectMaParams = new List<ParameterConfig>() { maConfigA1 }
                };
                yield return new ImportFromTestData()
                {
                    TestSrcMaParams = new List<ParameterConfig>() { },
                    TestSrcExParamDatas = new List<VRCExpressionParameters.Parameter>() { exParamA1 },
                    TestImportStrategy = ImportStrategy.NoOverwrite,
                    TestExpectMaParams = new List<ParameterConfig>() { maConfigA1 }
                };
                yield return new ImportFromTestData()
                {
                    TestSrcMaParams = new List<ParameterConfig>() { },
                    TestSrcExParamDatas = new List<VRCExpressionParameters.Parameter>() { exParamA1 },
                    TestImportStrategy = ImportStrategy.OnlyOverwrite,
                    TestExpectMaParams = new List<ParameterConfig>() { }
                };
                // ma->ex
                yield return new ImportFromTestData()
                {
                    TestSrcMaParams = new List<ParameterConfig>() { maConfigA1, maConfigB1, },
                    TestSrcExParamDatas = new List<VRCExpressionParameters.Parameter>() { exParamB2, exParamC2 },
                    TestImportStrategy = ImportStrategy.ApplyAll,
                    TestExpectMaParams = new List<ParameterConfig>() { maConfigA1, maConfigB2, maConfigC2 }
                };
                yield return new ImportFromTestData()
                {
                    TestSrcMaParams = new List<ParameterConfig>() { maConfigA1, maConfigB1, },
                    TestSrcExParamDatas = new List<VRCExpressionParameters.Parameter>() { exParamB2, exParamC2 },
                    TestImportStrategy = ImportStrategy.NoOverwrite,
                    TestExpectMaParams = new List<ParameterConfig>() { maConfigA1, maConfigB1, maConfigC2 }
                };
                yield return new ImportFromTestData()
                {
                    TestSrcMaParams = new List<ParameterConfig>() { maConfigA1, maConfigB1, },
                    TestSrcExParamDatas = new List<VRCExpressionParameters.Parameter>() { exParamB2, exParamC2 },
                    TestImportStrategy = ImportStrategy.OnlyOverwrite,
                    TestExpectMaParams = new List<ParameterConfig>() { maConfigA1, maConfigB2, }
                };
                // ma->ex->ex
                yield return new ImportFromTestData()
                {
                    TestSrcMaParams = new List<ParameterConfig>() { maConfigA1, },
                    TestSrcExParamDatas = new List<VRCExpressionParameters.Parameter>() { exParamA2, exParamA1, exParamA2, exParamB1, exParamB2 },
                    TestImportStrategy = ImportStrategy.ApplyAll,
                    TestExpectMaParams = new List<ParameterConfig>() { maConfigA2, maConfigB2 }
                };
                yield return new ImportFromTestData()
                {
                    TestSrcMaParams = new List<ParameterConfig>() { maConfigA1, },
                    TestSrcExParamDatas = new List<VRCExpressionParameters.Parameter>() { exParamA2, exParamA1, exParamA2, exParamB1, exParamB2 },
                    TestImportStrategy = ImportStrategy.NoOverwrite,
                    TestExpectMaParams = new List<ParameterConfig>() { maConfigA1, maConfigB1, } // A1 -> A2 [skip] , None -> B1, B1 -> B2 [skip] , 以後全部skip
                };
                yield return new ImportFromTestData()
                {
                    TestSrcMaParams = new List<ParameterConfig>() { maConfigA1, },
                    TestSrcExParamDatas = new List<VRCExpressionParameters.Parameter>() { exParamA2, exParamA1, exParamA2, exParamB1, exParamB2 },
                    TestImportStrategy = ImportStrategy.OnlyOverwrite,
                    TestExpectMaParams = new List<ParameterConfig>() { maConfigA2, }
                };

            }

        }
    }
}