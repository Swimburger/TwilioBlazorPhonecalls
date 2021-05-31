let response = await fetch("https://localhost:5003/token");
let jwtToken = await response.text();

let device = new Twilio.Device();
device.setup(jwtToken);

device.on('ready', () => console.log('ready'));
device.on('error', (error) => console.error(error));
device.on('connect', (connection) => 
{
    console.log('connect');
    console.log(connection);
});
device.on('disconnect', () => console.log('disconnect'));
device.on('incoming', (connection) => 
{
    console.log('incoming');
    console.log(connection);
});
device.on('cancel', () => console.log('cancel'));

// start call
device.connect({ "To": "+1234567890" });

// end call
device.activeConnection().disconnect();

// accept incoming call
device.activeConnection().accept();

// reject incoming call
device.activeConnection().reject();
device.destroy();
