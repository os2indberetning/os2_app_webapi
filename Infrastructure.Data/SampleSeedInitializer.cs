using Core.DomainModel;
using System;
using System.Data.Entity;

namespace Infrastructure.Data
{
    public class SampleSeedInitializer : DropCreateDatabaseIfModelChanges<SampleContext>
    {
        protected override void Seed(SampleContext context)
        {
            /*var item = new Test() { CreatedOn = DateTime.Now, ModifiedOn = DateTime.UtcNow };
            context.Tests.Add(item);
            context.SaveChanges();*/

           /* Profile p = new Profile() { HomeLatitude = "0.0001", HomeLongitude = "0.0002", FirstName = "Jacob", LastName = "Hansen" };

            
            Token t = new Token { TokenString = "0000-1111-2222-3333", Status = true};
            p.Tokens.Add(t);

            Employment e = new Employment() { EmploymentPosition="Sygeplejese", ProfileId=p.id};
            Employment e1 = new Employment() { EmploymentPosition="Fisker", ProfileId=p.id};
            Employment e2 = new Employment() { EmploymentPosition = "Byrådsmedlem", ProfileId=p.id};

            Rate r1 = new Rate() { Type = "Bil, Høj Takst" };
            Rate r2 = new Rate() { Type = "Bil, Lav Takst" };
            Rate r3 = new Rate() { Type = "Cykel, Lav Takst" };

            context.Profiles.Add(p);
            context.Tokens.Add(t);
            context.Employments.Add(e);
            context.Employments.Add(e1);
            context.Employments.Add(e2);

            context.Rates.Add(r1);
            context.Rates.Add(r2);
            context.Rates.Add(r3);

            context.SaveChanges();*/
        }
    }
}
