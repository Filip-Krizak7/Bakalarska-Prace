CREATE TABLE [dbo].[Review] (
  [id] [int] IDENTITY,
  [user_id] [int] NOT NULL,
  [practice_id] [int] NOT NULL,
  [text] [varchar](150) NULL,
  PRIMARY KEY CLUSTERED ([id])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[Review]
  ADD CONSTRAINT [FK_review_practiceid] FOREIGN KEY ([practice_id]) REFERENCES [dbo].[Practice] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[Review]
  ADD CONSTRAINT [FK_review_userid] FOREIGN KEY ([user_id]) REFERENCES [dbo].[User] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
GO