﻿namespace DNI.Core.Contracts.Enumerations
{
    //Specifies a set of Encryption methods
    public enum EncryptionMethod
    {
        /// <summary>
        /// One way encryption of data, this form of encryption can not be decrypted.
        /// </summary>
        Hashing = 1,
        /// <summary>
        /// Two way encryption, values encrypted can be decrypted.
        /// </summary>
        Encryption = 2
    }
}
