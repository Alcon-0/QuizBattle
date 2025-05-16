USE master;
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'QuizApp')
BEGIN
    CREATE DATABASE QuizApp;
    PRINT 'Database QuizApp created.';
END
ELSE
BEGIN
    PRINT 'Database QuizApp already exists.';
END
GO

USE QuizApp;
GO