//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KaraokeProject.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PhongHat
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhongHat()
        {
            this.DatPhongs = new HashSet<DatPhong>();
        }
    
        public string MaPhongHat { get; set; }
        public string TenPhongHat { get; set; }
        public string LoaiPhong { get; set; }
        public string TrangThai { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DatPhong> DatPhongs { get; set; }
        public virtual LoaiPhong LoaiPhong1 { get; set; }
        public virtual TrangThai TrangThai1 { get; set; }
    }
}
