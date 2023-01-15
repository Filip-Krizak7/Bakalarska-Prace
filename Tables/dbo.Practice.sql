CREATE TABLE [dbo].[Practice] (
  [id] [int] IDENTITY,
  [date] [date] NULL,
  [start] [time] NULL,
  [end] [time] NULL,
  [note] [varchar](50) NULL,
  [capacity] [int] NOT NULL,
  [subject_id] [int] NOT NULL,
  [teacher_id] [int] NOT NULL,
  PRIMARY KEY CLUSTERED ([id])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Practice]
  ADD FOREIGN KEY ([subject_id]) REFERENCES [dbo].[Subject] ([id])
GO

ALTER TABLE [dbo].[Practice]
  ADD FOREIGN KEY ([teacher_id]) REFERENCES [dbo].[User] ([id])
GO

ALTER TABLE [dbo].[Practice]
  ADD CONSTRAINT [FK_practice_userID] FOREIGN KEY ([id]) REFERENCES [dbo].[UserPractice] ([practice_id])
GO