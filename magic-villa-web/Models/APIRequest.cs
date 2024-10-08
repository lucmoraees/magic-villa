﻿using System;
using System.Security.AccessControl;
using static magic_villa_utillity.SD;

namespace magic_villa_web.Models
{
	public class APIRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string Token { get; set; }
    }
}