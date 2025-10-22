# TaskManager

Короткий опис, як запустити проект.

## Вимоги
- Встановлений .NET SDK 8.0

## Структура проекту
- **TaskManager** - основний API проект
- **TaskManagerTest** - юніт-тести

## Швидкий старт

## Швидкий старт
1) Відновити пакети
```bash
dotnet restore
```
2) Зібрати рішення
```bash
dotnet build
```
3) Запустити API
```bash
dotnet run --project TaskManager
```

Swagger: `http://localhost:5254/swagger` HTTP

## Приклад запиту (curl)
Створення задачі (POST `api/tasks`) на базовий URL `http://localhost:5254`:
```bash
curl -X POST "http://localhost:5254/api/tasks" \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Buy milk",
    "description": "Get 2L of milk"
  }'
```

Очікувана відповідь: обʼєкт `BaseResponseDto` з даними задачі у полі `data`.

База даних — InMemory (нічого додатково налаштовувати не потрібно).

### 4. Запуск тестів (TaskManagerTest)
```bash
dotnet test
```

або запуск конкретного тестового проекту:
```bash
dotnet test TaskManagerTest
```
