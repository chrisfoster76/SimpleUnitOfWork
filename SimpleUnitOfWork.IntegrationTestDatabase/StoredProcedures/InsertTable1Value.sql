CREATE PROCEDURE [dbo].[InsertTable1Value]
	@Value varchar(100)
AS

	insert into Table1 (TestValue) values (@Value);

GO

