--Dropping all table 
DROP TABLE Loan
DROP TABLE LoanType
DROP TABLE HOCCreditDetail
DROP TABLE HOCCredit
DROP TABLE HouseCompany
DROP TABLE [Transaction]
DROP TABLE CreditCardTransaction
DROP TABLE CreditCard
DROP TABLE PaymentType
DROP TABLE CreditCardCompany
DROP TABLE [Card]
DROP TABLE CardType
DROP TABLE BusinessAccount
DROP TABLE DepositAccount
DROP TABLE Currency
DROP TABLE SavingAccount
DROP TABLE StudentAccount
DROP TABLE RegularAccount
DROP TABLE IndividualAccount
DROP TABLE [Level]
DROP TABLE VirtualAccount
DROP TABLE CreditCard
DROP TABLE DebitCard
DROP TABLE Account

------------ACCOUNT------------
CREATE TABLE [Account](
	AccountId VARCHAR(5) PRIMARY KEY, 
	[Name] VARCHAR(255) NOT NULL, 
	Balance DECIMAL(20, 2) NOT NULL, 
	Dob DATETIME NOT NULL, 
	[Address] VARCHAR(255) NOT NULL, 
	Email VARCHAR(255) NOT NULL, 
	PIN VARCHAR(255) NOT NULL, 
	CreatedAt DATETIME NOT NULL
)
INSERT INTO Account VALUES
('AC001', 'Nico', 15000000, '2001-07-02 12:00:00', 'Jakarta', 'nico@mail.com', '123123', GETDATE()), 
('AC002', 'Michael', 7500000, '2001-06-25 12:00:00', 'Alam Sutera', 'john@mail.com', '123123', GETDATE()), 
('AC003', 'Donald', 500000, '2005-10-12 12:00:00', 'Jakarta', 'donald@mail.com', '123123', GETDATE())
------------------------------------------------

------------DEBIT CARD------------
CREATE TABLE [DebitCard] (
	CardId VARCHAR(12) PRIMARY KEY, 
	AccountId VARCHAR(5) NOT NULL, 
	ExpiredDate DATETIME NOT NULL, 

	FOREIGN KEY (AccountId) REFERENCES Account(AccountId)
)

INSERT INTO [DebitCard] VALUES
('123123123123', 'AC001', '2021-01-01 12:00:00'), 
('234234234234', 'AC002', '2021-03-01 12:00:00'),
('345345345345', 'AC003', '2021-03-01 12:00:00')
------------------------------------------------

------------VIRTUAL ACCOUNT------------
CREATE TABLE [VirtualAccount] (
	VirtualId VARCHAR(12) PRIMARY KEY, 
	AccountId VARCHAR(5) NOT NULL, 

	FOREIGN KEY (AccountId) REFERENCES Account(AccountId)
)

INSERT INTO [VirtualAccount] VALUES
('456456456456', 'AC001'), 
('987987987987', 'AC002'),
('123123123123', 'AC003')

------------------------------------------------

------------LEVEL------------
CREATE TABLE [Level] (
	LevelName VARCHAR(15) PRIMARY KEY, 
	MaximumWithdrawal DECIMAL(20, 2) NOT NULL,
	MaximumTransfers DECIMAL(20, 2) NOT NULL, 
	MonthlyInterest DECIMAL(10,2) NOT NULL
)

INSERT INTO [Level] VALUES 
('Bronze', 1000000, 1000000, 0.005), 
('Silver', 5000000, 2000000, 0.01), 
('Gold', 10000000, 5000000, 0.05), 
('Black', 25000000, 10000000, 0.1)
------------------------------------------------


