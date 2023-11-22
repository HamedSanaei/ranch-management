using Arash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dam
{
    class SheepViewModel
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public decimal Cost { get; set; }
        public PersianDate? Birthday { get; set; }
        public PersianDate? DateOfDeath { get; set; }
        public PersianDate? DateOfSell { get; set; }
        public PersianDate? DeliveryDate { get; set; }
        public int? Parent_Id { get; set; }
        public bool? IsSenior { get; set; }
        public bool? IsSold { get; set; }
        public string Description { get; set; }
        public bool? IsDead { get; set; }
        /// chert ha
        public string BaleghAst { get; set; }
        public string MordehAst { get; set; }
        public string Foroush { get; set; }
        public SheepViewModel(int id,int index, DateTime? birthday,DateTime? dateofdeath ,DateTime? dateofsell, DateTime? deliverydate,int? parent_id,bool? issenior,bool? issold,string description ,bool? isdead)
        {
           
            Id = id;
            Index = index;
            Birthday = new PersianDate(birthday.Value);
            DeliveryDate = new PersianDate(deliverydate.Value);
            if (isdead == true )
            {
                DateOfDeath = new PersianDate(dateofdeath.Value);
            }
            else
            {
                DateOfDeath = null;
               
            }
            if (issold == true)
            {
                DateOfSell = new PersianDate(dateofsell.Value);
            }
            else
            {
              
                DateOfSell = null;
            }
       
            Parent_Id = parent_id;
            IsSenior = issenior;
            IsSold = issold;
            Description = description;
            IsDead = isdead;
            CalculateCost();

            BaleghAst  = (issenior == true) ? "بلی" : "خیر";
            MordehAst  = (isdead == true) ? "بلی" : "خیر";
            Foroush    = (issold == true) ? "بلی" : "خیر";

        }
        void CalculateCost()
        {
            DateTime firstPoint;
            DateTime lastpoint;

            if ((!IsDead ?? false) && (!IsSold ?? false)) // agar namordeh va forosh narafte
            {
                lastpoint = DateTime.Now; // ta zaman hal mohasebe kon
            }
            else if ((IsDead ?? false) && (!IsSold ?? false)) // agar mordeh vali forosh narafte
            {
                lastpoint = DateOfDeath.Value.ToDateTime();// agar ayandeh morde ta ayande mohasebe kon
            }
            else if ((!IsDead ?? false) && (IsSold ?? false))// agar namorde va forosh rafte
            {
                lastpoint =DateOfSell.Value.ToDateTime();// ta zaman forosh mohasebe kon
            }
            else  // agar morde va forosh rafte
            {
                lastpoint = (DateOfSell.Value.ToDateTime() >= DateOfSell.Value.ToDateTime()) ? DateOfSell.Value.ToDateTime() : DateOfSell.Value.ToDateTime();  // har kodom kootah tar bood
            }


            firstPoint = (Birthday.Value.ToDateTime() + new TimeSpan(90, 0, 0, 0) >= DeliveryDate.Value.ToDateTime()) ? (Birthday.Value.ToDateTime() + new TimeSpan(90, 0, 0, 0)) : DeliveryDate.Value.ToDateTime();

            if ((lastpoint - firstPoint) >= new TimeSpan(1, 0, 0, 0, 0)) // agar hade aghal ye roz ye gosfand balegh negah dari shode bod
            {
                if (lastpoint > firstPoint)
                {
                    Cost = (lastpoint - firstPoint).Days;
                }
            }
            else
            {
                Cost = 0;
            }




        }
    }
}
