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
                    foreach (var maParamObj in ctx.AvatarRootObject.GetComponentsInChildren<ModularAvatarParameters>())
                    {
                        // If multiple components are assigned, process them in the order of Inspector display
                        foreach (var exParam in maParamObj.GetComponents<ExpressionParameterImport>())
                        {
                            // Expression Parameters not set
                            if (exParam.IsInsufficient)
                            {
                                if (exParam.IsDebug)
                                {
                                    Debug.Log($"Expression Parameters not set. ma:{maParamObj}, ex:{exParam}");
                                }
                                continue;
                            }
                            // TODO: Implement main process
                            // refs: https://gist.github.com/shino-hinaduki/d9e2da5632d271691c06e141d3dec482
                        }
                    }
                });
        }
    }
}