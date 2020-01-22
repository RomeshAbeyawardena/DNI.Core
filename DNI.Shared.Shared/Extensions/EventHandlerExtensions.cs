using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Shared.Extensions
{
    public static class EventHandlerExtensions
    {
        public static void InvokeIfAssigned(this EventHandler eventHandler, object sender, EventArgs eventArgs = default)
        {
            if(eventHandler == null)
                return;

            eventHandler(sender, eventArgs);
        }

        public static void InvokeIfAssigned<TEventArgs>(this EventHandler<TEventArgs> eventHandler, object sender, TEventArgs eventArgs = default)
        {
            if(eventHandler == null)
                return;

            eventHandler(sender, eventArgs);
        }
    }
}
