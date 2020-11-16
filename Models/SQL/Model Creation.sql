--1 
--CREATE TABLE [Account](
--	accountId VARCHAR(255) PRIMARY KEY, 
--	[name] VARCHAR(255), 
--	balance INT, 
--	dob DATETIME, 
--	email VARCHAR(255), 
--	PIN VARCHAR(255)
--)

CREATE TABLE [Individual Account](
	accountId VARCHAR(255) PRIMARY KEY, 
	[name] VARCHAR(255), 
	balance INT, 
	dob DATETIME, 
	email VARCHAR(255), 
	PIN VARCHAR(255),
	levelName VARCHAR(255), 
	interestRate DECIMAL(3),
	initialDeposit INT, 
	minimumSaving INT
)