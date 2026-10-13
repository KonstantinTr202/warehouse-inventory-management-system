# Warehouse Inventory Management System

Курсовой проект: Информационная система учёта товаров на складе.

## Быстрый старт
1. Восстановите пакеты: `dotnet restore`
2. Запустите API: `dotnet run --project src/WarehouseInventory.Api`
3. Откройте Swagger в браузере: `http://localhost:5299/swagger` *(порт может отличаться)*

## Стек технологий
- **Платформа:** .NET 9 (ASP.NET Core Web API)
- **Язык:** C#
- **Тестирование:** xUnit + Moq
- **Документация:** Swagger / OpenAPI

## Структура проекта
- `src/WarehouseInventory.Api` — Точка входа и контроллеры
- `src/WarehouseInventory.Contracts` — DTO и интерфейсы взаимодействия
- `src/WarehouseInventory.Inventory` — Логика учёта складских остатков
- `tests/` — Модульные тесты
- `docs/` — Проектная документация и архитектурные решения (ADR)

## Этапы разработки
- **Этап 1:** Проектирование архитектуры и контрактов
- **Этап 2:** Настройка Git, `.gitignore` и `.editorconfig`
- **Этап 3:** Реализация API и модульных тестов
- **Этап 4:** Отладка и исправление дефектов
