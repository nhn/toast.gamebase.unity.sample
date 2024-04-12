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
using UnityEngine;

namespace GamePlatform.Logger.ThirdParty
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
            Debug.LogWarning(string.Format("{0}[LitJson]JsonException token:{1}, inner_exception:{2}", typeof(GpLogger).Name, token, inner_exception));
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
            Debug.LogWarning(string.Format("{0}[LitJson]JsonException message:{1}", typeof(GpLogger).Name, message));
        }

        public JsonException (string message, Exception inner_exception) :
            base (message, inner_exception)
        {
        }
    }
}
