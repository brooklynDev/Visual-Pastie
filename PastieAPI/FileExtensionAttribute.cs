using System;

namespace PastieAPI
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class FileExtensionAttribute : Attribute
    {
        public FileExtensionAttribute(string extension)
        {
            Extension = extension;
        }

        public string Extension { get; private set; }
    }
}