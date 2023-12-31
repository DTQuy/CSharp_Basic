USE [csharp_basic]
GO
/****** Object:  Table [dbo].[cart]    Script Date: 11/13/2023 9:10:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cart](
	[cart_id] [uniqueidentifier] NOT NULL,
	[customer_id] [uniqueidentifier]NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[cart_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Drop table [cart]
/****** Object:  Table [dbo].[cart_details]    Script Date: 11/13/2023 9:10:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cart_details](
	[cart_id] [uniqueidentifier],
	[product_id] [uniqueidentifier]NOT NULL,
	[quantity] [int]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[customer]    Script Date: 11/13/2023 9:10:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[customer](
	[customer_id] [uniqueidentifier] NOT NULL,
	[first_name] [nvarchar](255) NOT NULL,
	[last_name] [nvarchar](255) NOT NULL,
	[email] [nvarchar](255) NOT NULL,
	[phone_number] [nvarchar](255) NOT NULL,
	[address] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[customer_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[order_details]    Script Date: 11/13/2023 9:10:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[order_details](
	[id_order] [uniqueidentifier] NULL,
	[product_id] [uniqueidentifier] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[orders]    Script Date: 11/13/2023 9:10:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[orders](
	[id_order] [uniqueidentifier] NOT NULL,
	[customer_id] [uniqueidentifier] NULL,
	[order_day] [datetime] NULL,
	[total_amount] [decimal](18, 0) NULL,
PRIMARY KEY CLUSTERED 
(
	[id_order] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[product]    Script Date: 11/13/2023 9:10:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[product](
	[product_id] [uniqueidentifier] NOT NULL,
	[name_product] [nvarchar](255) NOT NULL,
	[price] [decimal](18, 0) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[product_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[cart] ADD  DEFAULT (newid()) FOR [cart_id]
GO
ALTER TABLE [dbo].[customer] ADD  DEFAULT (newid()) FOR [customer_id]
GO
ALTER TABLE [dbo].[orders] ADD  DEFAULT (newid()) FOR [id_order]
GO
ALTER TABLE [dbo].[product] ADD  DEFAULT (newid()) FOR [product_id]
GO
ALTER TABLE [dbo].[cart_details]  WITH CHECK ADD FOREIGN KEY([cart_id])
REFERENCES [dbo].[cart] ([cart_id])
GO
ALTER TABLE [dbo].[order_details]  WITH CHECK ADD FOREIGN KEY([id_order])
REFERENCES [dbo].[orders] ([id_order])
GO


select * from cart
select * from cart_details
select * from customer
select * from product



INSERT INTO cart(cart_id) VALUEs (NEWID())
INSERT INTO cart_details(cart_id) VALUEs) 
