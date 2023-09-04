namespace PaymentGateway.Application.Common.Utility
{
    public static class MaskingUtility
    {
        /// <summary>
        /// A static function to mask data of a string value
        /// </summary>
        /// <param name="toMask">Value to be masked</param>
        /// <param name="visibleNumbers">How many characters need remain visible</param>
        /// <returns></returns>
        public static string MaskValues(string toMask, int visibleNumbers)
        {
            return string.Concat(
                new string('*', toMask.Length - visibleNumbers),
                toMask.AsSpan(toMask.Length - visibleNumbers)
            );
        }
    }
}
