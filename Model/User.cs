﻿namespace CryptoWallet.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Wallet Wallet { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
