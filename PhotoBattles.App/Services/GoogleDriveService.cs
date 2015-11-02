namespace PhotoBattles.App.Services
{
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Drive.v2;
    using Google.Apis.Services;

    public class GoogleDriveService
    {
        private static readonly string GoogleDriveServiceAccountEmail =
            "440839730918-bl8qakk064m5sahh1hapj6c3pkap1p44@developer.gserviceaccount.com";

        private static readonly string GoogleDrivePrivateKey =
            "-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCnkM8QBUyV+q+f\nJdTnTX1j+gCIFxJ8aN+m/nRlg/U8x18FtDCOQeGQCmLRllsAyMtJtPRMGulR6a2Z\nTNt/DPNzHaXMZ+VCBKvdVjbW7nM/8CxIxgIXIf7uNBLVgYIUQ6FxsnTA+zdpLJgu\nrx3hZQ2UZ/hNU8m224D7wI/bN7uzO4PdLsg3mNpbLxHLDlRnSGrL1gFFJkLTCpzU\nD6vNDqtbXxC9smJBkMjdZ+fSHQXeMqcvuTObD4f/pwH+CA0fokPstjL0ZCiZI2KS\neFJORkb4p0UQLdMKU41YVTTosQ6cdjtbZGtVa722vK4ogcM8N/A1d1oGUSEaxwu6\nEXQyZo+9AgMBAAECggEAINmSvmFLkluC8oBILNBUNLVeRU/AoAMvVV2tE/m6qh+r\n0UJyAnGH9uUSWmRPkufKcri26/SaqSRWlCctIMs5LugxCNGVcEvaJZoxRhGZJn+/\nLsUiw8Y2ZBxxVMn+5lob3F5P5UVap5PGgeByyy00bFO1qfMzKFYAA1rM2P0kHw7K\ndT58t6ZJvjdM90TCUY6+uw+8QfRlzmxptgE9AsP/ia/NNSqJpzuV/IOeclaqgPX0\nekyIQSePj6+JsCwvTGTbriDfHLzV7nW+/Bldc2FjY+1K0IrARiRdQIW8Il5+bbn+\nTTr5+kLS9MUJyZLbF0sbBKxCh5mMbdx77NhppK+jQQKBgQDdyCdZkWq/KLv9c6Np\nFAP14GrO+8HW4y5s+90tactYUST7x66S7DHiD8QfGHkHAOMWtT9Qp6RkA5zvhCzJ\nqEmCX59M7bZoO8KL5V+mnGOpY9i/6tzePG2ErwiJIjw/tzYm3TVO7mel7nSCoRFr\nmyJRHKOLvpD1zP4MZ0b4Eh3XjwKBgQDBaz8vFbOoVnQhwoBoK7bm2zC1mbfLNApA\nX8ovPpSS2NNKGjRKrPADVZPxu3OZpsk9Q/C/7a8IGqmbP7eRZwE+JOi6FCY00nc0\nr8ya3PJrves80f9TeK9zXQuzlXepWhVwe8R8k7ZcLIvJC89Ayrlw3+9l9E1Ru0a6\n6dFeiPxd8wKBgQCoUtzqsd9erj/foQCJI9PNrUHjTlhUC6CUvqOjcAQRR6TWIztv\n7yv88xJat2xD0HyKI42mXVX7QnK9poeHld4UwRZagKBMg+6n1rK99Tv+t/Ut08dG\nNH66kU3dJsqrYRYMcR+ghHjOCykKa8yY6ukkvqOx0DSdGbRHHatTZAyaKQKBgCoC\n/H9irFpFJQZsM+3siNbOB196mxRVImDnLYhjhGv9Eq/Gz1LlC2D+Bpt8Btj6BD39\nct6NZvZQrERfa6T2hauU4sQAOmhAysmz4bJFiZZjcyQLzPS5gwqAzFjef1ZWjc+X\n4o7YmsScnGQ333WIUw00ZtBzZxP4pJ1WyjZJdN+HAoGAHPn3Ju5jBCxOULPKyD+c\nn7RXRjxEI8Coc75OqdIoc8umN6wA5kC2fds+Y0UBsufvAJcRTA+WZaBxMt9kxP6Z\nQkwMR6SmFPploqcXhgCuZRgwlF3DiyteJNiiGY38hAyPCy3zk7p9CrZ5lfOaWmj4\n4zZmaq2JRdl8LCoNBjTwMkI\u003d\n-----END PRIVATE KEY-----\n";

        public static DriveService Get()
        {
            string[] scopes = { DriveService.Scope.Drive, DriveService.Scope.DriveFile };

            ServiceAccountCredential credentials =
                new ServiceAccountCredential(
                    new ServiceAccountCredential.Initializer(GoogleDriveServiceAccountEmail) { Scopes = scopes }
                        .FromPrivateKey(GoogleDrivePrivateKey));

            return new DriveService(new BaseClientService.Initializer { HttpClientInitializer = credentials });
        }
    }
}