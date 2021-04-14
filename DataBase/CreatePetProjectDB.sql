USE PetProjectDB;

CREATE TABLE [dbo].[Users] (
    [Id]					NVARCHAR (450)     NOT NULL,
    [UserName]				NVARCHAR (256)     NULL,
    [NormalizedUserName]	NVARCHAR (256)     NULL,
    [Email]					NVARCHAR (256)     NULL,
    [NormalizedEmail]		NVARCHAR (256)     NULL,
    [EmailConfirmed]		BIT                NOT NULL,
    [PasswordHash]			NVARCHAR (MAX)     NULL,
    [SecurityStamp]			NVARCHAR (MAX)     NULL,
    [ConcurrencyStamp]		NVARCHAR (MAX)     NULL,
    [PhoneNumber]			NVARCHAR (MAX)     NULL,
    [PhoneNumberConfirmed]	BIT                NOT NULL,
    [TwoFactorEnabled]		BIT                NOT NULL,
    [LockoutEnd]			DATETIMEOFFSET (7) NULL,
    [LockoutEnabled]		BIT                NOT NULL,
    [AccessFailedCount]		INT                NOT NULL,
	[FirstName]				NVARCHAR(256)	NULL,
	[SecondName]			NVARCHAR(256)	NULL,
	[RegistrationDate]		DATETIME		NULL
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex]
	ON [dbo].[Users]([NormalizedUserName] ASC) WHERE ([NormalizedUserName]) IS NOT NULL;

GO
CREATE NONCLUSTERED INDEX [EmailIndex]
	ON [dbo].[Users]([NormalizedEmail] ASC);


CREATE TABLE [dbo].[UserClaims] (
	[Id]			INT				IDENTITY (1, 1) NOT NULL,
	[UserId]		NVARCHAR(450)	NOT NULL,
	[ClaimType]		NVARCHAR(MAX)	NULL,
	[ClaimValues]	NVARCHAR(MAX)	NULL,
	CONSTRAINT [PK_UserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_UserClaims_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);

GO
CREATE NONCLUSTERED INDEX [IX_UserClaims_UserId]
	ON [dbo].[UserClaims]([UserId] ASC);


CREATE TABLE [dbo].[Roles] (
	[Id]				NVARCHAR(450)	NOT NULL,
	[Name]				NVARCHAR(256)	NULL,
	[NormalizedName]	NVARCHAR(256)	NULL,
	[ConcurrencyStamp]	NVARCHAR(MAX)	NULL,
	CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex]
	ON [dbo].[Roles]([NormalizedName] ASC) WHERE ([NormalizedName] IS NOT NULL);


CREATE TABLE [dbo].[RoleClaims] (
	[Id]			INT				IDENTITY (1, 1) NOT NULL,
	[RoleId]		NVARCHAR(450)	NOT NULL,
	[ClaimType]		NVARCHAR(MAX)	NULL,
	[ClaimValues]	NVARCHAR(MAX)	NULL,
	CONSTRAINT [PK_RoleClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_RoleClaims_Roles_RoleId] FOREIGN KEY (RoleId) REFERENCES [dbo].[Roles] ([Id]) ON DELETE CASCADE
);

GO
CREATE NONCLUSTERED INDEX [IX_RoleClaims_RoleId]
	ON [dbo].[RoleClaims]([RoleId]);


CREATE TABLE [dbo].[UserRoles] (
	[UserId]	NVARCHAR(450) NOT NULL,
	[RoleId]	NVARCHAR(450) NOT NULL,
	CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
	CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE,
);

GO
CREATE NONCLUSTERED INDEX [IX_UserRoles_RoleId]
	ON [dbo].[UserRoles]([RoleId] ASC)


CREATE TABLE [dbo].[UserLogins] (
	[LoginProvider]			NVARCHAR(128) NOT NULL,
	[ProviderKey]			NVARCHAR(128) NOT NULL,
	[ProviderDisplayName]	NVARCHAR(MAX) NULL,
	[UserId]				NVARCHAR(450) NOT NULL,
	CONSTRAINT [PK_UserLogins]	PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC),
	CONSTRAINT [FK_UserLogins_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);

GO
CREATE NONCLUSTERED INDEX [IX_UserLogins_UserId]
	ON [dbo].[UserLogins]([UserId] ASC);


CREATE TABLE [dbo].[UserTokens] (
	[UserId]		NVARCHAR(450) NOT NULL,
	[LoginProvider]	NVARCHAR(128) NOT NULL,
	[Name]			NVARCHAR(128) NOT NULL,
	[Value]			NVARCHAR(MAX) NOT NULL,
	CONSTRAINT [PK_UserTokens] PRIMARY KEY CLUSTERED ([UserId] ASC, [LoginProvider] ASC, [Name] ASC),
	CONSTRAINT [FK_UserTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);