------------INDIVIDUAL ACCOUNT------------
CREATE TABLE [IndividualAccount] (
	AccountId VARCHAR(5) PRIMARY KEY, 
	LevelName VARCHAR(15) NOT NULL, 
	InterestRate DECIMAL(10, 2) NOT NULL, 
	InitialDeposit DECIMAL(20, 2) NOT NULL, 
	MinimumSaving DECIMAL(20, 2) NOT NULL, 
	AdminFee DECIMAL(20, 2) NOT NULL, 

	FOREIGN KEY (LevelName) REFERENCES Level(LevelName), 
	FOREIGN KEY (AccountId) REFERENCES Account(AccountId)
)

INSERT INTO [IndividualAccount] VALUES
('AC001', 'Black', 0.05, 2000000, 100000, 15000), 
('AC002', 'Gold', 0.05, 2000000, 100000, 15000), 
('AC003', 'Gold', 0.05, 2000000, 100000, 15000)

------------------------------------------------


------------REGULAR ACCOUNT------------
CREATE TABLE [RegularAccount] (
	AccountId VARCHAR(5) PRIMARY KEY, 

	FOREIGN KEY (AccountId) REFERENCES IndividualAccount(AccountId)
)

INSERT INTO [RegularAccount] VALUES
('AC001'), 
('AC002')

------------------------------------------------


------------STUDENT ACCOUNT------------
CREATE TABLE [StudentAccount] (
	StudentAccountId VARCHAR(5) PRIMARY KEY, 
	AccountId VARCHAR(5) NOT NULL, 
	GuardianAccountId VARCHAR(5) NOT NULL, 

	FOREIGN KEY (AccountId) REFERENCES IndividualAccount(AccountId), 
	FOREIGN KEY (GuardianAccountId) REFERENCES RegularAccount(AccountId)
)

INSERT INTO [StudentAccount] VALUES 
('ST001', 'AC003', 'AC001') 

------------------------------------------------------------

------------SAVING ACCOUNT------------

CREATE TABLE [SavingAccount] (
	SavingAccountId VARCHAR(5) PRIMARY KEY,
	AccountId VARCHAR(5) NOT NULL, 
	Balance DECIMAL(20, 2) NOT NULL, 

	FOREIGN KEY (AccountId) REFERENCES RegularAccount(AccountId)
)

INSERT INTO [SavingAccount] VALUES
('SA001', 'AC001', 150000), 
('SA002', 'AC002', 200000)

------------------------------------------------------------

------------CURRENCY------------
CREATE TABLE [Currency] (
	CurrencyId VARCHAR(3) PRIMARY KEY, 
	[Name] VARCHAR(20) NOT NULL, 
	ExchangeRate DECIMAL(10, 2) NOT NULL
)

INSERT INTO Currency VALUES
('IDR', 'Indonesian Rupiah', 1), 
('USD', 'United States Dollar', 14000), 
('SGD', 'Singapore Dollar', 10500)

------------------------------------------------

------------DEPOSIT ACCOUNT------------
CREATE TABLE [DepositAccount] (
	DepositAccountId VARCHAR(5) PRIMARY KEY, 
	AccountId VARCHAR(5) NOT NULL, 
	CurrencyId VARCHAR(3) NOT NULL, 
	MinimumDeposit DECIMAL(20, 2) NOT NULL, 
	DepositBalance DECIMAL(20, 2) NOT NULL,

	FOREIGN KEY (AccountId) REFERENCES RegularAccount(AccountId), 
	FOREIGN KEY (CurrencyId) REFERENCES Currency(CurrencyId)
)

INSERT INTO [DepositAccount] VALUES
('DA001', 'AC001', 'IDR', 1000000, 1500000), 
('DA002', 'AC002', 'IDR', 1000000, 2000000)

------------------------------------------------

------------BUSINESS ACCOUNT------------
CREATE TABLE BusinessAccount (
	AccountId VARCHAR(5) PRIMARY KEY, 
	FOREIGN KEY (AccountId) REFERENCES IndividualAccount(AccountId)
)

------------------------------------------------

------------CARD TYPE------------
CREATE TABLE CardType (
	CardTypeId VARCHAR(5) PRIMARY KEY, 
	CardTypeName VARCHAR(50) NOT NULL 
)

