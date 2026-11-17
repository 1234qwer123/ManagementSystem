\# ManagementSystem: Освітня система університету



Система для управління \[Users, Items, Actions] з MVC-адмін-панеллю та MAUI-застосунком.



Проєкт для управління курсами в університеті. Адміністратор керує викладачами, курсами та пропозиціями через веб-адмін-панель. Викладач через мобільний застосунок бачить свої курси та подає пропозиції щодо створення або видалення курсів.



\*\*Автор:\*\* <ПІБ>, <група>

\*\*Варіант:\*\* 1 (Освітня система університету)



\---



\## Предметна область



| Загальна таблиця | У цьому проєкті | Опис |

|---|---|---|

| Users | \*\*Teachers\*\* | Викладачі (ID, Name, Email, PasswordHash) |

| Items | \*\*Courses\*\* | Курси поточного семестру (ID, Name, Description, Semester, TeacherID) |

| Actions | \*\*Proposals\*\* | Пропозиції щодо курсів (ID, TeacherID, CourseName, ProposalType \[Create/Delete], Status) |



Зв'язки (Foreign Keys):

\- `Courses.TeacherID` → `Teachers.ID`

\- `Proposals.TeacherID` → `Teachers.ID`



\### Сценарій

\- \*\*Адміністратор\*\* (один, логін і пароль задані в коді) керує викладачами, курсами та пропозиціями через адмін-панель (CRUD).

\- \*\*Викладач\*\* (дані в БД) входить у MAUI-застосунок, бачить свої курси та подає пропозиції (створення або видалення курсу).



\---



\## Технології



\- .NET 10, ASP.NET Core MVC

\- Entity Framework Core + SQLite

\- JWT-аутентифікація (адмін-панель через cookie, API через Bearer-токен)

\- BCrypt для хешування паролів

\- Swagger (Swashbuckle) для документації API

\- Bootstrap 5 (CDN) для інтерфейсу адмін-панелі

\- .NET MAUI + MVVM (CommunityToolkit.Mvvm)

\- Postman для тестування API



\---



\## Структура репозиторію



```

ManagementSystem/

├── Server/

│   └── ServerApp/            # ASP.NET Core (MVC-адмін-панель + REST API)

│       ├── Controllers/      # MVC-контролери (Teachers, Courses, Proposals, Auth)

│       │   └── Api/          # API-контролери (Auth, Courses, Proposals)

│       ├── Models/           # Моделі, AppDbContext, AuthSettings, DTO

│       ├── Views/            # Razor-представлення

│       ├── Migrations/       # Міграції EF Core

│       ├── Program.cs

│       └── appsettings.example.json

├── Client/

│   └── ClientApp/            # .NET MAUI застосунок

│       ├── Models/

│       ├── Services/         # ApiService (HttpClient)

│       ├── ViewModels/       # LoginViewModel, CoursesViewModel, ProposalViewModel

│       └── Views/            # LoginPage, CoursesPage, ProposalPage

├── Database/

│   └── init.sql              # Схема БД, індекси, тестові дані

├── Docs/

│   ├── init.sql

│   ├── swagger.json          # Експорт Swagger

│   └── Screenshots/          # Скріншоти тестування

├── .gitignore

└── README.md

```



\---



\## Вимоги



\- Visual Studio з навантаженнями \*\*ASP.NET and web development\*\* та \*\*.NET Multi-platform App UI development\*\*

\- .NET SDK 10

\- Android-емулятор (наприклад, Pixel, API 33 або новіший)

\- Postman (для тестування API)

