using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using VRC.SDK3.Avatars.ScriptableObjects;
using nadena.dev.modular_avatar.core;
using azarashino.info.expression_parameter_import.Runtime;

namespace azarashino.info.expression_parameter_import.Editor
{
    public static class ExpressionParameterImportCore
    {
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
        /// Returns whether or not Import is required
        /// </summary>
        /// <param name="importStrategy"></param>
        /// <param name="isDstParamExists"></param>
        /// <returns></returns>
        public static bool IsNeedImport(this ImportStrategy importStrategy, bool isDstParamExists)
        {
            switch (importStrategy)
            {
                case ImportStrategy.ApplyAll:
                    return true;
                case ImportStrategy.NoOverwrite:
                    return !isDstParamExists;
                case ImportStrategy.OnlyOverwrite:
                    return isDstParamExists;
                default:
                    Debug.LogError($"{importStrategy} is not implemented.");
                    return false;
            }
        }

        /// <summary>
        /// Apply ExpressionParameters to MA Parameters
        /// </summary>
        /// <param name="maParam"></param>
        /// <param name="srcParam"></param>
        /// <returns></returns>
        public static ModularAvatarParameters ImportFrom(this ModularAvatarParameters maParam, ExpressionParameterImport srcParam)
        {
            // invalid src
            if (srcParam?.IsInsufficient ?? true)
            {
                Debug.Log($"[{srcParam.gameObject.name}][skip] IsInsufficient");
                return maParam;
            }

            // sweep all entries
            foreach (var srcExParam in srcParam.SrcExpressionParameters.parameters)
            {
                // search dst param
                var maDstParamIndex = maParam.parameters.FindIndex(x => x.nameOrPrefix == srcExParam.name);
                var isDstParamExists = maDstParamIndex != -1;

                // decide whether to apply
                if (!srcParam.Strategy.IsNeedImport(isDstParamExists))
                {
                    if (srcParam.IsDebug)
                    {
                        Debug.Log($"[{srcParam.gameObject.name}][Skip] name={srcExParam.name}, storategy={srcParam.Strategy}, isDstParamExists={isDstParamExists}");
                    }
                    continue;
                }

                if (srcParam.IsDebug)
                {
                    Debug.Log($"[{srcParam.gameObject.name}][Import] name={srcExParam.name}, storategy={srcParam.Strategy}, isDstParamExists={isDstParamExists}");
                }

                // add new
                if (!isDstParamExists)
                {
                    maParam.parameters.Add(new ParameterConfig());
                    maDstParamIndex = maParam.parameters.Count - 1;
                }
                // copy config (struct copy)
                var dstMaParam = maParam.parameters[maDstParamIndex];
                // configure
                dstMaParam.nameOrPrefix = srcExParam.name;
                dstMaParam.remapTo = srcExParam.name;
                dstMaParam.isPrefix = false;
                switch (srcExParam.valueType)
                {
                    case VRCExpressionParameters.ValueType.Int:
                        dstMaParam.syncType = ParameterSyncType.Int;
                        break;
                    case VRCExpressionParameters.ValueType.Float:
                        dstMaParam.syncType = ParameterSyncType.Float;
                        break;
                    case VRCExpressionParameters.ValueType.Bool:
                        dstMaParam.syncType = ParameterSyncType.Bool;
                        break;
                    default:
                        Debug.LogError($"Invalid ValueType {srcExParam.valueType}");
                        return maParam;
                }
                dstMaParam.localOnly = !srcExParam.networkSynced;
                dstMaParam.defaultValue = srcExParam.defaultValue;
                dstMaParam.saved = srcExParam.saved;
                dstMaParam.internalParameter = false;
                // apply
                maParam.parameters[maDstParamIndex] = dstMaParam; // struct copy

            }
            return maParam;
        }
    }
}