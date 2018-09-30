using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Microservices.SignalR
{
    public class UserController : Controller
    {
        [HttpPost]
        [Route("/login")]
        public IActionResult Login(string name, string token)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(token))
                return Json(new { status = -1, message = "用户名错误" });
            var user = UserCache.List.FirstOrDefault(en => en.PrivateToken == token);
            if (user == null || !user.Online)
                return Json(new { status = -1, message = "err" });
            if (UserCache.List.Count(en => en.Name == name && en.Online) > 0)
                return Json(new { status = -1, message = "用户已存在" });
            user.LoginTime = DateTime.Now;
            user.Name = name;
            return Json(new { status = 1, message = "helo" });
        }

        [HttpPost]
        [Route("/list")]
        public IActionResult GetUserList(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return Json(new { status = -1, message = "" });
            var user = UserCache.List.FirstOrDefault(en => en.PrivateToken == token);
            if (user == null || !user.Online)
                return Json(new { status = -1, message = "err" });

            var list = UserCache.List.Where(en => en.Online && en.PrivateToken != token)
                .Select(en => new { name = en.Name, token = en.PublicToken }).ToList();
            return Json(new { status = 1, message = "", list = list, count = list.Count });
        }

    }
}
