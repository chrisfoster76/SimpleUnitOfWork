CREATE PROCEDURE [dbo].[SetupTable2Value]
	@Value varchar(100)
AS

	insert into Table2 (TestValue) values (@Value);

GO

