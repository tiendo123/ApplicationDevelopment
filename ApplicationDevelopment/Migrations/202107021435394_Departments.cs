namespace ApplicationDevelopment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Departments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DepartmentAssigns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DepartmentId = c.Int(nullable: false),
                        TrainerId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.TrainerId, cascadeDelete: true)
                .Index(t => t.DepartmentId)
                .Index(t => t.TrainerId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DepartmentAssigns", "TrainerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.DepartmentAssigns", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.DepartmentAssigns", new[] { "TrainerId" });
            DropIndex("dbo.DepartmentAssigns", new[] { "DepartmentId" });
            DropTable("dbo.Departments");
            DropTable("dbo.DepartmentAssigns");
        }
    }
}
