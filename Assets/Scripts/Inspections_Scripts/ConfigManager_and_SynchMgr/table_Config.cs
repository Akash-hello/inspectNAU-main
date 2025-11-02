using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DataBank

{
    public class table_Config : SqliteHelper

    //Now, lets create our Config table. I will call this class table_Config, however, it is just a table in the OrionDB, 
    //this tables communicates with the SqliteHelper. It seems to act like a database and we shall treat it that way too, however its just a table.
    {
        private const String Tag = "Orion: table_Config:\t";
        private const String TABLE_NAME = "Config";
        private const String KEY_ID = "Id";
        private const String KEY_MACHINEID = "MachineId";
        private const String KEY_HTTPS = "Https";
        private const String KEY_SERVERIP = "ServerIP";
        private const String KEY_SERVERPORT = "ServerPort";
        private const String KEY_ORIONCONNECT = "OrionConnect";
        private const String KEY_INCOMINGAPI = "IncomingAPI";
        private const String KEY_OUTGOINGAPI = "OutGoingAPI";
        private const String KEY_LASTLOGINDATETIME = "LastLoginDateTime";
        private const String KEY_TERMSANDCONDITIONS = "TermsAndConditions";
        private const String KEY_NAMEONCARD = "NameOnCard";
        private const String KEY_CREDITCARDNUMBER = "CreditCardNumber";
        private const String KEY_CREDITCARDCVV = "CreditCardCVV";
        private const String KEY_CREDITCARDEXPIRY = "CreditCardExpiry";
        private const String KEY_PAYMENTHISTORY = "PaymentHistory";
        private const String KEY_TIMESTAMP = "TimeStamp";
        private String[] COLUMNS = new String[] { KEY_ID, KEY_MACHINEID, KEY_SERVERIP, KEY_SERVERPORT,KEY_INCOMINGAPI ,KEY_OUTGOINGAPI,KEY_ORIONCONNECT, KEY_LASTLOGINDATETIME, KEY_TERMSANDCONDITIONS, KEY_NAMEONCARD, KEY_CREDITCARDNUMBER, KEY_CREDITCARDCVV, KEY_CREDITCARDEXPIRY, KEY_PAYMENTHISTORY, KEY_TIMESTAMP };

        public table_Config() : base()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " ( " +
                KEY_ID + " TEXT PRIMARY KEY, " +
                KEY_MACHINEID + " TEXT, " +
                KEY_HTTPS + " TEXT, " +
                KEY_SERVERIP + " TEXT, " +
                KEY_SERVERPORT + " TEXT, " +
                KEY_INCOMINGAPI + " TEXT, " +
                KEY_OUTGOINGAPI + " TEXT, " +
                KEY_ORIONCONNECT + " TEXT, " +
                KEY_LASTLOGINDATETIME + " DATETIME DEFAULT CURRENT_TIMESTAMP, " +
                KEY_TERMSANDCONDITIONS + " TEXT, " +
                KEY_NAMEONCARD + " TEXT, " +
                KEY_CREDITCARDNUMBER + " TEXT, " +
                KEY_CREDITCARDCVV + " TEXT, " +
                KEY_CREDITCARDEXPIRY + " TEXT, " +
                KEY_PAYMENTHISTORY + " TEXT, " +
                KEY_TIMESTAMP + " DATETIME DEFAULT CURRENT_TIMESTAMP )";

            dbcmd.ExecuteNonQuery();
           
            Debug.Log("TABLE CONFIG CREATED");
        }

        public void addData(ConfigEntity configdata)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "INSERT INTO " + TABLE_NAME
                + " (" + KEY_ID + ", " + KEY_MACHINEID + ", " + KEY_HTTPS + ", " + KEY_SERVERIP + ", " + KEY_SERVERPORT + ", " + KEY_INCOMINGAPI + ", " + KEY_OUTGOINGAPI + ", " + KEY_ORIONCONNECT + ", " + KEY_TERMSANDCONDITIONS + ", " + KEY_NAMEONCARD + ", " + KEY_CREDITCARDNUMBER + ", " + KEY_CREDITCARDCVV + ", " + KEY_CREDITCARDEXPIRY + ", " + KEY_PAYMENTHISTORY + ")"

                + "VALUES ( ' " + configdata._id + " ' ,' " + configdata._machineid + " ' ,' " + configdata._https + " ' , ' " + configdata._ServerIP + " ' , ' " + configdata._ServerPort + " ', ' " + configdata._IncomingAPI + " ', ' " + configdata._OutboundAPI + " ', ' " + configdata._orionconnect + " ' , ' " + configdata._termsandconditions + " ', ' " + configdata._nameoncard + " ', ' " + configdata._creditcardnumber + " ', ' " + configdata._creditcardcvv + " ', ' " + configdata._creditcardexpiry + " ', ' " + configdata._paymenthistory + " ')";

            dbcmd.ExecuteNonQuery();

        }

        public override IDataReader getDataByString()
        {
            return base.getAllData(TABLE_NAME);
        }

        public IDataReader NotyetProcessed()
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText =
                "SELECT * FROM " + TABLE_NAME + " WHERE " + KEY_SERVERIP + " LIKE '%N%'";
            return dbcmd.ExecuteReader();

        }
        public override void deleteAllData()
        {
            Debug.Log(Tag + "Deleting Table");

            base.deleteAllData(TABLE_NAME);

        }

        public override void vacuumAllData()
        {
            Debug.Log(Tag + "Table Vacuum");

            base.vacuumAllData(TABLE_NAME);

        }
        public override void DropTable()
        {
            Debug.Log(Tag + "Dropping Table");

            base.DropTable(TABLE_NAME);
            
        }

        public void Updatedata(string columndataquery)
        {
            IDbCommand dbcmd = getDbCommand();
            dbcmd.CommandText = "UPDATE " + TABLE_NAME + " SET " + columndataquery;

            Debug.Log("THIS WAS THE COMMAND EXECUTED" + dbcmd.CommandText.ToString());
            dbcmd.ExecuteNonQuery();
        }

    }
}