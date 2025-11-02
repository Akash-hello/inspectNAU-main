using DataBank;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteDatabase : MonoBehaviour
{
    table_MyStocks mlocationDb1;

   public void InitiliaseDropTables()
    {
        table_MyStocks mlocationDb1 = new table_MyStocks();
        using var connection1 = mlocationDb1.getConnection();
        mlocationDb1.DropTable();

        table_LoginConfig mlocationDb = new table_LoginConfig();
        using var connection = mlocationDb.getConnection();
        mlocationDb.DropTable();

        connection.Close();
        connection1.Close();
    }
}
