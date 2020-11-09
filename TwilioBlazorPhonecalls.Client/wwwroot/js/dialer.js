window.dialer = {
    device: new Twilio.Device(),
    connection: null,
    dotNetObjectReference: null,
    setDotNetObjectReference: function (dotNetObjectReference) {
        this.dotNetObjectReference = dotNetObjectReference;
    },
    setupTwilioDevice: function (jwtToken) {
        this.device.setup(jwtToken);
    },
    setupTwilioEvents: function () {
        var self = this;
        this.device.on('ready', function (device) {
            self.dotNetObjectReference.invokeMethod('OnTwilioDeviceReady');
        });

        this.device.on('error', function (error) {
            console.error(error);
            self.dotNetObjectReference.invokeMethod('OnTwilioDeviceError', error.message);
        });

        this.device.on('connect', function (connection) {
            self.connection = connection;
            if (connection.direction === "OUTGOING") {
                self.dotNetObjectReference.invokeMethod('OnTwilioDeviceConnected', connection.message['To']);
            } else {
                self.dotNetObjectReference.invokeMethod('OnTwilioDeviceConnected', connection.parameters['From']);
            }
        });

        this.device.on('disconnect', function (connection) {
            self.dotNetObjectReference.invokeMethod('OnTwilioDeviceDisconnected');
        });

        this.device.on('incoming', function (connection) {
            self.connection = connection;
            self.dotNetObjectReference.invokeMethod('OnTwilioDeviceIncomingConnection', connection.parameters['From']);
        });

        this.device.on('cancel', function (connection) {
            self.connection = connection;
            self.dotNetObjectReference.invokeMethod('OnTwilioDeviceCanceled');
        });
    },
    startCall: function (phoneNumber) {
        this.connection = this.device.connect({ "To": phoneNumber });
    },
    endCall: function () {
        if (this.connection) {
            this.connection.disconnect();
        }
    },
    acceptCall: function () {
        if (this.connection) {
            this.connection.accept();
        }
    },
    rejectCall: function () {
        if (this.connection) {
            this.connection.reject();
        }
    },
    destroy: function () {
        this.device.destroy();
    }
};