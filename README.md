# Collectively.Services.Storage

|Branch             |Build status                                                  
|-------------------|-----------------------------------------------------
|master             |[![master branch build status](https://api.travis-ci.org/noordwind/Collectively.Services.Storage.svg?branch=master)](https://travis-ci.org/noordwind/Collectively.Services.Storage)
|develop            |[![develop branch build status](https://api.travis-ci.org/noordwind/Collectively.Services.Storage.svg?branch=develop)](https://travis-ci.org/noordwind/Collectively.Services.Storage/branches)

**Let's go for the better, Collectively​​.**
----------------

**Collectively** is an open platform to enhance communication between counties and its residents​. It's made as a fully open source & cross-platform solution by [Noordwind](https://noordwind.com).

Find out more at [becollective.ly](http://becollective.ly)

**Collectively.Services.Storage**
----------------

The **Collectively.Services.Storage** is a service responsible for storing the already "flattened" objects in its internal database that can be accessed by the [Collectively.Api](https://github.com/noordwind/Collectively.Api).
It's a read-only data store that subscribes to the specific events and provides a transparent fallback (e.g. calls the cache or another service to fetch the data) in case the requested data was not available directly. 

**Quick start**
----------------

## Docker way

Collectively is built as a set of microservices, therefore the easiest way is to run the whole system using the *docker-compose*.

Clone the [Collectively.Docker](https://github.com/noordwind/Collectively.Docker) repository and run the *start.sh* script:

```
git clone https://github.com/noordwind/Collectively.Docker
./start.sh
```

For the list of available services and their endpoints [click here](https://github.com/noordwind/Collectively).

## Classic way

In order to run the **Collectively.Services.Storage** you need to have installed:
- [.NET Core](https://dotnet.github.io)
- [MongoDB](https://www.mongodb.com)
- [RabbitMQ](https://www.rabbitmq.com)

Clone the repository and start the application via *dotnet run* command:

```
git clone https://github.com/noordwind/Collectively.Services.Storage
cd Collectively.Services.Storage/Collectively.Services.Storage
dotnet restore --source https://api.nuget.org/v3/index.json --source https://www.myget.org/F/collectively/api/v3/index.json --no-cache
dotnet run --urls "http://*:10000"
```

Once executed, you shall be able to access the service at [http://localhost:10000](http://localhost:10000)

Please note that the following solution will only run the Storage Service which is merely one of the many parts required to run properly the whole Collectively system.

**Configuration**
----------------

Please edit the *appsettings.json* file in order to use the custom application settings. To configure the docker environment update the *dockerfile* - if you would like to change the exposed port, you need to also update it's value that can be found within *Program.cs*.
For the local testing purposes the *.local* or *.docker* configuration files are being used (for both *appsettings* and *dockerfile*), so feel free to create or edit them.

**Tech stack**
----------------
- **[.NET Core](https://dotnet.github.io)** - an open source & cross-platform framework for building applications using C# language.
- **[Nancy](http://nancyfx.org)** - an open source framework for building HTTP API.
- **[MongoDB](https://github.com/mongodb/mongo-csharp-driver)** - an open source library for integration with [MongoDB](https://www.mongodb.com) database.
- **[RawRabbit](https://github.com/pardahlman/RawRabbit)** - an open source library for integration with [RabbitMQ](https://www.rabbitmq.com) service bus.

**Solution structure**
----------------
- **Collectively.Services.Storage** - core and executable project via *dotnet run* command.
- **Collectively.Services.Storage.Tests** - unit & integration tests executable via *dotnet test* command.
- **Collectively.Services.Storage.Tests.EndToEnd** - End-to-End tests executable via *dotnet test* command.