INSERT INTO CardType VALUES
('CT001', 'Business Card'), 
('CT002', 'Petty Card'), 
('CT003', 'Deposit Card'), 
('CT004', 'Reward Card')

------------------------------------------------

------------CARD------------

CREATE TABLE [Card] (
	CardId VARCHAR(5) PRIMARY KEY, 
	AccountId VARCHAR(5) NOT NULL, 
	CardTypeId VARCHAR(5) NOT NULL, 
	TransactionLimit DECIMAL(20, 2) NOT NULL, 

	FOREIGN KEY (AccountId) REFERENCES BusinessAccount(AccountId), 
	FOREIGN KEY (CardTypeId) REFERENCES CardType(CardTypeId)
)

------------------------------------------------

------------CREDIT CARD COMPANY------------
CREATE TABLE [CreditCardCompany] (
    CompanyId VARCHAR(5) PRIMARY KEY, 
    [Name] VARCHAR(255) NOT NULL
)

INSERT INTO [CreditCardCompany] VALUES
('CO001', 'American Express'),
('CO002', 'Barclays'),
('CO003', 'Capital One')

------------------------------------------------

------------PAYMENT TYPE------------

CREATE TABLE PaymentType (
	PaymentTypeId VARCHAR(5) PRIMARY KEY, 
	[Name] VARCHAR(255) NOT NULL
)

INSERT INTO PaymentType VALUES
('PA001', 'Debit'), 
('PA002', 'Virtual Account')

------------------------------------------------

------------CREDIT CARD------------

CREATE TABLE [CreditCard] (
    CreditCardId VARCHAR(12) PRIMARY KEY, 
    AccountId VARCHAR(5) NOT NULL, 
    CompanyId VARCHAR(5) NOT NULL, 
    FamilyCard VARCHAR(255) NOT NULL, 
    IdentityCard VARCHAR(255) NOT NULL, 
    [Limit] DECIMAL(20, 2) NOT NULL, 
	IsApproved BIT NOT NULL, 
	IsActive BIT NOT NULL, 

	FOREIGN KEY (AccountId) REFERENCES Account(AccountId), 
	FOREIGN KEY (CompanyId) REFERENCES CreditCardCompany(CompanyId)
)

------------------------------------------------

------------CREDIT CARD TRANSACTION------------

CREATE TABLE CreditCardTransaction (
	TransactionId VARCHAR(5) PRIMARY KEY,  
	CreditCardId VARCHAR(12) NOT NULL, 
	PaymentTypeId VARCHAR(5) NOT NULL,  
	TransactionDate DATETIME NOT NULL, 
	DueDate DATETIME NOT NULL, 
	PaidDate DATETIME NOT NULL, 
	Amount DECIMAL(20, 2) NOT NULL, 
	AdditionalCharge DECIMAL(10, 2), 
	LatePenalty DECIMAL(10, 2), 

	FOREIGN KEY (CreditCardId) REFERENCES CreditCard(CreditCardId), 
	FOREIGN KEY (PaymentTypeId) REFERENCES PaymentType(PaymentTypeId)
)

------------------------------------------------

------------TRANSACTION------------
CREATE TABLE [Transaction] (
	TransactionId VARCHAR(5) PRIMARY KEY, 
	AccountId VARCHAR(5) NOT NULL, 
	RelatedAccountId VARCHAR(5), 
	EmployeeId VARCHAR(5) NOT NULL, 
	PaymentTypeId VARCHAR(5) NOT NULL, 
	DebitCardId VARCHAR(12), 
	VirtualAccountId VARCHAR(12), 
	Amount DECIMAL(20, 2) NOT NULL, 
	TransactionDate DATETIME NOT NULL,
	TransactionType VARCHAR(30) NOT NULL, 

	FOREIGN KEY (AccountId) REFERENCES Account(AccountId), 
	FOREIGN KEY (RelatedAccountId) REFERENCES Account(AccountId), 
	FOREIGN KEY (EmployeeId) REFERENCES Employee(EmployeeId), 
	FOREIGN KEY (PaymentTypeId) REFERENCES PaymentType(PaymentTypeId),
	FOREIGN KEY (DebitCardId) REFERENCES DebitCard(CardId), 
	FOREIGN KEY (VirtualAccountId) REFERENCES VirtualAccount(VirtualId)
)

