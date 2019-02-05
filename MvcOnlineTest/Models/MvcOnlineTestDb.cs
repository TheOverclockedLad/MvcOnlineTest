﻿using System.Data.Entity;

namespace MvcOnlineTest.Models
{
    public class MvcOnlineTestDb : DbContext
    {
        public MvcOnlineTestDb() : base("MvcOnlineTestDb") { }

        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentTest> StudentsTests { get; set; }
    }
}