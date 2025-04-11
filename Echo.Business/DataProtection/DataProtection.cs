using Microsoft.AspNetCore.DataProtection;

namespace Echo.Business.DataProtection
{
    public class DataProtection : IDataProtection
    {
        private readonly IDataProtector _protector;

        public DataProtection(IDataProtectionProvider provider)
        {
            // We create a protector that will be used to encrypt and decrypt sensitive data.
            // The "EchoApp-security" string makes this specific to our app context.
            _protector = provider.CreateProtector("EchoApp-security");
        }

        public string Protect(string text)
        {
            // This method encrypts the given text so that it becomes unreadable to others.
            // It is commonly used for things like passwords or tokens before saving to database.
            return _protector.Protect(text);
        }

        public string Unprotect(string protectedText)
        {
            // This method decrypts the previously protected text and returns its original version.
            return _protector.Unprotect(protectedText);
        }

        public string UnProtect(string passwordHash)
        {
            // Not used for now – might be implemented in the future if needed.
            throw new NotImplementedException();
        }
    }
}