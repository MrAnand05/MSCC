CREATE TABLE [dbo].[TSubClass] (
    [RegNo]   INT           NOT NULL,
    [Subject] NVARCHAR (50) NOT NULL,
    [Class]   INT           NOT NULL,
    CONSTRAINT [FK_TSubClass_TeacherDetails] FOREIGN KEY ([RegNo]) REFERENCES [dbo].[TeacherDetails] ([RegNo])
);

