// <auto-generated />
namespace ERPDAL.ProductionContextMigrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.2.0-61023")]
    public sealed partial class Production_AddPackagingLineIdInRequisitionItemInfo : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(Production_AddPackagingLineIdInRequisitionItemInfo));
        
        string IMigrationMetadata.Id
        {
            get { return "202007300928352_Production_AddPackagingLineIdInRequisitionItemInfo"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}
