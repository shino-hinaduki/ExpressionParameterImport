using System;
using UnityEngine;
using VRC.SDKBase;
using VRC.SDK3.Avatars.ScriptableObjects;

namespace azarashino.info.expression_parameter_import.Runtime
{
    [Serializable]
    public enum ImportStrategy
    {
        ApplyAll,
        NoOverwrite,
        OnlyOverwrite,
    }

    /// <summary>
    /// Transfer Expression Parameter to MA Parameter
    /// </summary>
    [AddComponentMenu("Azarashino/Expression Parameter Import")]
    public class ExpressionParameterImport : MonoBehaviour, IEditorOnly
    {
        [Header("Configuration")]
        [Tooltip("Source Expression Parameters")]
        [SerializeField]
        public VRCExpressionParameters SrcExpressionParameters;

        [SerializeField]
        [Tooltip("Strategies for importing parameters\n- ApplyAll: Overwrite all with the specified ExpressionParameters\n- NoOverwrite: Skip if the Parameter already exists\n- OnlyOverwrite: Skip if the Parameter does not exist and overwrite only if the Parameter does exist\n")]
        public ImportStrategy Storategy = ImportStrategy.ApplyAll;

        [Header("for Developer")]
        [SerializeField]
        public bool IsDebug = false;

        /// <summary>
        /// true if already set
        /// </summary>
        public bool IsInsufficient => SrcExpressionParameters == null;
        public override string ToString() => $"{nameof(ExpressionParameterImport)}({SrcExpressionParameters}, {Storategy})";

    }
}