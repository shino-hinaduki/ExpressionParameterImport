using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using nadena.dev.modular_avatar.core;
using azarashino.info.expression_parameter_import.Runtime;
using UnityEditor;

namespace azarashino.info.expression_parameter_import.Editor
{
    public static class ExpressionParameterImportCore
    {

        /// <summary>
        /// Returns a target in Tuple only when the target selected in Menu is available for processing
        /// </summary>
        /// <param name="menuCommand">target object</param>
        /// <returns></returns>
        public static (GameObject, ExpressionParameterImport, ModularAvatarParameters)? GetParameterImportTargets(this MenuCommand menuCommand)
        {
            // not ExpressionParameterImport, Parameter not set
            var exParam = menuCommand.context as ExpressionParameterImport;
            if (exParam?.IsInsufficient ?? true)
            {
                return null;
            }
            // not found MA Parameters
            var maParam = exParam.GetComponent<ModularAvatarParameters>();
            if (maParam == null)
            {
                return null;
            }
            return (exParam.gameObject, exParam, maParam);
        }

        /// <summary>
        /// Create Backup
        /// </summary>
        /// <param name="src"></param>
        /// <param name="namePrefix"></param>
        /// <returns></returns>
        public static GameObject MakeBackup(this GameObject src, string namePrefix = "Backup_ExpressionParameterImport_")
        {
            var dst = GameObject.Instantiate(src) as GameObject;
            dst.name = $"{namePrefix}{src.name}";
            dst.SetActive(false);
            return dst;
        }

        /// <summary>
        /// Apply ExpressionParameters to MA Parameters
        /// </summary>
        /// <param name="maParam"></param>
        /// <param name="exParam"></param>
        /// <returns></returns>
        public static ModularAvatarParameters ImportFrom(this ModularAvatarParameters maParam, ExpressionParameterImport exParam)
        {
            // TODO
            return maParam;
        }
    }
}