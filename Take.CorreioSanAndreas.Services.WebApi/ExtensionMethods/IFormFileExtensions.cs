using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Take.CorreioSanAndreas.Services.WebApi.ExtensionMethods
{
    public static class IFormFileExtensions
    {
        public static IEnumerable<string> GetTextLines(this IFormFile file)
        {
            return GetText(file).Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string GetText(this IFormFile file)
        {
            if (file.Length > 0)
            {
                byte[] fileBytes = null;

                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
                return Encoding.UTF8.GetString(fileBytes);
            }
            return null;
        }
    }
}
