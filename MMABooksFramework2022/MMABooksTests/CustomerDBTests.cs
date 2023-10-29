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
            Assert.AreEqual("1", p.CustomerID);
            Assert.AreEqual("Find In Database - Use SQL", p.Name);
        }

    }

}