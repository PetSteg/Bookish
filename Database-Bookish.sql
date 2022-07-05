USE [master]
GO
/****** Object:  Database [Bookish]    Script Date: 7/5/2022 11:40:28 AM ******/
CREATE DATABASE [Bookish]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Bookish', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.PELICAN\MSSQL\DATA\Bookish.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Bookish_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.PELICAN\MSSQL\DATA\Bookish_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Bookish] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Bookish].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Bookish] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Bookish] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Bookish] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Bookish] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Bookish] SET ARITHABORT OFF 
GO
ALTER DATABASE [Bookish] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Bookish] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Bookish] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Bookish] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Bookish] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Bookish] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Bookish] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Bookish] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Bookish] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Bookish] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Bookish] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Bookish] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Bookish] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Bookish] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Bookish] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Bookish] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Bookish] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Bookish] SET RECOVERY FULL 
GO
ALTER DATABASE [Bookish] SET  MULTI_USER 
GO
ALTER DATABASE [Bookish] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Bookish] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Bookish] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Bookish] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Bookish] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Bookish] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Bookish', N'ON'
GO
ALTER DATABASE [Bookish] SET QUERY_STORE = OFF
GO
USE [Bookish]
GO
/****** Object:  Table [dbo].[Author]    Script Date: 7/5/2022 11:40:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Author](
	[Id_author] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Author] PRIMARY KEY CLUSTERED 
(
	[Id_author] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Book]    Script Date: 7/5/2022 11:40:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Book](
	[ISBN] [nchar](13) NOT NULL,
	[Title] [varchar](50) NOT NULL,
	[Category] [varchar](50) NULL,
	[Publish_date] [date] NULL,
	[Subtitle] [varchar](50) NULL,
	[Cover_photo_url] [varchar](100) NULL,
	[Available_copies] [int] NOT NULL,
 CONSTRAINT [PK_Book] PRIMARY KEY CLUSTERED 
(
	[ISBN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Borrow]    Script Date: 7/5/2022 11:40:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Borrow](
	[Id_book] [nchar](13) NOT NULL,
	[Id_user] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contributions]    Script Date: 7/5/2022 11:40:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contributions](
	[Id_book] [nchar](13) NOT NULL,
	[Id_author] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 7/5/2022 11:40:28 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id_user] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Password] [varchar](256) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id_user] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Author] ([Id_author], [Name]) VALUES (100, N'J.K. Rowling')
INSERT [dbo].[Author] ([Id_author], [Name]) VALUES (101, N'Haruki Murakami')
GO
INSERT [dbo].[Book] ([ISBN], [Title], [Category], [Publish_date], [Subtitle], [Cover_photo_url], [Available_copies]) VALUES (N'9780307476463', N'1Q84', N'Magic Realism', CAST(N'2009-05-29' AS Date), NULL, NULL, 10)
INSERT [dbo].[Book] ([ISBN], [Title], [Category], [Publish_date], [Subtitle], [Cover_photo_url], [Available_copies]) VALUES (N'9780747532743', N'Harry Potter And The Philosopher''s Stone', N'Teen', CAST(N'1997-06-26' AS Date), NULL, N'', 30)
GO
INSERT [dbo].[Borrow] ([Id_book], [Id_user]) VALUES (N'9780307476463', 200)
INSERT [dbo].[Borrow] ([Id_book], [Id_user]) VALUES (N'9780747532743', 201)
GO
INSERT [dbo].[Contributions] ([Id_book], [Id_author]) VALUES (N'9780747532743', 100)
INSERT [dbo].[Contributions] ([Id_book], [Id_author]) VALUES (N'9780307476463', 101)
GO
INSERT [dbo].[User] ([Id_user], [Name], [Email], [Password]) VALUES (200, N'Stefan Matei', N'Stefan.Matei@softwire.com', N'12345')
INSERT [dbo].[User] ([Id_user], [Name], [Email], [Password]) VALUES (201, N'Pieter', N'Petre-Florin.Stegru@softwire.com', N'123123')
GO
ALTER TABLE [dbo].[Borrow]  WITH CHECK ADD  CONSTRAINT [FK_Borrow_Book] FOREIGN KEY([Id_book])
REFERENCES [dbo].[Book] ([ISBN])
GO
ALTER TABLE [dbo].[Borrow] CHECK CONSTRAINT [FK_Borrow_Book]
GO
ALTER TABLE [dbo].[Borrow]  WITH CHECK ADD  CONSTRAINT [FK_Borrow_User] FOREIGN KEY([Id_user])
REFERENCES [dbo].[User] ([Id_user])
GO
ALTER TABLE [dbo].[Borrow] CHECK CONSTRAINT [FK_Borrow_User]
GO
ALTER TABLE [dbo].[Contributions]  WITH CHECK ADD  CONSTRAINT [FK_Contributions_Author] FOREIGN KEY([Id_author])
REFERENCES [dbo].[Author] ([Id_author])
GO
ALTER TABLE [dbo].[Contributions] CHECK CONSTRAINT [FK_Contributions_Author]
GO
ALTER TABLE [dbo].[Contributions]  WITH CHECK ADD  CONSTRAINT [FK_Contributions_Book] FOREIGN KEY([Id_book])
REFERENCES [dbo].[Book] ([ISBN])
GO
ALTER TABLE [dbo].[Contributions] CHECK CONSTRAINT [FK_Contributions_Book]
GO
USE [master]
GO
ALTER DATABASE [Bookish] SET  READ_WRITE 
GO
