﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TPA_Desktop_NT20_2.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class KongBuBankEntities : DbContext
    {
        private static KongBuBankEntities instance = null;
        private static readonly object padlock = new object();

        public KongBuBankEntities()
            : base("name=KongBuBankEntities")
        {
        }

        public static KongBuBankEntities Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new KongBuBankEntities();
                    }
                    return instance;
                }
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<ATM> ATMs { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<BusinessAccount> BusinessAccounts { get; set; }
        public virtual DbSet<Candidate> Candidates { get; set; }
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<CardType> CardTypes { get; set; }
        public virtual DbSet<CreditCard> CreditCards { get; set; }
        public virtual DbSet<CreditCardCompany> CreditCardCompanies { get; set; }
        public virtual DbSet<CreditCardTransaction> CreditCardTransactions { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<DebitCard> DebitCards { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<DepositAccount> DepositAccounts { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<FiringRequest> FiringRequests { get; set; }
        public virtual DbSet<HOCCredit> HOCCredits { get; set; }
        public virtual DbSet<HOCCreditDetail> HOCCreditDetails { get; set; }
        public virtual DbSet<HouseCompany> HouseCompanies { get; set; }
        public virtual DbSet<HouseCompanyPartner> HouseCompanyPartners { get; set; }
        public virtual DbSet<IndividualAccount> IndividualAccounts { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemType> ItemTypes { get; set; }
        public virtual DbSet<LeavingPermit> LeavingPermits { get; set; }
        public virtual DbSet<Level> Levels { get; set; }
        public virtual DbSet<Loan> Loans { get; set; }
        public virtual DbSet<LoanType> LoanTypes { get; set; }
        public virtual DbSet<MaintenanceReport> MaintenanceReports { get; set; }
        public virtual DbSet<MaintenanceSchedule> MaintenanceSchedules { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<RegularAccount> RegularAccounts { get; set; }
        public virtual DbSet<RequestExpense> RequestExpenses { get; set; }
        public virtual DbSet<SalaryRaiseRequest> SalaryRaiseRequests { get; set; }
        public virtual DbSet<SavingAccount> SavingAccounts { get; set; }
        public virtual DbSet<StudentAccount> StudentAccounts { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<ViolationReport> ViolationReports { get; set; }
        public virtual DbSet<VirtualAccount> VirtualAccounts { get; set; }
    }
}
