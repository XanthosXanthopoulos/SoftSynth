using System.Security;

namespace PDADesktop
{
    /// <summary>
    /// An interface for a class that can provide a secure password
    /// </summary>
    public interface IHavePassword
    {
        /// <summary>
        /// The secure password
        /// </summary>
        SecureString SecurePassword { get; }

        /// <summary>
        /// The confirmation of the secure password
        /// </summary>
        SecureString ConfirmSecurePassword { get; }
    }
}