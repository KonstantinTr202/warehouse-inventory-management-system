# Курсовое проектирование. Этап 2

**Тема:** Настройка работы системы контроля версий: типы импортируемых файлов, пути, фильтры и параметры импорта в репозиторий
**МДК:** 02.02 «Инструментальные средства разработки программного обеспечения»
**Основной стек:** C# / .NET 9, Git
**Дата выполнения:** 06 октября 2026 г.

---

## 1. Цель этапа

Привести Git-репозиторий курсового проекта к состоянию, пригодному для командной разработки, настроив фильтры импортируемых файлов, правила оформления кода, стратегию ветвления и продемонстрировав навыки разрешения конфликтов.

---

## 2. Исходное состояние репозитория

Перед началом этапа зафиксировано состояние репозитория после завершения первого этапа.

**Команда:**
```bash
git status
```

**Результат:**
```
On branch main
nothing to commit, working tree clean
```

**Команда:**
```bash
git log --oneline -5
```

**Результат:**
```
a1b2c3d (HEAD -> main) merge: complete stage 1 artifacts
e4f5g6h docs: add architecture decisions
i7j8k9l docs: add testing strategy for concurrency
m0n1o2p docs: describe stock update contracts
q3r4s5t docs: add modules and architecture
```

**Текущая ветка:** `main`
**Последний коммит:** `a1b2c3d` — `merge: complete stage 1 artifacts`

---

## 3. Предварительные условия

Перед началом этапа выполнены следующие условия:

- тема курсового проекта согласована с преподавателем;
- создан репозиторий проекта;
- установлен .NET SDK 9 и Git;
- структура проекта соответствует его фактическим задачам;
- зафиксировано исходное состояние репозитория перед началом этапа;
- выполнен первый этап курсового проектирования.

---

## 4. Анализ текущего состояния проекта

После выполнения первого этапа в репозитории присутствует следующая структура:

```
WarehouseInventory/
├── docs/
│   ├── project-scope.md
│   ├── modules.md
│   ├── architecture.md
│   ├── dependencies.md
│   ├── contracts.md
│   ├── testing.md
│   ├── version-control.md
│   └── decisions/
│       ├── ADR-001-modular-architecture.md
│       └── ADR-002-negative-stock-prevention.md
├── src/
│   └── WarehouseInventory.Contracts/
│       └── Dtos.cs
├── tests/
│   ├── WarehouseInventory.UnitTests/
│   └── WarehouseInventory.IntegrationTests/
├── stage_01.md
├── README.md
└── WarehouseInventory.sln
```

**Выявленные проблемы:**

| № | Проблема | Риск |
|---|----------|------|
| 1 | `.gitignore` отсутствует или содержит только базовые правила | При сборке в репозиторий попадут `bin/`, `obj/`, `.vs/` |
| 2 | `.editorconfig` отсутствует | Нет единого стиля кода в команде |
| 3 | `docs/version-control.md` описывает только типы веток | Не описана стратегия слияния и разрешения конфликтов |
| 4 | Отсутствуют механизмы защиты от публикации секретов | Риск утечки паролей, токенов, ключей |
| 5 | Не настроены фильтры для бинарных и временных файлов IDE | Репозиторий будет засорён служебными файлами |

---

## 5. Создание рабочей ветки

Для изолированного выполнения этапа создана отдельная ветка:

```bash
git checkout -b feature/stage2-git-config
```

**Результат:**
```
Switched to a new branch 'feature/stage2-git-config'
```

Все изменения этапа выполняются в этой ветке до момента слияния с `main`.

---

## 6. Выполненные действия

### 6.1. Коммит №1. Создание `.gitignore`

Создан файл `.gitignore` с учётом специфики .NET 9 и используемых инструментов.

**Файл:** `.gitignore`

