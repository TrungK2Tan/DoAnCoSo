﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Booking_Dental_Clinic.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DentalClinicEntities : DbContext
    {
        public DentalClinicEntities()
            : base("name=DentalClinicEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<LoaiDichVu> LoaiDichVus { get; set; }
        public virtual DbSet<GoiDichVu> GoiDichVus { get; set; }
        public virtual DbSet<HinhThucThanhToan> HinhThucThanhToans { get; set; }
        public virtual DbSet<HoaDon> HoaDons { get; set; }
        public virtual DbSet<LichHen> LichHens { get; set; }
        public virtual DbSet<NhaSi> NhaSis { get; set; }
        public virtual DbSet<DienDan> DienDans { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
    }
}
