﻿using MahApps.Metro.Controls.Dialogs;
using NLog;

namespace BusanRestaurantApp.Helpers
{
    class Common
    {

        // NLog 인스턴스
        public static readonly Logger LOGGER = LogManager.GetCurrentClassLogger();

        // DB연결문자열을 한군데 저장(사용안 하면 그만임)
        public static readonly string CONNSTR = "Server=127.0.0.1;Database=moviefinder;Uid=root;Pwd=12345;Charset=utf8";

        // MahApps.Metro 다이얼로그
        public static IDialogCoordinator DIALOGCOORDINATOR;

    }
}
