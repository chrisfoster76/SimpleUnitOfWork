CREATE PROCEDURE [dbo].[FaultyStoredProcedure]
AS

RAISERROR (15600, 16, 1, 'Error raised in FaultyStoredProcedure');
