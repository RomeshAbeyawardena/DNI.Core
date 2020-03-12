namespace DNI.Core.Domains
{
    public class EncryptionKey
    {
        public string Salt { get; set; }
        public string Password { get; set; }
        public int Iterations { get; set; }
        public string InitialVector { get; set; }
    }
}
