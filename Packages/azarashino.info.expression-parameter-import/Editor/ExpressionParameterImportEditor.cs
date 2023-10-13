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
        [MenuItem("CONTEXT/ExpressionParameterImport/Bake to MA Parameters")]
        public static void Bake(MenuCommand menuCommand)
        {
            var srcParam = menuCommand.context as ExpressionParameterImport;
            if (srcParam?.IsInsufficient ?? true)
            {
                Debug.LogWarning($"invalid target. aborted.");
                return;
            }
            // Get or create MA Parameters
            var maParam = srcParam.gameObject.GetComponent<ModularAvatarParameters>(); // DisallowMultipleComponent
            if (maParam == null)
            {
                maParam = Undo.AddComponent<ModularAvatarParameters>(srcParam.gameObject);
            }
            // main process
            var gameObject = srcParam.gameObject;
            var backupGameObject = gameObject.MakeBackup();
            Undo.RegisterCreatedObjectUndo(backupGameObject, $"Bake ExParams to {gameObject.name}"); // for undo MakeBackup
            Undo.RegisterCompleteObjectUndo(maParam, $"Bake ExParams to {gameObject.name}"); // for undo ImportFrom
            maParam.ImportFrom(srcParam);
            Undo.DestroyObjectImmediate(srcParam); // remove src component & record undo
        }

        [MenuItem("CONTEXT/ExpressionParameterImport/Bake to MA Parameters", validate = true)]
        public static bool BakeValidation(MenuCommand menuCommand)
        {
            var srcParam = menuCommand.context as ExpressionParameterImport;
            return !(srcParam?.IsInsufficient ?? true);
        }
    }
}