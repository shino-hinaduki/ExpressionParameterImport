using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using nadena.dev.modular_avatar.core;
using azarashino.info.expression_parameter_import.Runtime;

namespace azarashino.info.expression_parameter_import.Editor
{

    [CustomEditor(typeof(ExpressionParameterImport))]
    public class ExpressionParameterImportEditor : UnityEditor.Editor
    {
        [MenuItem("CONTEXT/ExpressionParameterImport/Azarashino/Bake to MA Parameters")]
        public static void Bake(MenuCommand menuCommand)
        {
            var target = menuCommand.GetParameterImportTargets();
            if (target == null)
            {
                Debug.LogWarning($"invalid target. aborted.");
                return;
            }

            // main process
            var (gameObject, srcParam, maParam) = target.Value;
            gameObject.MakeBackup();
            maParam.ImportFrom(srcParam);
            GameObject.DestroyImmediate(srcParam); // remove src component
        }

        [MenuItem("CONTEXT/ExpressionParameterImport/Azarashino/Bake to MA Parameters", validate = true)]
        public static bool BakeValidation(MenuCommand menuCommand)
        {
            return menuCommand.GetParameterImportTargets() != null;
        }
    }
}