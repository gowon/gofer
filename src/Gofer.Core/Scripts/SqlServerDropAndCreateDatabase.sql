--DECLARE @DatabaseName nvarchar(50) = N'TestDb'
DECLARE @SQL varchar(max)

SELECT @SQL = COALESCE(@SQL,'') + 'Kill ' + Convert(varchar, SPId) + ';'
FROM MASTER..SysProcesses
WHERE DBId = DB_ID(@DatabaseName) AND SPId <> @@SPId

EXEC(@SQL)

USE master
IF EXISTS(SELECT * FROM sys.databases WHERE name = @DatabaseName)
	EXEC('DROP DATABASE ' + @DatabaseName)

EXEC('CREATE DATABASE ' + @DatabaseName)