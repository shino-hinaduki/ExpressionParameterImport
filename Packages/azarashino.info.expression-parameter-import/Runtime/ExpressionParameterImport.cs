﻿using System;
using UnityEngine;
using VRC.SDKBase;

namespace azarashino.info.expression_parameter_import
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
    public class ExpressionParameterImport : MonoBehaviour, IEditorOnly
    {
        [Header("Configuration")]
        [Tooltip("Source Expression Parameters")]
        [SerializeField]
        public ScriptableObject ExpressionParameters;

        [SerializeField]
        [Tooltip("Strategies for importing parameters\n- ApplyAll: Overwrite all with the specified ExpressionParameters\n- NoOverwrite: Skip if the Parameter already exists\n- OnlyOverwrite: Skip if the Parameter does not exist and overwrite only if the Parameter does exist\n")]
        public ImportStrategy Storategy = ImportStrategy.ApplyAll;

        [Header("for Developer")]
        [SerializeField]
        public bool IsDebug = false;

        public override string ToString() => $"{nameof(ExpressionParameterImport)}({ExpressionParameters}, {Storategy})";

    }
}