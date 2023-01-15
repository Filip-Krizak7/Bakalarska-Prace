CREATE TABLE [dbo].[User] (
  [id] [int] IDENTITY,
  [username] [varchar](50) NOT NULL,
  [password] [varchar](30) NOT NULL,
  [firstName] [varchar](20) NOT NULL,
  [secondName] [varchar](30) NOT NULL,
  [phoneNumber] [varchar](20) NOT NULL,
  [role_id] [int] NOT NULL,
  [locked] [bit] NULL,
  [enabled] [bit] NULL,
  [school_id] [int] NOT NULL,
  [teacher_practice_id] [int] NOT NULL,
  [student_practice_id] [int] NOT NULL,
  PRIMARY KEY CLUSTERED ([id])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[User]
  ADD CONSTRAINT [FK_user_practiceID] FOREIGN KEY ([teacher_practice_id]) REFERENCES [dbo].[Practice] ([id])
GO

ALTER TABLE [dbo].[User]
  ADD CONSTRAINT [FK_user_schoolID] FOREIGN KEY ([school_id]) REFERENCES [dbo].[School] ([id]) ON DELETE CASCADE ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[User]
  ADD CONSTRAINT [FK_user_userID] FOREIGN KEY ([student_practice_id]) REFERENCES [dbo].[UserPractice] ([user_id])
GO