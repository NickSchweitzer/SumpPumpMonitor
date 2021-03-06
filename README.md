# Sump Pump Monitor
The Sump Pump Monitor project is a combination of IoT Device and 
Azure Portal that is intended to allow the monitoring of one or many 
household Sump Pumps.

## Solution Components
### [Edison](Edison/README.md)
This is based on the [Intel Edison](https://software.intel.com/en-us/iot/hardware/edison) platform, and uses two different
sensors in order to monitor the Sump Pump:
* [eTape Water Level Sensor](https://www.parallax.com/product/29131)
* [Current Sensor](https://www.sparkfun.com/products/11005) to Monitor On/Off Status and Electrical Usage of the Pump

### WebJobs
This portion of the project runs on Azure, and takes in data generated by the IoT monitor, and pushes the Data to Azure Storage, and creates notifications to send through Notification Hub:
* Azure IoT Hub - Accepts messages from Edison Sump Pump Monitor
* Azure Web Job - Processes Messages
* Azure Table Storage - Inexpensive Storage used for all data collected

### Azure Monitoring Portal
This portion of the project runs on Azure as well, and allows someone to monitor the current status of the pump, change settings on the pump, and look at historical data.
* ASP.NET Core 
* Aurelia Single Page Application

### [Emulator](Emulator/README.md)

WPF Application which behaves like a Sump Pump, and sends data to IoT Hub. It is used for testing the IoT jobs, and creating sample data.

### Shared Libraries
* Data - Contains shared Azure Table Storage respository code for the Portal and WebJobs
* IoT - Contains shared definitions of messages and alerts used by the Emulator and WebJobs

### [Postman Collections](Postman/README.md)

Postman collections are available for testing the IoT Hub functionality as well as the Portal API