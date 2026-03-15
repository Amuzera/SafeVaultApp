using NUnit.Framework;
using SafeVault.Security;

namespace SafeVault.Tests
{
    [TestFixture]
    public class TestInputValidation
    {
        [Test]
        public void Username_SQLInjection_Payload_IsRejected()
        {
            var sanitizer = new Sanitizer();
            var validator = new Validator();

            var payload = "' OR 1=1 --";

            var cleaned = sanitizer.SanitizeInput(payload);
            var result = validator.ValidateUsername(cleaned);

            Assert.That(result.IsValid, Is.False);
        }

        [Test]
        public void Username_XSS_Payload_IsRejected()
        {
            var sanitizer = new Sanitizer();
            var validator = new Validator();

            var payload = "<script>alert(1)</script>";

            var cleaned = sanitizer.SanitizeInput(payload);
            var result = validator.ValidateUsername(cleaned);

            Assert.That(result.IsValid, Is.False);
        }

        [Test]
        public void Valid_Username_And_Email_Pass_Validation()
        {
            var sanitizer = new Sanitizer();
            var validator = new Validator();

            var username = "john_Doe123";
            var email = "johnDoe@example.com";

            var cleanUsername = sanitizer.SanitizeInput(username);
            var cleanEmail = sanitizer.SanitizeInput(email);

            var userResult = validator.ValidateUsername(cleanUsername);
            var emailResult = validator.ValidateEmail(cleanEmail);

            Assert.That(userResult.IsValid, Is.True);
            Assert.That(emailResult.IsValid, Is.True);
        }

        [Test]
        public void PasswordHasher_HashAndVerify_Works()
        {
            var hasher = new PasswordHasher();

            var password = "StrongPass123";
            var hash = hasher.HashPassword(password);

            Assert.That(hash, Is.Not.EqualTo(password));
            Assert.That(hasher.VerifyPassword(password, hash), Is.True);
            Assert.That(hasher.VerifyPassword("WrongPass123", hash), Is.False);
        }

        [Test]
        public void WeakPassword_IsRejected()
        {
            var validator = new Validator();

            var result = validator.ValidatePassword("weak");

            Assert.That(result.IsValid, Is.False);
        }

        [Test]
        public void StrongPassword_PassesValidation()
        {
            var validator = new Validator();

            var result = validator.ValidatePassword("StrongPass123");

            Assert.That(result.IsValid, Is.True);
        }
    }
}