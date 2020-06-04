using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWS_SRP_AUTH
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Enter your email: ");
            var username = Console.ReadLine();
            Console.WriteLine("Enter your password: ");
            var pass = Console.ReadLine();

            AuthenticateWithSrpAsync(username, pass);

            Console.ReadLine();
        }


        private static string LOGIN_URL = "https://cognito-idp.eu-west-1.amazonaws.com/";



        //Start of AWS vars
        private static string AWS_CLIENT_ID = "39sg678apec8n28fj4ekuuc8fl";

        private static string POOL_NAME = "eu-west-1_4InLtrJUj";

        public static async void AuthenticateWithSrpAsync(string email, string pass)
        {

            AmazonCognitoIdentityProviderConfig config = new AmazonCognitoIdentityProviderConfig();
            var provider = new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials(), Amazon.RegionEndpoint.EUWest1);
            var userPool = new CognitoUserPool(POOL_NAME, AWS_CLIENT_ID, provider);

            var user = new CognitoUser(email, AWS_CLIENT_ID, userPool, provider);

            AuthFlowResponse authResponse;

            try
            {
                InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
                {
                    Password = pass
                };

                authResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(false);

            }
            catch (Amazon.CognitoIdentityProvider.Model.NotAuthorizedException)
            {
                Console.WriteLine(email + " incorrect");
                return;
            }
            Console.WriteLine(authResponse);

        }

    }
}
