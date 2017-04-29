using System;
using System.Collections.Generic;

namespace Assets.Scripts.Utils
{
    public class HtmlUtil
    {
        public static string Link(string content, string eventName)
        {
            return "<a:" + eventName + ">" + content + "</a>";
        }
    }
}
