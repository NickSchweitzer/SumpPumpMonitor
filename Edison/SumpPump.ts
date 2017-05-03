import { Client } from 'azure-iot-device';
import * as macAddress from 'macaddress';

var config = require('./config.json');
var clientFromConnectionString = require('azure-iot-device-mqtt').clientFromConnectionString;

export class SumpPump {

    constructor() {

        macAddress.one('wlan0', (err, mac: string) => {

            this.deviceId = mac.replace(/:/g, '').toUpperCase();
            console.log('Device ID: ' + this.deviceId);
            var connString = 'HostName=' + config.iotHubHostName + ';DeviceId=' + this.deviceId + ';SharedAccessKey=' + config.iotHubSharedAccessKey;
            console.log('Connection String: ' + connString);
            var iotClient = clientFromConnectionString(connString);

            iotClient.open(err => {
                if (err) {
                    console.error('Could not open IoTHub Client: ' + err);
                } else {
                    iotClient.getTwin((err, twin) => {
                        if (err) {
                            console.error('Could not get Device Twin: ' + err);
                        } else {
                            twin.on('properties.desired', desiredChange => {
                                var currentSettings = twin.properties.reported as any;
                                if (currentSettings.deviceName != desiredChange.deviceName || currentSettings.maxWaterLevel != desiredChange.maxWaterLevel) {
                                    console.log('Receiving New Settings');

                                    this.deviceName = desiredChange.deviceName;
                                    this.maxWaterLevel = desiredChange.maxWaterLevel;

                                    console.log('Device Name: ' + this.deviceName);
                                    console.log('Max Water Level: ' + this.maxWaterLevel);
                                    var patch = {
                                        deviceName: desiredChange.deviceName,
                                        maxWaterLevel: desiredChange.maxWaterLevel
                                    };
                                    twin.properties.reported.update(patch, err => {
                                        if (err) {
                                            console.error('Error reporting updated properties: ' + err);
                                        }
                                    });
                                }
                            });
                        }
                    });
                }
            });
        });
    }

    public deviceId: string;
    public deviceName: string;
    public maxWaterLevel: number;

    public waterLevel: number;
    public pumpOn: boolean;
    public state: PumpState;
}

export enum PumpState {
    Off,
    Running,
    StuckFull,
    StuckEmpty
}