// Guids.cs
// MUST match guids.h
using System;

namespace Microsoft.VisualPastie
{
    static class GuidList
    {
        public const string guidVisualPastiePkgString = "f0d08ee9-f357-4fe4-9fb6-bb2d5eae1290";
        public const string guidVisualPastieCmdSetString = "20ff29bb-44bd-4733-8752-3fd6b87829d9";

        public static readonly Guid guidVisualPastieCmdSet = new Guid(guidVisualPastieCmdSetString);
    };
}