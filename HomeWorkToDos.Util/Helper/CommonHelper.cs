using System;

namespace HomeWorkToDos.Util.Helper
{
    /// <summary>
    /// CommonHelper
    /// </summary>
    public static class CommonHelper
    {
        /// <summary>
        /// Encodes password to base64
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>
        /// encodeed password.
        /// </returns>
        public static string EncodePasswordToBase64(string password)
        {
            byte[] encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
            string encodedData = Convert.ToBase64String(encData_byte);
            return encodedData;
        }
    }
}
