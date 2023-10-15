using UnityEngine;
using nadena.dev.ndmf;
using nadena.dev.modular_avatar.core;
using azarashino.info.expression_parameter_import.Editor;
using azarashino.info.expression_parameter_import.Runtime;

[assembly: ExportsPlugin(typeof(ExpressionParameterImportPlugin))]

namespace azarashino.info.expression_parameter_import.Editor
{
    public class ExpressionParameterImportPlugin : Plugin<ExpressionParameterImportPlugin>
    {
        public override string QualifiedName => "azarashino.info.expression-parameter-import";

        public override string DisplayName => "Expression Parameter Import";

        protected override void Configure()
        {
            InPhase(BuildPhase.Resolving)
                .BeforePlugin("nadena.dev.modular-avatar")
                .Run("Import ExpressionParameters", ctx =>
                {
                    foreach (var srcParam in ctx.AvatarRootObject.GetComponentsInChildren<ExpressionParameterImport>())
                    {
                        // Expression Parameters not set
                        if (srcParam.IsInsufficient) continue;
                        // Get or create MA Parameters
                        var maParam = srcParam.gameObject.GetComponent<ModularAvatarParameters>(); // DisallowMultipleComponent
                        if (maParam == null)
                        {
                            maParam = srcParam.gameObject.AddComponent<ModularAvatarParameters>();
                        }
                        // apply params
                        maParam.ImportFrom(srcParam);
                        // Removed for possible appearance as noise of unknown identification from other tools
                        Object.Destroy(srcParam);
                    }
                });
        }
    }
}