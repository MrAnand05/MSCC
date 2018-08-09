CREATE TABLE [dbo].[TeacherDetails] (
    [RegNo]          INT            IDENTITY (1, 1) NOT NULL,
    [TFName]         NVARCHAR (50)  NULL,
    [TMName]         NVARCHAR (50)  NULL,
    [TLName]         NVARCHAR (50)  NULL,
    [Gender]         CHAR (2)       NULL,
    [TQualification] NVARCHAR (50)  NULL,
    [TPic]           IMAGE          NULL,
    [TAddress1]      NVARCHAR (MAX) NULL,
    [TAddress2]      NVARCHAR (MAX) NULL,
    [TContact1]      BIGINT         NULL,
    [TContact2]      BIGINT         NULL,
    [DOB]            DATE           NULL,
    [DOJ]            DATE           NULL,
    CONSTRAINT [PK_TeacherDetails] PRIMARY KEY CLUSTERED ([RegNo] ASC)
);

