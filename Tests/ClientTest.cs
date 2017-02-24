using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using Salon.Objects;

namespace Salon
{
  public class ClientTest : IDisposable
  {
    public ClientTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=hair_salon_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_InputEqualsOutput()
    {
      //Arrange, Act
     int result = Client.GetAll().Count;

     //Assert
     Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfNamesAreTheSame()
    {
      Client firstClient = new Client("Roger", 2, 1);
      Client secondClient = new Client("Roger", 2, 1);
    Assert.Equal(firstClient, secondClient);
    }

    [Fact]
    public void Test_Save_ReturnsSavedClient()
    {
      Client testClient = new Client("McDonald", 2);
      testClient.Save();

      List<Client> totalClients = Client.GetAll();
      List<Client> testClients = new List<Client>{testClient};

      Assert.Equal(testClients, totalClients);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      Client testClient = new Client("Mcdonald", 2);

      testClient.Save();
      Client savedClient = Client.GetAll()[0];

      int result = savedClient.GetId();
      int testId = testClient.GetId();
      Assert.Equal(testId, result);
    }


    [Fact]
    public void Test_FindFindsClientInDatabase()
    {
      //Arrange
      Client testClient = new Client("Wendy", 2);
      testClient.Save();

      //Act
      Client foundClient = Client.Find(testClient.GetId());

      //Assert
      Assert.Equal(testClient, foundClient);
    }

    [Fact]
    public void Test_Update_UpdatesClientInDatabase()
    {
      //Arrange
      string name = "Mag";
      Client testClient = new Client(name, 2);
      testClient.Save();
      string newName = "meg";

      //Act
      testClient.Update(newName);

      string result = testClient.GetName();

      //Assert
      Assert.Equal(newName, result);
    }

    [Fact]
    public void Test_DeleteOneClientFromDatabase()
    {
        Client testClient = new Client("Morgan", 3);
        testClient.Save();
        Client testClient2 = new Client("Anna", 2);
        testClient2.Save();

        testClient.Delete();

        int result = Client.GetAll().Count;
        Assert.Equal(1, result);
    }

    public void Dispose()
    {
      Client.DeleteAll();
    }

  }
}
