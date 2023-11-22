using Arash;
using Dam.Data;
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

namespace Dam
{
    /// <summary>
    /// Interaction logic for AddSheepMenu.xaml
    /// </summary>
    public partial class AddSheepMenu : Window
    {
        internal bool AddOrEdit { get; set; } //add is true and edit is false
        private Sheep editsheep { get; set; }

        DamDBEntities damDBEntities = new DamDBEntities();
        public AddSheepMenu() //for add
        {
            InitializeComponent();
            AddOrEdit = true;
            soldnoradio.IsChecked = true;
            Isseniorno.IsChecked = true;
            Isdeadradiono.IsChecked = true;
            var q = from sh in damDBEntities.Sheep.ToList()
                    select sh.Id;
            if (q.Count() >= 0)
            {
                List<int> temp = new List<int>();
                foreach (var item in q.ToList())
                {

                    ParentCombo.Items.Add(Convert.ToInt32(item));
                }

            }
         
           
        }

        public AddSheepMenu(Sheep sheep)///for edit
        {
            InitializeComponent();
            AddOrEdit = false;
            editsheep = sheep;



            var q = from sh in damDBEntities.Sheep.ToList()
                    select sh.Id;
            if (q.Count() >= 0)
            {
                List<int> temp = new List<int>();
                foreach (var item in q.ToList())
                {
                    ParentCombo.Items.Add(Convert.ToInt32(item));
                }
                ParentCombo.Items.Remove(sheep.Id);// prevent a sheep be parent of itself !!!
            }


            if (editsheep.IsSold ?? false)
            {
                soldyesradio.IsChecked = true;

            }
            else
            {
                soldnoradio.IsChecked = true;
            }
            if (editsheep.IsSenior ?? false)
            {
                Issenioryes.IsChecked = true;

            }
            else
            {
                Isseniorno.IsChecked = true;
            }

            if (editsheep.IsDead ?? false)
            {
                Isdeadradioyes.IsChecked = true;
                DeathDatepicker.SelectedDate = new PersianDate((DateTime)editsheep.DateOfDeath);
            }
            else
            {
                Isdeadradiono.IsChecked = true;
            }

            if (editsheep.IsSold ?? false)
            {
               
                soldyesradio.IsChecked = true;
                SoldDatepicker.SelectedDate = new PersianDate((DateTime)editsheep.DateOfSell);
            }
            else
            {
                soldnoradio.IsChecked = true;
            }
            BirthdayDatepicker.SelectedDate = new PersianDate((DateTime)editsheep.Birthday);

            DeliveryDatepicker.SelectedDate = new PersianDate((DateTime)editsheep.DeliveryDate);
            
            DescriptionTextbox.Text = editsheep.Description;
            ParentCombo.SelectedValue = editsheep.Parent_Id;

            


        }
        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void AddSheep(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AddOrEdit)///add
                {
                    string errors=string.Empty;
                    bool haveError = false;
                    if (DeliveryDatepicker.SelectedDate < BirthdayDatepicker.SelectedDate)
                    {
                        errors += "تاریخ تحویل قبل از تاریخ تولد است! \n";
                         haveError = true;
                    }
                    //if (de)
                    //{

                    //}

                    if (haveError)
                    {
                        throw new Exception(errors);
                    }

                    bool issold = false;
                    bool issenior = false;
                    bool isdead = false;
                    if (Isdeadradioyes.IsChecked ?? false)
                    {
                        isdead = true;

                    }
                    else
                    {
                        isdead = false;
                    }
                    if (soldyesradio.IsChecked ?? false)
                    {
                        issold = true;
                    }
                    else if (soldnoradio.IsChecked ?? false)
                    {
                        issold = false;
                    }
                    else
                    {
                        issold = false;
                    }

                    if (Issenioryes.IsChecked ?? false)
                    {
                        issenior = true;
                    }
                    else if (Isseniorno.IsChecked ?? false)
                    {
                        issenior = false;
                    }
                    else
                    {
                        issenior = false;
                    }
                    Nullable<int> tempparentid;
                    if (ParentCombo.SelectedIndex != -1)
                    {
                        tempparentid = Convert.ToInt32(ParentCombo.SelectedValue);
                    }
                    else
                    {
                        tempparentid = null;

                    }
                    Sheep sheep = new Sheep()
                    {
                        Birthday = BirthdayDatepicker.SelectedDate.ToDateTime(),
                        DateOfDeath = DeathDatepicker.SelectedDate.ToDateTime(),
                        DateOfSell = SoldDatepicker.SelectedDate.ToDateTime(),
                        DeliveryDate = DeliveryDatepicker.SelectedDate.ToDateTime(),
                        Parent_Id = tempparentid,
                        Description = DescriptionTextbox.Text,
                        IsSenior = issenior,
                        IsSold = issold,
                        IsDead = isdead
                    };
                    if (!isdead)
                    {
                        sheep.DateOfDeath = null;
                    }
                     if (!issold)
                     {
                         sheep.DateOfSell = null;
                     }
                      damDBEntities.Sheep.Add(sheep);
                    // DamDBEntities2.People.Add(new Person() { Firstname = "hamed" });
                    damDBEntities.SaveChanges();

                    MessageBox.Show("دام مورد نظر اضافه شد ");
                    ClearValues();

                    //MessageBox.Show(BirthdayDatepicker.SelectedDate.ToDateTime().ToString());
                    //MessageBox.Show(BirthdayDatepicker.SelectedDate.ToString());
                    //DeathDatepicker.SelectedDate = new PersianDate(DateTime.Now);
                }
                else ///edit
                {
                    bool issold = false;
                    bool issenior = false;
                    bool isdead = false;
                    if (soldyesradio.IsChecked ?? false)
                    {
                        issold = true;
                    }
                    else if (soldnoradio.IsChecked ?? false)
                    {
                        issold = false;
                    }
                    else
                    {
                        issold = false;
                    }

                    if (Issenioryes.IsChecked ?? false)
                    {
                        issenior = true;
                    }
                    else if (Isseniorno.IsChecked ?? false)
                    {
                        issenior = false;
                    }
                    else
                    {
                        issenior = false;
                    }

                    if (Isdeadradioyes.IsChecked ?? false)
                    {
                        isdead = true;

                    }
                    else
                    {
                        isdead = false;
                    }

                    //editsheep.Birthday = BirthdayDatepicker.SelectedDate.ToDateTime();
                    //editsheep.DateOfDeath = DeathDatepicker.SelectedDate.ToDateTime();
                    //editsheep.DateOfSell = SoldDatepicker.SelectedDate.ToDateTime();
                    //editsheep.DeliveryDate = DeliveryDatepicker.SelectedDate.ToDateTime();
                    //editsheep.Parent_Id = Convert.ToInt32(ParentCombo.SelectedValue);
                    //editsheep.Description = DescriptionTextbox.Text;
                    //editsheep.IsSenior = issenior;
                    //editsheep.IsSold = issold;
                    using (var dammm = new DamDBEntities())
                    {
                        int myid = editsheep.Id;
                        Sheep temp = dammm.Sheep.Where(s => s.Id == myid).FirstOrDefault();

                        temp.Birthday = BirthdayDatepicker.SelectedDate.ToDateTime();
                        
                       
                        temp.DeliveryDate = DeliveryDatepicker.SelectedDate.ToDateTime();

                        temp.Description = DescriptionTextbox.Text;
                        temp.IsSenior = issenior;
                        temp.IsSold = issold;
                        temp.IsDead = isdead;

                        if (ParentCombo.SelectedIndex != -1)
                        {
                            temp.Parent_Id = Convert.ToInt32(ParentCombo.SelectedValue);
                        }
                        else
                        {
                            temp.Parent_Id = null;

                        }
                        if (!isdead)
                        {
                            temp.DateOfDeath = null;
                        }
                        else
                        {
                            temp.DateOfDeath = DeathDatepicker.SelectedDate.ToDateTime();
                        }
                    if (!issold)
                    {
                            temp.DateOfSell = null;
                    }
                    else
                    {
                        temp.DateOfSell = SoldDatepicker.SelectedDate.ToDateTime();
                    }

                        dammm.SaveChanges();
                        MessageBox.Show("تغییر یافت");
                    }

                    // damDBEntities.Entry(editsheep).CurrentValues.SetValues(editsheep);

                }
            }
            catch (Exception ex)
            {
                var mes = MessageBox.Show("در برنامه شما خطاهای زیر وجود دارد. آنهارا رفع و سپس مجددا تلاش کنید" + "\n" + ex.Message, "خطا", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void ClearValues()
        {
            var now = new PersianDate(DateTime.Now);
            DescriptionTextbox.Text = string.Empty ;
            BirthdayDatepicker.SelectedDate = now;
            DeathDatepicker.SelectedDate = now;
            SoldDatepicker.SelectedDate = now;
            DeliveryDatepicker.SelectedDate = now;

            soldnoradio.IsChecked = true;
            Isseniorno.IsChecked = true;
            Isdeadradiono.IsChecked = true;
            ParentCombo.SelectedIndex = -1;

            var q = from sh in damDBEntities.Sheep.ToList()
                    select sh.Id;
            if (q.Count() >= 0)
            {
                List<int> temp = new List<int>();
                foreach (var item in q.ToList())
                {
                    ParentCombo.Items.Add(Convert.ToInt32(item));
                }

            }

        }

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Isdeadradiono_Click(object sender, RoutedEventArgs e)
        {
            DeathDatepicker.IsEnabled = false;
            DeathDatepicker.Visibility = Visibility.Hidden;
            DeathLbl.Visibility = Visibility.Hidden;
        }

        private void Enabledeaddatetimepicker(object sender, RoutedEventArgs e)
        {
            DeathDatepicker.IsEnabled = true;
            DeathDatepicker.Visibility = Visibility.Visible;
            DeathLbl.Visibility = Visibility.Visible;
        }

        private void WithoutName_Click(object sender, RoutedEventArgs e)
        {
            ParentCombo.SelectedIndex = -1;
        }

        private void DisableSellDatetimePicker(object sender, RoutedEventArgs e)
        {
            SoldDatepicker.IsEnabled = false;
            SoldDatepicker.Visibility = Visibility.Hidden;
            SellLbl.Visibility = Visibility.Hidden;
        }

        private void EnableSellDatetimePicker(object sender, RoutedEventArgs e)
        {
            SoldDatepicker.IsEnabled = true;
            SoldDatepicker.Visibility = Visibility.Visible;
            SellLbl.Visibility = Visibility.Visible;
        }
    }
}
