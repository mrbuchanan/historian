using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Api
{
    internal class ChartHelper
    {
        private const string Colour_Unknown = "#efefef";
        private const string Colour_Debug = "#2aabd2";
        private const string Colour_Error = "#265a88";
        private const string Colour_Information = "#e0e0e0";
        private const string Colour_Warning = "#c12e2a";
        private const string Colour_WTF = "#eb9316";

        public static string GetColourByKindAsRGBA(MessageKind kind, decimal opacity = 1)
        {
            // get hex colour
            var hexCode = GetColourByKindAsHex(kind);

            // remove hash
            hexCode = hexCode.Replace("#", "");

            // get hex colour values
            var rHex = hexCode.Substring(0, 2);
            var gHex = hexCode.Substring(2, 2);
            var bHex = hexCode.Substring(4, 2);

            // convert to integer values
            var rInt = Convert.ToInt32(rHex, 16);
            var gInt = Convert.ToInt32(gHex, 16);
            var bInt = Convert.ToInt32(bHex, 16);

            // return as RGBA value
            return $"rgba({rInt},{gInt},{bInt},{opacity})";
        }

        public static string GetColourByKindAsHex(MessageKind kind)
        {
            // return hex code by kind
            switch (kind)
            {
                case MessageKind.Debug: return Colour_Debug;
                case MessageKind.Error: return Colour_Error;
                case MessageKind.Information: return Colour_Information;
                case MessageKind.Warning: return Colour_Warning;
                case MessageKind.WTF: return Colour_WTF;
                default: return Colour_Unknown;
            }
        }
    }
}
