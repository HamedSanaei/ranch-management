using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Data.SqlClient;


using Dam.Data;

namespace Dam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Forms.NotifyIcon noti = new System.Windows.Forms.NotifyIcon();

        public MainWindow()
        {
            InitializeComponent();
            DamDBEntities damDB = new DamDBEntities();
           
             Datagrid1.ItemsSource = damDB.Sheep.ToList();
            int counter = 0;
            //foreach (DataGridRow Row in Datagrid1.Items)
            //{
            //    counter++;
            //    Row.;

            //}
        }

        void UpdateIsSenior()
        {
           var timespan =  new TimeSpan(90, 0, 0, 1);
            var noww = DateTime.Now;
            DamDBEntities damDB = new DamDBEntities();

            foreach (Sheep item in damDB.Sheep.Where(p => DbFunctions.DiffDays(p.Birthday, p.DateOfDeath) >= 90 && (p.IsDead ?? false)))
            {
                item.IsSenior = true;// balegh bodeh va mordeh

            }

            foreach (Sheep item in damDB.Sheep.Where(p => DbFunctions.DiffDays(p.Birthday, noww) >= 90 && (!p.IsDead ?? false)  ))
            {
                item.IsSenior = true;// balegh ast va namordeh

            }
            damDB.SaveChanges();
        }

       
        
        private void Hamed(object sender, RoutedEventArgs e)
        {
            ////
        }

        internal void AddSheep(object sender, RoutedEventArgs e)
        {

           AddSheepMenu menu = new AddSheepMenu();
            menu.ShowDialog();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void EditSheep(object sender, RoutedEventArgs e)
        {
            try { 
            int id = Convert.ToInt32((Datagrid1.SelectedCells[1].Column.GetCellContent(Datagrid1.SelectedItem) as TextBlock).Text);
            if (MessageBox.Show("آیا از تغییر سطر انتخابی مطمئن هستید؟","تاییدیه",MessageBoxButton.YesNo ,MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DamDBEntities damDB = new DamDBEntities();
                // Datagrid1.SelectedCells[0].Column.GetCellContent(Datagrid1.SelectedItem);
             


                //MessageBox.Show((Datagrid1.SelectedCells[1].Column.GetCellContent(Datagrid1.SelectedItem) as TextBlock).Text);
                AddSheepMenu menu = new AddSheepMenu(damDB.Sheep.Where(p => p.Id == id).FirstOrDefault());
                menu.ShowDialog();

            }
            }
            catch(Exception ex)
            {
                MessageBox.Show("لطفا عملیات را روی یک سطر انجام دهید", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            //DamDBEntities21 fuck = new DamDBEntities21();
            // Datagrid1.ItemsSource = fuck.Sheep.ToList();
            UpdateIsSenior();
            Datagrid1.ItemsSource = GetViewModels();
            //DamDBEntities damDB = new DamDBEntities();
            //// Datagrid1.ItemsSource = damDB.Sheep.ToList() ;
            //int counter = 0;
            //List<SheepViewModel> sheeps = new List<SheepViewModel>();
            //foreach (Sheep item in damDB.Sheep)
            //{
            //    counter++;
            //    sheeps.Add(new SheepViewModel(item.Id, counter, item.Birthday, item.DateOfDeath, item.DateOfSell, item.DeliveryDate, item.Parent_Id, item.IsSenior, item.IsSold, item.Description, item.IsDead));

            //}
            //Datagrid1.ItemsSource = sheeps;

        } 
        List<SheepViewModel> GetViewModels()
        {
            DamDBEntities damDB = new DamDBEntities();
            int counter = 0;
            List<SheepViewModel> sheeps = new List<SheepViewModel>();
            foreach (Sheep item in damDB.Sheep)
            {
                counter++;
                sheeps.Add(new SheepViewModel(item.Id, counter, item.Birthday, item.DateOfDeath, item.DateOfSell, item.DeliveryDate, item.Parent_Id, item.IsSenior, item.IsSold, item.Description, item.IsDead));

            }
            return sheeps;
        }

        //private ObservableCollection<Sheep> GetAllEmployees()
        //{
        //    DamDBEntities damDBEntities = new DamDBEntities();
        //    var empResult = from emp in damDBEntities.Sheep
        //                    select new {Birthday = new PersianDate((DateTime)emp.Birthday) };
        //    return new ObservableCollection< typeof() >(empResult);
        //}
        private void RemoveSheep(object sender, RoutedEventArgs e)
        {


          try {
            int id = Convert.ToInt32((Datagrid1.SelectedCells[1].Column.GetCellContent(Datagrid1.SelectedItem) as TextBlock).Text);
            try
            {
                if (MessageBox.Show("آیا از حذف سطر انتخابی مطمئن هستید؟", "تاییدیه", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    DamDBEntities damDB = new DamDBEntities();
                    // Datagrid1.SelectedCells[0].Column.GetCellContent(Datagrid1.SelectedItem);
                    //  string fffff= (Datagrid1.SelectedCells[1].Column.GetCellContent(Datagrid1.SelectedItem) as TextBlock).Text;


                    Sheep tempsheep = damDB.Sheep.Where(s => s.Id == id).FirstOrDefault();
                    //MessageBox.Show((Datagrid1.SelectedCells[1].Column.GetCellContent(Datagrid1.SelectedItem) as TextBlock).Text);
                    damDB.Sheep.Remove(tempsheep);
                    damDB.SaveChanges();
                    MessageBox.Show("سطر مورد نظر با موفقیت حذف شد", "اطلاعیه", MessageBoxButton.OK, MessageBoxImage.Hand);



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(" شما نمیتوانید این دام را حذف کنید. این دام در حال حاضر به عنوان والد دام دیگری انتخاب شده است. اگر میخواهید آنرا حذف کنید  باید قسمت والد فرزندان آنرا خالی کنید یا از گزینه بدون فرزند کردن استفاده کنید", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
          }
           catch (Exception ex)
          {
                MessageBox.Show("لطفا عملیات را روی یک سطر انجام دهید", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
          }
        }

        private void Report(object sender, RoutedEventArgs e)
        {
           new ReportMenu().ShowDialog();
        }

        void UpdateDatagrid()
        {
            List<DataGridRow> dataGridRows = new List<DataGridRow>();
            DamDBEntities damDB = new DamDBEntities();
           int counter= 0;
           //  Datagrid1.Items.Clear();
            foreach (Sheep Row in damDB.Sheep)
            {
                
                counter++;
                Datagrid1.Items.Add(new DataGridRow());
                (Datagrid1.SelectedCells[1].Column.GetCellContent(Datagrid1.SelectedItem) as TextBlock).Text = Row.Id.ToString();
                (Datagrid1.SelectedCells[0].Column.GetCellContent(Datagrid1.SelectedItem) as TextBlock).Text = counter.ToString();
                (Datagrid1.SelectedCells[2].Column.GetCellContent(Datagrid1.SelectedItem) as TextBlock).Text = Row.Parent_Id.ToString();

            }
            Datagrid1.Items.Add(new DataGridRow());

          
        }

        private void RibbonButton_Click(object sender, RoutedEventArgs e)
        {
            //foreach (DataRowView row in Datagrid1.SelectedItems)
            //{
            //    string text = row.Row.ItemArray[1].ToString();
            //    MessageBox.Show(text);
            //}
            PersianDateTime a = PersianDateTime.Now;
            string ijgb=  a.ToDateTime().ToString();
            DateTime dig = DateTime.Now;
            
        }

        private void EditChildes(object sender, RoutedEventArgs e)
        {
            try { 
            int id = Convert.ToInt32((Datagrid1.SelectedCells[1].Column.GetCellContent(Datagrid1.SelectedItem) as TextBlock).Text);
            if (MessageBox.Show("در صورت تایید این دام دیگر والد دام دیگری نخواهد بود. آیا موافقید؟", "تاییدیه", MessageBoxButton.YesNo, MessageBoxImage.Asterisk) == MessageBoxResult.Yes)
            {
                DamDBEntities damDBEntities = new DamDBEntities();
                foreach (Sheep item in damDBEntities.Sheep.Where(p => p.Id == id).SingleOrDefault().Sheep1)
                {
                    item.Parent_Id = null;
                }
                damDBEntities.SaveChanges();
                Window_Activated(this, new EventArgs());
            }
        }
           catch (Exception ex)
          {
                MessageBox.Show("لطفا عملیات را روی یک سطر انجام دهید", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
          }
}

        private void ViewReport(object sender, RoutedEventArgs e)
        {
            new ReportMenu().ShowDialog();
        }

        private void CopyToClipboard(object sender, RoutedEventArgs e)
        {
          try { 
            int id = Convert.ToInt32((Datagrid1.SelectedCells[1].Column.GetCellContent(Datagrid1.SelectedItem) as TextBlock).Text);
            DamDBEntities damDBEntities = new DamDBEntities();
            string st = damDBEntities.Sheep.Where(p => p.Id == id).FirstOrDefault().ToString();
            Clipboard.SetDataObject(st);
          }
            catch (Exception ex)
            {
                MessageBox.Show("لطفا عملیات را روی یک سطر انجام دهید", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Guidance(object sender, RoutedEventArgs e)
        {
            new Guidance().Show();
        }

        private void AboutUs_Load(object sender, RoutedEventArgs e)
        {
            new AboutUs().Show();
        }

        private void Update_load(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("نسخه جدیدی وجود ندارد", "به روز رسانی", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
           // System.Drawing.Icon fa = new System.Drawing.Icon();
            noti.Icon =  Properties.Resources.sheep_bFK_icon;
            noti.Visible = true;
            noti.Click += new  EventHandler(NotifyIcon1_MouseDoubleClick);
          
        }
        private void NotifyIcon1_MouseDoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Show();
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Minimized;
                this.Hide();
            }
            this.Activate();
        }

        private void Backup(object sender, RoutedEventArgs e)
        {
            //System.Windows.Forms.SaveFileDialog openfd = new System.Windows.Forms.SaveFileDialog();
            //  openfd.Filter = "Backup File (*.Bak)|*.Bak";
            //  string pathString = AppDomain.CurrentDomain.BaseDirectory + @"Backup\";
            //  System.IO.Directory.CreateDirectory(pathString);
            //  openfd.InitialDirectory = pathString ;
            //  openfd.Filter = "Backup File (*.Bak)|*.Bak";
            //  openfd.FileName = PersianDateTime.Now.ToString("ddMMyyyy_HHmmss");
            //  if (openfd.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            //  {
            //     new BackAndRestore().Backup(openfd.FileName);
            //  }

            //string BackupConString = @"data source=(LocalDB)\MSSQLLocalDB;attachdbfilename=|DataDirectory|\Data\DamDB.mdf;initial catalog=DamDB integrated security=True;connect timeout=30;MultipleActiveResultSets=True;";
            //string RestoreConString = @"Data Source=(LocalDB)\v11.0;Integrated Security=True;multipleactiveresultsets=True";
            //using (SqlConnection con = new SqlConnection(BackupConString))
            //{
            //    ServerConnection srvConn = new ServerConnection(con);
            //    Server srvr = new Server(srvConn);
            //    if (srvr != null)
            //    {
            //        try
            //        {
            //            System.Windows.Forms.SaveFileDialog openfd = new System.Windows.Forms.SaveFileDialog();
            //            openfd.Filter = "Backup File (*.Bak)|*.Bak";

            //            if (openfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //            {
            //                Backup backupdb = new Backup();
            //                backupdb.Action = BackupActionType.Database;
            //                backupdb.Database = "DamDB";
            //                BackupDeviceItem bkpDevice = new BackupDeviceItem(openfd.FileName, DeviceType.File);
            //                backupdb.Devices.Add(bkpDevice);
            //                backupdb.SqlBackup(srvr);
            //                System.Windows.Forms.MessageBox.Show("Backup Database Successfully");
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            System.Windows.Forms.MessageBox.Show("Error", ex.Message);
            //        }
            //    }
            //}

            SqlConnection sqlConnection = new SqlConnection("Server=.;Database=DamDB;Trusted_Connection=True;");
            //ServerConnection serverconnection = new ServerConnection(new SqlConnection("Data Source = DESKTOP - FUDCNPR; Initial Catalog = DamDB; Integrated Security = True"));
            ServerConnection serverconnection = new ServerConnection(sqlConnection);
            Server server = new Server();
            Backup backup = new Backup() { Action = BackupActionType.Database , Database= "DamDB", };
            backup.Devices.AddDevice(@"C:\Data\hamed.bak", DeviceType.File);
            backup.Initialize = true;
            backup.SqlBackup(server);













        }
        private void Restore(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openfd = new System.Windows.Forms.OpenFileDialog();
            openfd.Filter = "Backup File (*.Bak)|*.Bak";
            string pathString = AppDomain.CurrentDomain.BaseDirectory + @"Backup\";
            System.IO.Directory.CreateDirectory(pathString);
            openfd.InitialDirectory = pathString;
            openfd.Filter = "Backup File (*.Bak)|*.Bak";
          
            if (openfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
             new BackAndRestore().Restore(openfd.FileName);
            }

        }
    }
}
