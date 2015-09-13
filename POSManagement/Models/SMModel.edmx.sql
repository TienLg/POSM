
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/06/2014 07:07:19
-- Generated from EDMX file: E:\Projects\Tmp\POSManagement\POSManagement\Models\SMModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SaleManagement];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ImportOrderItems_ImportOrders]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ImportOrderItems] DROP CONSTRAINT [FK_ImportOrderItems_ImportOrders];
GO
IF OBJECT_ID(N'[dbo].[FK_ImportOrderItems_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ImportOrderItems] DROP CONSTRAINT [FK_ImportOrderItems_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_ImportOrders_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ImportOrders] DROP CONSTRAINT [FK_ImportOrders_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_SaleOrderItems_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SaleOrderItems] DROP CONSTRAINT [FK_SaleOrderItems_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_SaleOrderItems_SaleOrders]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SaleOrderItems] DROP CONSTRAINT [FK_SaleOrderItems_SaleOrders];
GO
IF OBJECT_ID(N'[dbo].[FK_SaleOrders_Customers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SaleOrders] DROP CONSTRAINT [FK_SaleOrders_Customers];
GO
IF OBJECT_ID(N'[dbo].[FK_SaleOrders_Users]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SaleOrders] DROP CONSTRAINT [FK_SaleOrders_Users];
GO
IF OBJECT_ID(N'[dbo].[FK_Stocks_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Stocks] DROP CONSTRAINT [FK_Stocks_Products];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Customers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customers];
GO
IF OBJECT_ID(N'[dbo].[ImportOrderItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ImportOrderItems];
GO
IF OBJECT_ID(N'[dbo].[ImportOrders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ImportOrders];
GO
IF OBJECT_ID(N'[dbo].[Products]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Products];
GO
IF OBJECT_ID(N'[dbo].[SaleOrderItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SaleOrderItems];
GO
IF OBJECT_ID(N'[dbo].[SaleOrders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SaleOrders];
GO
IF OBJECT_ID(N'[dbo].[Stocks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Stocks];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [cust_id] int IDENTITY(1,1) NOT NULL,
    [cust_name] nchar(255)  NOT NULL,
    [cust_address] nchar(255)  NULL,
    [cust_phone] nchar(20)  NULL
);
GO

-- Creating table 'ImportOrderItems'
CREATE TABLE [dbo].[ImportOrderItems] (
    [order_id] int  NOT NULL,
    [order_item_id] int IDENTITY(1,1) NOT NULL,
    [prod_id] nchar(10)  NOT NULL,
    [base_price_by_unit] decimal(18,3)  NOT NULL,
    [base_price_by_stock] decimal(18,3)  NOT NULL,
    [quantity_by_unit] int  NOT NULL,
    [quantity_by_stock] int  NOT NULL,
    [quantity_control] int  NOT NULL
);
GO

-- Creating table 'ImportOrders'
CREATE TABLE [dbo].[ImportOrders] (
    [order_id] int IDENTITY(1,1) NOT NULL,
    [user_id] int  NOT NULL,
    [total_price] decimal(18,3)  NOT NULL,
    [order_status] nchar(50)  NOT NULL,
    [date_import] datetime  NOT NULL
);
GO

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [prod_id] nchar(10)  NOT NULL,
    [prod_name] nchar(255)  NOT NULL,
    [quantity_control] int  NOT NULL,
    [date_added] datetime  NOT NULL,
    [date_modified] datetime  NOT NULL,
    [quantity_by_unit] int  NOT NULL,
    [quantity_by_stock] int  NOT NULL,
    [sale_price_by_unit] decimal(18,3)  NOT NULL,
    [sale_price_by_stock] decimal(18,3)  NOT NULL
);
GO

-- Creating table 'SaleOrderItems'
CREATE TABLE [dbo].[SaleOrderItems] (
    [order_id] int  NOT NULL,
    [order_item_id] int IDENTITY(1,1) NOT NULL,
    [prod_id] nchar(10)  NOT NULL,
    [sale_price_by_unit] decimal(18,3)  NOT NULL,
    [sale_price_by_stock] decimal(18,3)  NOT NULL,
    [quantity_by_unit] int  NOT NULL,
    [quantity_by_stock] int  NOT NULL,
    [quantity_control] int  NOT NULL
);
GO

-- Creating table 'SaleOrders'
CREATE TABLE [dbo].[SaleOrders] (
    [order_id] int IDENTITY(1,1) NOT NULL,
    [cust_id] int  NOT NULL,
    [user_id] int  NOT NULL,
    [total_price] decimal(18,3)  NOT NULL,
    [date_created] datetime  NOT NULL,
    [date_ship] datetime  NOT NULL,
    [order_status] nchar(50)  NOT NULL,
    [taken_money] decimal(18,3)  NOT NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [user_id] int  NOT NULL,
    [user_name] nchar(255)  NOT NULL,
    [user_pwd] nvarchar(max)  NOT NULL,
    [user_role] nchar(50)  NOT NULL
);
GO

-- Creating table 'Stocks'
CREATE TABLE [dbo].[Stocks] (
    [stock_id] int IDENTITY(1,1) NOT NULL,
    [prod_id] nchar(10)  NOT NULL,
    [base_price_by_unit] decimal(18,3)  NOT NULL,
    [base_price_by_stock] decimal(18,3)  NOT NULL,
    [quantity_by_unit] int  NOT NULL,
    [quantity_by_stock] int  NOT NULL,
    [quantity_control] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [cust_id] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [PK_Customers]
    PRIMARY KEY CLUSTERED ([cust_id] ASC);
GO

-- Creating primary key on [order_item_id] in table 'ImportOrderItems'
ALTER TABLE [dbo].[ImportOrderItems]
ADD CONSTRAINT [PK_ImportOrderItems]
    PRIMARY KEY CLUSTERED ([order_item_id] ASC);
GO

-- Creating primary key on [order_id] in table 'ImportOrders'
ALTER TABLE [dbo].[ImportOrders]
ADD CONSTRAINT [PK_ImportOrders]
    PRIMARY KEY CLUSTERED ([order_id] ASC);
GO

-- Creating primary key on [prod_id] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [PK_Products]
    PRIMARY KEY CLUSTERED ([prod_id] ASC);
GO

-- Creating primary key on [order_item_id] in table 'SaleOrderItems'
ALTER TABLE [dbo].[SaleOrderItems]
ADD CONSTRAINT [PK_SaleOrderItems]
    PRIMARY KEY CLUSTERED ([order_item_id] ASC);
GO

-- Creating primary key on [order_id] in table 'SaleOrders'
ALTER TABLE [dbo].[SaleOrders]
ADD CONSTRAINT [PK_SaleOrders]
    PRIMARY KEY CLUSTERED ([order_id] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [user_id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([user_id] ASC);
GO

-- Creating primary key on [stock_id] in table 'Stocks'
ALTER TABLE [dbo].[Stocks]
ADD CONSTRAINT [PK_Stocks]
    PRIMARY KEY CLUSTERED ([stock_id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [cust_id] in table 'SaleOrders'
ALTER TABLE [dbo].[SaleOrders]
ADD CONSTRAINT [FK_SaleOrders_Customers]
    FOREIGN KEY ([cust_id])
    REFERENCES [dbo].[Customers]
        ([cust_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SaleOrders_Customers'
CREATE INDEX [IX_FK_SaleOrders_Customers]
ON [dbo].[SaleOrders]
    ([cust_id]);
GO

-- Creating foreign key on [order_id] in table 'ImportOrderItems'
ALTER TABLE [dbo].[ImportOrderItems]
ADD CONSTRAINT [FK_ImportOrderItems_ImportOrders1]
    FOREIGN KEY ([order_id])
    REFERENCES [dbo].[ImportOrders]
        ([order_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ImportOrderItems_ImportOrders1'
CREATE INDEX [IX_FK_ImportOrderItems_ImportOrders1]
ON [dbo].[ImportOrderItems]
    ([order_id]);
GO

-- Creating foreign key on [prod_id] in table 'ImportOrderItems'
ALTER TABLE [dbo].[ImportOrderItems]
ADD CONSTRAINT [FK_ImportOrderItems_Products]
    FOREIGN KEY ([prod_id])
    REFERENCES [dbo].[Products]
        ([prod_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ImportOrderItems_Products'
CREATE INDEX [IX_FK_ImportOrderItems_Products]
ON [dbo].[ImportOrderItems]
    ([prod_id]);
GO

-- Creating foreign key on [user_id] in table 'ImportOrders'
ALTER TABLE [dbo].[ImportOrders]
ADD CONSTRAINT [FK_ImportOrders_Users]
    FOREIGN KEY ([user_id])
    REFERENCES [dbo].[Users]
        ([user_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ImportOrders_Users'
CREATE INDEX [IX_FK_ImportOrders_Users]
ON [dbo].[ImportOrders]
    ([user_id]);
GO

-- Creating foreign key on [prod_id] in table 'SaleOrderItems'
ALTER TABLE [dbo].[SaleOrderItems]
ADD CONSTRAINT [FK_SaleOrderItems_Products]
    FOREIGN KEY ([prod_id])
    REFERENCES [dbo].[Products]
        ([prod_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SaleOrderItems_Products'
CREATE INDEX [IX_FK_SaleOrderItems_Products]
ON [dbo].[SaleOrderItems]
    ([prod_id]);
GO

-- Creating foreign key on [order_id] in table 'SaleOrderItems'
ALTER TABLE [dbo].[SaleOrderItems]
ADD CONSTRAINT [FK_SaleOrderItems_SaleOrders]
    FOREIGN KEY ([order_id])
    REFERENCES [dbo].[SaleOrders]
        ([order_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SaleOrderItems_SaleOrders'
CREATE INDEX [IX_FK_SaleOrderItems_SaleOrders]
ON [dbo].[SaleOrderItems]
    ([order_id]);
GO

-- Creating foreign key on [user_id] in table 'SaleOrders'
ALTER TABLE [dbo].[SaleOrders]
ADD CONSTRAINT [FK_SaleOrders_Users]
    FOREIGN KEY ([user_id])
    REFERENCES [dbo].[Users]
        ([user_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SaleOrders_Users'
CREATE INDEX [IX_FK_SaleOrders_Users]
ON [dbo].[SaleOrders]
    ([user_id]);
GO

-- Creating foreign key on [prod_id] in table 'Stocks'
ALTER TABLE [dbo].[Stocks]
ADD CONSTRAINT [FK_Stocks_Products]
    FOREIGN KEY ([prod_id])
    REFERENCES [dbo].[Products]
        ([prod_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Stocks_Products'
CREATE INDEX [IX_FK_Stocks_Products]
ON [dbo].[Stocks]
    ([prod_id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------