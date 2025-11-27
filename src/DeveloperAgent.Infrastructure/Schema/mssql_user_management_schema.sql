-- MSSQL schema for User Management and Audit Trails
-- Tables: Users, Roles, UserRoles, RefreshTokens, AuditTrails

CREATE TABLE [Users] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Username] NVARCHAR(100) NOT NULL UNIQUE,
    [Email] NVARCHAR(256) NOT NULL UNIQUE,
    [PasswordHash] NVARCHAR(MAX) NOT NULL,
    [DisplayName] NVARCHAR(200) NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedAtUtc] DATETIME2(3) NOT NULL DEFAULT SYSUTCDATETIME(),
    [UpdatedAtUtc] DATETIME2(3) NULL,
    [LastLoginAtUtc] DATETIME2(3) NULL
);

CREATE TABLE [Roles] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL UNIQUE,
    [Description] NVARCHAR(512) NULL,
    [CreatedAtUtc] DATETIME2(3) NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE TABLE [UserRoles] (
    [UserId] INT NOT NULL,
    [RoleId] INT NOT NULL,
    CONSTRAINT PK_UserRoles PRIMARY KEY (UserId, RoleId),
    CONSTRAINT FK_UserRoles_User FOREIGN KEY (UserId) REFERENCES [Users](Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserRoles_Role FOREIGN KEY (RoleId) REFERENCES [Roles](Id) ON DELETE CASCADE
);

CREATE TABLE [RefreshTokens] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserId] INT NOT NULL,
    [Token] NVARCHAR(500) NOT NULL,
    [ExpiresAtUtc] DATETIME2(3) NOT NULL,
    [RevokedAtUtc] DATETIME2(3) NULL,
    [CreatedAtUtc] DATETIME2(3) NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_RefreshTokens_User FOREIGN KEY (UserId) REFERENCES [Users](Id) ON DELETE CASCADE
);

CREATE TABLE [AuditTrails] (
    [Id] BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [EntityType] NVARCHAR(200) NOT NULL,
    [EntityId] NVARCHAR(200) NULL,
    [Action] NVARCHAR(50) NOT NULL, -- e.g., CREATE, UPDATE, DELETE, LOGIN, LOGOUT
    [ActorUserId] INT NULL,
    [TimestampUtc] DATETIME2(3) NOT NULL DEFAULT SYSUTCDATETIME(),
    [Changes] NVARCHAR(MAX) NULL, -- JSON payload describing the changes
    [IpAddress] NVARCHAR(45) NULL,
    CONSTRAINT FK_AuditTrails_ActorUser FOREIGN KEY (ActorUserId) REFERENCES [Users](Id) ON DELETE SET NULL
);

-- Indexes for common queries
CREATE INDEX IDX_Users_Username ON [Users](Username);
CREATE INDEX IDX_Users_Email ON [Users](Email);
CREATE INDEX IDX_AuditTrails_Entity ON [AuditTrails](EntityType, EntityId);

-- Example: seed an admin role
INSERT INTO [Roles] ([Name], [Description]) VALUES ('Administrator', 'System administrator with full privileges');