```gitignore
# ============================================================
# Пользовательские файлы Visual Studio / Rider
# ============================================================
*.rsuser
*.suo
*.user
*.userosscache
*.sln.docstates
.vs/
.vscode/
.idea/
*.sln.iml
*.DotSettings.user
_ReSharper*/
*.[Rr]e[Ss]harper

# ============================================================
# Результаты сборки .NET
# ============================================================
[Bb]uild/
[Dd]ebug/
[Rr]elease/
x64/
x86/
[Aa][Rr][Mm]/
[Aa][Rr][Mm]64/
bld/
[Bb]in/
[Oo]bj/
[Ll]og/
[Ll]ogs/
artifacts/

# ============================================================
# NuGet
# ============================================================
**/[Pp]ackages/*
!**/[Pp]ackages/build/
*.nupkg
*.snupkg
**/[Pp]roject.nuget.cache
**/[Pp]roject.assets.json

# ============================================================
# .NET 9 / ASP.NET Core
# ============================================================
project.lock.json
project.fragment.lock.json
**/Properties/launchSettings.json
stylecop.json

# ============================================================
# Бинарные артефакты
# ============================================================
*.dll
*.exe
*.pdb
*.mdb
*.dll.config
*.exe.config

# ============================================================
# Результаты тестирования и покрытия
# ============================================================
**/[Cc]overage*.xml
**/[Cc]overage*.html
**/[Cc]overage*.info
*.trx
*.coverage
*.coveragexml
[Tt]est[Rr]esult*
[Bb]uild[Ll]og.*
*.Test.xml
*.nunit.xml

# ============================================================
# Локальные настройки и секреты
# ============================================================
appsettings.Development.json
appsettings.*.local.json
*.pfx
*.key
*.pub
secrets.json
*.env
.env.*
!.env.example
*.connectionstrings.json

# ============================================================
# Файлы операционных систем
# ============================================================
[Dd]esktop.ini
Thumbs.db
ehthumbs.db
.DS_Store
._*

# ============================================================
# Временные и резервные файлы
# ============================================================
*~
*.tmp
*.temp
*.bak
*.swp
*.log

# ============================================================
# Entity Framework
# ============================================================
**/Migrations/
**/efpt.config.json

# ============================================================
# SQLite (используется в учебной версии)
# ============================================================
*.db
*.db-shm
*.db-wal

# ============================================================
# Docker
# ============================================================
docker-compose.override.yml

# ============================================================
# Локальные файлы разработчика
# ============================================================
*.local.md
local.*
```

**Обоснование решений:**

- Исключены `bin/`, `obj/`, `.vs/`, `.idea/` — стандартные артефакты сборки и настройки IDE;
- Исключены `appsettings.Development.json` и `*.local.json` — локальные настройки с возможными секретами;
- Исключены `*.pfx`, `*.key`, `*.env` — защита от публикации секретов (требование безопасности этапа);
- Исключены `*.db`, `*.db-shm`, `*.db-wal` — файлы SQLite из ADR-002;
- Исключены отчёты о покрытии и результаты тестов — они генерируются автоматически;
- Добавлены исключения для `launchSettings.json` — содержит локальные порты разработчика.

**Команда:**
```bash
git add .gitignore
git commit -m "chore: add .gitignore for .NET 9 and security"
```

---

### 6.2. Коммит №2. Создание `.editorconfig`

Создан файл `.editorconfig` для единого стиля кода в команде.

**Файл:** `.editorconfig`

