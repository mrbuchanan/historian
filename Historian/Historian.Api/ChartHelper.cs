using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Historian.Api
{
    internal class ChartHelper
    {
        public static string GetColourByKind(MessageKind kind, decimal opacity = 1)
        {
            var hexCode = "#efefef";
            switch (kind)
            {
                case MessageKind.Debug:
                    hexCode = "#2aabd2";
                    break;
                case MessageKind.Error:
                    hexCode = "#265a88";
                    break;
                case MessageKind.Information:
                    hexCode = "#e0e0e0";
                    break;
                case MessageKind.WTF:
                    hexCode = "#eb9316";
                    break;
                case MessageKind.Warning:
                    hexCode = "#c12e2a";
                    break;
            }

            hexCode = hexCode.Replace("#", "");

            var rHex = hexCode.Substring(0, 2);
            var gHex = hexCode.Substring(2, 2);
            var bHex = hexCode.Substring(4, 2);

            var rInt = Convert.ToInt32(rHex, 16);
            var gInt = Convert.ToInt32(gHex, 16);
            var bInt = Convert.ToInt32(bHex, 16);

            return string.Format("rgba({0},{1},{2},{3})", rInt, gInt, bInt, opacity);
        }
    }
}
