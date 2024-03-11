# About This Project

IoT addresses social issues such as climate change, but current solutions suffer from interoperability challenges due to their reliance on private protocols and cloud services. This project aims to develop middleware for the standardized exchange of IoT data, promoting social, environmental and economic benefits.
This project aims to develop a service-oriented middleware that standardizes data access, promoting interoperability and open data through the adoption of web services and a resource structure based on the web and open standards.
The project was developed using the tools:
- **Visual Studio Community** - to build the basis of the project
- **Postman** - to test the middleware
- **Mosquitto broker** - facilitates the exchange of messages between devices and applications in an IoT architecture

Based URL used in this project - (Port modified in visual studio)
- \http://localhost:50202/api/somiod

Inside the root of this project there is a file with the URLs to test with postman called **endpoints.txt**.

In Visual Studio caution with startup projects
- In Solution > Properties > Common Properties > Startup Project
- Check if SOMIOD is in last place **(wait for the browser to load while running before messing with apps)**
- Check App1 and App2 above ONLY if you want to test communications between the applications
- Or check AppEvaluation above SOMIOD if you want test endpoints of main controller

**Caution**, use this project at the root of the disk
# Softwares Requeriments

The software listed below is required to run this project:

- **Visual Studio Community**
	- .Net Desktop Development
	- Data Storage and Processing: select Microsoft SQL Data Tools support
	- ASP.NET Web Development _+_ in Individual components / Code Tools check the Class Designer option.
		- In the installation details check the "Windows Communication Foundation"

- [Mosquitto broker](https://mosquitto.org/)
	- After installation it is necessary to change the configuration file (mosquitto.conf) inside the mosquitto folder, inside "Program Files" (C:\\Program Files\\mosquitto).

```text
example of changes in the configuration file:

# listener port-number [ip address/host name/unix socket path]
listener 1883

# Defaults to false, unless there are no listeners defined in the configuration
# file, in which case it is set to true, but connections are only allowed from
# the local machine.
allow_anonymous true
```

- **Postman** (optional) - For test URLs