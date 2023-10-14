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
            public GameObject SrcGameObject { get; set; }
            public List<ParameterConfig> ExpectMaParams { get; set; }

            public ModularAvatarParameters SrcMaParam => SrcGameObject?.GetComponent<ModularAvatarParameters>();
            public IEnumerable<ExpressionParameterImport> SrcExParams => SrcGameObject?.GetComponents<ExpressionParameterImport>();

            /// <summary>
            /// テストデータ生成。パラメータだけに専念できるようにした
            /// </summary>
            /// <param name="srcMaParams"></param>
            /// <param name="srcExParamDatas"></param>
            /// <returns></returns>
            public static ImportFromTestData Create(IEnumerable<ParameterConfig> srcMaParams, IEnumerable<VRCExpressionParameters.Parameter> srcExParamDatas, IEnumerable<ParameterConfig> expectMaParams)
            {
                var data = new ImportFromTestData();
                data.SrcGameObject = new GameObject();

                var maParam = data.SrcGameObject.AddComponent<ModularAvatarParameters>();
                maParam.parameters = (srcMaParams ?? Enumerable.Empty<ParameterConfig>()).ToList();

                var srcParam = data.SrcGameObject.AddComponent<ExpressionParameterImport>();
                srcParam.SrcExpressionParameters = new VRCExpressionParameters
                {
                    parameters = (srcExParamDatas ?? Enumerable.Empty<VRCExpressionParameters.Parameter>()).ToArray()
                };

                data.ExpectMaParams = (expectMaParams ?? Enumerable.Empty<ParameterConfig>()).ToList();

                return data;
            }


            public void DoImport()
            {
                foreach (var srcExParam in SrcExParams)
                {
                    SrcMaParam.ImportFrom(srcExParam);
                }
            }

            public bool IsOk => Enumerable.SequenceEqual(SrcMaParam.parameters, ExpectMaParams);
        }
        public static IEnumerable<ImportFromTestData> ImportFromTestDatas
        {
            get
            {
                yield return ImportFromTestData.Create(null, null, null);
                // TODO: 増やす
            }
        }
    }

}