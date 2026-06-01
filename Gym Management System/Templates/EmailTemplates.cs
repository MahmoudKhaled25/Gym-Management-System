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


    public static string GetOtpEmailBody(string firstName, string otpCode) => $"""
<!DOCTYPE html>
<html>
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
</head>
<body style="font-family: Arial, sans-serif; background: #f5f5f5; padding: 20px; margin: 0;">
  <div style="max-width: 560px; margin: 0 auto; background: #fff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 10px rgba(0,0,0,0.05);">
    
    <div style="background: #1a1a2e; padding: 2.5rem 2rem; text-align: center;">
      <h1 style="color: #fff; margin: 0; font-size: 28px; letter-spacing: 1px;">🏋️ Gym Management</h1>
      <p style="color: rgba(255,255,255,0.6); margin: 8px 0 0; font-size: 14px;">
        Your fitness journey starts here
      </p>
    </div>
    
    <div style="padding: 2.5rem 2.5rem;">
      <p style="font-size: 16px; color: #333; margin-top: 0;">Hello, <strong>{firstName}</strong> 👋</p>
      <p style="color: #555; font-size: 15px; line-height: 1.6;">
        We received a request to reset your password for your Gym Management account. Use the verification code below to set a new password:
      </p>
      
      <div style="text-align: center; margin: 35px 0;">
        <div style="
            background: #f8f9fa;
            color: #1a1a2e;
            font-size: 36px;
            font-weight: bold;
            letter-spacing: 8px;
            padding: 18px;
            border-radius: 10px;
            display: inline-block;
            border: 2px dashed #1a1a2e;
            min-width: 200px;
            box-shadow: inset 0 2px 4px rgba(0,0,0,0.02);">
            {otpCode}
        </div>
      </div>
      
      <p style="color: #666; font-size: 14px; text-align: center; line-height: 1.5;">
        This code is valid for <strong>15 minutes</strong>.<br>
        <span style="color: #dc3545; font-weight: bold;">⚠️ Do not share this code with anyone.</span>
      </p>
      
      <hr style="border: 0; border-top: 1px solid #eee; margin: 30px 0;" />
      <p style="color: #999; font-size: 13px; line-height: 1.5; margin-bottom: 0;">
        If you didn't request a password reset, you can safely ignore this email. Your password will remain unchanged and your account stays secure.
      </p>
    </div>
    
  </div>
</body>
</html>
""";
}
