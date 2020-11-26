window.createDialer = function () {
    return {
        device: new Twilio.Device(),
        dotNetObjectReference: null,
        setDotNetObjectReference: function (dotNetObjectReference) {
            this.dotNetObjectReference = dotNetObjectReference;
        },
        setupTwilioDevice: function (jwtToken) {
            var options = {
                closeProtection: true // will warn user if you try to close browser window during an active call
            };
            this.device.setup(jwtToken, options);
        },
        setupTwilioEvents: function () {
            // Inside of Twilio events scope the 'this' context is redefined and will not point to 'window.dialer'
            // The variable 'self' is introduced to conveniently access the 'this' context from the outer scope inside of the events scope
            var self = this;
            this.device.on('ready', function () {
                self.dotNetObjectReference.invokeMethod('OnTwilioDeviceReady');
            });

            this.device.on('error', function (error) {
                console.error(error);
                self.dotNetObjectReference.invokeMethod('OnTwilioDeviceError', error.message);
            });

            this.device.on('connect', function (connection) {
                if (connection.direction === "OUTGOING") {
                    self.dotNetObjectReference.invokeMethod('OnTwilioDeviceConnected', connection.message['To']);
                } else {
                    self.dotNetObjectReference.invokeMethod('OnTwilioDeviceConnected', connection.parameters['From']);
                }
            });

            this.device.on('disconnect', function () {
                self.dotNetObjectReference.invokeMethod('OnTwilioDeviceDisconnected');
            });

            this.device.on('incoming', function (connection) {
                self.dotNetObjectReference.invokeMethod('OnTwilioDeviceIncomingConnection', connection.parameters['From']);
            });

            this.device.on('cancel', function () {
                self.dotNetObjectReference.invokeMethod('OnTwilioDeviceCanceled');
            });
        },
        startCall: function (phoneNumber) {
            // To parameter is a defined property by Twilio, but you could just as well use any other property name
            // and it will be passed to your TwiML webhook as meta data
            this.device.connect({ "To": phoneNumber });
        },
        endCall: function () {
            var connection = this.device.activeConnection();
            if (connection) {
                connection.disconnect();
            }
        },
        acceptCall: function () {
            var connection = this.device.activeConnection();
            if (connection) {
                connection.accept();
            }
        },
        rejectCall: function () {
            var connection = this.device.activeConnection();
            if (connection) {
                connection.reject();
            }
        },
        destroy: function () {
            this.device.destroy();
        }
    }
};