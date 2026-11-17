-- Освітня система університету. СУБД: SQLite
PRAGMA foreign_keys = ON;

DROP TABLE IF EXISTS Proposals;
DROP TABLE IF EXISTS Courses;
DROP TABLE IF EXISTS Teachers;

CREATE TABLE Teachers (
    Id           INTEGER PRIMARY KEY AUTOINCREMENT,
    Name         TEXT NOT NULL,
    Email        TEXT NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL
);

CREATE TABLE Courses (
    Id          INTEGER PRIMARY KEY AUTOINCREMENT,
    Name        TEXT NOT NULL,
    Description TEXT NOT NULL DEFAULT '',
    Semester    TEXT NOT NULL,
    TeacherId   INTEGER NOT NULL,
    FOREIGN KEY (TeacherId) REFERENCES Teachers(Id) ON DELETE CASCADE
);

CREATE TABLE Proposals (
    Id           INTEGER PRIMARY KEY AUTOINCREMENT,
    TeacherId    INTEGER NOT NULL,
    CourseId     INTEGER NULL,
    CourseName   TEXT NOT NULL,
    ProposalType TEXT NOT NULL CHECK (ProposalType IN ('Create','Delete')),
    Status       TEXT NOT NULL DEFAULT 'Pending'
                 CHECK (Status IN ('Pending','Approved','Rejected')),
    FOREIGN KEY (TeacherId) REFERENCES Teachers(Id) ON DELETE CASCADE,
    FOREIGN KEY (CourseId)  REFERENCES Courses(Id)  ON DELETE SET NULL
);

-- Індекси (Завдання 11)
CREATE INDEX IX_Courses_TeacherId   ON Courses(TeacherId);
CREATE INDEX IX_Proposals_TeacherId ON Proposals(TeacherId);
CREATE INDEX IX_Proposals_CourseId  ON Proposals(CourseId);
-- Унікальний індекс по Email створюється через UNIQUE у Teachers

-- Тестові дані.
-- ПРИМІТКА: 'REPLACE_WITH_BCRYPT' - заглушка. Застосунок при старті замінює її
-- на справжній BCrypt-хеш пароля Teacher123! (див. DbSeeder у завданні 3).
INSERT INTO Teachers (Name, Email, PasswordHash) VALUES
 ('Іван Петренко', 'ivan@univ.edu',  'REPLACE_WITH_BCRYPT'),
 ('Олена Коваль',  'olena@univ.edu', 'REPLACE_WITH_BCRYPT');

INSERT INTO Courses (Name, Description, Semester, TeacherId) VALUES
 ('Бази даних',         'Реляційні БД та SQL',         '2026-Autumn', 1),
 ('Алгоритми',          'Структури даних і алгоритми', '2026-Autumn', 1),
 ('Вебтехнології',      'HTML, CSS, ASP.NET Core',     '2026-Autumn', 2);

INSERT INTO Proposals (TeacherId, CourseId, CourseName, ProposalType, Status) VALUES
 (1, NULL, 'Машинне навчання', 'Create', 'Pending'),
 (2, 3,    'Вебтехнології',    'Delete', 'Pending');