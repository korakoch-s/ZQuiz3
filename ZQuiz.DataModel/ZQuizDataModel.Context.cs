﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZQuiz.DataModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ZQuiz3DBEntities : DbContext
    {
        public ZQuiz3DBEntities()
            : base("name=ZQuizDBModel")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Choice> Choices { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<TesterQuestion> TesterQuestions { get; set; }
        public virtual DbSet<Tester> Testers { get; set; }
    }
}
