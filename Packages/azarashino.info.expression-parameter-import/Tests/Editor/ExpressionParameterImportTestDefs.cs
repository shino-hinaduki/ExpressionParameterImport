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

namespace azarashino.info.expression_parameter_import.Tests.Editor
{
    public static class ExpressionParameterImportTestDefs
    {
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
    }
}