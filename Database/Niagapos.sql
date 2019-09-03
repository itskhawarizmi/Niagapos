
/****** Object:  Table [dbo].[Customer]    Script Date: 03/09/2019 13:46:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerId] [nvarchar](8) NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[Gender] [nvarchar](15) NOT NULL,
	[Address] [text] NULL,
	[Email] [nvarchar](50) NOT NULL,
	[PhoneNumber] [nvarchar](15) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Product]    Script Date: 03/09/2019 13:46:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductId] [nvarchar](12) NOT NULL,
	[ProductName] [nvarchar](100) NOT NULL,
	[Size] [nvarchar](20) NULL,
	[Color][nvarchar](25) NULL,
	[CurrentStock] [int] NOT NULL,
	[Price] [decimal](18, 0) NULL,
	[ProductCategoryId] [nvarchar](6) NOT NULL,
	[ProductSupplierId] [nvarchar](6) NOT NULL,
	[Description] [text] NULL,
	[ImgUrl] [text] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProductCategory]    Script Date: 03/09/2019 13:46:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategory](
	[ProductCategoryId] [nvarchar](6) NOT NULL,
	[Category] [nvarchar](50) NOT NULL,
	[Description] [text] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProductSupplier]    Script Date: 03/09/2019 13:46:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductSupplier](
	[ProductSupplierId] [nvarchar](6) NOT NULL,
	[CompanyName] [nvarchar](100) NOT NULL,
	[Brand] [nvarchar](50) NOT NULL,
	[Address] [text] NULL,
	[Email] [nvarchar](50) NOT NULL,
	[PhoneNumber] [nvarchar](15) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductSupplierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TransactionDetail]    Script Date: 03/09/2019 13:46:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionDetail](
	[TransactionDetailId] [nvarchar](10) NOT NULL,
	[TransactionId] [nvarchar](10) NOT NULL,
	[ProductId] [nvarchar](12) NOT NULL,
	[PurchaseTotal] [int] NOT NULL,
	[UnitPrice] [decimal](18, 0) NOT NULL,
	[Disc] [decimal](18, 0) NULL,
	[Total] [decimal](18, 0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TransactionDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 03/09/2019 13:46:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[TransactionId] [nvarchar](10) NOT NULL,
	[Bill] [nvarchar](15) NOT NULL,
	[Date] [datetime] NOT NULL,
	[GrandTotal] [decimal](18, 0) NOT NULL,
	[Cash] [decimal](18, 0) NOT NULL,
	[Note] [text] NULL,
	[UsersId] [nvarchar](8) NOT NULL,
	[CustomerId] [nvarchar](8) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserAccess]    Script Date: 03/09/2019 13:46:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAccess](
	[UserAccessId] [nvarchar](8) NOT NULL,
	[Caption] [nvarchar](25) NOT NULL,
	[LevelAccess] [nvarchar](25) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserAccessId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 03/09/2019 13:46:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [nvarchar](8) NOT NULL,
	[Username] [nvarchar](15) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](25) NOT NULL,
	[UserAccessId] [nvarchar](8) NOT NULL,
	[ActiveStatus] [nvarchar](15) NOT NULL,
	[Administrator] [nvarchar](5) NOT NULL,
	[LastLoggedIn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_ProductCategory_Product] FOREIGN KEY([ProductCategoryId])
REFERENCES [dbo].[ProductCategory] ([ProductCategoryId])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_ProductCategory_Product]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_ProductSupplier_Product] FOREIGN KEY([ProductSupplierId])
REFERENCES [dbo].[ProductSupplier] ([ProductSupplierId])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_ProductSupplier_Product]
GO
ALTER TABLE [dbo].[TransactionDetail]  WITH CHECK ADD  CONSTRAINT [FK_Product_TransactionDetail] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO
ALTER TABLE [dbo].[TransactionDetail] CHECK CONSTRAINT [FK_Product_TransactionDetail]
GO
ALTER TABLE [dbo].[TransactionDetail]  WITH CHECK ADD  CONSTRAINT [FK_Transaction_TransactionDetail] FOREIGN KEY([TransactionId])
REFERENCES [dbo].[Transactions] ([TransactionId])
GO
ALTER TABLE [dbo].[TransactionDetail] CHECK CONSTRAINT [FK_Transaction_TransactionDetail]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Customer_Transaction] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Customer_Transaction]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Users_Transaction] FOREIGN KEY([UsersId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Users_Transaction]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_UserAccess_Users] FOREIGN KEY([UserAccessId])
REFERENCES [dbo].[UserAccess] ([UserAccessId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_UserAccess_Users]
GO
USE [master]
GO
ALTER DATABASE [Niagapos] SET  READ_WRITE 
GO
