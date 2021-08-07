# Service Agent by SAP

## Background

Builing Multi tenancy agent service where clients can use it to execute service calls. These Service Calls can be executed immediatey or scheduled to be executed later.
This Service Agent is able to work with Json formatted input and it supports HTTP/HTTPS as messaging protocol.


## Requirements
The product requirements for this initial phase are the following:
1. A client should be able to immediately execute any service call and receive either a
successful or unsuccessful response
2. A client should be able to schedule a service call to be executed later.
3. A client should be able to retrieve list of all service calls.
4. A client should be able to retrieve details of specific service call.
5. A client should be able to mark service call as favourite/unfavourite.
6. A client should be able to filter service calls by favourites.

## Deliverables

Built an API service that allows the client to
  • Execute service call for Get, Post, put and Delete.
  • Execute service call immediately or schedule it to be executed later.
        Using API url Post action ```/api/v1/service-agent/schedule-service-agent```
          Input Dto like
```
                {
                  "ApiEndpoint": "https://jsonplaceholder.typicode.com/posts",
                  "Body": {
                      "title": "foo",
                      "body": "bar",
                      "userId": 1
                  },
                  "Name": "create new post",
                  "ApiEndpointAction": "Post",
                  "Headers": [
                      {
                          "Key": "key1",
                          "Value": "Value1"
                      },
                      {
                          "Key": "key2",
                          "Value": "Value2"
                      }
                  ]
               }
```
        • In case of Accepted result will be like :-
          Http status 200 Ok
```
                {
                    "serviceAgentId": "7145d5c0-ccb8-4f3a-81b4-d0558f76d06c",
                    "responseBody": "{ "title": "foo", "body": "body", "userId": 1, "id": 101}",
                    "responseHeaders": [
                        {
                            "key": "Content-Type",
                            "value": "application/json; charset=utf-8"
                        },
                        {
                            "key": "Content-Length",
                            "value": "60"
                        },
                        {
                            "key": "Expires",
                            "value": "-1"
                        }
                    ],
                    "status": "Completed"
                }
```
          serviceAgentId:- is id generating by our system and it's unique for every transaction.

  • Retrieve list of all service calls or details of specefic one.
  • Mark service call as favourite/unfavourite.
  • Filtering service call by favourites.

This API Service is able to work with Json formatted input and it supports HTTP/HTTPS as messaging protocol.


##  Description

A simple web api. It consists of a backend services that allow clients to execute service calls, fetch service call details, mark service call as favourite/unfavourite.

## Solution Description

## Target Frameworks
Target frameworks: .NET core 3.1

## Technology
 - C#
 - .Net core
 - Swagger
 - NUnit
 - Moq
 - Dapper
 - SQL Database
 - Docker conatins SQL Database
 - Docker to build the project

## Architecture
### CQRS
It clears that we need two paths one path to write and other path to read. so CQRS is best soluation.
CQRS stands for Command Query Responsibility Segregation. We use a different model to update information than the model you use to read information.

### EventSourcing
The execution service should be stable when doing a huge number of requests so we are using Event sourcing.
Capture all changes to an application state as a sequence of events.
Event Sourcing ensures that all changes to application state are stored as a sequence of events.
1- we query these events,
2- we can also use the event log to reconstruct past states, and as a foundation to automatically adjust the state to cope with retroactive changes.

### Onion architecture
The Onion Architecture is an Architectural Pattern that enables maintainable and evolutionary enterprise systems.

### Unit tests
 It validates if that code results in the expected state (state testing) or executes the expected sequence of events (behavior testing).
 It covers a lot of code areas.

### Integration tests
individual software modules are combined and tested as a group

### Swagger documentation
  - Swagger generate file for last version of api under this link ```/swagger/v1/swagger.json```
### Database scripts
Path under ```WebApi\DBScript\```
Database scripts to create SQL tables

##  How to run the code
To start the internal service and its dependencies locally, execute:
the file docker-compose.yml will be under api folder direct.
```
    docker-compose up --build
```
    run from visual studio 2019

## ToDo
1. Add more unit tests.
2. Replace Inmemory Service bus by any message bus service like Azure service bus or Amazon.
3. Implement pagination for service calls list.
4. Revisit Types and variables namings, it could have been better.
5. Add simulation for the Execution service.
6. Using graphql at Api controller