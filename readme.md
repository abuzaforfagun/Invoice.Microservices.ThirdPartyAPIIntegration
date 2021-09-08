# Invoice Client - Scaleable Microservice

A web application to create and read invoices from third party services with fault tolerance and in high performance.

## Motivation

* Develop a distributed application with microservices.
* Implement the microservices to provide better performanceand the ability to scale
independently.
* Ensure audit of the user operations.
* Implement the application with SOLID and DRY principles.
* Create reusable components.
* Develop the application following DDD approach.


## Components and their communication
![Components and their communication](https://user-images.githubusercontent.com/24603959/132550255-f6ea5d6d-7e4e-4eb7-9aea-b8651a3133cc.jpg)

#### Client Application

A single page application to display the invoice listand create a new one.
Framework and Libraries: Angular 12, Angular Material, NgRx

#### Gateway

Accessible API to transfer read and write requests to desired microservices.
Framework and Libraries: ASP .NET 5 Web API, Ocelot

#### Invoice Processor

Microservice to store user requests to Outbox, sendthe invoice request to Likvdo and, update
the outbox. Sending an invoice will run inside a transaction. When third party service is unreachable,
insisting on changing the outbox status, it will reschedule the queue.
Framework and Libraries: ASP .NET 5 Web API, AzureService Bus, EF Core, MediatR,
Automapper

#### Scheduler

Schedule the execution of the processing pending request command periodically.
Framework and Libraries: Quartz

#### Database

Store user requests in the outbox and contain userevents for better audit and debugging.

#### Invoice Reader

Microservice to get data from third party service, store and invalidatethem inside Redis Cache for faster delivery.
Framework and Libraries: ASP .NET 5 Web API, RedisCache, Azure Service Bus, MediatR,
Automapper

#### Redis Cache

Store the cache of invoice lists for faster delivery.

#### Service Bus

Used as the way of communication among microservie(s) in an asynchronous way.


### Stack holder and application communication
![Stack holder and application communication](https://user-images.githubusercontent.com/24603959/132550386-64d732ff-f2a2-4ff9-8bc1-16066394019d.jpg)
### Processing pending requests
![Processing pending requests](https://user-images.githubusercontent.com/24603959/132551004-216d98c3-cee1-4590-ac4f-02a3211d317b.jpg)

### Send request to third party service
![Send request to third party service](https://user-images.githubusercontent.com/24603959/132551009-ed01bee4-0985-4018-b125-49e86ca11b34.jpg)

### When third party service is unreachable
![When third party service is unreachable](https://user-images.githubusercontent.com/24603959/132551011-cc39e510-b8a4-4c88-be42-f372596b5ee1.jpg)

### Process failed request
![Process failed request](https://user-images.githubusercontent.com/24603959/132551016-000ed0f2-e205-4193-bacf-ed8c607772b4.jpg)

### Cache Invalidation
![Cache Invalidation](https://user-images.githubusercontent.com/24603959/132551018-3878b2d3-b8ef-433c-8987-1f30d8ec9c95.jpg)

### Behind the scenes of retrieving Invoice
![Behind the scenes of retrieving Invoice](https://user-images.githubusercontent.com/24603959/132551026-5b5455cb-e44d-4a50-8ee2-d8261d2b3538.jpg)

## Tradeoffs of the Architecture

Advantages:
* Ensure invoice creation requests would be processed, maybe not now, but for sure at
later.
* Full audit of the customer operations.
* As we are storing every customer operation event insideour database. So there is no
chance of data loss.
* Caching helps us to deliver faster requests.
* Individual teams can autonomously develop/deploy microservices with minimum/no
communication among other teams.
* Scale read and write microservice separately, dependingon demand.
* Because of Gateway, our client application requiresless modification during the internal
microservice API changes.

Disadvantages:
* No matter whether third party service is available or not, customers need to wait for a period of
time to get their invoices live.
* The logic behind the application gets complex whencompared to a simple CRUD
applications.
* Creating deployment infrastructure and CI/CD wouldtake more time than monolithic
application.
* Because of caching there are some chances to get outdateddata when anything gets
changed from the back office of third party service.


## Installation and running locally

Prerequisite:
* .NET 5 SDK and runtime
* Visual Studio / .NET CLI
* MS SQL Server
* Node
* Angular CLI

Running Microservies:

1. Open Invoice.sln from the root folder.
2. Navigate to the appsetting.json of the InvoiceProcessorproject and insert the following
    data. FYI, in the appsettings.development json file, I have added my test service bus
    connection string, Likvido API key, and Quartzcron expression. You can
    generate your corn expression fromhere.
![Invoice Processor App Settings](https://user-images.githubusercontent.com/24603959/132551029-b42706c7-e5b3-4db7-9568-4e17e3ecf22c.png)

3. Open the appsettings.json of the InvoiceReader project.And provide necessary settings
    as mentioned below. FYI, you can use the test servicebus and Redis connection string
    that is included inside appsettings.development.json.

![Invoice Reader App Settings](https://user-images.githubusercontent.com/24603959/132551038-6b547077-72da-4f0c-a813-ab0b22235784.png)

4. SetGateway, InvoiceReader and InvoiceProcessorasstartup projects.


5. Run the application by pressing F5 or from the toolbar.
6. Make sure Invoice Reader is running on 5002 port byvisiting
    [http://localhost:5002/swagger/,](http://localhost:5002/swagger/,) Invoice Processor running on 5001 port by browsing
    [http://localhost:5001/swagger/](http://localhost:5001/swagger/) And, finally Gateway is running on 44300 by visiting
    https://localhost:44300/swagger/.

## Running Front End

1. Open terminal from the ClientApp folder
2. Execute ```yarn install``` command
3. Run the application by executing ```ng serve``` command. You can find the application at
    [http://localhost:4200](http://localhost:4200)

## Possible Improvements

* Remove hard-coded values from the application(Mostly in the client application).
* Add proper validation in invoice creation.
* Create a circuit breaker, when third party service fails to process requests continuously.
* Better naming and folder structure(mostly in client application).
* Add more unit, integration and E2E test cases.
* Impose better logging.
