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
                    foreach (var maParam in ctx.AvatarRootObject.GetComponentsInChildren<ModularAvatarParameters>())
                    {
                        // If multiple components are assigned, process them in the order of Inspector display
                        foreach (var srcParam in maParam.GetComponents<ExpressionParameterImport>())
                        {
                            // Expression Parameters not set
                            if (srcParam.IsInsufficient) continue;
                            // apply params
                            maParam.ImportFrom(srcParam);
                        }
                    }
                });
        }
    }
}