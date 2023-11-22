using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Arash;
using Dam.Data;

namespace Dam
{
    /// <summary>
    /// Interaction logic for ReportMenu.xaml
    /// </summary>
    public partial class ReportMenu : Window
    {
        public ReportMenu()
        {
            InitializeComponent();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
           
            this.Close();

        }

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Calculate();
        }
        private void RemoveSheep(object sender, RoutedEventArgs e)
        {
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
                MessageBox.Show("شما نمیتوانید این دام را حذف کنید. این دام در حال حاضر به عنوان والد دام دیگری انتخاب شده است. اگر میخواهید آنرا حذف کنید  باید قسمت والد فرزندان آنرا خالی کنید!", "خطا", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Calculate();
        }
        internal void AddSheep(object sender, RoutedEventArgs e)
        {
            AddSheepMenu menu = new AddSheepMenu();
            menu.ShowDialog();
        }
        private void EditSheep(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32((Datagrid1.SelectedCells[1].Column.GetCellContent(Datagrid1.SelectedItem) as TextBlock).Text);
            if (MessageBox.Show("آیا از تغییر سطر انتخابی مطمئن هستید؟", "تاییدیه", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                DamDBEntities damDB = new DamDBEntities();
                // Datagrid1.SelectedCells[0].Column.GetCellContent(Datagrid1.SelectedItem);



                //MessageBox.Show((Datagrid1.SelectedCells[1].Column.GetCellContent(Datagrid1.SelectedItem) as TextBlock).Text);
                AddSheepMenu menu = new AddSheepMenu(damDB.Sheep.Where(p => p.Id == id).FirstOrDefault());
                menu.ShowDialog();
                
            }

        }
        List<SheepViewModel> GetViewModelsWithConsiderConditions()
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
        private void Calculate()
        {
            DateTime fromdate = FromDateDatepicker.SelectedDate.ToDateTime();
            DateTime todate = ToDateDatepicker.SelectedDate.ToDateTime();
            string today = DateTime.Now.ToShortDateString();

            DamDBEntities damDBEntities = new DamDBEntities();
            IQueryable<Sheep> queryable = damDBEntities.Sheep.Where(p => p.DeliveryDate >= fromdate && p.DeliveryDate <= todate);
            int counter = 0;
            List<SheepViewModel> sheeps = new List<SheepViewModel>();
            foreach (Sheep item in queryable)
            {
                counter++;
                sheeps.Add(new SheepViewModel(item.Id, counter, item.Birthday, item.DateOfDeath, item.DateOfSell, item.DeliveryDate, item.Parent_Id, item.IsSenior, item.IsSold, item.Description, item.IsDead));
            }
            Datagrid1.ItemsSource = sheeps;

            int numberofdeadsheeps = queryable.Where(p => p.IsDead == true).Count();
            int numberofalivesheeps = queryable.Where(p => p.IsDead == false).Count();
            int numberofallsheeps = queryable.Count();
            int numberofjoniorsheeps = queryable.Where(p => p.IsSenior == false).Count();
            int numberofseniorsheeps = queryable.Where(p=> p.IsSenior == true).Count();
            int nemberofSoldSheeps = queryable.Where(p => p.IsSold == true).Count();
            int nemberofnotSoldSheeps = queryable.Where(p => p.IsSold == false).Count();
            int SumOfCost = 0;
            foreach (Sheep item in queryable)
            {
                DateTime firstPoint;
                DateTime lastpoint;




                if ((!item.IsDead ?? false) && (!item.IsSold ?? false)) // agar namordeh va forosh narafte
                {
                    lastpoint = DateTime.Now; // ta zaman hal mohasebe kon
                }
                else if ((item.IsDead ?? false) && (!item.IsSold ?? false)) // agar mordeh vali forosh narafte
                {
                    lastpoint = item.DateOfDeath.Value ;// agar ayandeh morde ta ayande mohasebe kon
                }
                else if ((!item.IsDead ?? false) && (item.IsSold ?? false))// agar namorde va forosh rafte
                {
                    lastpoint = item.DateOfSell.Value;// ta zaman forosh mohasebe kon
                }
                else  // agar morde va forosh rafte
                {
                    lastpoint = (item.DateOfSell.Value >= item.DateOfSell.Value) ? item.DateOfSell.Value : item.DateOfSell.Value ;  // har kodom kootah tar bood
                }


                 firstPoint =(item.Birthday +new TimeSpan(90, 0, 0, 0) >= item.DeliveryDate) ? (item.Birthday + new TimeSpan(90, 0, 0, 0)).Value : item.DeliveryDate.Value;
                
                if ((lastpoint - firstPoint)>=new TimeSpan(1,0,0,0,0)) // agar hade aghal ye roz ye gosfand balegh negah dari shode bod
                {
                    if (lastpoint>firstPoint)
                    {
                        SumOfCost += (lastpoint - firstPoint).Days;
                    }
                }
              

            }
            TotalCost.Content = SumOfCost.ToString();
            Deadlbl.Content = numberofdeadsheeps;
            Alivelbl.Content = numberofalivesheeps;
            Alllbl.Content = numberofallsheeps;
            Joniorlbl.Content= numberofjoniorsheeps;
            Seniorlbl.Content = numberofseniorsheeps;
            Soldlbl.Content = nemberofSoldSheeps;
            NotSoldlbl.Content = nemberofnotSoldSheeps;
        }

     
    }
}
