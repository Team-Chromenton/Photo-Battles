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
            "-----BEGIN PRIVATE KEY-----\nMIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQClxSXiirIm+TdR\n/zmggDCjeQ34H4OyEy3EYIfndgfrnRCvv28cfyx54WuELapUD7hGqA1Kb2HINSnT\n6KK9m6vX6AGKBI4ybGY2GSZZS5NpywX9OJ9+cCV1/IXiZOIg1JRly6ww0N/wTlud\nUgWBxNS0+Xe4y3+zljUIeIOLZkuqLN7qfFM2+2ogMw6yvp1vmwY/IzqiacanFVlS\n0DNdVwmJhaOU1ipbO3ljzaXmW8M/OEQG136StWYsWpGVnMsJRTQs2Blga8sB3pKx\n/H/UFgRvw5DhRI7usc6OqYEnxAvw1PoYV53YoemHg+GJ16DmBB6pwUapxWgnEbOo\ny4gVERJrAgMBAAECggEAUwU3ggugSXCKyiAehkltEpHv8xIlbKu6Qh1uMvej49ZX\now0m4oi2h5vjsuED6vGsgoZrkZnSyOgHOL/LsVYjbBAfpTW+2XX8gp9520LEYzlw\nmG8hQbt8p2h+zPFlZrfQUmL8q74QSVOVAmcvptwjAzflQ8F3BUP273UbvFbREjp0\nMfOIAsw296pO7/ejNOajUPNFo6sSfuMj8FEv1qQhYlu9vlXcQ1CkbbR/aTBlHzRu\nA4C/NEmR9aV9xQ1ZkIoHLSmF9BrwVssBkYciyG/brk0DkIBoe/54H7Pa363/Z4Pl\nmRdwGD8bOtb2KJQopiWeLvp1Idn1T3ClijrinhBGMQKBgQDWM7Hi1kbIV+F2Drmy\nu4Z/Wirl4E0jC2YrB2XjHVTm661s4X3gnZ9mtjS3d1xfefR/uWv4Qd/bKQ2Iszye\nTDLNxejEUNksJFS0v7IzmZr8q3v076tc6010TAvEW5WNMn34OWYmQTusetUlY7E/\nl1Jadwjeqg6G7prR/dFr58fHNwKBgQDGHhNpquDjlMzHd7Z1dHesod92Ei1wMXS5\nTdsIYquTFWuEWm6XIpunXiyV6PhSIYG/t31Ba9Sa6ii9XSYj/PxAUJ9YktV1x2iD\nS3dEoonZR5LUerdnTBvEpWQb9PeLwdLvXMZPbPm730mA3BT+L+Y5aBSc4Mm+8CF7\nvDn7iSjAbQKBgAoFXRsZnzIcOmYFhlaAQ2iN1VfvyGwNSnhU3kVjx+cNu41Od08c\n0KpDd+/lW6Lz16ZqDE0O9+QO5Z0xlXJqai4KRjt49NAF2xihpzqWwxNzCleJJuEv\n+O2p6RsEldiHNjF4rfi4MIx/Kp2XLmGlOKsWyolwN0HEw+VzFGsR6ty9AoGBAL76\nKQ54SXc4bjpt3PXWQrKoC+geV6zKD9G9CkJIE0qioFUic26/TrqE3ofX3uAVKqTr\ntROGZZi71m/MRkL00RkSXricOqbhhY7jBWKlKCrKgvDOJtOtmyrxn9IPTz1qpvJU\nRVo1G3uOH6XXAWmSNOTrvssHKZMkmbJUXLRroFv1AoGAfspYcOgaqeY6rVuSrdfx\nEYBSsfFDo15xsoaKbB45pXEYu6MfdqVmgvDL1x6vhIajrl5+PXrTdHZ8uq65Em6q\n+9+YSqcRBTsU3X10JE+Ocpk+q+FIo7jmi8rJmotstPVzjRnbyS30ZaR+42cNLk6L\nBklsYj9JsIJyqih8udpHDzg\u003d\n-----END PRIVATE KEY-----\n";

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