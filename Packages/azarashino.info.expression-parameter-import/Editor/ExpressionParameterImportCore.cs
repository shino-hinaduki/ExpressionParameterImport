using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using ExpressionParameters = VRC.SDK3.Avatars.ScriptableObjects.VRCExpressionParameters;
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
            // sweep all entries
            var exParams = new SerializedObject(srcParam.ExpressionParameters).FindProperty("parameters");
            for (var srcIndex = 0; srcIndex < exParams.arraySize; srcIndex++)
            {
                // parse src param
                var srcExParam = exParams.GetArrayElementAtIndex(srcIndex);

                var name = srcExParam.FindPropertyRelative("name").stringValue;
                var valueTypeRaw = srcExParam.FindPropertyRelative("valueType").intValue;
                var valueType = (ExpressionParameters.ValueType)valueTypeRaw;
                var saved = srcExParam.FindPropertyRelative("saved").boolValue;
                var defaultValue = srcExParam.FindPropertyRelative("defaultValue").floatValue;
                var networkSynced = srcExParam.FindPropertyRelative("networkSynced").boolValue;

                // search dst param
                var maDstParamIndex = maParam.parameters.FindIndex(x => x.nameOrPrefix == name);
                var isDstParamExists = maDstParamIndex != -1;

                // decide whether to apply
                if (!srcParam.Storategy.IsNeedImport(isDstParamExists))
                {
                    if (srcParam.IsDebug)
                    {
                        Debug.Log($"[{srcParam.gameObject.name}][Skip  ] name={name}, storategy={srcParam.Storategy}, isDstParamExists={isDstParamExists}");
                    }
                    continue;
                }

                if (srcParam.IsDebug)
                {
                    Debug.Log($"[{srcParam.gameObject.name}][Import] name={name}, storategy={srcParam.Storategy}, isDstParamExists={isDstParamExists}");
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
                dstMaParam.nameOrPrefix = name;
                dstMaParam.remapTo = name;
                dstMaParam.isPrefix = false;
                switch (valueType)
                {
                    case ExpressionParameters.ValueType.Int:
                        dstMaParam.syncType = ParameterSyncType.Int;
                        break;
                    case ExpressionParameters.ValueType.Float:
                        dstMaParam.syncType = ParameterSyncType.Float;
                        break;
                    case ExpressionParameters.ValueType.Bool:
                        dstMaParam.syncType = ParameterSyncType.Bool;
                        break;
                    default:
                        Debug.LogError($"Invalid ValueType {valueType}");
                        return maParam;
                }
                dstMaParam.localOnly = !networkSynced;
                dstMaParam.defaultValue = defaultValue;
                dstMaParam.saved = saved;
                // apply
                maParam.parameters[maDstParamIndex] = dstMaParam; // struct copy

            }
            return maParam;
        }
    }
}