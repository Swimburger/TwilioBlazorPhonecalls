$TwilioAccountSid		= Read-Host 'Enter your Twilio Account SID'
$TwilioPhoneNumber		= Read-Host 'Enter your Twilio phone number'
$TwilioApiKey			= Read-Host 'Enter your Twilio API key'
$TwilioApiSecret		= Read-Host 'Enter your Twilio API secret' -AsSecureString
$TwiMLApplicationSid	= Read-Host 'Enter your TwiML Application SID'

$ProjectName = "TwilioBlazorPhonecalls.Server"
dotnet user-secrets init --project $ProjectName
dotnet user-secrets set "TwilioAccountSid" $TwilioAccountSid --project $ProjectName
dotnet user-secrets set "TwilioPhoneNumber" $TwilioPhoneNumber --project $ProjectName
dotnet user-secrets set "TwilioApiKey" $TwilioApiKey --project $ProjectName

# set secret and mask the output with ***
(dotnet user-secrets set "TwilioApiSecret" (ConvertFrom-SecureString $TwilioApiSecret -AsPlainText) --project $ProjectName)`
	-replace (ConvertFrom-SecureString $TwilioApiSecret -AsPlainText), ((ConvertFrom-SecureString $TwilioApiSecret -AsPlainText) -replace ".", "*")

dotnet user-secrets set "TwiMLApplicationSid" $TwiMLApplicationSid --project $ProjectName