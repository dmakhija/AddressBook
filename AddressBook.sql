USE [master]
GO
/****** Object:  Database [AddressBook]    Script Date: 12/5/2015 11:00:13 PM ******/
CREATE DATABASE [AddressBook]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AddressBook', FILENAME = N'C:\Databases\AddressBook.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'AddressBook_log', FILENAME = N'C:\Databases\AddressBook_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [AddressBook] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AddressBook].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AddressBook] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AddressBook] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AddressBook] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AddressBook] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AddressBook] SET ARITHABORT OFF 
GO
ALTER DATABASE [AddressBook] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [AddressBook] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [AddressBook] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AddressBook] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AddressBook] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AddressBook] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AddressBook] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AddressBook] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AddressBook] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AddressBook] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AddressBook] SET  DISABLE_BROKER 
GO
ALTER DATABASE [AddressBook] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AddressBook] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AddressBook] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AddressBook] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AddressBook] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AddressBook] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AddressBook] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AddressBook] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [AddressBook] SET  MULTI_USER 
GO
ALTER DATABASE [AddressBook] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AddressBook] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AddressBook] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AddressBook] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [AddressBook]
GO
/****** Object:  Table [dbo].[Contacts]    Script Date: 12/5/2015 11:00:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contacts](
	[ContactId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NULL,
	[Phone] [nvarchar](15) NOT NULL,
	[Email] [nvarchar](50) NULL,
	[Street] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[Province] [nvarchar](50) NULL,
	[Postal] [nvarchar](6) NULL,
	[Country] [nvarchar](50) NULL,
	[Notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
(
	[ContactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Roles]    Script Date: 12/5/2015 11:00:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 12/5/2015 11:00:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UsersContacts]    Script Date: 12/5/2015 11:00:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersContacts](
	[UserId] [int] NOT NULL,
	[ContactId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[ContactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UsersRoles]    Script Date: 12/5/2015 11:00:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Contacts] ON 

INSERT [dbo].[Contacts] ([ContactId], [FirstName], [LastName], [Phone], [Email], [Street], [City], [Province], [Postal], [Country], [Notes]) VALUES (1, N'Luna', N'Mnnn', N'5197819999', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Contacts] ([ContactId], [FirstName], [LastName], [Phone], [Email], [Street], [City], [Province], [Postal], [Country], [Notes]) VALUES (2, N'Anya', N'Patel', N'5197814444', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Contacts] ([ContactId], [FirstName], [LastName], [Phone], [Email], [Street], [City], [Province], [Postal], [Country], [Notes]) VALUES (3, N'Rose', N'Sh', N'2267814444', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Contacts] ([ContactId], [FirstName], [LastName], [Phone], [Email], [Street], [City], [Province], [Postal], [Country], [Notes]) VALUES (4, N'Heta', N'Shah', N'987562333', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Contacts] ([ContactId], [FirstName], [LastName], [Phone], [Email], [Street], [City], [Province], [Postal], [Country], [Notes]) VALUES (5, N'bruce', N'dvorski', N'789789789', NULL, NULL, NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Contacts] OFF
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([RoleId], [RoleName]) VALUES (1, N'admin')
INSERT [dbo].[Roles] ([RoleId], [RoleName]) VALUES (2, N'user')
SET IDENTITY_INSERT [dbo].[Roles] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [Email], [Password]) VALUES (1, N'admin', N'admin', N'admin@admin.com', N'admin123')
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [Email], [Password]) VALUES (2, N'Durga', N'makhija', N'durgamakhija14@gmail.com', N'durga123')
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [Email], [Password]) VALUES (3, N'Saby', N'R', N'sr@yahoo.com', N'saby123')
SET IDENTITY_INSERT [dbo].[Users] OFF
INSERT [dbo].[UsersContacts] ([UserId], [ContactId]) VALUES (2, 1)
INSERT [dbo].[UsersContacts] ([UserId], [ContactId]) VALUES (2, 2)
INSERT [dbo].[UsersRoles] ([UserId], [RoleId]) VALUES (1, 1)
INSERT [dbo].[UsersRoles] ([UserId], [RoleId]) VALUES (2, 2)
INSERT [dbo].[UsersRoles] ([UserId], [RoleId]) VALUES (3, 2)
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ_Contact]    Script Date: 12/5/2015 11:00:14 PM ******/
ALTER TABLE [dbo].[Contacts] ADD  CONSTRAINT [UQ_Contact] UNIQUE NONCLUSTERED 
(
	[FirstName] ASC,
	[LastName] ASC,
	[Phone] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Roles__8AFACE1B0F6A721C]    Script Date: 12/5/2015 11:00:14 PM ******/
ALTER TABLE [dbo].[Roles] ADD PRIMARY KEY NONCLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__Roles__8A2B6160B8A52039]    Script Date: 12/5/2015 11:00:14 PM ******/
ALTER TABLE [dbo].[Roles] ADD UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [PK__Users__1788CC4D210F2D4D]    Script Date: 12/5/2015 11:00:14 PM ******/
ALTER TABLE [dbo].[Users] ADD PRIMARY KEY NONCLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__Users__A9D1053407738735]    Script Date: 12/5/2015 11:00:14 PM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UsersContacts]  WITH CHECK ADD FOREIGN KEY([ContactId])
REFERENCES [dbo].[Contacts] ([ContactId])
GO
ALTER TABLE [dbo].[UsersContacts]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[UsersRoles]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[UsersRoles]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
USE [master]
GO
ALTER DATABASE [AddressBook] SET  READ_WRITE 
GO
