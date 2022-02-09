# MangoRestarauntCoffee

Реализация продуктового магазина с использованием микросервисной архитектуры, служебной шины Azure .NET 6.
В качестве оснвого web-проекта выступает Mango.Web. ASP.NET Core MVC.

API-проекты с использованием синхронного вызова: Mango.Services.CouponAPI, Mango.Services.ProductAPI, Mango.Services.ShoppingCartAPI, Mango.Services Identity.

API-проекты с использованием служебной шины Azure: Mango.Services.OrderAPI, Mango.Services.Email, Mango.Services.PaymentAPI
Integration/MangoMessageBus - посредник между сообщениями между проектами и шиной.

В ветке Gateway Ocelot - используется шлюз Gateway Ocelot для микросервисов.

Для запуска:
1. git clone https://github.com/adichev17/MangoRestarauntCoffee.git
2. Провести миграции в БД(update-database) для каждого из проектов в папке Services.
3. Настроить мультизапуск для всех проектов.
