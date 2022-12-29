using System;
using System.Collections;

namespace AudienceSDK.Scripts {

    internal static class WeakReferenceExtension {
        public static object NullOrValue(this WeakReference reference) {
            return reference.IsAlive ? reference.Target : null;
        }
    }
}
