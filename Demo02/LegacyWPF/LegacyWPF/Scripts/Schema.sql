DROP TABLE [Containers];
GO
CREATE TABLE [Containers] (
  [ContainerId] int NOT NULL
, [Sign] nchar(4) NOT NULL
, [Number] int NOT NULL
, [Checkdigit] nchar(1) NOT NULL
, [Type] nvarchar(6) NOT NULL
);
GO
ALTER TABLE [Containers] ADD CONSTRAINT [PK_Containers] PRIMARY KEY ([ContainerId]);
GO

DROP TABLE [Shippings];
GO
CREATE TABLE [Shippings] (
  [ShippingId] int NOT NULL
, [ContainerId] int NOT NULL
, [Type] nchar(1) NOT NULL
, [Seal] int NOT NULL
, [Weight] int NOT NULL
, [ToPort] nvarchar(100) NULL
, [FromPort] nvarchar(100) NULL
, [Ship] nvarchar(50) NULL
, [Forwarder] nvarchar(100) NULL
, [BoardingDate] datetime NULL
, [LandingDate] datetime NULL
);
GO
ALTER TABLE [Shippings] ADD CONSTRAINT [PK_Shippings] PRIMARY KEY ([ShippingId]);
GO