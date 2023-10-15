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
            public IEnumerable<ParameterConfig> TestExpectMaParams { get; set; }
            #endregion

            #region Test Internal
            internal GameObject _srcGameObject { get; set; }
            internal List<ParameterConfig> _expectMaParams { get; set; }

            internal ModularAvatarParameters _srcMaParam => _srcGameObject?.GetComponent<ModularAvatarParameters>();
            internal IEnumerable<ExpressionParameterImport> _srcExParams => _srcGameObject?.GetComponents<ExpressionParameterImport>();
            #endregion

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
                srcParam.Storategy = TestImportStrategy;
                srcParam.SrcExpressionParameters = ScriptableObject.CreateInstance<VRCExpressionParameters>();
                srcParam.SrcExpressionParameters.parameters = (TestSrcExParamDatas ?? Enumerable.Empty<VRCExpressionParameters.Parameter>()).ToArray();

                this._expectMaParams = (TestExpectMaParams ?? Enumerable.Empty<ParameterConfig>()).ToList();
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

            public bool IsOk => Enumerable.SequenceEqual(_srcMaParam.parameters, _expectMaParams);
        }
        public static IEnumerable<ImportFromTestData> ImportFromTestDatas
        {
            get
            {
                // null data
                yield return new ImportFromTestData()
                {

                    TestSrcMaParams = null,
                    TestSrcExParamDatas = null,
                    TestImportStrategy = ImportStrategy.ApplyAll,
                    TestExpectMaParams = null,
                };
                // TODO: 増やす
            }

        }
    }
}