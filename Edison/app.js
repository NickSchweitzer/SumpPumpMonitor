var Edison = require('./EdisonUtilities');
var SumpPump = require('./SumpPump');

const AD_RESOLUTION = 1023;
const SYSTEM_VOLTAGE = 5.0;
const PULL_UP_RESISTOR = 10000.0;

console.log('hi');
require('cylon').robot({
    connections: { edison: { adaptor: 'intel-iot' } },
    devices: {
        led: { driver: 'led', pin: 13 },
        button: { driver: 'button', pin: 2, connection: 'edison' },
        screen: { driver: "jhd1313m1", connection: 'edison' },
        pressure: { driver: 'analog-sensor', pin: 0, connection: 'edison' }
    },

    work: function (my) {
        var that = this;
        var adConvert = new Edison.AtoDConverter(AD_RESOLUTION, SYSTEM_VOLTAGE);
        var pressureVoltage = 0;
        var pump = new SumpPump.SumpPump();

        my.screen.clear();
        my.screen.setCursor(0, 0);
        my.screen.write('XXXXXXXXXXXXXXXX');
        my.screen.setCursor(0, 1);
        my.screen.write('XXXXXXXXXXXXXXXX');

        setTimeout(function () {
            pump.initialize().then(function () {

            every((0.1).second(), function () {
                my.led.toggle();
            });

            my.pressure.on('analogRead', function (val) {
                that.pressureVoltage = adConvert.digitalToVoltage(val);
            });

            every((0.1).second(), function () {
                var color = my.button.isPressed() ? 'Green' : 'Red';
                var localVolts = that.pressureVoltage;
                Edison.updateScreen(my, localVolts.toFixed(4),
                    adConvert.voltageToResistance(localVolts, PULL_UP_RESISTOR).toFixed(4),
                    color);
            });
            });
        }, 30000);
    }
}).start();
