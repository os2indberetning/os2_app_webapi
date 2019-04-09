using System;
using System.Linq;
using System.Resources;
using Infrastructure.Data;
using Microsoft.Build.BuildEngine;

namespace DBMigrator
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Select an option");
            Console.WriteLine("1. Add migrations");
            Console.WriteLine("2. Update database");

            var choice = Console.ReadKey().KeyChar;

            if (choice == '1')
            {
                Console.WriteLine();
                AddMigrations();
                return;
            }

            if (choice == '2')
            {
                Console.WriteLine();
                UpdateDatabase();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Invalid option, quitting");
        }

        private static void AddMigrations()
        {
            Console.Write("Enter migration name: ");
            var migrationName = Console.ReadLine();

            if (string.IsNullOrEmpty(migrationName))
            {
                Console.WriteLine("No name specified, quitting");
                return;
            }

            var targetProjectFile = Environment
                .GetEnvironmentVariable("TARGET_PROJECT_FILE");
            var migrationsOutputDirectory = Environment
                .GetEnvironmentVariable("MIGRATIONS_OUTPUT_DIRECTORY");

            var migrationInfo = MigrationsRunner.CreateMigrations(
                migrationName,
                migrationsOutputDirectory,
                (output) => new ResXResourceWriter(output));

            UpdateProjectFile(targetProjectFile, migrationInfo);
        }

        private static void UpdateDatabase()
        {
            MigrationsRunner.ApplyMigrations();
        }

        private static void UpdateProjectFile(string targetProjectFile, MigrationInfo migrationInfo)
        {
            var engine = new Engine();
            var project = new Project(engine);
            project.Load(targetProjectFile);

            var itemGroups = project.ItemGroups.OfType<BuildItemGroup>();

            var compileItemGroup = itemGroups
                .FirstOrDefault(g => g.OfType<BuildItem>()
                    .Any(item => item.Name == "Compile"));

            compileItemGroup.AddNewItem("Compile", $"Migrations\\{migrationInfo.CodeFileName}");
            var newDesignerCompileItem = compileItemGroup
                .AddNewItem("Compile", $"Migrations\\{migrationInfo.DesignerFileName}");

            newDesignerCompileItem.SetMetadata("DependentUpon", migrationInfo.CodeFileName);

            var embeddedResourceItemGroup = itemGroups
                .FirstOrDefault(g => g.OfType<BuildItem>()
                    .Any(item => item.Name == "EmbeddedResource"));

            var newEmbeddedResource = embeddedResourceItemGroup
                .AddNewItem("EmbeddedResource", $"Migrations\\{migrationInfo.ResourceFileName}");
            newEmbeddedResource.SetMetadata("DependentUpon", migrationInfo.CodeFileName);

            project.Save(targetProjectFile);
        }
    }
}
