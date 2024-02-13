using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachineAPI.Utils;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace VendingMachineTest.Utilts
{
    public static class Helper
    {
        private static string _role;
        private static string _userId;
        private static string _userName;
        public static string Role => _role;
        public static string UserId => _userId;
        public static string UserName => _userName;

        public static void Configure(string role, string userId, string userName)
        {
            _role = role;
            _userId = userId;
            _userName = userName;
        }
    }
}
