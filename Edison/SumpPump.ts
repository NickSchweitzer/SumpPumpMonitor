import { Client } from 'azure-iot-device';
import * as macAddress from 'macaddress';

var Promise = require('bluebird');
var config = require('./config.json');
var clientFromConnectionString = require('azure-iot-device-mqtt').clientFromConnectionString;
var macAddress = require('macaddress');
var jsonFile = require('jsonfile');

export class SumpPump {

    private deviceId: string;
    private connectionString: string;
    private iotClient: Client;

    public deviceName: string;
    public maxWaterLevel: number;

    public waterLevel: number;
    public pumpOn: boolean;
    public state: PumpState;

    public initialize() {
        var that = this;
        return that.setDeviceId()
        .then(function (deviceId) {
            return that.readDeviceSettings()
        })
        .then(function (settings) {
            that.initIotClient()
        });
    }

    private setDeviceId() {
        var that = this;

        return new Promise(function (resolve, reject) {
            macAddress.one('wlan0', (err, mac) => {
                that.deviceId = mac.replace(/:/g, '').toUpperCase();
                console.log('Device ID: ' + that.deviceId);
                resolve(that.deviceId);
            })
        });
    }

    private readDeviceSettings() {
        var that = this;

        return new Promise(function (resolve, reject) {
            jsonFile.readFile('./settings.json', function (error, obj) {
                if (error == null) {
                    that.deviceName = obj.deviceName;
                    that.maxWaterLevel = obj.maxWaterLevel;
                }
                resolve(obj);
            })
        });
    }

    private initIotClient()
    {
        var that = this;
        that.connectionString = 'HostName=' + config.iotHubHostName + ';DeviceId=' + that.deviceId + ';SharedAccessKey=' + config.iotHubSharedAccessKey;
        console.log('Connection String: ' + that.connectionString);
        that.iotClient = clientFromConnectionString(that.connectionString);

        that.iotClient.open(err => {
            if (err) {
                console.error('Could not open IoTHub Client: ' + err);
            } else {
                that.iotClient.getTwin((err, twin) => {
                    if (err) {
                        console.error('Could not get Device Twin: ' + err);
                    } else {
                        twin.on('properties.desired', desiredChange => {
                            //var currentSettings = twin.properties.reported as any;
                            if (that.deviceName != desiredChange.deviceName || that.maxWaterLevel != desiredChange.maxWaterLevel) {
                                console.log('Receiving New Settings');

                                that.deviceName = desiredChange.deviceName;
                                that.maxWaterLevel = desiredChange.maxWaterLevel;

                                console.log('Device Name: ' + that.deviceName);
                                console.log('Max Water Level: ' + that.maxWaterLevel);
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

                            // Save settings to file
                            jsonFile.writeFile('./settings.json', {
                                deviceName: that.deviceName,
                                maxWaterLevel: that.maxWaterLevel
                            });
                        });
                    }
                });
            }
        });

    }
}

export enum PumpState {
    Off,
    Running,
    StuckFull,
    StuckEmpty
}