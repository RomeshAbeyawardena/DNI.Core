using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
namespace DNI.Core.Shared.Extensions
{
    public static class ComponentBaseExtensions
    {
        public static RenderFragment RenderComponent(this ComponentBase componentBase, string componentName, string nameSpace, IDictionary<string, object> parameters)
        {
            void renderFragment(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
            {

                var componentType = Assembly
                     .GetEntryAssembly()
                    .GetType(string.Format("{0}.{1}",
                    nameSpace, componentName));

                if (componentType == null)
                    return;
                var sequence = 0;

                builder.OpenComponent(sequence++, componentType);

                if (parameters != null)
                    foreach (var keyValue in parameters)
                        builder.AddAttribute(sequence++, keyValue.Key, keyValue.Value);

                builder.CloseComponent();
            }
            
            return renderFragment;
        }
    }
}
