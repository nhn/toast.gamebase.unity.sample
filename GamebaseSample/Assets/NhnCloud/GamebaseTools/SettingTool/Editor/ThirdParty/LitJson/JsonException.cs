#region Header
/**
 * JsonException.cs
 *   Base class throwed by LitJSON when a parsing error occurs.
 *
 * The authors disclaim copyright to this source code. For more details, see
 * the COPYING file included with this distribution.
 **/
#endregion

using System;

namespace NhnCloud.GamebaseTools.SettingTool.ThirdParty
{
    public class JsonException : ApplicationException
    {
        public JsonException () : base ()
        {
        }

        internal JsonException (ParserToken token) :
            base (String.Format (
                    "Invalid token '{0}' in input string", token))
        {
        }

        internal JsonException (ParserToken token,
                                Exception inner_exception) :
            base (String.Format (
                    "Invalid token '{0}' in input string", token),
                inner_exception)
        {
            SettingToolLog.Debug(string.Format("JsonException token:{0}, inner_exception:{1}", token, inner_exception), GetType());
        }

        internal JsonException (int c) :
            base (String.Format (
                    "Invalid character '{0}' in input string", (char) c))
        {
        }

        internal JsonException (int c, Exception inner_exception) :
            base (String.Format (
                    "Invalid character '{0}' in input string", (char) c),
                inner_exception)
        {
        }


        public JsonException (string message) : base (message)
        {
            SettingToolLog.Debug(string.Format("JsonException message:{0}", message), GetType());
        }

        public JsonException (string message, Exception inner_exception) :
            base (message, inner_exception)
        {
        }
    }
}
