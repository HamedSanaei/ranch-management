//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dam.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sheep
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sheep()
        {
            this.Sheep1 = new HashSet<Sheep>();
        }
    
        public int Id { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<System.DateTime> DateOfDeath { get; set; }
        public Nullable<System.DateTime> DateOfSell { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
        public Nullable<int> Parent_Id { get; set; }
        public Nullable<bool> IsSenior { get; set; }
        public Nullable<bool> IsSold { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsDead { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sheep> Sheep1 { get; set; }
        public virtual Sheep Sheep2 { get; set; }
    }
}
