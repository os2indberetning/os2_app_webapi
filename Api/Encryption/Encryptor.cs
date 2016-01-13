using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Encryption
{
    public static class Encryptor
    {

        private const string EncryptKey = "putsomethinghere";

        //Encryption
        public static DriveReportViewModel EncryptDriveReport(DriveReportViewModel driveReport)
        {
            driveReport.route = EncryptRoute(driveReport.route);

            return driveReport;
        }

        public static RouteViewModel EncryptRoute(RouteViewModel route)
        {
            for (int i = 0; i < route.GPSCoordinates.Count(); i++)
            {
                GPSCoordinateModel g = route.GPSCoordinates.ElementAt(i);
                g = EncryptGPSCoordinate(g);
            }

            return route;
        }

        public static GPSCoordinateModel EncryptGPSCoordinate(GPSCoordinateModel gpscoord)
        {
            gpscoord.Latitude = StringCipher.Encrypt(gpscoord.Latitude, EncryptKey);
            gpscoord.Longitude = StringCipher.Encrypt(gpscoord.Longitude, EncryptKey);
            return gpscoord;
        }

        public static TokenViewModel EncryptToken(TokenViewModel token)
        {
            if (token.GuId != null)
            token.GuId = StringCipher.Encrypt(token.GuId, EncryptKey);

            if (token.TokenString != null)
            token.TokenString = StringCipher.Encrypt(token.TokenString, EncryptKey);

            return token;
        }

        //Decrypt
        public static ProfileViewModel DecryptProfile(ProfileViewModel profile)
        {
            profile.Firstname = StringCipher.Decrypt(profile.Firstname, EncryptKey);
            profile.Lastname = StringCipher.Decrypt(profile.Lastname, EncryptKey);
            profile.HomeLatitude = StringCipher.Decrypt(profile.HomeLatitude, EncryptKey);
            profile.HomeLongitude = StringCipher.Decrypt(profile.HomeLongitude, EncryptKey);

            for (int i = 0; i < profile.Employments.Count(); i++)
            {
                EmploymentViewModel e = profile.Employments.ElementAt(i);
                e = DecryptEmployment(e);
            }

            for (int i = 0; i < profile.Tokens.Count(); i++)
            {
                TokenViewModel t = profile.Tokens.ElementAt(i);
                t = DecryptToken(t);
            }

            return profile;
        }

        public static EmploymentViewModel DecryptEmployment(EmploymentViewModel employment)
        {
            employment.EmploymentPosition = StringCipher.Decrypt(employment.EmploymentPosition, EncryptKey);
            return employment;
        }

        public static TokenViewModel DecryptToken(TokenViewModel token)
        {
            if (token.GuId != null)
            token.GuId = StringCipher.Decrypt(token.GuId, EncryptKey);

            if (token.TokenString != null)
            token.TokenString = StringCipher.Decrypt(token.TokenString, EncryptKey);
            return token;
        }
    }
}
