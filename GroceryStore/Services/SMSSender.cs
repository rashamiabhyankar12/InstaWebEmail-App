using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace GroceryStore.Services
{
    public class SMSSender
    {
        // To access configuration settings from appsettings.json.
        private readonly IConfiguration _configuration;

        // Variables to store Twilio account credentials and the sender's phone number
        private readonly string? AccountSID;
        private readonly string? AuthToken;
        private readonly string? FromNumber;

        public SMSSender(IConfiguration configuration)
        {
            _configuration = configuration;

            // Retrieve Twilio Account SID, Auth Token, and FromNumber from appsettings.json.
            AccountSID = _configuration["SMSSettings:AccountSID"];
            AuthToken = _configuration["SMSSettings:AuthToken"];
            FromNumber = _configuration["SMSSettings:FromNumber"];
        }

        // Asynchronous method to send an SMS message.
        public async Task<bool> SendSmsAsync(string toPhoneNumber, string message)
        {
            try
            {
                // Validate input parameters to ensure 'to' and 'message' are not empty.
                // Return false if invalid.
                if (string.IsNullOrEmpty(toPhoneNumber) || string.IsNullOrEmpty(message))
                {
                    return false;
                }

                // Check if Twilio credentials are missing, and throw an exception if so.
                if (string.IsNullOrEmpty(AccountSID) || string.IsNullOrEmpty(AuthToken))
                {
                    throw new ArgumentException("Twilio Account SID and Auth Token must be provided in the configuration.");
                }

                //Please check + symbol before the Phone Number which is required by Twilio
                if (!toPhoneNumber.StartsWith("+"))
                {
                    toPhoneNumber = "+" + toPhoneNumber;
                }

                // Initialize the Twilio client with the provided AccountSID and AuthToken for authentication.
                TwilioClient.Init(AccountSID, AuthToken);

                // Create a new instance of CreateMessageOptions to configure the SMS details.
                var messageOptions = new CreateMessageOptions(new PhoneNumber(toPhoneNumber)) // Set the recipient phone number.
                {
                    From = new PhoneNumber(FromNumber), // Set the sender phone number (from the configuration).
                    Body = message // Set the body content of the message.
                };

                // Use the Twilio API to send the SMS asynchronously.
                var msg = await MessageResource.CreateAsync(messageOptions);

                // If the SMS is successfully sent, return true.
                return true;
            }
            catch (Exception)
            {
                // Handle any exceptions (e.g., network issues, Twilio errors) and return false.
                return false;
            }
        }
    }
}