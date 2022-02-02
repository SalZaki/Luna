<img src="https://github.com/SalZaki/Luna/blob/main/assests/Luna.png" width="15%" height="15%">

# Table of contents

- [What is Luna](#luna)
- [Overview](#overview)
- [Requirements](#requirements)
- [Architecture](#architecture)
  - [Design Approach](#design-approach)
  - [Solution Structure](#solution-structure)
- [Microservices](#microservices)
  - [Payment](#payment)
  - [3rd Party Bank](#bank)
- [Getting Started](#getting-started)

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
Lu na's engineering team tried to tackle this challenge by adapting following design approach and mindset,

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

<a name="requirements"/>

## Requirements

Process payment requests

<a name="architecture"/>

## Architecture

One of the key principles in employing a microservices-based architecture is Divide and Conquer. The decomposition of the system into discrete and isolated subsystems communicating over well-defined protocols.

Isolation is a prerequisite for resilience and elasticity and requires asynchronous communication boundaries between services to decouple them in,

- Time
  - Allowing concurrency
- Space
  - Allowing distribution and mobility—the ability to move services around

When adopting Microservices, it is also essential to eliminate shared mutable state and thereby minimize coordination, contention and coherency cost, as defined in the Universal Scalability Law2 by embracing a Share-Nothing Architecture.

TBA

### Solution Structure

```bash
    .
    ├─ luna
    │
    ├─ payment
    │   ├─ Luna.Services.Payment.Api            # Payment api
    │   ├─ Luna.Services.Payment.Application
    │   ├─ Luna.Services.Payment.Domain
    │   └─ Luna.Services.Payment.Infrastructe
    │
    ├─ lib
    │   ├─ Luna.Framework.AspNetCore
    │   └─ Luna.Framework.Common
    │
    ├─ 3rd-party-bank
    │   └─ AcquirerBank                         # Acquirer bank api
    │
    ├─ docs
    │   ├─ TBA
    │   └─ TBA
    │
    ├─ assests
    │   ├─ drawio
    │   └─ images
    │
    └─ README.md
        
```

<a name="microservices"/>

## Microservices

### Microservice Architecture

<a name="payment"/>

### Payment
TBA

<a name="bank"/>

### 3rd Part Bank
TBA

### Solution structure and top-level directory layout

```bash
    .
    ├─ lib               # Lib folder contains common and cross cutting concerns
    ├─ payment           # Payment api
    ├─ payment-network   # Source files (alternatively `lib` or `app`)
    ├─ 3rd-party-bank    # Automated tests (alternatively `spec` or `tests`)
    ├─ docs              # Automated tests (alternatively `spec` or `tests`)
    ├─ assests           # Tools and utilities
    └─ README.md
```
<a name="gettingstarted"/>

## Getting Started

Clone the repository using the command git clone https://github.com/SalZaki/Luna.git and checkout the main branch.

## How to start the solution?

Open `luna` directory and execute:

```
docker-compose -f up -d
```
Then the following containers should be running on `docker ps`:

| Application 	      | Docker Image          | URL                                                   |
|-------------------- | --------------------- | ----------------------------------------------------- |
| Luna Payment Api    | luna-payment-api      | http://localhost:8100                                |
| 3rd Party Bank 	    | 3rd-party-bank        | http://localhost:9100                                |


### Test

Coming soon

```bash
# unit tests
$ npm run test

# e2e tests
$ npm run test:e2e

# test coverage
$ npm run test:cov
```

### Development Tools

- MacOS Mojave 🍎
- JetBrains Rider
- .NET Core 6
- Docker 🐳

> To move, to breathe, to fly, to float,  
> To gain all while you give,  
> To roam the roads of lands remote,  
> To travel is to live.
>
> **[H.C. Andersen](https://en.wikipedia.org/wiki/Hans_Christian_Andersen)**
