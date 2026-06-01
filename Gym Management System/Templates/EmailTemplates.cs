namespace GymManagementSystem.Templates;

public class EmailTemplates
{
    public static string GetConfirmationEmailBody(string firstName, string confirmationLink) => $"""
<!DOCTYPE html>
<html>
<body style="font-family: Arial, sans-serif; background: #f5f5f5; padding: 20px;">
  <div style="max-width: 560px; margin: 0 auto; background: #fff; border-radius: 12px; overflow: hidden;">
    
    <div style="background: #1a1a2e; padding: 2rem; text-align: center;">
      <h1 style="color: #fff; margin: 0;">🏋️ Gym Management</h1>
      <p style="color: rgba(255,255,255,0.6); margin: 8px 0 0;">
        Your fitness journey starts here
      </p>
    </div>

    <div style="padding: 2rem 2.5rem;">
      <p>Hello, <strong>{firstName}</strong> 👋</p>

      <p style="color: #666;">
        Thank you for registering! Please confirm your email address by clicking the button below.
      </p>

      <div style="text-align: center; margin: 32px 0;">
        <a href="{confirmationLink}"
           style="
              background:#1a1a2e;
              color:white;
              text-decoration:none;
              padding:14px 28px;
              border-radius:8px;
              display:inline-block;
              font-weight:bold;">
            Confirm Email
        </a>
      </div>

      <p style="color: #666; font-size: 14px;">
        If the button doesn't work, copy and paste the following link into your browser:
      </p>

      <p style="
          word-break: break-all;
          background:#f5f5f5;
          padding:12px;
          border-radius:8px;
          font-size:13px;">
        {confirmationLink}
      </p>

      <p style="color: #999; font-size: 13px;">
        If you didn't create an account, you can safely ignore this email.
      </p>
    </div>

  </div>
</body>
</html>
""";
}
