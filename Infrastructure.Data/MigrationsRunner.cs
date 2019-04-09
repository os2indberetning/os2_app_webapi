using System;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Design;
using System.IO;
using System.Resources;
using Infrastructure.Data.Migrations;

namespace Infrastructure.Data
{
    public struct MigrationInfo
    {
        public readonly string CodeFileName;
        public readonly string DesignerFileName;
        public readonly string ResourceFileName;

        public MigrationInfo(string codeFileName, string designerFileName, string resourceFileName)
        {
            CodeFileName = codeFileName;
            DesignerFileName = designerFileName;
            ResourceFileName = resourceFileName;
        }
    }

    public static class MigrationsRunner
    {
        public static void ApplyMigrations()
        {
            var configuration = new Configuration();
            var migrator = new DbMigrator(configuration);

            migrator.Update();
        }

        public static MigrationInfo CreateMigrations(string migrationName, string migrationDirectory, Func<string, IResourceWriter> createResourceWriter)
        {
            var configuration = new Configuration();
            var scaffolder = new MigrationScaffolder(configuration);
            var migration = scaffolder.Scaffold(migrationName);

            var codeFileName = migration.MigrationId + ".cs";
            var designerFileName = migration.MigrationId + ".Designer.cs";
            var resourceFileName = migration.MigrationId + ".resx";

            File.WriteAllText($"{migrationDirectory}/{codeFileName}", migration.UserCode);
            File.WriteAllText($"{migrationDirectory}/{designerFileName}", migration.DesignerCode);

            using (var resourceWriter = createResourceWriter($"{migrationDirectory}/{resourceFileName}"))
            {
                foreach (var resource in migration.Resources)
                {
                    resourceWriter.AddResource(resource.Key, resource.Value);
                }
            }

            return new MigrationInfo(codeFileName, designerFileName, resourceFileName);
        }
    }
}
