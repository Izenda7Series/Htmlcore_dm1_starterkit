USE [master]
GO
/****** Object:  Database [HTMLCore]    Script Date: 5/6/2020 14:00:00 PM ******/
CREATE DATABASE [HTMLCore];

GO
ALTER DATABASE [HTMLCore] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HTMLCore].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HTMLCore] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HTMLCore] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HTMLCore] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HTMLCore] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HTMLCore] SET ARITHABORT OFF 
GO
ALTER DATABASE [HTMLCore] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [HTMLCore] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HTMLCore] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HTMLCore] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HTMLCore] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HTMLCore] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HTMLCore] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HTMLCore] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HTMLCore] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HTMLCore] SET  DISABLE_BROKER 
GO
ALTER DATABASE [HTMLCore] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HTMLCore] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HTMLCore] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HTMLCore] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [HTMLCore] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HTMLCore] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [HTMLCore] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HTMLCore] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [HTMLCore] SET  MULTI_USER 
GO
ALTER DATABASE [HTMLCore] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [HTMLCore] SET DB_CHAINING OFF 
GO
ALTER DATABASE [HTMLCore] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [HTMLCore] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [HTMLCore] SET DELAYED_DURABILITY = DISABLED 
GO
USE [HTMLCore]
GO

/****** Object:  Table [dbo].[ApplicationUsers]    Script Date: 5/6/2020 14:00:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[Tenant_Id] [int] NULL,
 CONSTRAINT [PK_dbo.ApplicationUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Tenants]    Script Date: 5/6/2020 14:00:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Tenants](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Tenants] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

INSERT [dbo].[ApplicationUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Tenant_Id]) VALUES (N'0923f6f7-3e43-486b-a3fd-43a3072ef427', N'manager@retcl.com', 0, N'kunc1dS+RxSFGZbi5af3gw==', N'7089bb8c-2624-495a-bb96-a3f3e451a538', NULL, 0, 0, NULL, 1, 0, N'manager@retcl.com', 3)
INSERT [dbo].[ApplicationUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Tenant_Id]) VALUES (N'0ecc32e9-e852-42ca-bfb5-78641bde4fd7', N'employee@retcl.com', 0, N'kunc1dS+RxSFGZbi5af3gw==', N'df3cbc75-cafc-4abb-93d2-c443b6e5a6b8', NULL, 0, 0, NULL, 1, 0, N'employee@retcl.com', 3)
INSERT [dbo].[ApplicationUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Tenant_Id]) VALUES (N'39bf0d97-1833-4b80-84dc-8c2293df3f97', N'manager@natwr.com', 0, N'kunc1dS+RxSFGZbi5af3gw==', N'20a2d154-b7f8-4159-8503-23cca683fbbb', NULL, 0, 0, NULL, 1, 0, N'manager@natwr.com', 2)
INSERT [dbo].[ApplicationUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Tenant_Id]) VALUES (N'3b4f2455-60df-4820-8554-a1444e2be657', N'employee@natwr.com', 0, N'kunc1dS+RxSFGZbi5af3gw==', N'0b7c3dd7-c380-44f8-835c-37005130c67f', NULL, 0, 0, NULL, 1, 0, N'employee@natwr.com', 2)
INSERT [dbo].[ApplicationUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Tenant_Id]) VALUES (N'4d543ed3-b644-4c62-8bb1-3ef95b07f81a', N'test@test.com', 0, N'kunc1dS+RxSFGZbi5af3gw==', N'a162b531-c757-4fdc-9bb8-dc24632b7a1f', NULL, 0, 0, NULL, 1, 0, N'test@test.com', 4)
INSERT [dbo].[ApplicationUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Tenant_Id]) VALUES (N'53389f0d-a2a0-4d69-80e3-989d8b79f337', N'vp@deldg.com', 0, N'kunc1dS+RxSFGZbi5af3gw==', N'f93620e4-b5d8-4f1a-b8a8-6bcbed3e517f', NULL, 0, 0, NULL, 1, 0, N'vp@deldg.com', 1)
INSERT [dbo].[ApplicationUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Tenant_Id]) VALUES (N'54f61138-05b0-4afa-9966-cc8f61c95516', N'employee@deldg.com', 0, N'kunc1dS+RxSFGZbi5af3gw==', N'0c2865da-36ad-49fd-a999-5a5ed9b62e7a', NULL, 0, 0, NULL, 1, 0, N'employee@deldg.com', 1)
INSERT [dbo].[ApplicationUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Tenant_Id]) VALUES (N'55218e81-fbfa-4af6-8cb9-dfc3943b3dd0', N'IzendaAdmin@system.com', 0, N'kunc1dS+RxSFGZbi5af3gw==', N'c45d0f3b-cb5c-499c-996d-c7c801a564c9', NULL, 0, 0, NULL, 1, 0, N'IzendaAdmin@system.com', NULL)
INSERT [dbo].[ApplicationUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Tenant_Id]) VALUES (N'6c8e807d-1d50-43e7-9101-58e64a6016d7', N'manager@deldg.com', 0, N'kunc1dS+RxSFGZbi5af3gw==', N'9cf892dc-2d2a-4022-b75c-1de7d63d5d04', NULL, 0, 0, NULL, 1, 0, N'manager@deldg.com', 1)
INSERT [dbo].[ApplicationUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Tenant_Id]) VALUES (N'989dc685-24c9-48c4-83b2-8c6eb41583e2', N'VP@natwr.com', 0, N'kunc1dS+RxSFGZbi5af3gw==', N'10f4a7f0-7b7f-4286-b516-7a17d836a26a', NULL, 0, 0, NULL, 1, 0, N'VP@natwr.com', 2)
INSERT [dbo].[ApplicationUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [Tenant_Id]) VALUES (N'fe0bc489-10d5-44d2-97c4-a7b24fae0cf2', N'vp@retcl.com', 0, N'kunc1dS+RxSFGZbi5af3gw==', N'0d9be60f-5ae5-47f1-9f91-a5d7663415d9', NULL, 0, 0, NULL, 1, 0, N'vp@retcl.com', 3)
SET IDENTITY_INSERT [dbo].[Tenants] ON 

INSERT [dbo].[Tenants] ([Id], [Name]) VALUES (1, N'DELDG')
INSERT [dbo].[Tenants] ([Id], [Name]) VALUES (2, N'NATWR')
INSERT [dbo].[Tenants] ([Id], [Name]) VALUES (3, N'RETCL')
SET IDENTITY_INSERT [dbo].[Tenants] OFF
SET ANSI_PADDING ON

GO

/****** Object:  Index [IX_Tenant_Id]    Script Date: 5/6/2020 14:00:00 PM ******/
CREATE NONCLUSTERED INDEX [IX_Tenant_Id] ON [dbo].[ApplicationUsers]
(
	[Tenant_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [UserNameIndex]    Script Date: 5/6/2020 14:00:00 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[ApplicationUsers]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ApplicationUsers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUsers_dbo.Tenants_Tenant_Id] FOREIGN KEY([Tenant_Id])
REFERENCES [dbo].[Tenants] ([Id])
GO

ALTER TABLE [dbo].[ApplicationUsers] CHECK CONSTRAINT [FK_dbo.AspNetUsers_dbo.Tenants_Tenant_Id]
GO

USE [master]
GO

ALTER DATABASE [HTMLCore] SET  READ_WRITE 
GO
