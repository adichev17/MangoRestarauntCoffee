![Build Status](https://github.com/adichev17/Patterns/workflows/.NET%20Core%20Desktop/badge.svg)]
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

