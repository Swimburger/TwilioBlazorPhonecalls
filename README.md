# Twilio Blazor Phonecalls

**Read the following Twilio Tutorial to learn how to create this application: [Making Phone Calls from Blazor WebAssembly with Twilio Voice](https://www.twilio.com/blog/making-phone-calls-from-blazor-webassembly-with-twilio-voice).**    

This application demonstrates how to create a Dialer Blazor WASM component to make phone calls from the browser using Twilio's Programmable Voice service.
The application has been split up into two projects:
- Client: Responsible for rendering the UI and initiating/receiving the voice calls using Twilio's Client JS SDK
- Server: Responsible for generating auth tokens and handling Twilio webhooks to instruct what to do with outgoing/incoming voice calls.

Ideally the server would be split into two project, one for generating auth tokens and one for handling webhooks.
This would allow you to host your auth server on a private network while stil exposing the webhooks publicly.
To keep the demo simple, these two responsiblities have been rolled into one server project.

This project has been built using .NET Core 3.1, but an additional branch is available with [.NET 5 which uses the newly introduced capabilities](https://github.com/Swimburger/TwilioBlazorPhonecalls/tree/dotnet-5).

Here's a preview GIF of what the project looks like:
![Animated screenshot of the app showing the dialer in use!](./imgs/browser-call.gif "Animated screenshot of the app showing the dialer in use")
