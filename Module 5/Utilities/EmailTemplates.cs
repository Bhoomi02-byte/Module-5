using Module_5.Collections;

namespace Module_5.Utilities
{
    public class EmailTemplates
    {
        public static string WelcomeEmail(string name, string userRole)
        {
            return $@"
                <h2>Hi {name},</h2>
                <p>Welcome to our blogging platform! 🎉</p>
                <p>Thank you for signing up. We’re excited to have you on board.</p>
                <p><b>Your Role:</b> {userRole}</p>
                <br/>
                <p>Best Regards,</p>
                <p><b>Your Blog Team</b></p>";
        }

        internal static string WelcomeEmail(string name, UserRole userRole)
        {
            return WelcomeEmail(name, userRole.ToString());
        }
    }
}