------------------------------------------------

------------HOUSE COMPANY------------

CREATE TABLE HouseCompany (
	HouseCompanyId VARCHAR(5) PRIMARY KEY, 
	[Name] VARCHAR(255) NOT NULL 
)

INSERT INTO HouseCompany VALUES 
('HO001', 'The Housing Company'), 
('HO002', 'Best Housing')

------------------------------------------------


------------HOC CREDIT------------

CREATE TABLE HOCCredit (
	HOCId VARCHAR(5) PRIMARY KEY, 
	AccountId VARCHAR(5) NOT NULL, 
	CompanyId VARCHAR(5) NOT NULL, 
	HousePrice DECIMAL(20, 2) NOT NULL, 
	InitialDeposit DECIMAL(20, 2) NOT NULL, 
	CreditPeriod INT NOT NULL, 
	StartDate DATETIME NOT NULL,
	GrossIncome DECIMAL(20, 2) NOT NULL, 
	IsApproved BIT NOT NULL, 
	Amount DECIMAL(20, 2) NOT NULL, 
	
	FOREIGN KEY (AccountId) REFERENCES Account(AccountId), 
	FOREIGN KEY (CompanyId) REFERENCES HouseCompany(HouseCompanyId)
)

------------------------------------------------

------------HOC CREDIT DETAIL------------
CREATE TABLE HOCCreditDetail (
	HOCId VARCHAR(5) PRIMARY KEY,
	PaymentTypeId VARCHAR(5) NOT NULL, 
	DueDate DATETIME NOT NULL, 
	PaidDate DATETIME NOT NULL, 
	PeriodMonth INT NOT NULL, 
	Amount DECIMAL(20, 2) NOT NULL, 
	AdditionalCharge DECIMAL(20, 2) NOT NULL, 
	LatePenalty DECIMAL(20, 2) NOT NULL, 
	TotalAmount DECIMAL(20, 2) NOT NULL, 
	 
	FOREIGN KEY (HOCId) REFERENCES HOCCredit(HOCId), 
	FOREIGN KEY (PaymentTypeId) REFERENCES PaymentType(PaymentTypeId)
)

------------------------------------------------

------------LOAN TYPE------------
CREATE TABLE LoanType(
	LoanTypeName VARCHAR(50) PRIMARY KEY, 
	MinimumAmount DECIMAL(20, 2) NOT NULL, 
	MaximumAmount DECIMAL(20, 2) NOT NULL 
)

INSERT INTO LoanType VALUES
('Individual', 10000000, 100000000), 
('Business', 10000000, 5000000000)

------------------------------------------------

------------LOAN------------
CREATE TABLE Loan (
	LoanId VARCHAR(5) PRIMARY KEY, 
	AccountId VARCHAR(5) NOT NULL, 
	LoanTypeName VARCHAR(50) NOT NULL,
	LoanAmount DECIMAL(20, 2) NOT NULL, 
	GuranteeDocument VARCHAR(255) NOT NULL, 
	LoanReason VARCHAR(255) NOT NULL, 
	IsApproved BIT NOT NULL, 
	IsPaid BIT NOT NULL, 
	DueDate DATETIME NOT NULL, 
	PaidDate DATETIME NOT NULL,

	FOREIGN KEY (AccountId) REFERENCES Account(AccountId), 
	FOREIGN KEY (LoanTypeName) REFERENCES LoanType(LoanTypeName)
)

------------------------------------------------


