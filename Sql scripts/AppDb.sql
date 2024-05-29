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
GO

CREATE TABLE [Arplogs] (
    [Id] int NOT NULL IDENTITY,
    [ArplogTimestamp] datetime2 NOT NULL,
    [Flag] nvarchar(3) NOT NULL,
    [Address] nvarchar(max) NOT NULL,
    [MacAddress] nvarchar(18) NULL,
    [Interface] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Arplogs] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Connections] (
    [Id] int NOT NULL IDENTITY,
    [ConnectionTimestamp] datetime2 NOT NULL,
    [Flag] nvarchar(6) NULL,
    [NatFlag] nvarchar(1) NULL,
    [Protocol] nvarchar(max) NOT NULL,
    [SRCAddress] nvarchar(max) NOT NULL,
    [SRCPort] nvarchar(max) NOT NULL,
    [DSTAddress] nvarchar(max) NOT NULL,
    [DSTPort] nvarchar(max) NOT NULL,
    [TCPState] nvarchar(max) NULL,
    CONSTRAINT [PK_Connections] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [FileHashes] (
    [Id] int NOT NULL IDENTITY,
    [FileName] nvarchar(max) NOT NULL,
    [Hash] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_FileHashes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Rules] (
    [Id] int NOT NULL IDENTITY,
    [Contenuto] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Rules] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Violations] (
    [Id] int NOT NULL IDENTITY,
    [ConnectionId] int NOT NULL,
    [RuleId] int NOT NULL,
    CONSTRAINT [PK_Violations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Violations_Connections_ConnectionId] FOREIGN KEY ([ConnectionId]) REFERENCES [Connections] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Violations_Rules_RuleId] FOREIGN KEY ([RuleId]) REFERENCES [Rules] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Violations_ConnectionId] ON [Violations] ([ConnectionId]);
GO

CREATE INDEX [IX_Violations_RuleId] ON [Violations] ([RuleId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240404080842_Initial-Create', N'8.0.3');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Rules] ADD [Tipo] nvarchar(max) NOT NULL DEFAULT N'';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240404201233_Update-Create', N'8.0.3');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Rules] ADD [Descrizione] nvarchar(max) NOT NULL DEFAULT N'';
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240404202822_Update-Database2', N'8.0.3');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [ArplogViolation] (
    [Id] int NOT NULL IDENTITY,
    [ArplogId] int NOT NULL,
    [RuleId] int NOT NULL,
    CONSTRAINT [PK_ArplogViolation] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ArplogViolation_Arplogs_ArplogId] FOREIGN KEY ([ArplogId]) REFERENCES [Arplogs] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ArplogViolation_Rules_RuleId] FOREIGN KEY ([RuleId]) REFERENCES [Rules] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_ArplogViolation_ArplogId] ON [ArplogViolation] ([ArplogId]);
GO

CREATE INDEX [IX_ArplogViolation_RuleId] ON [ArplogViolation] ([RuleId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240408104114_UpdateArplog', N'8.0.3');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240408161225_FixArplog', N'8.0.3');
GO

COMMIT;
GO


