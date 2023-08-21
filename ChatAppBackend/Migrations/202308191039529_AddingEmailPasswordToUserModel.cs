namespace ChatAppBackend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingEmailPasswordToUserModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Email", c => c.String());
            AddColumn("dbo.Users", "Password", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Password");
            DropColumn("dbo.Users", "Email");
        }
    }
}
