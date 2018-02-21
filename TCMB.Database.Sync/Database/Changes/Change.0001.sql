IF OBJECT_ID(N'dbo.tbl_LanguageLocalize', N'U') IS NULL
BEGIN
        CREATE TABLE [DBO].[tbl_LanguageLocalize](
	    [LanguageLocalizeID] [INT] IDENTITY(1,1) NOT NULL,
	    [LanguageLocalizeGUID] [UNIQUEIDENTIFIER] NOT NULL,
	    [LanguageLocalizeJS] [BIT] NOT NULL,
	    [LanguageLocalizeDeleted] [BIT] NOT NULL,
	    [LanguageLocalizeCreateDate] [DATETIME] NOT NULL,
	    [LanguageLocalize1] [NVARCHAR](MAX) NOT NULL,   --English
	    [LanguageLocalize2] [NVARCHAR](MAX) NOT NULL,   --Turkish
	    [LanguageLocalize3] [NVARCHAR](MAX) NOT NULL,   --Arabic
     CONSTRAINT [PK_tbl_LanguageLocalize] PRIMARY KEY CLUSTERED 
    (
	    [LanguageLocalizeID] ASC
    )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
END