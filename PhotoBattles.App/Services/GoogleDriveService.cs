namespace PhotoBattles.App.Services
{
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Drive.v2;
    using Google.Apis.Services;

    using PhotoBattles.App.Resources;

    public class GoogleDriveService
    {
        private static readonly string GoogleDriveServiceAccountEmail = GoogleDriveAuth.GoogleDriveServiceAccountEmail;

        private static readonly string GoogleDrivePrivateKey = GoogleDriveAuth.GoogleDrivePrivateKey;

        public static DriveService Get()
        {
            string[] scopes = { DriveService.Scope.Drive, DriveService.Scope.DriveFile };

            var credentials =
                new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer(GoogleDriveServiceAccountEmail) { Scopes = scopes }
                        .FromPrivateKey(GoogleDrivePrivateKey));

            return new DriveService(new BaseClientService.Initializer { HttpClientInitializer = credentials });
        }
    }
}