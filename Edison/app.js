var Edison = require('./EdisonUtilities');

var AD_RESOLUTION = 1023;
var SYSTEM_VOLTAGE = 5.0;
var PULL_UP_RESISTOR = 10000.0;

console.log('hi');
require('cylon').robot({
    connections: { edison: { adaptor: 'intel-iot' } },
    devices: {
        led: { driver: 'led', pin: 13 },
        button: { driver: 'button', pin: 2, connection: 'edison' },
        screen: { driver: "upm-jhd1313m1", connection: 'edison' },
        pressure: { driver: 'analog-sensor', pin: 0, connection: 'edison' }
    },

    work: function (my) {
        var that = this;
        var adConvert = new Edison.AtoDConverter(AD_RESOLUTION, SYSTEM_VOLTAGE);
        var pressureVoltage = 0;

        every((0.1).second(), function () {
            my.led.toggle();
        });

        my.pressure.on('analogRead', function (val) {
            that.pressureVoltage = adConvert.digitalToVoltage(val);
        });

        every((0.1).second(), function () {
            var color = my.button.isPressed() ? 'Green' : 'Red';
            var localVolts = that.pressureVoltage;
            Edison.updateScreen(my, 'Volts: ' + localVolts,
                'Ohms: ' + adConvert.voltageToResistance(localVolts, PULL_UP_RESISTOR),
                color);
        });
    }
}).start();
