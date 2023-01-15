CREATE TABLE [dbo].[UserPractice] (
  [practice_id] [int] NOT NULL,
  [user_id] [int] NOT NULL,
  UNIQUE ([practice_id]),
  UNIQUE ([user_id])
)
ON [PRIMARY]
GO