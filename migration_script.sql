IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
ALTER TABLE [ProjectTasks] DROP CONSTRAINT [FK_ProjectTasks_Projects_ProjectId];

ALTER TABLE [WorkSheets] DROP CONSTRAINT [FK_WorkSheets_ProjectTasks_ProjectTaskId];

ALTER TABLE [WorkSheets] DROP CONSTRAINT [FK_WorkSheets_Projects_ProjectId];

ALTER TABLE [ProjectTasks] ADD CONSTRAINT [FK_ProjectTasks_Projects_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [Projects] ([Id]);

ALTER TABLE [WorkSheets] ADD CONSTRAINT [FK_WorkSheets_ProjectTasks_ProjectTaskId] FOREIGN KEY ([ProjectTaskId]) REFERENCES [ProjectTasks] ([Id]);

ALTER TABLE [WorkSheets] ADD CONSTRAINT [FK_WorkSheets_Projects_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [Projects] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250720145914_InitialCreate', N'9.0.7');

COMMIT;
GO

