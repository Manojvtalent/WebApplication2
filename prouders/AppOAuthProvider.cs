using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;

namespace OAuthWithAdoNetExample
{
    public class AppOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _connectionString = "Constr"; // Replace with your actual connection string

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                // Validate user credentials (username and password) from the database
                string username = context.UserName;
                string password = context.Password;

                if (ValidateUserCredentials(username, password))
                {
                    // Create claims for the authenticated user
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, username),
                        // Add other relevant claims (e.g., roles, user ID, etc.)
                    };

                    // Create an authentication ticket
                    var identity = new ClaimsIdentity(claims, context.Options.AuthenticationType);
                    context.Validated(identity);
                }
                else
                {
                    context.SetError("invalid_grant", "Invalid username or password.");
                }
            }
            catch (Exception ex)
            {
                context.SetError("server_error", ex.Message);
            }
        }

        private bool ValidateUserCredentials(string username, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                // Execute a SQL query to validate user credentials
                // Example: SELECT COUNT(*) FROM Users WHERE Username = @username AND Password = @password
                // You should hash passwords and use parameterized queries in production code
                // ...

                // Simulated validation (replace with actual logic)
                return username == "john" && password == "secret";
            }
        }
    }
}