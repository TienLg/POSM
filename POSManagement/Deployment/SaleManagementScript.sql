USE [master]
GO
/****** Object:  Database [SaleManagement]    Script Date: 8/27/2014 8:13:57 PM ******/
CREATE DATABASE [SaleManagement]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SaleManagement', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\SaleManagement.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'SaleManagement_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\SaleManagement_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [SaleManagement] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SaleManagement].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SaleManagement] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SaleManagement] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SaleManagement] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SaleManagement] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SaleManagement] SET ARITHABORT OFF 
GO
ALTER DATABASE [SaleManagement] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [SaleManagement] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SaleManagement] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SaleManagement] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SaleManagement] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SaleManagement] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SaleManagement] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SaleManagement] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SaleManagement] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SaleManagement] SET  DISABLE_BROKER 
GO
ALTER DATABASE [SaleManagement] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SaleManagement] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SaleManagement] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SaleManagement] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SaleManagement] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SaleManagement] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SaleManagement] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SaleManagement] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [SaleManagement] SET  MULTI_USER 
GO
ALTER DATABASE [SaleManagement] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SaleManagement] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SaleManagement] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SaleManagement] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [SaleManagement] SET DELAYED_DURABILITY = DISABLED 
GO
USE [SaleManagement]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 9/6/2014 6:59:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[cust_id] [int] IDENTITY(1,1) NOT NULL,
	[cust_name] [nchar](255) NOT NULL,
	[cust_address] [nchar](255) NULL,
	[cust_phone] [nchar](20) NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[cust_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ImportOrderItems]    Script Date: 9/6/2014 6:59:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImportOrderItems](
	[order_item_id] [int] IDENTITY(1,1) NOT NULL,
	[order_id] [int] NOT NULL,
	[prod_id] [nchar](10) NOT NULL,
	[base_price_by_unit] [decimal](18, 3) NOT NULL,
	[base_price_by_stock] [decimal](18, 3) NOT NULL,
	[quantity_by_unit] [int] NOT NULL,
	[quantity_by_stock] [int] NOT NULL,
	[quantity_control] [int] NOT NULL,
	[date_expired] [datetime2](7) NULL,
 CONSTRAINT [PK_ImportOrderItems_1] PRIMARY KEY CLUSTERED 
(
	[order_item_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ImportOrders]    Script Date: 9/6/2014 6:59:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImportOrders](
	[order_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[total_price] [decimal](18, 3) NOT NULL,
	[date_import] [datetime2](7) NOT NULL,
	[order_status] [nchar](50) NOT NULL,
 CONSTRAINT [PK_ImportOrders] PRIMARY KEY CLUSTERED 
(
	[order_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Products]    Script Date: 9/6/2014 6:59:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[prod_id] [nchar](10) NOT NULL,
	[prod_name] [nchar](255) NOT NULL,
	[quantity_control] [int] NOT NULL,
	[quantity_by_unit] [int] NOT NULL,
	[quantity_by_stock] [int] NOT NULL,
	[sale_price_by_unit] [decimal](18, 3) NOT NULL,
	[sale_price_by_stock] [decimal](18, 3) NOT NULL,
	[date_added] [datetime2](7) NOT NULL,
	[date_modified] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[prod_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SaleOrderItems]    Script Date: 9/6/2014 6:59:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SaleOrderItems](
	[order_item_id] [int] IDENTITY(1,1) NOT NULL,
	[order_id] [int] NOT NULL,
	[prod_id] [nchar](10) NOT NULL,
	[sale_price_by_unit] [decimal](18, 3) NOT NULL,
	[sale_price_by_stock] [decimal](18, 3) NOT NULL,
	[quantity_by_unit] [int] NOT NULL,
	[quantity_by_stock] [int] NOT NULL,
	[quantity_control] [int] NOT NULL,
 CONSTRAINT [PK_SaleOrderItems] PRIMARY KEY CLUSTERED 
(
	[order_item_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SaleOrders]    Script Date: 9/6/2014 6:59:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SaleOrders](
	[order_id] [int] IDENTITY(1,1) NOT NULL,
	[cust_id] [int] NOT NULL,
	[user_id] [int] NOT NULL,
	[total_price] [decimal](18, 3) NOT NULL,
	[taken_money] [decimal](18, 3) NOT NULL,
	[date_created] [datetime2](7) NOT NULL,
	[date_ship] [datetime2](7) NOT NULL,
	[order_status] [nchar](50) NOT NULL,
 CONSTRAINT [PK_SaleOrders] PRIMARY KEY CLUSTERED 
(
	[order_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Stocks]    Script Date: 9/6/2014 6:59:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stocks](
	[stock_id] [int] IDENTITY(1,1) NOT NULL,
	[prod_id] [nchar](10) NOT NULL,
	[base_price_by_unit] [decimal](18, 3) NOT NULL,
	[base_price_by_stock] [decimal](18, 3) NOT NULL,
	[quantity_by_unit] [int] NOT NULL,
	[quantity_by_stock] [int] NOT NULL,
	[quantity_control] [int] NOT NULL,
	[date_expired] [datetime2](7) NULL,
 CONSTRAINT [PK_Stocks] PRIMARY KEY CLUSTERED 
(
	[stock_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 9/6/2014 6:59:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[user_id] [int] NOT NULL,
	[user_name] [nchar](255) NOT NULL,
	[user_pwd] [nvarchar](max) NOT NULL,
	[user_role] [nchar](50) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[ImportOrderItems]  WITH CHECK ADD  CONSTRAINT [FK_ImportOrderItems_ImportOrders] FOREIGN KEY([order_id])
REFERENCES [dbo].[ImportOrders] ([order_id])
GO
ALTER TABLE [dbo].[ImportOrderItems] CHECK CONSTRAINT [FK_ImportOrderItems_ImportOrders]
GO
ALTER TABLE [dbo].[ImportOrderItems]  WITH CHECK ADD  CONSTRAINT [FK_ImportOrderItems_Products] FOREIGN KEY([prod_id])
REFERENCES [dbo].[Products] ([prod_id])
GO
ALTER TABLE [dbo].[ImportOrderItems] CHECK CONSTRAINT [FK_ImportOrderItems_Products]
GO
ALTER TABLE [dbo].[ImportOrders]  WITH CHECK ADD  CONSTRAINT [FK_ImportOrders_Users] FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[ImportOrders] CHECK CONSTRAINT [FK_ImportOrders_Users]
GO
ALTER TABLE [dbo].[SaleOrderItems]  WITH CHECK ADD  CONSTRAINT [FK_SaleOrderItems_Products] FOREIGN KEY([prod_id])
REFERENCES [dbo].[Products] ([prod_id])
GO
ALTER TABLE [dbo].[SaleOrderItems] CHECK CONSTRAINT [FK_SaleOrderItems_Products]
GO
ALTER TABLE [dbo].[SaleOrderItems]  WITH CHECK ADD  CONSTRAINT [FK_SaleOrderItems_SaleOrders] FOREIGN KEY([order_id])
REFERENCES [dbo].[SaleOrders] ([order_id])
GO
ALTER TABLE [dbo].[SaleOrderItems] CHECK CONSTRAINT [FK_SaleOrderItems_SaleOrders]
GO
ALTER TABLE [dbo].[SaleOrders]  WITH CHECK ADD  CONSTRAINT [FK_SaleOrders_Customers] FOREIGN KEY([cust_id])
REFERENCES [dbo].[Customers] ([cust_id])
GO
ALTER TABLE [dbo].[SaleOrders] CHECK CONSTRAINT [FK_SaleOrders_Customers]
GO
ALTER TABLE [dbo].[SaleOrders]  WITH CHECK ADD  CONSTRAINT [FK_SaleOrders_Users] FOREIGN KEY([user_id])
REFERENCES [dbo].[Users] ([user_id])
GO
ALTER TABLE [dbo].[SaleOrders] CHECK CONSTRAINT [FK_SaleOrders_Users]
GO
ALTER TABLE [dbo].[Stocks]  WITH CHECK ADD  CONSTRAINT [FK_Stocks_Products] FOREIGN KEY([prod_id])
REFERENCES [dbo].[Products] ([prod_id])
GO
ALTER TABLE [dbo].[Stocks] CHECK CONSTRAINT [FK_Stocks_Products]
GO
insert into Users(user_id, user_name, user_pwd, user_role)
values(1, 'Admin', 'Admin', 'Admin');
GO
insert into Users(user_id, user_name, user_pwd, user_role)
values(2, 'Saler', 'Admin', 'Admin');
GO
insert into Users(user_id, user_name, user_pwd, user_role)
values(3, 'Stock Manager', 'Admin', 'Admin');
GO
insert into Customers(cust_name, cust_address, cust_phone)
values('Unknown', 'Unknown', 'Unknown');
GO