<img src="https://github.com/SalZaki/Luna/blob/main/assets/luna-logo.png" width="25%" height="25%">

# Table of contents

- [What is Luna](#luna)
- [Overview](#overview)
- [Architecture](#architecture)
  - [Design Approach](#design-approach)
  - [Solution Structure](#solution-structure)
- [Microservices](#microservices)
  - [Payment Api](#payment-api)
    - [Features](#features)
    - [Api Settings](#api-settings)
    - [Api Key](#api-key)
    - [Example Post Request Header](#example-request-header)
    - [Example Post Request Body](#example-request-body)
    - [Example Post Response Header](#example-response-header)
    - [Example Post Response Body](#example-response-body)
    - [Example Faulted Post Response](#faulted-post-response)
  - [3rd Party Bank Api](#3rd-party-bank-api)
- [Getting Started](#getting-started)
  - [How to start solution](#start-solution)
- [Test](#test)
- [Further Development and Architecture](#further)
- [Development Tools](#development-tools)
- [Fun Qoutes](#fun-qoutes)

<a name="luna"/>

# What is Luna

Luna is a fictitious financial services and software as a service company, with offices around the globe. The company primarily offers payment processing software and application programming interfaces for e-commerce websites and mobile applications.

> **Luna** is an Italian and Spanish given name of Latin origin. It means Moon in English.

Luna is aiming higher in 2022, and embarking on a ambitious project to modernise their payment platform, by designing it be as API first platform and delivering value with their API to enterprise developer partners.

<a name="overview"/>

# Overview

As the new payment platform is designed to be **API First** and RESTful, Luna's engineering team has decided to implement a microservices-based architecture which uses most common cloud native technologies (cloud-agnostic approach, containerization mechanism, container orchestration and so on) for this platform.

<a name="design"/>

## Design Approach
At Luna, the engineering team tried to tackle this challenge by adapting following design approach and mindset,

### Don’t surprise users
We are being mindful of the decisions we are making and making sure to communicate our intent clearly and consistently. Arbitrary decisions made in a rush frequently come back to bite, when those decisions lead to developer confusion.

### Focus on use cases
If we can’t describe what developers want to do with our API, they won’t know what we’re expecting, and we won’t have any guiding vision to drive the development of the API.

### Copy successful APIs
We are standing on the shoulders of giants. There’s no shame in cribbing from a successful API to make the experience of our developers that much more consistent and easy.

### REST is not always best
Although the focus of this application is on REST API, it’s important to keep a critical eye on the development to make sure that idealism isn’t trumping usability.

### Focus on the developer experience
Again, it’s worth reiterating here that a great developer experience is the number one way to ensure success for our API.

<a name="architecture"/>

## Architecture

One of the key principles in employing a microservices-based architecture is Divide and Conquer. The decomposition of the system into discrete and isolated subsystems communicating over well-defined protocols.

Isolation is a prerequisite for resilience and elasticity and requires asynchronous communication boundaries between services to decouple them in,

- Time
  - Allowing concurrency
- Space
  - Allowing distribution and mobility—the ability to move services around

When adopting Microservices, it is also essential to eliminate shared mutable state and thereby minimize coordination, contention and coherency cost, as defined in the Universal Scalability Law2 by embracing a Share-Nothing Architecture.

<img src="https://github.com/SalZaki/Luna/blob/main/assets/luna-ms-layered-architecture.png" width="50%" height="50%">

### Solution Structure

```bash
    .
    │
    ├─ payment                                 # Payment api
    │   ├─ Luna.Services.Payment.Api            
    │   ├─ Luna.Services.Payment.Application
    │   ├─ Luna.Services.Payment.Domain
    │   └─ Luna.Services.Payment.Infrastructe
    │
    ├─ lib                                      # Lib folder contains common and cross cutting concerns
    │   ├─ Luna.Framework.AspNetCore
    │   └─ Luna.Framework.Common
    │
    ├─ 3rd-party-bank-api                       # Acquirer bank api
    │   └─ AcquirerBank                         
    │
    ├─ docs
    │   ├─ TBA
    │   └─ TBA
    │
    ├─ assests
    │   └─ images
    │
    └─ README.md
        
```

<a name="microservices"/>

## Microservices

### Microservice Architecture

<img src="https://github.com/SalZaki/Luna/blob/main/assets/luna-ms-architecture.png" width="50%" height="50%">

<a name="payment-api"/>

### Payment Api

<a name="features"/>

#### Features

Luna Payment api is build with following features

- [x] Idempotent support for creating a payment request, based on unique id (UUID [RFC4122])
- [x] Clients can attach there own meta data to a payment request, e.g customer id, invoice id etc. in a form of key value pair,
 ```
 "meta_data": [
  {
    "name": "string",
    "value": "string"
  }]
 ```    
- [x] CQRS
- [x] Supports versioning
- [x] Polly for resiliency when calling 3rd party bank api
- [x] Security header middleware
- [x] Exception handling middleware
- [x] Api key
- [x] Configuration driven api design via api settings
- [x] Document urls are included in all exceptions, helping clients to fix the issues with their requests
- [x] Calculated estimated cost per transaction for merchants
- [x] Fluent validation
- [x] Swagger and OpenApi support
- [x] Docker compose support

<a name="api-settings"/>

#### Api Settings

```json
 "ApiSettings": {
    "Name": "Luna Payment Api",
    "DbName": "LunaPaymentDB",
    "DocumentationUrl": "https://api.payment.luna.com/v1/documentation/",
    "Title": "Luna Payment Api",
    "Version": "1.0",
    "Description": "Luna Payment Api provides functionality to customer's payment instruments.",
    "ContactName": "Luna Payment Support",
    "ContactEmail": "support@luna.com",
    "TermOfServiceUrl": "https://api.payment.luna.com/terms",
    "LicenseName": "Enterprise",
    "LicenseUrl": "https://api.payment.luna.com/license",
    "BasePath": "/v1",
    "HostPath": "https://api.payment.luna.com",
    "DefaultPageNumber": 1,
    "DefaultPageSize": 20,
    "EnableBanner": true,
    "EnableSwagger": true,
    "IncludeSecurityHeader": true,
    "IncludeAuthHeader": false,
    "ApiKey": "47BBEB996A9249AC8AC6180DE925A118",
    "GatewayId": "604DEACB865546848D026272284213E8",
    "RequestMasking": {
      "Enabled": true,
      "MaskTemplate": "*"
    }
  }

```

```json
  "HttpClientSettings": {
    "Name": "AcquirerBankApiClient",
    "BaseAddress": "http://localhost:9100/",
    "EndPoints": [
      {
        "Name": "Charge",
        "Url": "api/charge"
      }
    ],
    "TransactionCostPercentage" : 0.05,
    "CacheExpirationInMin": "15",
    "CacheKey": "acquirer-bank-cache-{0}",
    "NumberOfExceptionsBeforeCircuitBreaker": 20,
    "CircuitBreakerFailurePeriodInMin": 1,
    "RetryTimeSpansInSec": [ 1, 3, 5 ]
  }
```


<a name="api-key"/>

#### Api key
Please use following api key as this is a must, when using Luna Payment Api,

```
"ApiKey": "47BBEB996A9249AC8AC6180DE925A118"
```

<a name="example-request-header"/>

#### Example Post Request Header

<img src="https://github.com/SalZaki/Luna/blob/main/assets/luna-api-request-header.png" width="100%" height="100%">

<a name="example-request-body"/>

#### Example Post Request Body

Please use following example post request in either Postman or Swagger,

```
{
  "merchant_id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "meta_data": [
    {
      "name": "customer_Id",
      "value": "38771"
    }
  ],
  "card": {
    "card_type": "VISA",
    "exp_month": "03",
    "exp_year": "24",
    "cvv": "233",
    "number": "4242 4242 4242 4242",
    "name_on_card": "Mr J Smith"
  },
  "amount": 120,
  "currency": "GBP"
}
```

<a name="example-response-header"/>

#### Example Post Response Header

```
 api-supported-versions: 1.0 
 content-type: application/json; charset=utf-8 
 date: Thu,03 Feb 2022 02:40:06 GMT 
 location: https://localhost:7083/v1/payment/886e27ab-d393-4a39-849b-c69033aa2219 
 server: Kestrel 
 transfer-encoding: chunked 
 x-content-type-options: nosniff 
 x-frame-options: DENY 
 x-permitted-cross-domain-policies: none 
 x-xss-protection: 0 
 ```

<a name="example-response-body"/>

#### Example Post Response Body
```
{
  "data": {
    "payment_id": "886e27ab-d393-4a39-849b-c69033aa2219",
    "status": "Completed",
    "finalised_on": "2022-02-03T02:40:06.069942Z",
    "updated_on": "0001-01-01T00:00:00",
    "submitted_on": "2022-02-03T02:40:05.940032Z",
    "estimated_settlement_cost": 0.06,
    "bank_code": "123",
    "bank_status": "Approved",
    "bank_reason": "Success",
    "idempotent_key": "12c19825-af41-4651-a927-e99c9b52b1b5",
    "merchant_id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "meta_data": [
      {
        "name": "customer_Id",
        "value": "38771"
      }
    ],
    "card": {
      "card_type": "VISA",
      "exp_month": "03",
      "exp_year": "24",
      "cvv": "233",
      "number": "4242 4242 4242 4242",
      "name_on_card": "Mr J Smith"
    },
    "amount": 120,
    "currency": "GBP"
  },
  "status": "Success",
  "version": "1.0"
}
```

<a name="faulted-post-response"/>

#### Example Faulted Post Response
When ever a fault has occuered, api will send a response with documentation url, which will direct clients to fix the issue with their request, as shown below,

##### Response body

```
{
  "DocumentationUrl": "https://api.payment.luna.com/v1/documentation/payments/30001",
  "title": "Access Denied",
  "status": 401,
  "detail": "You do not have permission to perform this action or access this resource. Api key does not exist or 
  is invalid in request header.",
}
```

<a name="3rd-party-bank-api"/>

### 3rd Party Bank Api
3rd part bank api is a mock api, which acts a bank process the payment requests. At the moment there is not much functionality developed in this microservice. Currently, the api is only processing Visa card types and GBP as currecny as shown below,

```
"card_type": "VISA"
```
```
"currency": "GBP"
```

<a name="getting-started"/>

## Getting Started

Clone the repository using the command git clone https://github.com/SalZaki/Luna.git and checkout the main branch.

<a name="start-solution"/>

### How to start the solution?

Open `Luna` directory and execute:

```
docker-compose -f up -d
```

<a name="test"/>

## Test

Work in progress and coming very soon

```bash
# unit tests
$ npm run test

# e2e tests
$ npm run test:e2e

# test coverage
$ npm run test:cov
```
<a name="further"/>

## Further Development and Architecture

<img src="https://github.com/SalZaki/Luna/blob/main/assets/luna-architecture.png" width="100%" height="100%">

<a name="development-tools"/>

## Development Tools

- macOS Catalina 🍎
- JetBrains Rider
- .NET Core 6
- Docker 🐳

<a name="fun-qoutes"/>

## Fun Qoutes

> To move, to breathe, to fly, to float,  
> To gain all while you give,  
> To roam the roads of lands remote,  
> To travel is to live.
>
> **[H.C. Andersen](https://en.wikipedia.org/wiki/Hans_Christian_Andersen)**




> Do anything, but let it produce joy
>
> **[Walt Whitman](https://en.wikipedia.org/wiki/Walt_Whitman)**

