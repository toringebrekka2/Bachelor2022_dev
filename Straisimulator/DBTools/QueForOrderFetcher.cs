using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Globalization;
using Microsoft.Data.SqlClient;
using Straisimulator.AppTools;

namespace Straisimulator.DBTools;

public class QueForOrderFetcher
{
    private int _orderFetched;
    private QueForOrder _queForOrder;
    
    public QueForOrderFetcher(int orderFetched, int queForOrderId)
    {
        _orderFetched = orderFetched;
        _queForOrder = new QueForOrder(queForOrderId);
        Fetch();
    }

    public void Fetch()
    {
        string _connString = @"Server=localhost; Database=straisimulator; Trusted_Connection=True; User Id=sa; password=Alphabravocharlie123; MultipleActiveResultSets=true";

        try
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                string query = @"SELECT id, QueStart, QueEnd FROM Test_Data;";
                SqlCommand command = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader dr = command.ExecuteReader();
                
                Console.WriteLine(Environment.NewLine + "Retrieving data from database..." + Environment.NewLine);
                Console.WriteLine("Retrieved records:");
                
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        int testDataId = dr.GetInt32(0);
                        DateTime queStart = dr.GetDateTime(1);
                        DateTime queEnd = dr.GetDateTime(2);
                        Que queObj = new Que(testDataId, queStart, queEnd);
                        _queForOrder.AddGeneralQue(queObj);
                        
                        //display retrieved records
                        Console.WriteLine("Que on id {0}: {1} - {2}", testDataId, queStart, queEnd);
                    }
                }
                else
                {
                    Console.WriteLine("No data found.");
                }
                dr.Close();
                conn.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
        }
    }

    public QueForOrder GetQueForOrder()
    {
        return _queForOrder;
    }
}