﻿namespace FirstProject_Praksa.Database
{
    public class User
    {
        public int Id { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string UserName { get; set; }
        public List<Character> Characters { get; set; }
        public string Role { get; set; }
    }
}
