﻿using System;
using ZZZ.Framework;
using ZZZ.Framework.Extentions;
using ZZZ.KNI.GameProject.Services;

namespace ZZZ.KNI.GameProject
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using(MainGame game = new MainGame())
            {
                game.Run();
            }
        }
    }
#endif
}