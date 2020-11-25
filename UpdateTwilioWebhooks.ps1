$ApplicationSid = "AP357100fa6dc6dc0fea75fbd6108e60cb";
$TwilioPhonenumber = "+14324232980";

# Make sure ngrok is running before running this
$NgrokApi = 'http://127.0.0.1:4040/api';
$TwilioTunnelsObject = (Invoke-WebRequest "$NgrokApi/tunnels").Content | ConvertFrom-Json;
$PublicBaseUrl = $TwilioTunnelsObject.tunnels.public_url | Where-Object {$_.StartsWith('https')};

$PublicOutgoingVoiceUrl = "$PublicBaseUrl/voice/outgoing";
twilio api:core:applications:update --sid=$ApplicationSid --voice-url=$PublicOutgoingVoiceUrl --voice-method=POST;

$PublicIncomingVoiceUrl = "$PublicBaseUrl/voice/incoming";
twilio phone-numbers:update $TwilioPhonenumber --voice-url=$PublicIncomingVoiceUrl --voice-method=POST;