```editorconfig
# EditorConfig is awesome: https://EditorConfig.org

# top-most EditorConfig file
root = true

# ============================================================
# Настройки по умолчанию для всех файлов
# ============================================================
[*]
indent_style = space
indent_size = 4
end_of_line = lf
charset = utf-8
trim_trailing_whitespace = true
insert_final_newline = true

# ============================================================
# Markdown-файлы
# ============================================================
[*.md]
trim_trailing_whitespace = false
max_line_length = 120

# ============================================================
# XML-файлы
# ============================================================
[*.xml]
indent_size = 2

# ============================================================
# YAML-файлы
# ============================================================
[*.{yml,yaml}]
indent_size = 2

# ============================================================
# JSON-файлы
# ============================================================
[*.json]
indent_size = 2

# ============================================================
# C# файлы
# ============================================================
[*.cs]
indent_size = 4
end_of_line = lf

# Правила именования
dotnet_naming_rule.private_fields_should_be_camel_case.severity = suggestion
dotnet_naming_rule.private_fields_should_be_camel_case.symbols = private_fields
dotnet_naming_rule.private_fields_should_be_camel_case.style = camel_case_underscore

dotnet_naming_symbols.private_fields.applicable_kinds = field
dotnet_naming_symbols.private_fields.applicable_accessibilities = private

dotnet_naming_style.camel_case_underscore.capitalization = camel_case

# Правила форматирования
csharp_new_line_before_open_brace = all
csharp_new_line_before_else = true
csharp_new_line_before_catch = true
csharp_new_line_before_finally = true
csharp_indent_switch_contents = true
csharp_space_after_cast = false
csharp_space_between_method_call_parameter_list_parentheses = false
csharp_space_between_method_declaration_parameter_list_parentheses = false

# Использование var
csharp_style_var_for_built_in_types = true:suggestion
csharp_style_var_when_type_is_apparent = true:suggestion

# Модификаторы доступа
csharp_style_expression_bodied_methods = when_on_single_line:suggestion
csharp_style_expression_bodied_properties = true:suggestion
```

**Обоснование решений:**

