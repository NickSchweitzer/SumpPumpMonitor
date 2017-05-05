# Postman Collections
These files contain Postman Collections useful for testiong

## Azure IoT Hub.postman_collection.json
Contains API Calls into Azure Hub, especially useful for working with Device Twins. Requires a Postman Environment with the following values in it:

| Key | Value |
| --- | --- |
| hubName | Azure IoT Hub Name (without the https) |
| deviceId | ID of the Device you'd like to make calls on behalf of |
| policyName | iothubowner or equivalent |
| signingKey | Primary Key for the policyName user specified |
| deviceName | The name of the device this environment is for |
| token | Used by the pre-request script for calls |

## Sump Pump Monitor.postman_collection.json
Contains API Calls into the Web Portal API defined in this solution