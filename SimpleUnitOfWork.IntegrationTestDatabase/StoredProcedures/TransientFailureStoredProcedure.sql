CREATE PROCEDURE [dbo].[TransientFailureStoredProcedure]
AS


RAISERROR (40501, 18, 1, 'Tranisent error raised in [TransientFailureStoredProcedure]');