using NUnit.Framework;

using MMABooksProps;
using MMABooksDB;

using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;

using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace MMABooksTests
{
    internal class CustomerDBTest
    {
        CustomerDB db;
        [SetUp]
        public void ResetData()
        {
            db = new CustomerDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }
        [Test]
        public void TestRetrieve()
        {
            CustomerProps p = (CustomerProps)db.Retrieve(1);
            Assert.AreEqual(1, p.CustomerID);
            Assert.AreEqual("Molunguri, A", p.Name);
        }
        [Test]
        public void TestUpdate()
        {
            CustomerProps p = (CustomerProps)db.Retrieve(150);
            p.Name = "Molunguri, A";
            Assert.True(db.Update(p));
            p = (CustomerProps)db.Retrieve(150);
            Assert.AreEqual("Molunguri, A", p.Name);
        }

        [Test]
        public void TestDelete()
        {
            CustomerDB db = new CustomerDB(); 
            CustomerProps customerToDelete = new CustomerProps
            {
                CustomerID = 6969,  
                Name = "Molunguri, A" 
            };
            bool deletionResult = db.Delete(customerToDelete);
            Assert.True(deletionResult, "Customer deletion should be successful.");

        }

    }
}