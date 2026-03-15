using MySql.Data.MySqlClient;
using SafeVault.DTOs;

namespace SafeVault.Data
{
    public class Database
    {
        private readonly string _connectionString;

        public Database(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int InsertUser(string username, string email)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Users (Username, Email)
                VALUES (@Username, @Email);
            ";

            command.Parameters.Add("@Username", MySqlDbType.VarChar).Value = username;
            command.Parameters.Add("@Email", MySqlDbType.VarChar).Value = email;

            return command.ExecuteNonQuery();
        }

        public int CreateUser(string username, string email, string passwordHash, string role)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Users (Username, Email, PasswordHash, Role)
                VALUES (@Username, @Email, @PasswordHash, @Role);
            ";

            command.Parameters.Add("@Username", MySqlDbType.VarChar).Value = username;
            command.Parameters.Add("@Email", MySqlDbType.VarChar).Value = email;
            command.Parameters.Add("@PasswordHash", MySqlDbType.VarChar).Value = passwordHash;
            command.Parameters.Add("@Role", MySqlDbType.VarChar).Value = role;

            return command.ExecuteNonQuery();
        }

        public UserAuthRecord? GetUserForLogin(string username)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Username, PasswordHash, Role
                FROM Users
                WHERE Username = @Username
                LIMIT 1;
            ";

            command.Parameters.Add("@Username", MySqlDbType.VarChar).Value = username;

            using var reader = command.ExecuteReader();
            if (!reader.Read())
                return null;

            var foundUsername = reader.GetString("Username");
            var foundPasswordHash = reader.GetString("PasswordHash");
            var foundRole = reader.GetString("Role");

            return new UserAuthRecord(foundUsername, foundPasswordHash, foundRole);
        }

        public Dto? GetUserByUsername(string username)
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Username, Email
                FROM Users
                WHERE Username = @Username
                LIMIT 1;
            ";

            command.Parameters.Add("@Username", MySqlDbType.VarChar).Value = username;

            using var reader = command.ExecuteReader();
            if (!reader.Read())
                return null;

            var foundUsername = reader.GetString("Username");
            var foundEmail = reader.GetString("Email");

            return new Dto(foundUsername, foundEmail);
        }
    }

}