\- DB Browser for SQLite (необов'язково, для перегляду БД)



\---



\## Встановлення і запуск



\### 1. Клонування

```bash

git clone <ПОСИЛАННЯ\_НА\_РЕПОЗИТОРІЙ>

cd ManagementSystem

```



\### 2. Налаштування сервера

Файл `appsettings.json` не зберігається в репозиторії (він у `.gitignore`). Створіть його на основі прикладу:



```bash

cp Server/ServerApp/appsettings.example.json Server/ServerApp/appsettings.json

```



Вміст `appsettings.json`:

```json

{

&#x20; "ConnectionStrings": { "DefaultConnection": "Data Source=app.db" },

&#x20; "AllowedHosts": "\*"

}

```



\### 3. База даних

БД створюється автоматично при першому запуску сервера (застосовуються міграції та додаються тестові дані).



Альтернатива: виконайте `Database/init.sql` у SQLite (наприклад, у DB Browser for SQLite).



Вручну через Package Manager Console:

```

Add-Migration InitialCreate

Update-Database

```



\### 4. Запуск сервера

1\. Відкрийте рішення у Visual Studio.

2\. Оберіть стартовий проєкт \*\*ServerApp\*\* і профіль запуску \*\*http\*\* (для роботи з емулятором).

3\. Натисніть \*\*F5\*\*.



Адреси (порт дивіться у `Properties/launchSettings.json`):

\- Адмін-панель: `http://localhost:5000/auth/login`

\- Swagger: `http://localhost:5000/swagger`



\### 5. Запуск MAUI-застосунку

1\. Запустіть Android-емулятор (\*\*Tools → Android → Android Device Manager\*\*).

2\. Переконайтесь, що сервер запущений.

3\. Оберіть проєкт \*\*ClientApp\*\*, цільовий пристрій Android-емулятор і натисніть \*\*F5\*\*.



> Android-емулятор звертається до комп'ютера за адресою `10.0.2.2`. Базова адреса API задана в `Services/ApiService.cs`. Для HTTP у `AndroidManifest.xml` увімкнено `usesCleartextTraffic`.



\---



\## Облікові дані для тестування



| Роль | Логін / Email | Пароль |

|---|---|---|

| Адміністратор (адмін-панель) | `admin` | `admin123` |

| Викладач (MAUI, API) | `ivan@univ.ua` | `123456` |

| Викладач (MAUI, API) | `maria@univ.ua` | `123456` |



\---



\## Адмін-панель



Доступ тільки для адміністратора (JWT у cookie `jwt`). Розділи:



\- \*\*Викладачі\*\*: перегляд, створення, редагування, видалення (пароль зберігається як BCrypt-хеш)

\- \*\*Курси\*\*: CRUD, вибір викладача зі списку

\- \*\*Пропозиції\*\*: CRUD, зміна статусу



\---



\## REST API



Базова адреса: `http://localhost:5000`. Документація: `/swagger`.

Захищені ендпоінти потребують заголовок `Authorization: Bearer <token>`.



| Метод | URL | Авторизація | Опис |

|---|---|---|---|

| POST | `/api/auth/login` | Ні | Логін викладача, повертає JWT |

| GET | `/api/courses` | Так | Список курсів поточного викладача |

| POST | `/api/proposals` | Так | Створення нової пропозиції |



\### POST /api/auth/login

Запит:

```json

{ "email": "ivan@univ.ua", "password": "123456" }

```

Відповідь `200 OK`:

```json

{ "token": "<JWT>", "name": "Іван Петренко" }

```

Помилка `401 Unauthorized`: невірний email або пароль.



\### GET /api/courses

Відповідь `200 OK`:

```json

\[

&#x20; { "id": 1, "name": "Бази даних", "description": "Основи SQL та проєктування БД", "semester": "Осінь 2026" }

]

```



\### POST /api/proposals

Запит:

```json

{ "courseName": "Machine Learning", "proposalType": "Create" }

```

`proposalType`: `Create` або `Delete`. Відповідь `201 Created`:

```json

{ "id": 3, "courseName": "Machine Learning", "proposalType": "Create", "status": "Pending" }

```

Помилка `400 Bad Request`: невалідні дані. `401 Unauthorized`: немає або недійсний токен.



\---



\## MAUI-застосунок (MVVM)



| Сторінка | ViewModel | Функціонал |

|---|---|---|

| LoginPage | LoginViewModel | Вхід за email і паролем, збереження JWT у `Preferences` |

| CoursesPage | CoursesViewModel | Список курсів (CollectionView), кешування, оновлення свайпом |

| ProposalPage | ProposalViewModel | Форма створення пропозиції (Create/Delete) |



Взаємодія з API виконується через `ApiService` (`HttpClient`), JWT додається в заголовок `Authorization` автоматично.



\---



\## Оптимізація



\- Індекси БД: `Teachers.Email` (унікальний), `Courses.TeacherID`, `Proposals.TeacherID`

\- Асинхронні запити EF Core, `Include` для зв'язків, `AsNoTracking` для читання

\- Кешування списку курсів у ViewModel

\- Хешування паролів BCrypt



\---



\## Тестування



Виконано:

\- Адмін-панель у браузері: логін адміністратора, CRUD для всіх таблиць

\- Swagger UI: `GET /api/courses`, `POST /api/proposals`

\- Postman: логін, авторизовані запити з Bearer-токеном

\- MAUI на Android-емуляторі: логін, перегляд курсів, створення пропозиції

\- Перевірка Foreign Keys у БД



Скріншоти: `Docs/Screenshots/`



\---



\## Документація



\- `Database/init.sql`: схема БД, індекси, тестові дані

\- `Docs/swagger.json`: експорт Swagger-документації API



\---



\## Ліцензія



Навчальний проєкт.

