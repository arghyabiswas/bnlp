USE [DBNLP]
GO
/****** Object:  Table [dbo].[Module]    Script Date: 07/20/2014 21:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Module](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[CorrectionConstant] [int] NOT NULL,
	[CorrectionParameter] [decimal](18, 15) NULL,
	[ParameterCount] [int] NOT NULL,
 CONSTRAINT [PK_Module] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OutcomePattern]    Script Date: 07/20/2014 21:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OutcomePattern](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ModuleID] [int] NULL,
 CONSTRAINT [PK_OutcomePattern] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OutcomeLabel]    Script Date: 07/20/2014 21:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OutcomeLabel](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Value] [varchar](50) NULL,
	[ModuleID] [int] NULL,
 CONSTRAINT [PK_OutcomeLabel] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Predicate]    Script Date: 07/20/2014 21:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Predicate](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[OutcomePattern] [int] NOT NULL,
	[ModuleID] [int] NOT NULL,
 CONSTRAINT [PK_Predicate] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Parameter]    Script Date: 07/20/2014 21:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Parameter](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Value] [decimal](18, 15) NOT NULL,
	[PredicateID] [int] NULL,
 CONSTRAINT [PK_Parameter] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Items]    Script Date: 07/20/2014 21:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Items](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Value] [int] NOT NULL,
	[PatternID] [int] NULL,
 CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_Items_OutcomePattern]    Script Date: 07/20/2014 21:11:23 ******/
ALTER TABLE [dbo].[Items]  WITH CHECK ADD  CONSTRAINT [FK_Items_OutcomePattern] FOREIGN KEY([PatternID])
REFERENCES [dbo].[OutcomePattern] ([ID])
GO
ALTER TABLE [dbo].[Items] CHECK CONSTRAINT [FK_Items_OutcomePattern]
GO
/****** Object:  ForeignKey [FK_OutcomeLabel_Module]    Script Date: 07/20/2014 21:11:23 ******/
ALTER TABLE [dbo].[OutcomeLabel]  WITH CHECK ADD  CONSTRAINT [FK_OutcomeLabel_Module] FOREIGN KEY([ModuleID])
REFERENCES [dbo].[Module] ([ID])
GO
ALTER TABLE [dbo].[OutcomeLabel] CHECK CONSTRAINT [FK_OutcomeLabel_Module]
GO
/****** Object:  ForeignKey [FK_OutcomePattern_Module]    Script Date: 07/20/2014 21:11:23 ******/
ALTER TABLE [dbo].[OutcomePattern]  WITH CHECK ADD  CONSTRAINT [FK_OutcomePattern_Module] FOREIGN KEY([ModuleID])
REFERENCES [dbo].[Module] ([ID])
GO
ALTER TABLE [dbo].[OutcomePattern] CHECK CONSTRAINT [FK_OutcomePattern_Module]
GO
/****** Object:  ForeignKey [FK_Parameter_Predicate]    Script Date: 07/20/2014 21:11:23 ******/
ALTER TABLE [dbo].[Parameter]  WITH CHECK ADD  CONSTRAINT [FK_Parameter_Predicate] FOREIGN KEY([PredicateID])
REFERENCES [dbo].[Predicate] ([ID])
GO
ALTER TABLE [dbo].[Parameter] CHECK CONSTRAINT [FK_Parameter_Predicate]
GO
/****** Object:  ForeignKey [FK_Predicate_Module]    Script Date: 07/20/2014 21:11:23 ******/
ALTER TABLE [dbo].[Predicate]  WITH CHECK ADD  CONSTRAINT [FK_Predicate_Module] FOREIGN KEY([ModuleID])
REFERENCES [dbo].[Module] ([ID])
GO
ALTER TABLE [dbo].[Predicate] CHECK CONSTRAINT [FK_Predicate_Module]
GO

