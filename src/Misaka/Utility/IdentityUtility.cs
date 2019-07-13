using System;

namespace Misaka.Utility
{
    public static class IdentityUtility
    {
        public static string NewGuidString() => Guid.NewGuid()
                                                    .ToString()
                                                    .ToUpper()
                                                    .Replace("-", string.Empty);
    }
}
