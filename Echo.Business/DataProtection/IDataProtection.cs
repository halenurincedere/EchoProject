namespace Echo.Business.DataProtection
{
    public interface IDataProtection
    {
        // Encrypts the given plain text into a secure, unreadable string.
        string Protect(string text);

        // Decrypts the previously encrypted string back to its original plain form.
        string Unprotect(string protectedText);

        // This method seems to be a placeholder or reserved for a different logic.
        // Currently not used – can be removed or implemented later.
        string UnProtect(string passwordHash);
    }
}