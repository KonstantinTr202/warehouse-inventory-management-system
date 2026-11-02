# Warehouse Inventory Management System

Учебный API учёта складских операций на C# и .NET 10. Сервис принимает приход и отгрузку, проверяет количество и недостаток остатка. Документы этапов: [1](stage_01.md), [2](stage_02.md), [3](stage_03.md), [4](stage_04.md), [5](stage_05.md).

> **Ограничения:** это демонстрация, а не готовая складская система. Один общий остаток начинается с 10 и хранится только в памяти процесса, независимо от `itemId` и `warehouseId`; после перезапуска он сбрасывается. Базы данных, проверки существования товара/склада, идемпотентности, учётных записей и авторизации нет. Не используйте реальные данные и не открывайте API в сеть.

## Запуск и проверка

Нужны .NET SDK 10 и доступ к NuGet при первом восстановлении. Из корня репозитория в Git Bash:

```bash
dotnet restore WarehouseInventory.slnx
dotnet build WarehouseInventory.slnx
dotnet test WarehouseInventory.slnx
dotnet format WarehouseInventory.slnx --verify-no-changes
dotnet run --project src/WarehouseInventory.Api --no-launch-profile --urls http://127.0.0.1:5299
```

Swagger доступен в окружении `Development`: в Git Bash задайте `export ASPNETCORE_ENVIRONMENT=Development` перед запуском и откройте `http://127.0.0.1:5299/swagger`. Чтобы завершить работу сервера, нажмите Ctrl+C.

## Пример запроса

В другом окне Git Bash отправьте приход:

```bash
curl -sS -X POST http://127.0.0.1:5299/api/Movement/receipt \
  -H 'Content-Type: application/json' \
  -d '{"itemId":"416f13ce-c185-4cf2-8a3f-30c1d99e5cc2","warehouseId":"3dc3a71d-9fd6-4e02-89fb-8c12199d1c83","quantity":20,"movementDocumentId":"660e86e8-d3ad-4681-8a46-9a08ac0b1122"}'
```

Отгрузка выполняется аналогичным запросом на `/api/Movement/shipment`; можно добавить поле `comment`. Успех - HTTP 200 с `newBalance`, неверное количество или недостаток товара - HTTP 400 с `error` и `currentBalance`. Например, после прихода 20 остаток равен 30, а отгрузка 999 вернёт `INSUFFICIENT_STOCK` и не изменит его. Повтор успешного запроса проведёт операцию ещё раз: проверка `movementDocumentId` пока не реализована.

## Состав

- `src/WarehouseInventory.Api` - контроллер прихода и отгрузки.
- `src/WarehouseInventory.Contracts` - запросы, ответ и интерфейс.
- `src/WarehouseInventory.Inventory` - учебный сервис остатка.
- `tests/WarehouseInventory.UnitTests` - исполняемые модульные тесты.
- `docs/` - проектные материалы; исходные планы могут описывать ещё не реализованные возможности. [Уточнение контрактов](docs/contracts.md) и [проверки](docs/testing.md).
