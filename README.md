# API Integration Tests

Integration tests evaluate an app's components on a broader level than unit tests. Unit tests are used to test isolated software components, such as individual class methods. Integration tests confirm that two or more app components work together to produce an expected result, possibly including every component required to fully process a request.

This document describes the rules for writing API Integration tests. First of all the goal for writing API Integration tests is to be sure that out API endpoints are accessible and full data flow is implemented correctly with all data mappings and external calls to third-party services. We should not mock any services and storages in the code. For testing our API we are going to use black box testing approach. API Integration tests should cover all the endpoints of our backend service.

## Structuring API Integration Tests

We have the following structure for the tests:

```
Project
│
└─── ControllerTests
│   
└─── Shared
│   │   ApiWebApplicationFactory.cs
│   │   TestServerStartup.cs
│   │   TestAuthHandler.cs
│   │   FakePolicyEvaluator.cs
│   │   ...
│   │
│   └─── Utilities
│       │   ...
```

The directory Project/ControllerTests contains the integration tests for API controllers using custom `ApiWebApplicationFactory` class. For each new controller we create a separate file with tests for each endpoint.

`ApiWebApplicationFactory` is used to create a `TestServer` for the integration tests. Web host configuration is created independently of the test classes by inheriting from `WebApplicationFactory` to create custom factory.

`TestServerStartup` is an integration-specific Startup class based on API Startup.

The test app can mock an `AuthenticationHandler<TOptions>` in `ConfigureTestServices` in order to test aspects of authentication and authorization. The `TestAuthHandler` is called to authenticate a user when the authentication scheme is set to `Test` where `AddAuthentication` is registered for `ConfigureTestServices`. It's important for the `Test` scheme to match the scheme the app expects. Otherwise, authentication will not work.

`FakePolicyEvaluator` is used for bypassing JWT bearer authentication.

## Naming Convention

The name of the class which contains test methods should be like <ControllerName>Tests.cs. Each test method should be called similar to the method it tests. However we also need to add information about what result is expected and which special conditions are applied to the test.

## Test Execution Details + Utilizing AAA Pattern

Integration tests follow a sequence of events that include the usual Arrange, Act, and Assert test steps:

- The host is configured.
- A test server client is created to submit requests to the application.
- The `Arrange` test step is executed: the test app prepares a request.
- The `Act` test step is executed: the client submits the request and receives the response.
- The `Assert` test step is executed: the actual response is validated as a `pass` or `fail` based on an expected response.
- The process continues until all of the tests are executed.
- The test results are reported.

## Tools

This is a list of tools being used:

- MSTest v2
- Microsoft.NET.Test.Sdk
- Microsoft.AspNetCore.Mvc.Testing
- Microsoft.AspNetCore.WebUtilities