using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Data.SqlClient;

namespace Dam
{


  public  class  BackAndRestore
    {
        string BackupConString = @"data source=(LocalDB)\MSSQLLocalDB;attachdbfilename=|DataDirectory|\Data\DamDB.mdf;initial catalog=DamDB integrated security=True;connect timeout=30;MultipleActiveResultSets=True;";
        string RestoreConString = @"Data Source=(LocalDB)\v11.0;Integrated Security=True;multipleactiveresultsets=True";


        public  void Backup(string Filepath)
        {
            using (SqlConnection con = new SqlConnection(BackupConString))
            {
                ServerConnection srvConn = new ServerConnection(con);
                Server srvr = new Server(srvConn);

                if (srvr != null)
                {
                    try
                    {
                        System.Windows.Forms.SaveFileDialog openfd = new System.Windows.Forms.SaveFileDialog();
                        openfd.Filter = "Backup File (*.Bak)|*.Bak";

                        if (openfd.ShowDialog()== System.Windows.Forms.DialogResult.OK)
                        {
                            Backup backupdb = new Backup();
                            backupdb.Action = BackupActionType.Database;
                            backupdb.Database = "DamDB";
                            BackupDeviceItem bkpDevice = new BackupDeviceItem(openfd.FileName, DeviceType.File);
                            backupdb.Devices.Add(bkpDevice);
                            backupdb.SqlBackup(srvr);
                            System.Windows.Forms.MessageBox.Show("Backup Database Successfully");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Error", ex.Message);
                    }
                }
            }
        }




        internal  void Restore(string Filepath)
        {
            SqlConnection.ClearAllPools();
            using (SqlConnection con = new SqlConnection(RestoreConString))
            {
                ServerConnection srvConn = new ServerConnection(con);
                Server srvr = new Server(srvConn);

                if (srvr != null)
                {
                    try
                    {
                        Restore restoredb = new Restore();
                        restoredb.Action = RestoreActionType.Database;
                        restoredb.Database = "DamDB";
                     

                      
                            BackupDeviceItem bkpDevice = new BackupDeviceItem(Filepath, DeviceType.File);

                            restoredb.Devices.Add(bkpDevice);
                            restoredb.ReplaceDatabase = true;
                            restoredb.SqlRestore(srvr);
                           System.Windows.Forms.MessageBox.Show("Restore Database Successfully");
                        
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("E rror",ex.Message);
                    }
                }
            }
        }
    }
    public static class Extensions
    {
        public static DateTime? GetSafeDateTime(this DateTime? input)
        {
            if (input == null)
            {
                return DateTime.MinValue;
            }
            else
            {
                return input;
            }
        }


    }
}
