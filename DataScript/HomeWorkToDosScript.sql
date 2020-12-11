USE [master]
GO
/****** Object:  Database [HomeWorkToDos]    Script Date: 2020-12-11 14:45:57 ******/
CREATE DATABASE [HomeWorkToDos]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'HomeWorkToDos', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\HomeWorkToDos.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'HomeWorkToDos_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\HomeWorkToDos_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [HomeWorkToDos] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HomeWorkToDos].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HomeWorkToDos] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HomeWorkToDos] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HomeWorkToDos] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HomeWorkToDos] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HomeWorkToDos] SET ARITHABORT OFF 
GO
ALTER DATABASE [HomeWorkToDos] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [HomeWorkToDos] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HomeWorkToDos] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HomeWorkToDos] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HomeWorkToDos] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HomeWorkToDos] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HomeWorkToDos] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HomeWorkToDos] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HomeWorkToDos] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HomeWorkToDos] SET  DISABLE_BROKER 
GO
ALTER DATABASE [HomeWorkToDos] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HomeWorkToDos] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HomeWorkToDos] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HomeWorkToDos] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [HomeWorkToDos] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HomeWorkToDos] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [HomeWorkToDos] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HomeWorkToDos] SET RECOVERY FULL 
GO
ALTER DATABASE [HomeWorkToDos] SET  MULTI_USER 
GO
ALTER DATABASE [HomeWorkToDos] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [HomeWorkToDos] SET DB_CHAINING OFF 
GO
ALTER DATABASE [HomeWorkToDos] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [HomeWorkToDos] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [HomeWorkToDos] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'HomeWorkToDos', N'ON'
GO
ALTER DATABASE [HomeWorkToDos] SET QUERY_STORE = OFF
GO
USE [HomeWorkToDos]
GO
/****** Object:  Table [dbo].[Label]    Script Date: 2020-12-11 14:45:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Label](
	[LabelId] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[UserId] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_Label] PRIMARY KEY CLUSTERED 
(
	[LabelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ToDoItem]    Script Date: 2020-12-11 14:45:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ToDoItem](
	[ToDoItemId] [int] IDENTITY(1,1) NOT NULL,
	[ToDoListId] [int] NULL,
	[LabelId] [int] NULL,
	[Description] [nvarchar](100) NULL,
	[IsActive] [bit] NOT NULL,
	[UserId] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_ToDoItem] PRIMARY KEY CLUSTERED 
(
	[ToDoItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ToDoList]    Script Date: 2020-12-11 14:45:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ToDoList](
	[ToDoListId] [int] IDENTITY(1,1) NOT NULL,
	[LabelId] [int] NULL,
	[Description] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[UserId] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedOn] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_ToDoList] PRIMARY KEY CLUSTERED 
(
	[ToDoListId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 2020-12-11 14:45:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Contact] [nvarchar](10) NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedOn] [datetime] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [Contact], [UserName], [Password], [CreatedOn], [ModifiedOn]) VALUES (1, N'Jyotsana', N'Goyal', N'jyotsana.goyal@nagarro.com', N'7777777777', N'jyotsana', N'123', CAST(N'2020-12-06T14:41:46.080' AS DateTime), CAST(N'2020-12-06T14:41:46.080' AS DateTime))
SET IDENTITY_INSERT [dbo].[User] OFF
ALTER TABLE [dbo].[Label] ADD  CONSTRAINT [DF_Label_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Label] ADD  CONSTRAINT [DF_Label_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Label] ADD  CONSTRAINT [DF_Label_ModifiedOn]  DEFAULT (getdate()) FOR [ModifiedOn]
GO
ALTER TABLE [dbo].[ToDoItem] ADD  CONSTRAINT [DF_ToDoItem_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ToDoItem] ADD  CONSTRAINT [DF_ToDoItem_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[ToDoItem] ADD  CONSTRAINT [DF_ToDoItem_ModifiedOn]  DEFAULT (getdate()) FOR [ModifiedOn]
GO
ALTER TABLE [dbo].[ToDoList] ADD  CONSTRAINT [DF_ToDoList_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ToDoList] ADD  CONSTRAINT [DF_ToDoList_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[ToDoList] ADD  CONSTRAINT [DF_ToDoList_ModifiedOn]  DEFAULT (getdate()) FOR [ModifiedOn]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_ModifiedOn]  DEFAULT (getdate()) FOR [ModifiedOn]
GO
ALTER TABLE [dbo].[Label]  WITH CHECK ADD  CONSTRAINT [FK_Label_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Label] CHECK CONSTRAINT [FK_Label_User]
GO
ALTER TABLE [dbo].[ToDoItem]  WITH CHECK ADD  CONSTRAINT [FK_ToDoItem_Label] FOREIGN KEY([LabelId])
REFERENCES [dbo].[Label] ([LabelId])
GO
ALTER TABLE [dbo].[ToDoItem] CHECK CONSTRAINT [FK_ToDoItem_Label]
GO
ALTER TABLE [dbo].[ToDoItem]  WITH CHECK ADD  CONSTRAINT [FK_ToDoItem_ToDoList] FOREIGN KEY([ToDoListId])
REFERENCES [dbo].[ToDoList] ([ToDoListId])
GO
ALTER TABLE [dbo].[ToDoItem] CHECK CONSTRAINT [FK_ToDoItem_ToDoList]
GO
ALTER TABLE [dbo].[ToDoItem]  WITH CHECK ADD  CONSTRAINT [FK_ToDoItem_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[ToDoItem] CHECK CONSTRAINT [FK_ToDoItem_User]
GO
ALTER TABLE [dbo].[ToDoList]  WITH CHECK ADD  CONSTRAINT [FK_ToDoList_Label] FOREIGN KEY([LabelId])
REFERENCES [dbo].[Label] ([LabelId])
GO
ALTER TABLE [dbo].[ToDoList] CHECK CONSTRAINT [FK_ToDoList_Label]
GO
ALTER TABLE [dbo].[ToDoList]  WITH CHECK ADD  CONSTRAINT [FK_ToDoList_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[ToDoList] CHECK CONSTRAINT [FK_ToDoList_User]
GO
USE [master]
GO
ALTER DATABASE [HomeWorkToDos] SET  READ_WRITE 
GO
