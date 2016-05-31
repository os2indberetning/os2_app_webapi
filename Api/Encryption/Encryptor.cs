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

        private const string EncryptKey = "testpasswordkey";

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

        public static AuthorizationViewModel EncryptAuthorization(AuthorizationViewModel auth)
        {
            if (auth.GuId != null)
                auth.GuId = StringCipher.Encrypt(auth.GuId, EncryptKey);

            return auth;
        }

        public static AuthorizationViewModel DecryptAuthorization(AuthorizationViewModel auth)
        {
            if (auth.GuId != null)
                auth.GuId = StringCipher.Decrypt(auth.GuId, EncryptKey);

            return auth;
        }

        public static TokenViewModel EncryptToken(TokenViewModel token)
        {
            if (token.GuId != null)
            token.GuId = StringCipher.Encrypt(token.GuId, EncryptKey);

            if (token.TokenString != null)
            token.TokenString = StringCipher.Encrypt(token.TokenString, EncryptKey);

            return token;
        }

        public static AuthRequestViewModel EncryptAuthRequest(AuthRequestViewModel auth)
        {
            if (auth.UserName != null)
                auth.UserName = StringCipher.Encrypt(auth.UserName, EncryptKey);

            return auth;
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
