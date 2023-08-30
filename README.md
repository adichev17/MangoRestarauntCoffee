![Build Status](https://github.com/adichev17/MangoRestarauntCoffee.Microservices/workflows/Actions/badge.svg)
![GitHub repo size](https://img.shields.io/github/repo-size/adichev17/MangoRestarauntCoffee.Microservices)
![MicrosoftSQLServer](https://img.shields.io/badge/Microsoft%20SQL%20Server-CC2927?style=for-the-badge&logo=microsoft%20sql%20server&logoColor=white)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Azure](https://img.shields.io/badge/azure-%230072C6.svg?style=for-the-badge&logo=microsoftazure&logoColor=white)

# Project Title

Онлайн магазин товаров

## Description

Реализация продуктового магазина с использованием микросервисной архитектуры, служебной шины Azure .NET 6

## Stack

- .NET 6.
- ASP.NET Web-Api, MVC
- Azure Service Bus
- Identity Server 6
- Entity Framework Core 6
- MS Sql Server

## Detalization

* В качестве оснвого web-проекта выступает Mango.Web. ASP.NET Core MVC.
* API-проекты с использованием синхронного вызова: Mango.Services.CouponAPI, Mango.Services.ProductAPI, Mango.Services.ShoppingCartAPI, Mango.Services Identity.
* API-проекты с использованием служебной шины Azure: Mango.Services.OrderAPI, Mango.Services.Email, Mango.Services.PaymentAPI
* Integration/MangoMessageBus - посредник между сообщениями между проектами и шиной.

## Getting Started

### Installing

```
git clone https://github.com/adichev17/MangoRestarauntCoffee.Microservices.git
```