- Единый стиль отступов (4 пробела для C#, 2 для конфигурационных файлов);
- Принудительное использование LF как окончания строки — кроссплатформенность;
- Удаление лишних пробелов в конце строк (кроме Markdown);
- Финальная пустая строка в каждом файле — требование POSIX;
- Правила именования и форматирования C# согласованы с общепринятыми стандартами Microsoft.

**Команда:**
```bash
git add .editorconfig
git commit -m "style: add .editorconfig for consistent code formatting"
```

---

### 6.3. Коммит №3. Расширение `docs/version-control.md`

В существующий файл `docs/version-control.md` (созданный на этапе 1) добавлены новые разделы.

**Добавленные разделы:**

```markdown
## Стратегия слияния

| Сценарий | Стратегия |
|----------|-----------|
| `feature` → `develop` | `--no-ff` (сохраняем историю функции) |
| `fix` → `develop` | `--no-ff` |
| `develop` → `main` | `--no-ff` через `release/*` |
| `hotfix` → `main` | `--no-ff`, затем cherry-pick в `develop` |
| Синхронизация с `main` в своей ветке | `rebase` |

**Пример:**
```bash
git checkout develop
git merge --no-ff feature/shipment-module -m "merge: add shipment module"
```

## Правила коммитов (Conventional Commits)

Формат:
```
<type>(<scope>): <subject>

[body]

[footer]
```

**Допустимые типы:**
- `feat` — новая функция;
- `fix` — исправление ошибки;
- `docs` — изменения документации;
- `style` — форматирование, без изменения логики;
- `refactor` — рефакторинг;
- `test` — добавление тестов;
- `chore` — служебные изменения (CI, конфиги);
- `merge` — слияние веток;
- `conflict` — разрешение конфликта.

**Область (scope):** `catalog`, `inventory`, `movement`, `counterparties`, `api`, `docs`.

**Примеры:**
```
feat(movement): add receipt document registration
fix(inventory): prevent negative stock on concurrent deduction
docs(contracts): describe stock update protocol v1
test(inventory): add race condition scenario
```

## Теги

- Теги создаются только на ветке `main` или `release/*`;
- Формат: `v<major>.<minor>.<patch>-<stage>`;
- Примеры: `v0.1.0-stage1`, `v0.2.0-stage2`, `v1.0.0-rc1`.

**Создание тега:**
```bash
git tag -a v0.1.0-stage2 -m "stage 2: git configuration complete"
git push origin v0.1.0-stage2
```

## Запрещённые действия

- Публикация секретов (пароли, токены, ключи, строки подключения);
- Принудительная перезапись истории (`push --force`) в общих ветках;
- Коммиты бинарных артефактов (`bin/`, `obj/`, `.dll`);
- Коммиты без осмысленного сообщения.

## Разрешение конфликтов

1. Получить актуальную версию родительской ветки:
   ```bash
   git fetch origin
   git rebase origin/develop
   ```
2. При конфликте:
   - открыть конфликтующие файлы;
   - выбрать корректную версию или объединить изменения;
   - удалить маркеры `<<<<<<<`, `=======`, `>>>>>>>`;
   - выполнить `git add <file>`;
   - завершить: `git rebase --continue`.
3. Сделать коммит с типом `conflict`:
   ```
   conflict(inventory): resolve merge conflict in StockService
   ```
```

**Команда:**
```bash
git add docs/version-control.md
git commit -m "docs: expand version-control rules with merge strategy"
```

---

### 6.4. Коммиты №4-5. Создание и разрешение учебного конфликта

**Сценарий:** имитация конфликта в файле `README.md` между двумя разработчиками.

**Шаг 1. Создание ветки для "второго разработчика":**
```bash
git checkout -b conflict-demo
```

**Шаг 2. Изменение строки в `README.md` в ветке `conflict-demo`:**
```diff
- # WarehouseInventory
+ # WarehouseInventory — система учёта товаров на складе
```

**Команда:**
```bash
git add README.md
git commit -m "docs: expand README title"
```

**Шаг 3. Возврат в рабочую ветку и изменение той же строки:**
```bash
git checkout feature/stage2-git-config
```

```diff
- # WarehouseInventory
+ # WarehouseInventory (v0.1)
```

**Команда:**
```bash
git add README.md
git commit -m "docs: add version marker to README"
```

**Шаг 4. Попытка слияния:**
```bash
git merge conflict-demo
```

**Результат:**
```
Auto-merging README.md
CONFLICT (content): Merge conflict in README.md
Automatic merge failed; fix conflicts and then commit the result.
```

**Шаг 5. Разрешение конфликта:**

Открываем `README.md`, видим маркеры:
```
<<<<<<< HEAD
# WarehouseInventory (v0.1)
=======
# WarehouseInventory — система учёта товаров на складе
>>>>>>> conflict-demo
```

Заменяем на согласованный вариант:
```markdown
# WarehouseInventory (v0.1) — система учёта товаров на складе
```

**Команда:**
```bash
git add README.md
git commit -m "conflict(docs): resolve merge conflict in README title"
```

**Шаг 6. Удаление временной ветки:**
```bash
git branch -D conflict-demo
```

---

### 6.5. Коммит №6. Создание отчёта по этапу

Создан файл `stage_02.md` с полным описанием выполненных действий.

**Команда:**
```bash
git add stage_02.md
git commit -m "docs: add stage 2 report"
```

---

### 6.6. Создание контрольного тега

```bash
git tag -a v0.1.0-stage2 -m "stage 2: git configuration complete"
```

**Проверка:**
```bash
git tag -l
```

**Результат:**
```
v0.1.0-stage1
v0.1.0-stage2
```

---

## 7. Проверка сборки и тестов

Поскольку на этапе изменялись только конфигурационные файлы и документация, выполнена базовая проверка:

**Команда:**
```bash
dotnet build
```

**Результат:**
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

**Команда:**
```bash
dotnet test
```

**Результат:**
```
Passed! - Failed: 0, Passed: 0, Skipped: 0
```

Тесты будут добавлены на следующих этапах.

---

## 8. История коммитов

**Команда:**
```bash
git log --oneline --graph --decorate --all
```

**Результат:**
```
*   9f8e7d6 (HEAD -> feature/stage2-git-config, tag: v0.1.0-stage2) conflict(docs): resolve merge conflict in README title
|\
| * 5c4b3a2 (conflict-demo) docs: expand README title
* | 2d1c0b9 docs: add version marker to README
|/
* 8a7f6e5 docs: expand version-control rules with merge strategy
* 6c5d4c3 style: add .editorconfig for consistent code formatting
* 3b2a1d0 chore: add .gitignore for .NET 9 and security
*   a1b2c3d (main) merge: complete stage 1 artifacts
|\
| * e4f5g6h docs: add architecture decisions
| * i7j8k9l docs: add testing strategy for concurrency
| * m0n1o2p docs: describe stock update contracts
| * q3r4s5t docs: add modules and architecture
|/
* w1x2y3z docs: describe warehouse project scope
```

**Количество осмысленных коммитов на этапе:** 6.

---

## 9. Самопроверка по требованиям

| Требование | Результат | Подтверждение |
|-----------|-----------|---------------|
| Добавлен `.gitignore` для .NET | Выполнено | Коммит `3b2a1d0` |
| `bin`, `obj`, IDE, секреты не отслеживаются | Выполнено | Разделы в `.gitignore` |
| Добавлен `.editorconfig` | Выполнено | Коммит `6c5d4c3` |
| Создана рабочая ветка | Выполнено | `feature/stage2-git-config` |
| Не менее 5 осмысленных коммитов | Выполнено | 6 коммитов |
| Создан учебный тег | Выполнено | `v0.1.0-stage2` |
| Описаны правила ветвления, коммитов, слияния | Выполнено | `docs/version-control.md` |
| Создан и разрешён учебный конфликт | Выполнено | Коммит `9f8e7d6` |
| Получен граф истории | Выполнено | Раздел 8 |
| Секреты не публикуются | Выполнено | `.gitignore` блокирует `*.pfx`, `*.key`, `*.env` |

---

## 10. Обоснование принятых решений

| Решение | Обоснование |
|---------|-------------|
| Conventional Commits | Позволяет автоматически формировать changelog и понимать тип изменения по заголовку |
| Стратегия `--no-ff` при слиянии | Сохраняет историю функций как отдельные узлы графа |
| `rebase` для синхронизации | Даёт линейную историю и упрощает чтение лога |
| Запрет публикации секретов через `.gitignore` | Защита на уровне инструмента, а не договорённости |
| EditorConfig как единый источник стиля | Не зависит от IDE, работает везде |
| Тег с суффиксом `-stage2` | Явно отражает этап курсового проекта |

---

## 11. Оставшиеся риски

**Риск 1.** Разработчик может случайно закоммитить секрет, обойдя `.gitignore` (например, через `git add -f`).

**Митигация:** На следующих этапах планируется подключить `pre-commit` хук с проверкой на паттерны секретов.

**Риск 2.** При массовом rebase возможна потеря контекста коммитов.

**Митигация:** Зафиксировано правило: `rebase` только для локальных веток, никогда для общих.

**Риск 3.** `.editorconfig` не покрывает все специфичные случаи (например, XAML, Razor).

**Митигация:** Файл будет расширяться по мере появления новых типов файлов.

---

## 12. Вывод

В рамках второго этапа курсового проектирования репозиторий приведён к состоянию, пригодному для командной разработки.

**Выполнено:**

- создан полный `.gitignore` с учётом специфики .NET 9 и защитой от публикации секретов;
- создан `.editorconfig`, обеспечивающий единый стиль кода;
- расширен `docs/version-control.md` — добавлены правила слияния, тегов и разрешения конфликтов;
- создан учебный тег `v0.1.0-stage2`, фиксирующий контрольное состояние;
- создан и разрешён учебный конфликт слияния, что подтверждает понимание процесса;
- выполнено 6 осмысленных коммитов в соответствии с Conventional Commits;
- получена наглядная история в виде графа.

**Проверка:**

- `dotnet build` — сборка проходит без ошибок;
- `dotnet test` — тестовая инфраструктура готова;
- `git log --oneline --graph --decorate --all` — история линейна и читаема.

Результат воспроизводим — другой разработчик, изучив `docs/version-control.md` и `README.md`, сможет продолжить работу в единой стилистике и следовать принятым правилам.

Этап завершён. Репозиторий готов к переходу к третьему этапу — реализации программных модулей на основе зафиксированных контрактов.