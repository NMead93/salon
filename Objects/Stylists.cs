using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Salon.Objects
{
    public class Stylist
    {
        private int _id;
        private string _name;

        public Stylist(string name, int id = 0)
        {
            _id = id;
            _name = name;
        }

        public string GetName()
        {
            return _name;
        }

        public int GetId()
        {
            return _id;
        }

        public override bool Equals(System.Object otherStylist)
        {
            if (!(otherStylist is Stylist))
            {
                return false;
            }
            else
            {
                Stylist newStylist = (Stylist) otherStylist;
                bool idEquality = this.GetId() == newStylist.GetId();
                bool nameEquality = this.GetName() == newStylist.GetName();
                return (idEquality && nameEquality);
            }
        }

        public static List<Stylist> GetAll()
        {
            List<Stylist> allStylists = new List<Stylist>{};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM stylists;", conn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                int StylistId = rdr.GetInt32(0);
                string StylistName = rdr.GetString(1);
                Stylist newStylist = new Stylist(StylistName, StylistId);
                allStylists.Add(newStylist);
            }

            return allStylists;
        }

        public void Save()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO stylists (name) OUTPUT INSERTED.id VALUES (@StylistName);", conn);

            SqlParameter nameParameter = new SqlParameter("@StylistName", this.GetName());
            cmd.Parameters.Add(nameParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if(conn != null)
            {
                conn.Close();
            }
        }

        public static Stylist Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM stylists WHERE id = @StylistId;", conn);
            SqlParameter StylistIdParameter = new SqlParameter("@StylistId", id);
            cmd.Parameters.Add(StylistIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            int foundStylistId = 0;
            string foundStylistName = null;

            while(rdr.Read())
            {
                foundStylistId = rdr.GetInt32(0);
                foundStylistName = rdr.GetString(1);
            }
            Stylist foundStylist = new Stylist(foundStylistName, foundStylistId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return foundStylist;
        }

        public List<Client> GetClients()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();


            SqlCommand cmd = new SqlCommand("SELECT * FROM clients WHERE stylist_id = @Stylist_Id;", conn);
            SqlParameter stylistIdParameter = new SqlParameter("@Stylist_Id", this.GetId());

            cmd.Parameters.Add(stylistIdParameter);
            SqlDataReader rdr = cmd.ExecuteReader();

            List<Client> allClients = new List<Client>{};
            while(rdr.Read())
            {
                int ClientId = rdr.GetInt32(0);
                string ClientName = rdr.GetString(1);
                int StylistId = rdr.GetInt32(2);
                Client newClient = new Client(ClientName, StylistId, ClientId);

                allClients.Add(newClient);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return allClients;
        }

        public void Update(string newName)
        {
          SqlConnection conn = DB.Connection();
          conn.Open();

          SqlCommand cmd = new SqlCommand("UPDATE stylists SET name = @NewName OUTPUT INSERTED.name WHERE id = @StylistId;", conn);

          SqlParameter newNameParameter = new SqlParameter();
          newNameParameter.ParameterName = "@NewName";
          newNameParameter.Value = newName;
          cmd.Parameters.Add(newNameParameter);


          SqlParameter stylistIdParameter = new SqlParameter();
          stylistIdParameter.ParameterName = "@StylistId";
          stylistIdParameter.Value = this.GetId();
          cmd.Parameters.Add(stylistIdParameter);
          SqlDataReader rdr = cmd.ExecuteReader();

          while(rdr.Read())
          {
            this._name = rdr.GetString(0);
          }

          if (rdr != null)
          {
            rdr.Close();
          }

          if (conn != null)
          {
            conn.Close();
          }
        }

        public void Delete()
       {
          SqlConnection conn = DB.Connection();
          conn.Open();

          SqlCommand cmd = new SqlCommand("DELETE FROM stylists WHERE id = @StylistId; DELETE FROM clients WHERE stylist_id = @StylistId;", conn);

          SqlParameter stylistIdParameter = new SqlParameter("@StylistId", this.GetId());

          cmd.Parameters.Add(stylistIdParameter);
          cmd.ExecuteNonQuery();

          if (conn != null)
          {
            conn.Close();
          }

       }

        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM stylists;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}
