# Правила ведения репозитория

## 1. Ветки

- `main` — стабильная версия
- `develop` — интеграция
- `feature/*` — новые функции (например, `feature/shipment-module`)
- `fix/*` — исправления (например, `fix/stock-balance-calculation`)
- `docs/*` — документация

## 2. Правила коммитов

Один коммит должен содержать одно логически завершённое изменение.

**Примеры:**
feat: add receipt module
fix: prevent negative stock balance
docs: add inventory architecture
test: add stock deduction tests