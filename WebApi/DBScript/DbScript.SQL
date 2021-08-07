USE [db];
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Events' and xtype='U')
BEGIN
	CREATE TABLE [dbo].[Events](
			[Id]               [bigint] IDENTITY(1, 1) NOT NULL,
			[AggregateId]      [uniqueidentifier] NOT NULL,
			[EventData]        [nvarchar](max) NOT NULL,
			[Version]		   [int] not null default 0,
			[CreatedOn]        [datetime2] DEFAULT GETDATE() NOT NULL,	
			[Type]			   [varchar](max) not null
		PRIMARY KEY CLUSTERED
		(
			[Id] ASC
		)
	)
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Dispatchs' and xtype='U')
BEGIN
	CREATE TABLE [dbo].[Dispatchs](
		[Id]		[bigint] IDENTITY(1,1) NOT NULL,
		[EventData] [nvarchar](max) NOT NULL,
		[CreatedOn] [datetime2](7) NOT NULL,
		[Type]		[varchar](max) not null
	PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ServiceAgentProjections' and xtype='U')
BEGIN
	CREATE TABLE [dbo].[ServiceAgentProjections](
		[ServiceAgentId]	[uniqueidentifier] NOT NULL,
		[Name]		[nvarchar](max) NOT NULL,
		[ApiEndpoint]	  [nvarchar](max) NOT NULL,
		[ApiEndpointAction]		[nvarchar](20) NOT NULL,
		[Body]		[nvarchar](max) NULL,
		[Headers]		[nvarchar](max) NULL,
		[Status]		[nvarchar](20) NOT NULL,
		[ResponseBody]		[nvarchar](max) NULL,
		[ResponseHeaders]		[nvarchar](max) NULL,
		[SubmittedDate] [datetime2](7) NOT NULL,
		[ExecutionDate] [datetime2](7) NULL,
		[LastUpdatedDate] [datetime2](7) NOT NULL,
		[FailedDetails]	  [nvarchar](max) null,
		[IsFavourite]	 bit null,
		PRIMARY KEY CLUSTERED
		(
			[ServiceAgentId] ASC
		)
	)
END
