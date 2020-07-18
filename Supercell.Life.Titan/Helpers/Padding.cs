namespace Supercell.Life.Titan.Helpers
{
    using System;

    public static class Padding
    {
        /// <summary>
        /// Pads the specified message.
        /// </summary>
        public static string Pad(this string message, int limit = 25, string replaceWith = "...")
        {
            if (message.Length > limit)
            {
                message         = message.Substring(0, limit - replaceWith.Length);
                message        += replaceWith;
            }
            else if (message.Length < limit)
            {
                int length      = limit - message.Length;

                int leftPad     = (int) Math.Round((double) length / 2, MidpointRounding.AwayFromZero);
                int rightPad    = (int) Math.Round((double) length / 2, MidpointRounding.AwayFromZero);

                if (length % 2 != 0)
                {
                    rightPad   -= 1;
                }

                for (int i = 0; i < rightPad; i++)
                {
                    message    += " ";
                }

                for (int i = 0; i < leftPad; i++)
                {
                    message = " " + message;
                }
            }

            return message;
        }
    }
}