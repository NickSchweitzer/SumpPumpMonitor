# Edison

*Currently this is a sample application which is not fully functional as a Sump Pump Monitor yet.*

## Configuration

**The Device ID is the MAC Address of the Wireless Network Adapter on the Edison**  

Build and Deployment configuration is stored in config.json (not checked in for security reasons)
```json
{
    "host": "",
    "user": "root",
    "password": "",
    "projectName": "SumpPumpEdison",
    "startFile": "app.js",
    "sshPort": 22,
    "iotHubHostName": "",
    "iotHubSharedAccessKey": ""
}
```
**host:** This is the IP Address of the Edison for Deployment  
**user:** This is the username for the Edison that is capable of ssh access  
**password:** The password for user  
**projectFile:** Used as the directory under user to deploy to  
**startFile:** The root application file for node to run  
**sshPort:** 22 unless you have a custom port you use  
**iotHubHostName:** Host name of the Azure IoT Hub without http(s)  
**iotHubSharedAccessKey:** Primary key for the Device ID you create in Device Explorer

## Settings

The Edison application uses Device Twins to retreive configuration settings from the IoT Hub. These are locally stored to settings.json
```json
{
    "deviceName": "Home Sump Pump",
    "maxWaterLevel": 10
}
```
**deviceName:** Device name that will be shown in the Portal  
**maxWaterLevel:** Height (in inches) when the Monitor should send an alert