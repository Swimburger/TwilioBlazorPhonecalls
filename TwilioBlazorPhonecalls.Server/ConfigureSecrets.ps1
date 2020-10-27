$TwilioAuthToken = Read-Host 'Enter your Twilio auth token (visit https://www.twilio.com/console)' -AsSecureString;
$TwilioAccountSid = Read-Host 'Enter your Twilio account sid (twilio profiles:list)';
$TwiMLApplicationSid = Read-Host 'Enter your twiml applciation sid (twilio api:core:applications:list)';
$TwilioPhoneNumber = Read-Host 'Enter your Twilio phone number (twilio phone-numbers:list)';

$ProjectName = "TwilioBlazorPhonecalls.Server.csproj";
dotnet user-secrets init --project $ProjectName;
dotnet user-secrets set "TwilioAuthToken" (ConvertFrom-SecureString $TwilioAuthToken -AsPlainText) --project $ProjectName;
dotnet user-secrets set "TwilioAccountSid" $TwilioAccountSid --project $ProjectName;
dotnet user-secrets set "TwiMLApplicationSid" $TwiMLApplicationSid --project $ProjectName;
dotnet user-secrets set "TwilioPhoneNumber" $TwilioPhoneNumber --project $ProjectName;