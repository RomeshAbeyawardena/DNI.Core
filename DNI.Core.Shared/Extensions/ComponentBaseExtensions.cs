namespace DNI.Core.Shared.Extensions
{
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.AspNetCore.Components;

    public static class ComponentBaseExtensions
    {
        public static RenderFragment RenderComponent(this ComponentBase componentBase, string componentName, string nameSpace, IDictionary<string, object> parameters)
        {
            void RenderFragment(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
            {
                var componentType = Assembly
                     .GetEntryAssembly()
                        .GetType(string.Format(
                            "{0}.{1}",
                            nameSpace,
                            componentName));

                if (componentType == null)
                {
                    return;
                }

                var sequence = 0;

                builder.OpenComponent(sequence++, componentType);

                if (parameters != null)
                {
                    foreach (var keyValue in parameters)
                    {
                        builder.AddAttribute(sequence++, keyValue.Key, keyValue.Value);
                    }
                }

                builder.CloseComponent();
            }

            return RenderFragment;
        }
    }
}
